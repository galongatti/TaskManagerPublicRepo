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
        private (UserService, Mock<IUserRepository>, Mock<IConfiguration>, Mock<IAssignmentService>) SetupUserService()
        {
            Mock<IUserRepository> mockRepository = new();
            Mock<IConfiguration> mockConfiguration = new();
            Mock<IAssignmentService> mockAssignmentService = new();
            mockConfiguration.Setup(config => config["HashPepper"]).Returns("testPepper");

            UserService userService = new(mockRepository.Object, mockConfiguration.Object, mockAssignmentService.Object);
            return (userService, mockRepository, mockConfiguration, mockAssignmentService);
        }
        
        [Fact]
        public void TestUserCreation_UserDoesNotExist()
        {
            (UserService userService, Mock<IUserRepository> mockRepository, Mock<IConfiguration> mockConfiguration, Mock<IAssignmentService> mockAssignmentService) = SetupUserService();
            
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
            mockRepository.Setup(repo => repo.GetUserByEmail(userDto.Email)).Returns((User)null!);
            
            //Mocking the AddMember method to return true, simulating that the user was successfully added
            mockRepository.Setup(repo => repo.AddMember(It.IsAny<User>()
            )).Returns(true);
            
            //Calling the AddMember method from the UserService class
            bool result = userService.AddMember(userDto);
            
            //Asserting that the result is true
            Assert.True(result);
            mockRepository.Verify(repo => repo.GetUserByEmail(userDto.Email), Times.Once);
            mockRepository.Verify(repo => repo.AddMember(It.Is<User>(u => u.Email == userDto.Email && u.Name == userDto.Name)), Times.Once);
            
        }

        [Fact]
        public void TestUserCreation_UserExists()
        {
            (UserService userService, Mock<IUserRepository> mockRepository, Mock<IConfiguration> mockConfiguration, Mock<IAssignmentService> mockAssignmentService) = SetupUserService();
            
            mockRepository.Setup(repo => repo.GetUserByEmail(It.IsAny<string>())).Returns(new User());
            
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

            mockRepository.Setup(repo => repo.GetUserByEmail(It.IsAny<string>())).Returns(existingUser);

            Exception exception = Assert.Throws<Exception>(() => userService.AddMember(userDto));
            Assert.Equal("User already exists", exception.Message);
            mockRepository.Verify(repo => repo.GetUserByEmail(It.IsAny<string>()), Times.Once);
            mockRepository.Verify(repo => repo.AddMember(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public void TestUserUpdate_UserExists()
        {
            (UserService userService, Mock<IUserRepository> mockRepository, Mock<IConfiguration> mockConfiguration, Mock<IAssignmentService> mockAssignmentService) = SetupUserService();
            
            UserUpdateDto userDto = new()
            {
                Email = "test2@example.com",
                Post = "Developer",
                Enabled = true,
                Name = "Test User",
                IdTeam = 1,
                IdUser = 1
            };
            
            mockRepository.Setup(repo => repo.GetUserById(It.IsAny<int>())).Returns(new User());

            mockRepository.Setup(repo => repo.UpdateMember(It.IsAny<User>())).Returns(true);
            
            bool result = userService.UpdateMember(userDto);
            
            Assert.True(result);
            
            mockRepository.Verify(repo => repo.GetUserById(It.IsAny<int>()), Times.Once);
            mockRepository.Verify(repo => repo.UpdateMember(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public void TestUpdate_UserDoesNotExist()
        {
            (UserService userService, Mock<IUserRepository> mockRepository, Mock<IConfiguration> mockConfiguration, Mock<IAssignmentService> mockAssignmentService) = SetupUserService();
            
            mockRepository.Setup(repo => repo.GetUserById(It.IsAny<int>())).Returns((User)null!);
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

            Exception exception = Assert.Throws<Exception>(() => userService.UpdateMember(userDto));
            Assert.Equal("User does not exists", exception.Message);
            
            mockRepository.Verify(repo => repo.GetUserById(It.IsAny<int>()), Times.Once);
            mockRepository.Verify(repo => repo.UpdateMember(It.IsAny<User>()), Times.Never);
        }
        
    }
}