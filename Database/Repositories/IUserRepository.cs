using Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Repositories
{
    public interface IUserRepository
    {
        ICollection<UserModel> GetAllUsers();
        Task<UserModel?> GetUserByIdAsync(string Email);
        Task AddUserAsync(UserModel User);
        Task UpdateUserAsync(UserModel User);
        Task DeleteUserByIdAsync(string Email);
    }
}
