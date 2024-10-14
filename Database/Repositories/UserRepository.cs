using Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly DatabaseContext _context;

        public UserRepository(DatabaseContext context)
        {
            _context = context;
        }

        public ICollection<UserModel> GetAllUsers()
        {
            return _context.Users
                .Include(u => u.PrescriptionList)
                .ThenInclude(p => p.MedicineList).ToList();
        }


        public async Task<UserModel?> GetUserByIdAsync(string Email)
        {
            return await _context.Users
                .Include(u=> u.PrescriptionList)
                .ThenInclude(p => p.MedicineList)
                .FirstOrDefaultAsync(c => string.Equals(c.Email, Email));
        }

        public async Task AddUserAsync(UserModel User)
        {
            await _context.Users.AddAsync(User);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(UserModel User)
        {
            _context.Users.Update(User);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserByIdAsync(string Email)
        {
            var User = await GetUserByIdAsync(Email);
            if (User != null) _context.Users.Remove(User);
            await _context.SaveChangesAsync();
        }
    }
}
