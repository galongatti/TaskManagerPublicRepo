using Microsoft.Extensions.Configuration;
using Moq;
using src.TaskManagerBackEnd;
using src.TaskManagerBackEnd.Repository;
using TaskManagerBackEnd.DTO;
using TaskManagerBackEnd.Model;
using TaskManagerBackEnd.Service;
using UserService = src.TaskManagerBackEnd.UserService;


namespace TaskManagerBackEndTest
{
    public class UserServiceTest
    {
        private (TaskManagerBackEnd.Service.UserService, Mock<IUserRepository>, Mock<IConfiguration>, Mock<IAssignmentService>) SetupUserService()
        {
            Mock<IUserRepository> mockRepository = new();
            Mock<IConfiguration> mockConfiguration = new();
            Mock<IAssignmentService> mockAssignmentService = new();
            Mock<IServiceProvider> mockServiceProvider = new();
            mockConfiguration.Setup(config => config["HashPepper"]).Returns("testPepper");

            TaskManagerBackEnd.Service.UserService userService = new(mockRepository.Object, mockConfiguration.Object, mockAssignmentService.Object, mockServiceProvider.Object);
            return (userService, mockRepository, mockConfiguration, mockAssignmentService);
        }
        
        [Fact]
        public void TestUserCreation_UserDoesNotExist()
        {
            (TaskManagerBackEnd.Service.UserService userService, Mock<IUserRepository> mockRepository, Mock<IConfiguration> mockConfiguration, Mock<IAssignmentService> mockAssignmentService) = SetupUserService();
            
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
            mockRepository.Setup(repo => repo.GetUserByEmail(userDto.Email)).Returns((UserService)null!);
            
            //Mocking the AddMember method to return true, simulating that the user was successfully added
            mockRepository.Setup(repo => repo.AddMember(It.IsAny<UserService>()
            )).Returns(true);
            
            //Calling the AddMember method from the UserService class
            bool result = userService.AddUser(userDto);
            
            //Asserting that the result is true
            Assert.True(result);
            mockRepository.Verify(repo => repo.GetUserByEmail(userDto.Email), Times.Once);
            mockRepository.Verify(repo => repo.AddMember(It.Is<UserService>(u => u.Email == userDto.Email && u.Name == userDto.Name)), Times.Once);
            
        }

        [Fact]
        public void TestUserCreation_UserExists()
        {
            (TaskManagerBackEnd.Service.UserService userService, Mock<IUserRepository> mockRepository, Mock<IConfiguration> mockConfiguration, Mock<IAssignmentService> mockAssignmentService) = SetupUserService();
            
            mockRepository.Setup(repo => repo.GetUserByEmail(It.IsAny<string>())).Returns(new UserService());
            
            UserInsertDTO userDto = new()
            {
                Email = "test@example.com",
                Password = "password",
                Post = "Developer",
                Enabled = true,
                Name = "Test User",
                IdTeam = 1
            };

            UserService existingUserService = new()
            {
                Email = "test@example.com"
            };

            mockRepository.Setup(repo => repo.GetUserByEmail(It.IsAny<string>())).Returns(existingUserService);

            Exception exception = Assert.Throws<Exception>(() => userService.AddUser(userDto));
            Assert.Equal("User already exists", exception.Message);
            mockRepository.Verify(repo => repo.GetUserByEmail(It.IsAny<string>()), Times.Once);
            mockRepository.Verify(repo => repo.AddMember(It.IsAny<UserService>()), Times.Never);
        }

        [Fact]
        public void TestUserUpdate_UserExists()
        {
            (TaskManagerBackEnd.Service.UserService userService, Mock<IUserRepository> mockRepository, Mock<IConfiguration> mockConfiguration, Mock<IAssignmentService> mockAssignmentService) = SetupUserService();
            
            UserUpdateDto userDto = new()
            {
                Email = "test2@example.com",
                Post = "Developer",
                Enabled = true,
                Name = "Test User",
                IdTeam = 1,
                IdUser = 1
            };
            
            mockRepository.Setup(repo => repo.GetUserById(It.IsAny<int>())).Returns(new UserService());

            mockRepository.Setup(repo => repo.UpdateMember(It.IsAny<UserService>())).Returns(true);
            
            bool result = userService.UpdateUser(userDto);
            
            Assert.True(result);
            
            mockRepository.Verify(repo => repo.GetUserById(It.IsAny<int>()), Times.Once);
            mockRepository.Verify(repo => repo.UpdateMember(It.IsAny<UserService>()), Times.Once);
        }

        [Fact]
        public void TestUpdate_UserDoesNotExist()
        {
            (TaskManagerBackEnd.Service.UserService userService, Mock<IUserRepository> mockRepository, Mock<IConfiguration> mockConfiguration, Mock<IAssignmentService> mockAssignmentService) = SetupUserService();
            
            mockRepository.Setup(repo => repo.GetUserById(It.IsAny<int>())).Returns((UserService)null!);
            mockRepository.Setup(repo => repo.UpdateMember(It.IsAny<UserService>())).Returns(false);

            UserUpdateDto userDto = new()
            {
                Email = "test2@example.com",
                Post = "Developer",
                Enabled = true,
                Name = "Test User",
                IdTeam = 1,
                IdUser = 1
            };

            Exception exception = Assert.Throws<Exception>(() => userService.UpdateUser(userDto));
            Assert.Equal("User does not exists", exception.Message);
            
            mockRepository.Verify(repo => repo.GetUserById(It.IsAny<int>()), Times.Once);
            mockRepository.Verify(repo => repo.UpdateMember(It.IsAny<UserService>()), Times.Never);
        }

        [Fact]
        public void TestUserDelete_UserHasAssignment()
        {
            (TaskManagerBackEnd.Service.UserService userService, Mock<IUserRepository> mockRepository, Mock<IConfiguration> mockConfiguration, Mock<IAssignmentService> mockAssignmentService) = SetupUserService();

            mockAssignmentService.Setup(service => service.GetAssignmentsByUserId(It.IsAny<int[]>())).Returns([new Assignment(), new Assignment()]);
            
            Exception exception = Assert.Throws<Exception>(() => userService.DeleteUser(It.IsAny<int>()));
            
            Assert.Equal("The user has assignments found.", exception.Message);
            
            mockAssignmentService.Verify(service => service.GetAssignmentsByUserId(It.IsAny<int[]>()), Times.Once);
            mockRepository.Verify(repo => repo.DeleteUser(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void TestUserDelete_UserDoesNoHasAssignment()
        {
            (TaskManagerBackEnd.Service.UserService userService, Mock<IUserRepository> mockRepository, Mock<IConfiguration> mockConfiguration, Mock<IAssignmentService> mockAssignmentService) = SetupUserService();
            
            mockAssignmentService.Setup(service => service.GetAssignmentsByUserId(It.IsAny<int[]>())).Returns(new List<Assignment>());

            mockRepository.Setup(repo => repo.DeleteUser(It.IsAny<int>())).Returns(true);
            
            bool result = userService.DeleteUser(It.IsAny<int>());
            
            Assert.True(result);
            mockAssignmentService.Verify(service => service.GetAssignmentsByUserId(It.IsAny<int[]>()), Times.Once);
            
            mockRepository.Verify(repo => repo.DeleteUser(It.IsAny<int>()), Times.Once);
        }
        
    }
}