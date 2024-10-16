using AutoMapper;
using Database.Models;
using Database.Repositories;
using Moq;
using Process.DTOs;
using Process.DTOs.Entities;
using Process.Providers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tests.Process.Entities;
using Tests.Repositories.Models;
using Xunit;

namespace Tests.Process.Providers
{
    public class MedicineProviderTests
    {
        private readonly Mock<IMedicineRepository> _mockMedicineRepository;
        private readonly Mock<IPrescriptionRepository> _mockPrescriptionRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly MedicineProvider _provider;

        public MedicineProviderTests()
        {
            _mockMedicineRepository = new Mock<IMedicineRepository>();
            _mockPrescriptionRepository = new Mock<IPrescriptionRepository>();
            _mockMapper = new Mock<IMapper>();
            _provider = new MedicineProvider(_mockMedicineRepository.Object, _mockPrescriptionRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task AddMedicine_ShouldReturnOk_WhenMedicineIsAddedSuccessfully()
        {
            var newMedicine = MockMedicine.Mock_Default();

            _mockMapper
                .Setup(mapper => mapper.Map<MedicineModel>(It.IsAny<Medicine>()))
                .Returns(MockMedicineModel.Mock_Default());

            _mockMedicineRepository
                .Setup(repo => repo.AddMedicineAsync(It.IsAny<MedicineModel>()))
                .Returns(Task.CompletedTask);

            var result = await _provider.AddMedicine(newMedicine);

            Assert.True(result.Success);
            Assert.True(string.IsNullOrEmpty(result.Error));
            Assert.IsType<MedicineModel>(result.Data);
            _mockMedicineRepository.Verify(repo => repo.AddMedicineAsync(It.IsAny<MedicineModel>()), Times.Once);
        }

        [Fact]
        public async Task AddMedicine_ShouldReturnError_WhenExceptionIsThrown()
        {
            var newMedicine = MockMedicine.Mock_Default();

            _mockMapper
                .Setup(mapper => mapper.Map<MedicineModel>(It.IsAny<Medicine>()))
                .Returns(MockMedicineModel.Mock_Default());

            _mockMedicineRepository
                .Setup(repo => repo.AddMedicineAsync(It.IsAny<MedicineModel>()))
                .ThrowsAsync(new Exception("Error adding medicine"));

            var result = await _provider.AddMedicine(newMedicine);

            Assert.False(result.Success);
            Assert.Equal("Error adding medicine", result.Error);
        }

        [Fact]
        public async Task DeleteMedicine_ShouldReturnOk_WhenMedicineIsDeletedSuccessfully()
        {
            var name = MockMedicine.Mock_Default().Name;

            _mockMedicineRepository
                .Setup(repo => repo.DeleteMedicineByNameAsync(name))
                .Returns(Task.CompletedTask);

            var result = await _provider.DeleteMedicine(name);

            Assert.True(result.Success);
            Assert.True(string.IsNullOrEmpty(result.Error));
            _mockMedicineRepository.Verify(repo => repo.DeleteMedicineByNameAsync(name), Times.Once);
        }

        [Fact]
        public async Task DeleteMedicine_ShouldReturnError_WhenExceptionIsThrown()
        {
            var name = MockMedicine.Mock_Default().Name;

            _mockMedicineRepository
                .Setup(repo => repo.DeleteMedicineByNameAsync(name))
                .ThrowsAsync(new Exception("Error deleting medicine"));

            var result = await _provider.DeleteMedicine(name);

            Assert.False(result.Success);
            Assert.Equal("Error deleting medicine", result.Error);
        }

        [Fact]
        public void GetAll_ShouldReturnAllMedicines()
        {
            var medicineList = new List<MedicineModel>
            {
                MockMedicineModel.Mock_Default(),
                MockMedicineModel.Mock(),
            };

            _mockMedicineRepository
                .Setup(repo => repo.GetAllMedicines())
                .Returns(medicineList);

            _mockMapper
                .Setup(mapper => mapper.Map<ICollection<Medicine>>(It.IsAny<ICollection<MedicineModel>>()))
                .Returns(new List<Medicine>
                {
                    MockMedicine.Mock_Default(),
                    MockMedicine.Mock()
                });

            var result = _provider.GetAll();

            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.IsType<List<Medicine>>(result.Data);
            Assert.Equal(2, ((List<Medicine>)result.Data).Count);
        }
        [Fact]
        public void GetAll_ShouldReturnError_WhenExceptionIsThrown()
        {
            var medicineList = new List<MedicineModel>
            {
                MockMedicineModel.Mock_Default(),
                MockMedicineModel.Mock(),
            };

            _mockMedicineRepository
                .Setup(repo => repo.GetAllMedicines())
                .Throws(new Exception("Error retrieving data"));

          
            var result = _provider.GetAll();

            Assert.False(result.Success);
            Assert.Equal("Error retrieving data", result.Error);
        }


        [Fact]
        public async Task GetByNameAsync_ShouldReturnMedicine_WhenMedicineExists()
        {
            var name = MockMedicine.Mock_Default().Name;

            _mockMedicineRepository
                .Setup(repo => repo.GetMedicineByNameAsync(name))
                .ReturnsAsync(MockMedicineModel.Mock_Default());

            _mockMapper
                .Setup(mapper => mapper.Map<Medicine>(It.IsAny<MedicineModel>()))
                .Returns(MockMedicine.Mock_Default());

            var result = await _provider.GetByNameAsync(name);

            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.IsType<Medicine>(result.Data);
            Assert.Equal(name, ((Medicine)result.Data).Name);
        }

        [Fact]
        public async Task GetByNameAsync_ShouldReturnNotFound_WhenMedicineDoesNotExist()
        {
            var name = MockMedicine.Mock_Default().Name;

            _mockMedicineRepository
                .Setup(repo => repo.GetMedicineByNameAsync(name))
                .ReturnsAsync((MedicineModel?)null);

            var result = await _provider.GetByNameAsync(name);

            Assert.False(result.Success);
            Assert.Equal(StatusResponseDTO.NotFoundError().Error, result.Error);
        }
        [Fact]
        public async Task GetByNameAsync_ShouldReturnError_WhenExceptionIsThrown()
        {
            var name = MockMedicine.Mock_Default().Name;

            _mockMedicineRepository
                .Setup(repo => repo.GetMedicineByNameAsync(name))
                .ThrowsAsync(new Exception("Failed To Get User"));

            var result = await _provider.GetByNameAsync(name);

            Assert.False(result.Success);
            Assert.Equal("Failed To Get User", result.Error);
        }


        [Fact]
        public async Task UpdateMedicine_ShouldReturnOk_WhenMedicineIsUpdatedSuccessfully()
        {
            var medicine = MockMedicine.Mock_Default();
            medicine.Quantity = 100;

            _mockMedicineRepository
                .Setup(repo => repo.GetMedicineByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(MockMedicineModel.Mock_Default());

            _mockMedicineRepository
                .Setup(repo => repo.UpdateMedicineAsync(It.IsAny<MedicineModel>()))
                .Returns(Task.CompletedTask);

            var result = await _provider.UpdateMedicine(medicine.Name, medicine);

            Assert.True(result.Success);
            _mockMedicineRepository.Verify(repo => repo.UpdateMedicineAsync(It.IsAny<MedicineModel>()), Times.Once);
        }

        [Fact]
        public async Task UpdateMedicine_ShouldReturnNotFound_WhenMedicineDoesNotExist()
        {
            var medicine = MockMedicine.Mock_Default();

            _mockMedicineRepository
                .Setup(repo => repo.GetMedicineByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((MedicineModel?)null);

            var result = await _provider.UpdateMedicine(medicine.Name, medicine);

            Assert.False(result.Success);
            Assert.Equal(StatusResponseDTO.NotFoundError().Error, result.Error);
        }

        [Fact]
        public async Task UpdateMedicine_ShouldReturnError_WhenExceptionIsThrown()
        {
            var medicine = MockMedicine.Mock_Default();

            _mockMedicineRepository
                .Setup(repo => repo.GetMedicineByNameAsync(It.IsAny<string>()))
                .ThrowsAsync(new Exception("Failed To Update User"));

            var result = await _provider.UpdateMedicine(medicine.Name, medicine);


            Assert.False(result.Success);
            Assert.Equal("Failed To Update User", result.Error);
        }
    }
}
