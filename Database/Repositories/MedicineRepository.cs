using Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Repositories
{
    public class MedicineRepository: IMedicineRepository
    {
        private readonly DatabaseContext _context;

        public MedicineRepository(DatabaseContext context)
        {
            _context = context;
        }

        public ICollection<MedicineModel> GetAllMedicines()
        {
            return _context.Medicines.ToList();
        }

     
        public async Task<MedicineModel?> GetMedicineByNameAsync(string name)
        {
            return await _context.Medicines.FirstOrDefaultAsync(c => string.Equals(c.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        public async Task AddMedicineAsync(MedicineModel medicine)
        {
            await _context.Medicines.AddAsync(medicine);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMedicineAsync(MedicineModel medicine)
        {
            _context.Medicines.Update(medicine);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMedicineByNameAsync(string Name)
        {
            var medicine = new MedicineModel { Name = Name };
            _context.Medicines.Remove(medicine);
            await _context.SaveChangesAsync();
        }
    }
}
