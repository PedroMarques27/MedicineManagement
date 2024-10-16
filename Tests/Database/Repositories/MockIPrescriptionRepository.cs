using Database;
using Database.Models;
using Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Repositories.Models;

namespace Tests.Database.Repositories
{
    public class MockIPrescriptionRepository 
    {
        private readonly DatabaseContext _context;
        private readonly IPrescriptionRepository _prescriptionRepository;
        public MockIPrescriptionRepository()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;

            _context = new DatabaseContext(options);
            _prescriptionRepository = new PrescriptionRepository(_context);
        }

        public async Task SeedPrescriptionsAsync()
        {
            var prescription = MockPrescriptionModel.Mock_Default();

            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();
        }

        public ICollection<PrescriptionModel> GetAllPrescriptions()
        {
            return _prescriptionRepository.GetAllPrescriptions();
        }

        public PrescriptionModel? GetPrescriptionById(Guid id)
        {
            return _prescriptionRepository.GetPrescriptionById(id);
        }

        public async Task AddPrescriptionAsync(PrescriptionModel prescription)
        {
            await _prescriptionRepository.AddPrescriptionAsync(prescription);
        }

        public async Task UpdatePrescriptionAsync(PrescriptionModel prescription)
        {
            await _prescriptionRepository.UpdatePrescriptionAsync(prescription);
        }

        public async Task DeletePrescriptionByIdAsync(Guid id)
        {
            await _prescriptionRepository.DeletePrescriptionByIdAsync(id);
        }
        public async Task<bool> Exists(Guid id)
        {
            return await _prescriptionRepository.Exists(id);
        }

    }
}
