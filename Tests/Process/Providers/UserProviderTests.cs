using AutoMapper;
using Database.Models;
using Database.Repositories;
using Moq;
using Process.DTOs.Entities;
using Process.DTOs;
using Process.Providers;
using Tests.Repositories.Models;
using Tests.Process.Entities;
using Tests.Process.DTOs;

namespace Tests.Process.Providers
{
    public class UserProviderTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly UsersProvider _provider;
        public UserProviderTests() 
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockMapper = new Mock<IMapper>();
            _provider = new UsersProvider(_mockMapper.Object, _mockUserRepository.Object);
        }

        [Fact]
        public async Task AddUser_ShouldReturnOk_WhenUserIsAddedSuccessfully()
        {
            var newUser = MockUser.Mock_Default();
         
            _mockUserRepository
                .Setup(repo => repo.AddUserAsync(It.IsAny<UserModel>()))
                .Returns(Task.CompletedTask);

            var result = await _provider.AddUser(newUser);

            Assert.True(result.Success);
            Assert.Equal(StatusResponseDTO.Ok(null).Error, result.Error);
            Assert.Null(result.Data);

            _mockUserRepository.Verify(repo => repo.AddUserAsync(It.IsAny<UserModel>()), Times.Once);
        }

        [Fact]
        public async Task AddUser_ShouldReturnError_WhenExceptionIsThrown()
        {
            var newUser = MockUser.Mock_Default();

            _mockUserRepository
                .Setup(repo => repo.AddUserAsync(It.IsAny<UserModel>()))
                .ThrowsAsync(new Exception("Error adding user"));

            var result = await _provider.AddUser(newUser);

            Assert.False(result.Success);
            Assert.Equal("Error adding user", result.Error);
        }

        [Fact]
        public async Task DeleteUser_ShouldReturnOk_WhenUserIsDeletedSuccessfully()
        {
            var email = MockUser.Mock_Default().Email;

            _mockUserRepository
                .Setup(repo => repo.DeleteUserByIdAsync(email))
                .Returns(Task.CompletedTask);

            var result = await _provider.DeleteUser(email);

            Assert.True(result.Success);
            Assert.Equal(StatusResponseDTO.Ok(null).Error, result.Error);
            Assert.Null(result.Data);

            _mockUserRepository.Verify(repo => repo.DeleteUserByIdAsync(email), Times.Once);
        }

        [Fact]
        public async Task DeleteUser_ShouldReturnError_WhenExceptionIsThrown()
        {
            var email = MockUser.Mock_Default().Email;
            _mockUserRepository
                .Setup(repo => repo.DeleteUserByIdAsync(email))
                .ThrowsAsync(new Exception("Error deleting user"));

            var result = await _provider.DeleteUser(email);

            Assert.False(result.Success);
            Assert.Equal("Error deleting user", result.Error);
        }

        [Fact]
        public async Task GetUserByEmail_ShouldReturnUser_WhenUserExists()
        {
            var email = MockUser.Mock_Default().Email;

            _mockUserRepository
                .Setup(repo => repo.GetUserByIdAsync(email))
                .ReturnsAsync(MockUserModel.Mock_Default());

            _mockMapper
                .Setup(mapper => mapper.Map<User>(It.IsAny<UserModel>()))
                .Returns(MockUser.Mock_Default());

            var result = await _provider.GetUserByEmail(email);

            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.IsType<User>(result.Data);
            Assert.Equal(email, ((User)result.Data).Email);
        }

        [Fact]
        public async Task GetUserByEmail_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            var email = MockUser.Mock_Default().Email;
            _mockUserRepository
                .Setup(repo => repo.GetUserByIdAsync(email))
                .Returns(Task.FromResult<UserModel?>(null));

            var result = await _provider.GetUserByEmail(email);

            Assert.False(result.Success);
            Assert.Equal(StatusResponseDTO.NotFoundError().Error, result.Error);
            Assert.Null(result.Data);
        }
        [Fact]
        public async Task GetUserByEmail_ShouldReturnError_WhenExceptionIsThrown()
        {
            var email = MockUser.Mock_Default().Email;
            _mockUserRepository
                .Setup(repo => repo.GetUserByIdAsync(email))
                .ThrowsAsync(new Exception("Failed to retrieve user"));

            var result = await _provider.GetUserByEmail(email);

            Assert.False(result.Success);
            Assert.Equal("Failed to retrieve user", result.Error);
        }

        [Fact]
        public void GetUsers_ShouldReturnAllUsers()
        {
            var userList = new List<UserModel>
            {
                MockUserModel.Mock_Default(),
                MockUserModel.Mock(),
            };

            _mockUserRepository
                .Setup(repo => repo.GetAllUsers())
                .Returns(userList);

            _mockMapper
                .Setup(mapper => mapper.Map<ICollection<User>>(It.IsAny<ICollection<UserModel>>()))
                .Returns(new List<User>
                {
                   MockUser.Mock_Default(),
                   MockUser.Mock()
                });

            var result = _provider.GetUsers();

            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.IsType<List<User>>(result.Data);
            Assert.Equal(2, ((List<User>)result.Data).Count);
        }

        [Fact]
        public void GetUsers_ShouldReturnError_WhenExceptionIsThrown()
        {

            _mockUserRepository
                .Setup(repo => repo.GetAllUsers())
                .Throws(new Exception("Failed to retrieve users"));

            var result = _provider.GetUsers();

            Assert.False(result.Success);
            Assert.Equal("Failed to retrieve users", result.Error);
        }

        [Fact]
        public async Task UpdateUser_ShouldReturnOk_WhenUserIsUpdatedSuccessfully()
        {
            _mockUserRepository
                .Setup(repo => repo.GetUserByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(MockUserModel.Mock_Default());
            
            var userEntity = MockUser.Mock_Default();
            userEntity.Name = "Updated User";

            _mockMapper
                .Setup(mapper => mapper.Map<User>(It.IsAny<UserModel>()))
                .Returns(userEntity);


            var user = MockUserInputDto.Mock_Default();
            user.Name = "Updated User";
            var result = await _provider.UpdateUser(user.Email, user);

            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            _mockUserRepository.Verify(repo => repo.UpdateUserAsync(It.IsAny<UserModel>()), Times.Once);
            Assert.Equal("Updated User", ((User)result.Data).Name);
        }

        [Fact]
        public async Task UpdateUser_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            var user = MockUserInputDto.Mock_Default();

            _mockUserRepository
               .Setup(repo => repo.GetUserByIdAsync(It.IsAny<string>()))
               .ReturnsAsync((UserModel?)null);

            var result = await _provider.UpdateUser(user.Email, user);

            Assert.False(result.Success);
            Assert.Equal(StatusResponseDTO.NotFoundError().Error, result.Error);
        }
        [Fact]
        public async Task UpdateUser_ShouldReturnError_WhenExceptionIsThrown()
        {
            var userModel = MockUserModel.Mock_Default();
            _mockUserRepository
                .Setup(repo => repo.GetUserByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(userModel);

            var user = MockUserInputDto.Mock_Default();
            
            _mockUserRepository
               .Setup(repo => repo.UpdateUserAsync(It.IsAny<UserModel>()))
                .ThrowsAsync(new Exception("Failed to update user"));

            var result = await _provider.UpdateUser(user.Email, user);

            Assert.False(result.Success);
            Assert.Equal("Failed to update user", result.Error);
        }
    }
}
