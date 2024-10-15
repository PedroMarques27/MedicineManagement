using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Database.Repositories;
using Tests.Repositories.Models;

namespace Tests.Database
{
    public class PrescriptionRepositoryTests
    {
        private readonly MockIPrescriptionRepository _mockPrescriptionRepository;

        public PrescriptionRepositoryTests()
        {
            _mockPrescriptionRepository = new MockIPrescriptionRepository();
        }

        [Fact]
        public async Task GetAllPrescriptions_ShouldReturnListOfPrescriptions()
        {
            await _mockPrescriptionRepository.SeedPrescriptionsAsync();

            var result = _mockPrescriptionRepository.GetAllPrescriptions();

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(MockPrescriptionModel.Mock_Default().Id, result.First().Id);
        }

        [Fact]
        public async Task GetPrescriptionById_ShouldReturnPrescription_IfExists()
        {
            await _mockPrescriptionRepository.SeedPrescriptionsAsync();

            var result = _mockPrescriptionRepository.GetPrescriptionById(MockPrescriptionModel.Mock_Default().Id);

            Assert.NotNull(result);
            Assert.Equal(MockPrescriptionModel.Mock_Default().Id, result.Id);
        }

        [Fact]
        public void GetPrescriptionById_ShouldReturnNull_IfNotFound()
        {
            var result = _mockPrescriptionRepository.GetPrescriptionById(Guid.NewGuid());

            Assert.Null(result);
        }

        [Fact]
        public async Task AddPrescriptionAsync_ShouldAddPrescriptionSuccessfully()
        {
            var newPrescription = MockPrescriptionModel.Mock_Default(); 

            await _mockPrescriptionRepository.AddPrescriptionAsync(newPrescription);

            var result = _mockPrescriptionRepository.GetPrescriptionById(newPrescription.Id);

            Assert.NotNull(result);
            Assert.Equal(newPrescription.Id, result.Id);
        }

        [Fact]
        public async Task UpdatePrescriptionAsync_ShouldUpdatePrescriptionSuccessfully()
        {
            await _mockPrescriptionRepository.SeedPrescriptionsAsync();
            var existingPrescription = _mockPrescriptionRepository.GetPrescriptionById(MockPrescriptionModel.Mock_Default().Id);

            var currentDate = DateTime.Now;

            Assert.NotNull(existingPrescription);
            existingPrescription.CreationDate = currentDate;

            await _mockPrescriptionRepository.UpdatePrescriptionAsync(existingPrescription);
            var result = _mockPrescriptionRepository.GetPrescriptionById(existingPrescription.Id);

            Assert.NotNull(result);
            Assert.Equal(currentDate, result.CreationDate);
        }

        [Fact]
        public async Task DeletePrescriptionByIdAsync_ShouldDeletePrescriptionSuccessfully()
        {
            await _mockPrescriptionRepository.SeedPrescriptionsAsync();

            await _mockPrescriptionRepository.DeletePrescriptionByIdAsync(MockPrescriptionModel.Mock_Default().Id);
            var result = _mockPrescriptionRepository.GetPrescriptionById(MockPrescriptionModel.Mock_Default().Id);

            Assert.Null(result);
        }
        [Fact]
        public async Task DeletePrescriptionByIdAsync_ShouldFailToDeletePrescription_IfNotInDatabase()
        {
            await _mockPrescriptionRepository.SeedPrescriptionsAsync();

            await _mockPrescriptionRepository.DeletePrescriptionByIdAsync(MockPrescriptionModel.Mock().Id);
            var result = _mockPrescriptionRepository.GetPrescriptionById(MockPrescriptionModel.Mock().Id);

            Assert.Null(result);
        }


    }
}
