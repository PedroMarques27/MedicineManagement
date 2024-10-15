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
    public class PrescriptionProviderTests
    {
        private readonly Mock<IPrescriptionRepository> _mockPrescriptionRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IMedicineRepository> _mockMedicineRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly PrescriptionProvider _provider;

        public PrescriptionProviderTests()
        {
            _mockPrescriptionRepository = new Mock<IPrescriptionRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockMedicineRepository = new Mock<IMedicineRepository>();
            _mockMapper = new Mock<IMapper>();
            _provider = new PrescriptionProvider(
                _mockMapper.Object,
                _mockUserRepository.Object,
                _mockMedicineRepository.Object,
                _mockPrescriptionRepository.Object
            );
        }

        [Fact]
        public async Task CreatePrescriptionAsync_ShouldReturnOk_WhenPrescriptionIsCreatedSuccessfully()
        {
            var email = MockUser.Mock_Default().Email;
            var medicines = new List<string> { "Paracetamol", "Default" };

            _mockUserRepository
                .Setup(repo => repo.GetUserByIdAsync(email))
                .ReturnsAsync(MockUserModel.Mock_Default());

            _mockMedicineRepository
                .Setup(repo => repo.GetMedicineByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(MockMedicineModel.Mock_Default());

            _mockPrescriptionRepository
                .Setup(repo => repo.AddPrescriptionAsync(It.IsAny<PrescriptionModel>()))
                .Returns(Task.CompletedTask);

            var result = await _provider.CreatePrescriptionAsync(email, medicines);

            Assert.True(result.Success);
            Assert.Equal(StatusResponseDTO.Ok(null).Error, result.Error);

            _mockPrescriptionRepository.Verify(repo => repo.AddPrescriptionAsync(It.IsAny<PrescriptionModel>()), Times.Once);
        }

        [Fact]
        public async Task CreatePrescriptionAsync_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            var email = MockUser.Mock_Default().Email;
            var medicines = new List<string> { "Paracetamol", "Default" };

            _mockUserRepository
                .Setup(repo => repo.GetUserByIdAsync(email))
                .Returns(Task.FromResult<UserModel?>(null));

            var result = await _provider.CreatePrescriptionAsync(email, medicines);

            Assert.False(result.Success);
            Assert.Equal(StatusResponseDTO.NotFoundError().Error, result.Error);
        }

        [Fact]
        public async Task CreatePrescriptionAsync_ShouldReturnError_WhenExceptionIsThrown()
        {
            var email = MockUser.Mock_Default().Email;
            var medicines = new List<string> { "Paracetamol", "Default" };

            _mockUserRepository
                .Setup(repo => repo.GetUserByIdAsync(email))
                .ThrowsAsync(new Exception("Error fetching user"));

            var result = await _provider.CreatePrescriptionAsync(email, medicines);

            Assert.False(result.Success);
            Assert.Equal("Error fetching user", result.Error);
        }

        [Fact]
        public async Task DeletePrescriptionByIdAsync_ShouldReturnOk_WhenPrescriptionIsDeletedSuccessfully()
        {
            var id = Guid.NewGuid();

            _mockPrescriptionRepository
                .Setup(repo => repo.DeletePrescriptionByIdAsync(id))
                .Returns(Task.CompletedTask);

            var result = await _provider.DeletePrescriptionByIdAsync(id);

            Assert.True(result.Success);
            Assert.Equal(StatusResponseDTO.Ok(null).Error, result.Error);

            _mockPrescriptionRepository.Verify(repo => repo.DeletePrescriptionByIdAsync(id), Times.Once);
        }

        [Fact]
        public async Task DeletePrescriptionByIdAsync_ShouldReturnError_WhenExceptionIsThrown()
        {
            var id = Guid.NewGuid();

            _mockPrescriptionRepository
                .Setup(repo => repo.DeletePrescriptionByIdAsync(id))
                .ThrowsAsync(new Exception("Error deleting prescription"));

            var result = await _provider.DeletePrescriptionByIdAsync(id);

            Assert.False(result.Success);
            Assert.Equal("Error deleting prescription", result.Error);
        }

        [Fact]
        public void GetAllPrescriptions_ShouldReturnAllPrescriptions()
        {
            var prescriptionList = new List<PrescriptionModel>
            {
                MockPrescriptionModel.Mock_Default(),
                MockPrescriptionModel.Mock()
            };

            _mockPrescriptionRepository
                .Setup(repo => repo.GetAllPrescriptions())
                .Returns(prescriptionList);

            _mockMapper
                .Setup(mapper => mapper.Map<ICollection<Prescription>>(It.IsAny<ICollection<PrescriptionModel>>()))
                .Returns(new List<Prescription>
                {
                   MockPrescription.Mock_Default(),
                   MockPrescription.Mock()
                });

            var result = _provider.GetAllPrescriptions();

            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.IsType<List<Prescription>>(result.Data);
            Assert.Equal(2, ((List<Prescription>)result.Data).Count);
        }

        [Fact]
        public void GetAllPrescriptions_ShouldReturnError_WhenExceptionIsThrown()
        {
            _mockPrescriptionRepository
                .Setup(repo => repo.GetAllPrescriptions())
                .Throws(new Exception("Failed to retrieve prescriptions"));

            var result =  _provider.GetAllPrescriptions();

            Assert.False(result.Success);
            Assert.Equal("Failed to retrieve prescriptions", result.Error);
        }

        [Fact]
        public async Task GetPrescriptionByEmailAsync_ShouldReturnPrescription_WhenPrescriptionExists()
        {
            var email = MockUser.Mock_Default().Email;

            _mockUserRepository
                .Setup(repo => repo.GetUserByIdAsync(email))
                .ReturnsAsync(MockUserModel.Mock_Default());

            _mockMapper
                .Setup(mapper => mapper.Map<ICollection<Prescription>>(It.IsAny<ICollection<PrescriptionModel>>()))
                .Returns(new List<Prescription>
                {
                   MockPrescription.Mock_Default()
                });

            var result = await _provider.GetPrescriptionByEmailAsync(email);

            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.IsType<List<Prescription>>(result.Data);
        }

        [Fact]
        public async Task GetPrescriptionByEmailAsync_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            var email = MockUser.Mock_Default().Email;

            _mockUserRepository
                .Setup(repo => repo.GetUserByIdAsync(email))
                .Returns(Task.FromResult<UserModel?>(null));

            var result = await _provider.GetPrescriptionByEmailAsync(email);

            Assert.False(result.Success);
            Assert.Equal(StatusResponseDTO.NotFoundError().Error, result.Error);
        }

        [Fact]
        public async Task GetPrescriptionByEmailAsync_ShouldReturnError_WhenExceptionIsThrown()
        {
            var email = MockUser.Mock_Default().Email;

            _mockUserRepository
                .Setup(repo => repo.GetUserByIdAsync(email))
                .ThrowsAsync(new Exception("Error retrieving prescription by user email"));

            var result = await _provider.GetPrescriptionByEmailAsync(email);

            Assert.False(result.Success);
            Assert.Equal("Error retrieving prescription by user email", result.Error);
        }

        [Fact]
        public void GetPrescriptionById_ShouldReturnPrescription_WhenPrescriptionExists()
        {
            var id = Guid.NewGuid();

            _mockPrescriptionRepository
                .Setup(repo => repo.GetPrescriptionById(id))
                .Returns(MockPrescriptionModel.Mock_Default());

            _mockMapper
                .Setup(mapper => mapper.Map<Prescription>(It.IsAny<PrescriptionModel>()))
                .Returns(MockPrescription.Mock_Default());

            var result = _provider.GetPrescriptionById(id);

            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.IsType<Prescription>(result.Data);
        }

        [Fact]
        public void GetPrescriptionById_ShouldReturnNotFound_WhenPrescriptionDoesNotExist()
        {
            var id = Guid.NewGuid();

            _mockPrescriptionRepository
                .Setup(repo => repo.GetPrescriptionById(id))
                .Returns((PrescriptionModel?)null);

            var result = _provider.GetPrescriptionById(id);

            Assert.False(result.Success);
            Assert.Equal(StatusResponseDTO.NotFoundError().Error, result.Error);
        }
        [Fact]
        public void GetPrescriptionById_ShouldReturnError_WhenExceptionIsThrown()
        {
            var id = Guid.NewGuid();

            _mockPrescriptionRepository
                .Setup(repo => repo.GetPrescriptionById(id))
                .Throws(new Exception("Error retrieving prescription by id"));

            var result = _provider.GetPrescriptionById(id);

            Assert.False(result.Success);
            Assert.Equal("Error retrieving prescription by id", result.Error);
        }

        [Fact]
        public async Task UpdatePrescriptionAsync_ShouldReturnOk_WhenPrescriptionIsUpdatedSuccessfully()
        {
            var id = Guid.NewGuid();
            var medicines = new List<string> { "Paracetamol", "Default" };

            _mockPrescriptionRepository
                .Setup(repo => repo.GetPrescriptionById(id))
                .Returns(MockPrescriptionModel.Mock_Default());

            _mockMedicineRepository
                .Setup(repo => repo.GetMedicineByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(MockMedicineModel.Mock_Default());

            var result = await _provider.UpdatePrescriptionAsync(id, medicines);

            Assert.True(result.Success);
            Assert.Equal(StatusResponseDTO.Ok(null).Error, result.Error);
        }

        [Fact]
        public async Task UpdatePrescriptionAsync_ShouldReturnNotFound_WhenPrescriptionDoesNotExist()
        {
            var id = Guid.NewGuid();
            var medicines = new List<string> { "Paracetamol", "Default" };

            _mockPrescriptionRepository
                .Setup(repo => repo.GetPrescriptionById(id))
                .Returns((PrescriptionModel?)null);

            var result = await _provider.UpdatePrescriptionAsync(id, medicines);

            Assert.False(result.Success);
            Assert.Equal(StatusResponseDTO.NotFoundError().Error, result.Error);
        }

        [Fact]
        public async Task UpdatePrescriptionAsync_ShouldReturnError_WhenExceptionIsThrown()
        {
            var id = Guid.NewGuid();
            var medicines = new List<string> { "Paracetamol", "Default" };

            _mockPrescriptionRepository
                .Setup(repo => repo.GetPrescriptionById(id))
                .Throws(new Exception("Error updating prescription"));

            var result = await _provider.UpdatePrescriptionAsync(id, medicines);

            Assert.False(result.Success);
            Assert.Equal("Error updating prescription", result.Error);
        }
    }
}
