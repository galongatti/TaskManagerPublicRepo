using Microsoft.Extensions.Configuration;
using Moq;
using src.TaskManagerBackEnd;
using src.TaskManagerBackEnd.Repository;
using TaskManagerBackEnd.DTO;
using TaskManagerBackEnd.Service;


namespace TaskManagerBackEndTest
{
    public class UserTest
    {
        [Fact]
        public void TestUserCreation_UserDoesNotExist()
        {
            //Mocking the repository and configuration
            Mock<IUserRepository> mockRepository = new();
            Mock<IConfiguration> mockConfiguration = new();
            mockConfiguration.Setup(config => config["HashPepper"]).Returns("testPepper");
            
            //Creating the UserService class, passing the mocked repository and configuration as parameters
            UserService userService = new(mockRepository.Object, mockConfiguration.Object);
            
            //Creating a UserInsertDTO object with the data of the user to be created
            UserInsertDTO userDto = new()
            {
                Email = "test2@example.com",
                Password = "password",
                Post = "Developer",
                Enabled = true,
                Name = "Test User",
                IdTeam = 1
            };
            
            //Mocking the GetMemberByEmail method to return null, simulating that the user does not exist
            mockRepository.Setup(repo => repo.GetMemberByEmail(userDto.Email)).Returns((User)null!);
            
            //Mocking the AddMember method to return true, simulating that the user was successfully added
            mockRepository.Setup(repo => repo.AddMember(It.IsAny<User>()
            )).Returns(true);
            
            //Calling the AddMember method from the UserService class
            bool result = userService.AddMember(userDto);
            
            //Asserting that the result is true
            Assert.True(result);
            mockRepository.Verify(repo => repo.GetMemberByEmail(userDto.Email), Times.Once);
            mockRepository.Verify(repo => repo.AddMember(It.Is<User>(u => u.Email == userDto.Email && u.Name == userDto.Name)), Times.Once);
            
        }

        [Fact]
        public void TestUserCreation_UserExists()
        {
            Mock<IUserRepository> mockRepository = new();
            Mock<IConfiguration> mockConfiguration = new();
            mockConfiguration.Setup(config => config["HashPepper"]).Returns("testPepper");

            UserService userService = new(mockRepository.Object, mockConfiguration.Object);
            
            mockRepository.Setup(repo => repo.GetMemberByEmail(It.IsAny<string>())).Returns(new User());
            
            UserInsertDTO userDto = new()
            {
                Email = "test@example.com",
                Password = "password",
                Post = "Developer",
                Enabled = true,
                Name = "Test User",
                IdTeam = 1
            };

            User existingUser = new()
            {
                Email = "test@example.com"
            };

            mockRepository.Setup(repo => repo.GetMemberByEmail(It.IsAny<string>())).Returns(existingUser);

            Exception exception = Assert.Throws<Exception>(() => userService.AddMember(userDto));
            Assert.Equal("User already exists", exception.Message);
            mockRepository.Verify(repo => repo.GetMemberByEmail(It.IsAny<string>()), Times.Once);
            mockRepository.Verify(repo => repo.AddMember(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public void TestUserUpdate_UserExists()
        {
            Mock<IUserRepository> mockRepository = new();
            Mock<IConfiguration> mockConfiguration = new();
            mockConfiguration.Setup(config => config["HashPepper"]).Returns("testPepper");
            
            UserService userService = new(mockRepository.Object, mockConfiguration.Object);
            
            UserUpdateDto userDto = new()
            {
                Email = "test2@example.com",
                Post = "Developer",
                Enabled = true,
                Name = "Test User",
                IdTeam = 1,
                IdUser = 1
            };
            
            mockRepository.Setup(repo => repo.GetMemberById(It.IsAny<int>())).Returns(new User());

            mockRepository.Setup(repo => repo.UpdateMember(It.IsAny<User>())).Returns(true);
            
            bool result = userService.UpdateMember(userDto);
            
            Assert.True(result);
            
            mockRepository.Verify(repo => repo.GetMemberById(It.IsAny<int>()), Times.Once);
            mockRepository.Verify(repo => repo.UpdateMember(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public void TestUpdate_UserDoesNotExist()
        {
            Mock<IUserRepository> mockRepository = new();
            Mock<IConfiguration> mockConfiguration = new();
            mockConfiguration.Setup(config => config["HashPepper"]).Returns("testPepper");

            UserService userService = new(mockRepository.Object, mockConfiguration.Object);
            
            mockRepository.Setup(repo => repo.GetMemberById(It.IsAny<int>())).Returns((User)null!);
            mockRepository.Setup(repo => repo.UpdateMember(It.IsAny<User>())).Returns(false);

            UserUpdateDto userDto = new()
            {
                Email = "test2@example.com",
                Post = "Developer",
                Enabled = true,
                Name = "Test User",
                IdTeam = 1,
                IdUser = 1
            };
            
            bool result = userService.UpdateMember(userDto);
            
            
            




        }
        
    }
}