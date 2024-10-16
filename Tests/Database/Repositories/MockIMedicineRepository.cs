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
    public class MockIMedicineRepository
    {
        private readonly DatabaseContext _context;
        private readonly IMedicineRepository _MedicineRepository;

        public MockIMedicineRepository()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new DatabaseContext(options);
            _MedicineRepository = new MedicineRepository(_context);
        }

        public async Task SeedMedicinesAsync()
        {
            var Medicine = MockMedicineModel.Mock_Default();

            _context.Medicines.Add(Medicine);
            await _context.SaveChangesAsync();
        }
        public ICollection<MedicineModel> GetAllMedicines()
        {
            return _MedicineRepository.GetAllMedicines();
        }

        public async Task<MedicineModel?> GetMedicineByIdAsync(string Name)
        {
            return await _MedicineRepository.GetMedicineByNameAsync(Name);
        }

        public async Task AddMedicineAsync(MedicineModel Medicine)
        {
            await _MedicineRepository.AddMedicineAsync(Medicine);
        }

        public async Task UpdateMedicineAsync(MedicineModel Medicine)
        {
            await _MedicineRepository.UpdateMedicineAsync(Medicine);
        }

        public async Task DeleteMedicineByIdAsync(string Name)
        {
            await _MedicineRepository.DeleteMedicineByNameAsync(Name);
        }
        public async Task<bool> Exists(string Name)
        {
            return await _MedicineRepository.Exists(Name);
        }
    }
}
