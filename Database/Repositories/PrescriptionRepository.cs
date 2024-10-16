using Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Repositories
{
    public class PrescriptionRepository : IPrescriptionRepository
    {
        private readonly DatabaseContext _context;

        public PrescriptionRepository(DatabaseContext context)
        {
            _context = context;
        }

        public ICollection<PrescriptionModel> GetAllPrescriptions()
        {
            return _context.Prescriptions
                .Include(p => p.MedicineList)
                .ThenInclude(pm => pm.Medicine).ToList();
        }


        public PrescriptionModel? GetPrescriptionById(Guid id)
        {
            return _context.Prescriptions
                .Include(u => u.MedicineList)
                .ThenInclude(pm => pm.Medicine)
                .FirstOrDefault(m=> m.Id == id);
        }

        public async Task AddPrescriptionAsync(PrescriptionModel Prescription)
        {
            await _context.Prescriptions.AddAsync(Prescription);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePrescriptionAsync(PrescriptionModel Prescription)
        {
            _context.Prescriptions.Update(Prescription);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePrescriptionByIdAsync(Guid id)
        {
            var Prescription = GetPrescriptionById(id);
            if (Prescription == null)
            {
                return;
            }
            _context.Prescriptions.Remove(Prescription);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Exists(Guid id)
        {
            return await _context.Prescriptions.AnyAsync(p => p.Id == id);
        }
    }
}
