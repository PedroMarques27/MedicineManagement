using Microsoft.AspNetCore.Mvc;
using Moq;
using Process.DTOs;
using Process.DTOs.Entities;
using Process.Providers;
using Process.Operations;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Tests.Process.Entities;
using System;

namespace Tests.Process.Operations
{
    public class MedicinesControllerTests
    {
        private readonly Mock<IMedicineProvider> _mockMedicineProvider;
        private readonly MedicinesController _controller;

        public MedicinesControllerTests()
        {
            _mockMedicineProvider = new Mock<IMedicineProvider>();
            _controller = new MedicinesController(_mockMedicineProvider.Object);
        }

        [Fact]
        public void GetAllMedicines_ShouldReturnOk_WhenMedicinesExist()
        {
            var medicines = new List<Medicine> 
            { 
                MockMedicine.Mock_Default() 
            };
            
            var response = StatusResponseDTO.Ok(medicines);

            _mockMedicineProvider
                .Setup(provider => provider.GetAll())
                .Returns(response);

            var result = _controller.GetAllMedicines();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(medicines, okResult.Value);
        }

        [Fact]
        public void GetAllMedicines_ShouldReturnBadRequest_WhenErrorOccurs()
        {
            var response = StatusResponseDTO.GetError("Error fetching medicines");

            _mockMedicineProvider
                .Setup(provider => provider.GetAll())
                .Returns(response);
            
            var result = _controller.GetAllMedicines();

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal("Error fetching medicines", badRequestResult.Value);
        }

        [Fact]
        public async Task GetMedicineByName_ShouldReturnOk_WhenMedicineExists()
        {
            var medicine = MockMedicine.Mock_Default();
            var response = StatusResponseDTO.Ok(medicine);

            _mockMedicineProvider
                .Setup(provider => provider.GetByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(response);

            var result = await _controller.GetMedicineByName(medicine.Name);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(medicine, okResult.Value);
        }

        [Fact]
        public async Task GetMedicineByName_ShouldReturnNotFound_WhenMedicineDoesNotExist()
        {
            var response = StatusResponseDTO.NotFoundError();

            _mockMedicineProvider
                .Setup(provider => provider.GetByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(response);

            var result = await _controller.GetMedicineByName("NonExistentMedicine");

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
            Assert.Equal(response.Error, notFoundResult.Value);
        }

        [Fact]
        public async Task AddMedicine_ShouldReturnCreated_WhenMedicineIsAddedSuccessfully()
        {
            var newMedicine = MockMedicine.Mock_Default();
            var response = StatusResponseDTO.Ok(newMedicine);

            _mockMedicineProvider
                .Setup(provider => provider.AddMedicine(It.IsAny<Medicine>()))
                .ReturnsAsync(response);

            var result = await _controller.AddMedicine(newMedicine);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(201, createdAtActionResult.StatusCode);
            Assert.Equal(newMedicine, createdAtActionResult.Value);
            Assert.Equal("GetMedicineByName", createdAtActionResult.ActionName);
        }

        [Fact]
        public async Task AddMedicine_ShouldReturnBadRequest_WhenErrorOccurs()
        {
            var newMedicine = MockMedicine.Mock_Default();
            var response = StatusResponseDTO.GetError("Error adding medicine");

            _mockMedicineProvider
                .Setup(provider => provider.AddMedicine(It.IsAny<Medicine>()))
                .ReturnsAsync(response);

            var result = await _controller.AddMedicine(newMedicine);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal("Error adding medicine", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateMedicine_ShouldReturnOk_WhenMedicineIsUpdatedSuccessfully()
        {
            var medicine = MockMedicine.Mock_Default();
            var response = StatusResponseDTO.Ok(medicine);

            _mockMedicineProvider
                .Setup(provider => provider.UpdateMedicine(It.IsAny<string>(), It.IsAny<Medicine>()))
                .ReturnsAsync(response);

            var result = await _controller.UpdateMedicine(medicine.Name, medicine);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(medicine, okResult.Value);
        }

 
        [Fact]
        public async Task UpdateMedicine_ShouldReturnBadRequest_WhenErrorOccurs()
        {
            var medicine = MockMedicine.Mock_Default();
            var response = StatusResponseDTO.GetError("Error updating medicine");

            _mockMedicineProvider
                .Setup(provider => provider.UpdateMedicine(It.IsAny<string>(), It.IsAny<Medicine>()))
                .ReturnsAsync(response);

            var result = await _controller.UpdateMedicine(medicine.Name, medicine);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal("Error updating medicine", badRequestResult.Value);
        }
     

        [Fact]
        public async Task DeleteMedicine_ShouldReturnOk_WhenMedicineIsDeletedSuccessfully()
        {
            var response = StatusResponseDTO.Ok(null);

            _mockMedicineProvider
                .Setup(provider => provider.DeleteMedicine(It.IsAny<string>()))
                .ReturnsAsync(response);

            var result = await _controller.DeleteMedicine(MockMedicine.Mock_Default().Name);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task DeleteMedicine_ShouldReturnNotFound_WhenMedicineDoesNotExist()
        {
            var response = StatusResponseDTO.NotFoundError();

            _mockMedicineProvider
                .Setup(provider => provider.DeleteMedicine(It.IsAny<string>()))
                .ReturnsAsync(response);

            var result = await _controller.DeleteMedicine("NonExistentMedicine");

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
            Assert.Equal(response.Error, notFoundResult.Value);
        }
    }
}
