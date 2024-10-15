using Database.Models;
using Database.Repositories;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Database;
using Tests.Repositories.Models;

namespace Tests.Database.Repositories
{
    public class MockIUserRepository
    {
        private readonly DatabaseContext _context;
        private readonly IUserRepository _userRepository;

        public MockIUserRepository()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new DatabaseContext(options);
            _userRepository = new UserRepository(_context);
        }

   

        public async Task SeedUsersAsync()
        {
            var user = MockUserModel.Mock_Default();

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public ICollection<UserModel> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }

        public async Task<UserModel?> GetUserByIdAsync(string email)
        {
            return await _userRepository.GetUserByIdAsync(email);
        }

        public async Task AddUserAsync(UserModel user)
        {
            await _userRepository.AddUserAsync(user);
        }

        public async Task UpdateUserAsync(UserModel user)
        {
            await _userRepository.UpdateUserAsync(user);
        }

        public async Task DeleteUserByIdAsync(string email)
        {
            await _userRepository.DeleteUserByIdAsync(email);
        }
    }
}
