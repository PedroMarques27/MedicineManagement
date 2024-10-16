using Database.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tests.Database.Repositories;
using Tests.Repositories.Models;
using Xunit;

namespace Tests.Database
{
    public class MedicineRepositoryTests
    {
        private readonly MockIMedicineRepository _mockMedicineRepository;

        public MedicineRepositoryTests()
        {
            _mockMedicineRepository = new MockIMedicineRepository();
        }

        [Fact]
        public async Task GetAllMedicines_ShouldReturnListOfMedicines()
        {
            await _mockMedicineRepository.SeedMedicinesAsync();

            var result = _mockMedicineRepository.GetAllMedicines();

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(MockMedicineModel.Mock_Default().Name, result.First().Name);
        }

        [Fact]
        public async Task GetMedicineByNameAsync_ShouldReturnMedicine_IfExists()
        {
            await _mockMedicineRepository.SeedMedicinesAsync();

            var result = await _mockMedicineRepository.GetMedicineByIdAsync(MockMedicineModel.Mock_Default().Name);

            Assert.NotNull(result);
            Assert.Equal(MockMedicineModel.Mock_Default().Name, result.Name); 
        }

        [Fact]
        public async Task GetMedicineByNameAsync_ShouldReturnNull_IfNotFound()
        {
            await _mockMedicineRepository.SeedMedicinesAsync();

            var result = await _mockMedicineRepository.GetMedicineByIdAsync("NonExistent");

            Assert.Null(result);
        }

        [Fact]
        public async Task AddMedicineAsync_ShouldAddMedicineSuccessfully()
        {
            var newMedicine = MockMedicineModel.Mock();
            
            await _mockMedicineRepository.AddMedicineAsync(newMedicine);
            var result = await _mockMedicineRepository.GetMedicineByIdAsync(newMedicine.Name);

            Assert.NotNull(result);
            Assert.Equal(newMedicine.Name, result.Name);
        }

        [Fact]
        public async Task UpdateMedicineAsync_ShouldUpdateMedicineSuccessfully()
        {
            await _mockMedicineRepository.SeedMedicinesAsync();

            var medicineToUpdate = await _mockMedicineRepository.GetMedicineByIdAsync(MockMedicineModel.Mock_Default().Name);
            
            Assert.NotNull(medicineToUpdate);

            medicineToUpdate.Quantity = 2024;

            await _mockMedicineRepository.UpdateMedicineAsync(medicineToUpdate);
            var result = await _mockMedicineRepository.GetMedicineByIdAsync(medicineToUpdate.Name);

            Assert.NotNull(result);
            Assert.Equal(2024, result.Quantity);
        }

        [Fact]
        public async Task DeleteMedicineByIdAsync_ShouldDeleteMedicineSuccessfully()
        {
            await _mockMedicineRepository.SeedMedicinesAsync();

            await _mockMedicineRepository.DeleteMedicineByIdAsync(MockMedicineModel.Mock_Default().Name);
            var result = await _mockMedicineRepository.GetMedicineByIdAsync(MockMedicineModel.Mock_Default().Name);

            Assert.Null(result);
        }
        [Fact]
        public async Task Exists_ShouldReturnTrueIfExist()
        {
            await _mockMedicineRepository.SeedMedicinesAsync();

            var result = await _mockMedicineRepository.Exists(MockMedicineModel.Mock_Default().Name);

            Assert.True(result);
        }
        [Fact]
        public async Task Exists_ShouldReturnFalseIfNoItemExist()
        {

            var result = await _mockMedicineRepository.Exists(MockMedicineModel.Mock_Default().Name);

            Assert.False(result);
        }
    }
}
