using Microsoft.AspNetCore.Mvc;
using Moq;
using Process.DTOs;
using Process.Operations;
using Process.Providers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Process.DTOs.Entities;
using Tests.Process.Entities;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Tests.Process.DTOs;
using Microsoft.DotNet.Scaffolding.Shared;
using Tests.Database.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Tests.Process.Operations
{
    public class UserControllerTests
    {
        private readonly Mock<IUsersProvider> _mockUsersProvider;
        private readonly Mock<IPrescriptionProvider> _mockPrescriptionProvider;
        private readonly UserController _userController;

        public UserControllerTests()
        {
            _mockUsersProvider = new Mock<IUsersProvider>();
            _mockPrescriptionProvider = new Mock<IPrescriptionProvider>();
            _userController = new UserController(_mockUsersProvider.Object, _mockPrescriptionProvider.Object);
        }

        [Fact]
        public async Task AddUser_ShouldReturnCreated_WhenSuccessful()
        {
            var userInput = MockUserInputDto.Mock_Default();
            
            var userEntity = MockUser.Mock_Default();
            var response = StatusResponseDTO.Ok(userEntity);

            _mockUsersProvider
                .Setup(p => p.AddUser(It.IsAny<User>()))
                .ReturnsAsync(response);

            var result = await _userController.AddUser(userInput);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);

            Assert.Equal("GetUserByEmail", createdAtActionResult.ActionName);
            Assert.NotNull(createdAtActionResult.RouteValues);
            Assert.Equal(MockUser.Mock_Default().Email, createdAtActionResult.RouteValues["email"]);
            Assert.Equal(userEntity, createdAtActionResult.Value);
        }
        [Fact]
        public async Task AddUser_ShouldReturnBadRequest_UserInputIsNull()
        {
            UserInputDto? user = null;

            var result = await _userController.AddUser(user: user);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("User data is required.", badRequestResult.Value);
        }

        [Fact]
        public async Task AddUser_ShouldReturnBadRequest_WhenAddUserFails()
        {
            var userInput = MockUserInputDto.Mock_Default();

            var response = StatusResponseDTO.GetError("Failed to Add User");

            _mockUsersProvider
                .Setup(p => p.AddUser(It.IsAny<User>()))
                .ReturnsAsync(response);

            var result = await _userController.AddUser(userInput);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Failed to Add User", badRequestResult.Value);
        }

        [Fact]
        public async Task DeleteUser_ShouldReturnNoContent_WhenSuccessful()
        {
         
            var email = MockUser.Mock_Default().Email;
            _mockUsersProvider
                .Setup(p => p.DeleteUser(email))
                .ReturnsAsync(StatusResponseDTO.Ok(null));

            var result = await _userController.DeleteUser(email);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteUser_ShouldReturnNotFound_WhenUserNotFound()
        {
            
            _mockUsersProvider
                .Setup(p => p.DeleteUser(It.IsAny<string>()))
                .ReturnsAsync(StatusResponseDTO.GetError("User not found"));

            var email = MockUser.Mock_Default().Email;
            var result = await _userController.DeleteUser(email);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("User not found", notFoundResult.Value);
        }

        [Fact]
        public async Task GetUserByEmail_ShouldReturnOk_WhenUserFound()
        {
            var userEntity = MockUser.Mock_Default();
            
            _mockUsersProvider
                .Setup(p => p.GetUserByEmail(It.IsAny<string>()))
                .ReturnsAsync(StatusResponseDTO.Ok(userEntity));

            var result = await _userController.GetUserByEmail(userEntity.Email);
            
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(userEntity, okResult.Value);
        }

        [Fact]
        public async Task GetUserByEmail_ShouldReturnNotFound_WhenUserNotFound()
        {
            _mockUsersProvider
                .Setup(p => p.GetUserByEmail(It.IsAny<string>()))
                .ReturnsAsync(StatusResponseDTO.GetError("User not found"));

            var userEntity = MockUser.Mock_Default();
            var result = await _userController.GetUserByEmail(userEntity.Email);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("User not found", notFoundResult.Value);
        }

        [Fact]
        public void GetAllUsers_ShouldReturnOk_WhenUsersExist()
        {
            var users = new List<User>
            {
                MockUser.Mock_Default(),
                MockUser.Mock()
            };

            _mockUsersProvider
                .Setup(p => p.GetUsers())
                .Returns(StatusResponseDTO.Ok(users));

            var result = _userController.GetAllUsers();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUsers = Assert.IsAssignableFrom<List<User>>(okResult.Value);
            Assert.Equal(2, returnedUsers.Count);
        }

        [Fact]
        public void GetAllUsers_ShouldReturnNotFound_WhenNoUsersExist()
        {
            _mockUsersProvider
                .Setup(p => p.GetUsers())
                .Returns(StatusResponseDTO.GetError("No users found"));

            var result = _userController.GetAllUsers();

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No users found", notFoundResult.Value);
        }

        [Fact]
        public async Task UpdateUser_ShouldReturnOk_WhenSuccessful()
        {
            var userEntity = MockUser.Mock_Default();
            userEntity.Name = "Updated Name";

            var input = MockUserInputDto.Mock_Default();
            input.Name = "Updated Name";

            var response = StatusResponseDTO.Ok(userEntity);
            
            _mockUsersProvider
                .Setup(p => p.UpdateUser(It.IsAny<string>(), It.IsAny<UserInputDto>()))
                .ReturnsAsync(response);

            var result = await _userController.UpdateUser(userEntity.Email,input);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(userEntity, okResult.Value);
        }

      

        [Fact]
        public async Task UpdateUser_ShouldReturnNotFound_WhenUserNotFound()
        {

            var input = MockUserInputDto.Mock_Default();
            input.Name = "Updated Name";

            _mockUsersProvider
                .Setup(p => p.UpdateUser(It.IsAny<string>(), It.IsAny<UserInputDto>()))
                .ReturnsAsync(StatusResponseDTO.GetError("User not found"));

            var result = await _userController.UpdateUser(input.Email,input);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("User not found", notFoundResult.Value);
        }
        [Fact]
        public async Task UpdateUser_ShouldReturnBadRequest_UserInputIsNull()
        {
            UserInputDto? user = null;

            var result = await _userController.UpdateUser(MockUserInputDto.Mock_Default().Email, user);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("User data is required.", badRequestResult.Value);
        }


        [Fact]
        public async Task CreatePrescription_ShouldReturnCreated_WhenSuccessful()
        {
            var email = MockUser.Mock_Default().Email;
            var medicines = new List<string> { "Medicine1", "Medicine2" };
         
            var response =  _mockPrescriptionProvider
                .Setup(p => p.CreatePrescriptionAsync(It.IsAny<string>(), It.IsAny<ICollection<string>>()))
                .ReturnsAsync(StatusResponseDTO.Ok(null));

            var result = await _userController.CreatePrescription(email, medicines);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetPrescriptionByEmail", createdAtActionResult.ActionName);
            Assert.NotNull(createdAtActionResult.RouteValues);
            Assert.Equal(email, createdAtActionResult.RouteValues["email"]);
        }
        [Fact]
        public async Task CreatePrescription_ShouldReturnBadRequest_MedicinesListIsNullOrEmpty()
        {
            var medicineList = new List<string>();

            var result = await _userController.CreatePrescription(MockUserInputDto.Mock_Default().Email, medicineList);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("List of medicines is required.", badRequestResult.Value);
        }

        [Fact]
        public async Task CreatePrescription_ShouldReturnBadRequest_WhenExceptionIsThrown()
        {
            var email = MockUser.Mock_Default().Email;
            var medicines = new List<string> { "Medicine1", "Medicine2" };

            var response = _mockPrescriptionProvider
                .Setup(p => p.CreatePrescriptionAsync(It.IsAny<string>(), It.IsAny<ICollection<string>>()))
                .ReturnsAsync(StatusResponseDTO.GetError("Failed To Create Prescription"));

            var result = await _userController.CreatePrescription(email, medicines);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Failed To Create Prescription", badRequestResult.Value);
        }


        [Fact]
        public async Task GetPrescriptionByEmail_ShouldReturnOk_WhenPrescriptionExists()
        {
            var email = MockUser.Mock_Default().Email;
            var prescriptionData = new List<string> { "Medicine1", "Medicine2" };

            _mockPrescriptionProvider
                .Setup(p => p.GetPrescriptionByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(StatusResponseDTO.Ok(prescriptionData));

            var result = await _userController.GetPrescriptionByEmail(email);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedPrescriptions = Assert.IsAssignableFrom<List<string>>(okResult.Value);
            Assert.Equal(prescriptionData, returnedPrescriptions);
        }

        [Fact]
        public async Task GetPrescriptionByEmail_ShouldReturnNotFound_WhenPrescriptionNotFound()
        {
            var email = MockUser.Mock_Default().Email;
            _mockPrescriptionProvider
                .Setup(p => p.GetPrescriptionByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(StatusResponseDTO.GetError("Prescription not found"));

            var result = await _userController.GetPrescriptionByEmail(email);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Prescription not found", notFoundResult.Value);
        }
    }
}
