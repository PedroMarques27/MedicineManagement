using Microsoft.AspNetCore.Mvc;
using Moq;
using Process.DTOs;
using Process.Operations;
using Process.Providers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Process.Operations
{
    public class PrescriptionsControllerTests
    {
        private readonly Mock<IPrescriptionProvider> _mockPrescriptionProvider;
        private readonly PrescriptionController _prescriptionController;

        public PrescriptionsControllerTests()
        {
            _mockPrescriptionProvider = new Mock<IPrescriptionProvider>();
            _prescriptionController = new PrescriptionController(_mockPrescriptionProvider.Object);
        }

        [Fact]
        public async Task DeletePrescription_ShouldReturnNoContent_WhenSuccessful()
        {
            // Arrange
            var prescriptionId = Guid.NewGuid();
            _mockPrescriptionProvider.Setup(p => p.DeletePrescriptionByIdAsync(prescriptionId))
                .ReturnsAsync(StatusResponseDTO.Ok(null));

            // Act
            var result = await _prescriptionController.DeletePrescription(prescriptionId);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, noContentResult.StatusCode);
        }

        [Fact]
        public async Task DeletePrescription_ShouldReturnNotFound_WhenNotSuccessful()
        {
            // Arrange
            var prescriptionId = Guid.NewGuid();
            _mockPrescriptionProvider.Setup(p => p.DeletePrescriptionByIdAsync(prescriptionId))
                .ReturnsAsync(StatusResponseDTO.GetError("Prescription not found"));

            // Act
            var result = await _prescriptionController.DeletePrescription(prescriptionId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Prescription not found", notFoundResult.Value);
        }

        [Fact]
        public void GetAllPrescriptions_ShouldReturnOk_WhenSuccessful()
        {
            // Arrange
            var prescriptions = new List<string> { "Prescription1", "Prescription2" };
            _mockPrescriptionProvider.Setup(p => p.GetAllPrescriptions())
                .Returns(StatusResponseDTO.Ok(prescriptions));

            // Act
            var result = _prescriptionController.GetAllPrescriptions();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedPrescriptions = Assert.IsAssignableFrom<List<string>>(okResult.Value);
            Assert.Equal(2, returnedPrescriptions.Count);
        }

        [Fact]
        public void GetAllPrescriptions_ShouldReturnNotFound_WhenNotSuccessful()
        {
            // Arrange
            _mockPrescriptionProvider.Setup(p => p.GetAllPrescriptions())
                .Returns(StatusResponseDTO.GetError("No prescriptions found"));

            // Act
            var result = _prescriptionController.GetAllPrescriptions();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No prescriptions found", notFoundResult.Value);
        }

        [Fact]
        public void GetPrescriptionById_ShouldReturnOk_WhenSuccessful()
        {
            // Arrange
            var prescriptionId = Guid.NewGuid();
            var prescriptionData = new List<string> { "Prescription1", "Prescription2" };
            _mockPrescriptionProvider.Setup(p => p.GetPrescriptionById(prescriptionId))
                .Returns(StatusResponseDTO.Ok(prescriptionData));

            // Act
            var result = _prescriptionController.GetPrescriptionById(prescriptionId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedPrescription = Assert.IsAssignableFrom<List<string>>(okResult.Value);
            Assert.Equal(prescriptionData, returnedPrescription);
        }

        [Fact]
        public void GetPrescriptionById_ShouldReturnNotFound_WhenNotSuccessful()
        {
            // Arrange
            var prescriptionId = Guid.NewGuid();
            _mockPrescriptionProvider.Setup(p => p.GetPrescriptionById(prescriptionId))
                .Returns(StatusResponseDTO.GetError("Prescription not found"));

            // Act
            var result = _prescriptionController.GetPrescriptionById(prescriptionId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Prescription not found", notFoundResult.Value);
        }

        [Fact]
        public async Task UpdatePrescription_ShouldReturnOk_WhenSuccessful()
        {
            // Arrange
            var prescriptionId = Guid.NewGuid();
            var medicines = new List<string> { "Medicine1", "Medicine2" };
            var updatedPrescription = new List<string> { "UpdatedMedicine1", "UpdatedMedicine2" };
            _mockPrescriptionProvider.Setup(p => p.UpdatePrescriptionAsync(prescriptionId, medicines))
                .ReturnsAsync(StatusResponseDTO.Ok(updatedPrescription));

            // Act
            var result = await _prescriptionController.UpdatePrescription(prescriptionId, medicines);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedPrescription = Assert.IsAssignableFrom<List<string>>(okResult.Value);
            Assert.Equal(updatedPrescription, returnedPrescription);
        }

        [Fact]
        public async Task UpdatePrescription_ShouldReturnBadRequest_WhenMedicinesListIsNullOrEmpty()
        {
            // Arrange
            var prescriptionId = Guid.NewGuid();
            ICollection<string> medicines = null; // or new List<string>();

            // Act
            var result = await _prescriptionController.UpdatePrescription(prescriptionId, medicines);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("List of medicines is required.", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdatePrescription_ShouldReturnNotFound_WhenNotSuccessful()
        {
            // Arrange
            var prescriptionId = Guid.NewGuid();
            var medicines = new List<string> { "Medicine1", "Medicine2" };
            _mockPrescriptionProvider.Setup(p => p.UpdatePrescriptionAsync(prescriptionId, medicines))
                .ReturnsAsync(StatusResponseDTO.GetError("Prescription not found"));

            // Act
            var result = await _prescriptionController.UpdatePrescription(prescriptionId, medicines);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Prescription not found", notFoundResult.Value);
        }
    }
}
