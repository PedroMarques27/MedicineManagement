using Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Tests.Database.Repositories;
using Tests.Repositories.Models;

namespace Tests.Database
{
    public class UserRepositoryTests
    {
        private readonly MockIUserRepository _mockUserRepository;

        public UserRepositoryTests()
        {
            _mockUserRepository = new MockIUserRepository();
        }

        [Fact]
        public async Task GetAllUsers_ShouldReturnListOfUsers()
        {
            await _mockUserRepository.SeedUsersAsync();

            var result = _mockUserRepository.GetAllUsers();

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(MockUserModel.Mock_Default().Email, result.First().Email);
            Assert.Equal(MockUserModel.Mock_Default().Name, result.First().Name);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnUser_IfExists()
        {
            await _mockUserRepository.SeedUsersAsync();

            var result = await _mockUserRepository.GetUserByIdAsync(MockUserModel.Mock_Default().Email);

            Assert.NotNull(result);
            Assert.Equal(MockUserModel.Mock_Default().Email, result.Email);
            Assert.Equal(MockUserModel.Mock_Default().Name, result.Name);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnNull_IfNotFound()
        {
            var result = await _mockUserRepository.GetUserByIdAsync(MockUserModel.Mock().Email);

            Assert.Null(result);
        }

        [Fact]
        public async Task AddUserAsync_ShouldAddUserSuccessfully()
        {
            await _mockUserRepository.SeedUsersAsync();
            var newUser = MockUserModel.Mock();

            await _mockUserRepository.AddUserAsync(newUser);

            var users = _mockUserRepository.GetAllUsers();
            Assert.Equal(2, users.Count);
        }

        [Fact]
        public async Task UpdateUserAsync_ShouldUpdateUserSuccessfully()
        {
            await _mockUserRepository.SeedUsersAsync();
            var data = await _mockUserRepository.GetUserByIdAsync(MockUserModel.Mock_Default().Email);

            Assert.NotNull(data);
            data.Name = "Updated User";

            await _mockUserRepository.UpdateUserAsync(data);

            var result = await _mockUserRepository.GetUserByIdAsync(MockUserModel.Mock_Default().Email);
            Assert.NotNull(result);
            Assert.Equal("Updated User", result.Name);
        }

        [Fact]
        public async Task DeleteUserByIdAsync_ShouldDeleteUserSuccessfully()
        {
            await _mockUserRepository.SeedUsersAsync();

            await _mockUserRepository.DeleteUserByIdAsync(MockUserModel.Mock_Default().Email);

            var result = await _mockUserRepository.GetUserByIdAsync(MockUserModel.Mock_Default().Email);
            Assert.Null(result);
        }
    }
}
