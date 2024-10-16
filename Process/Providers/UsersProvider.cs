using AutoMapper;
using Database.Models;
using Database.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Process.DTOs;
using Process.DTOs.Entities;

namespace Process.Providers
{
    public class UsersProvider: IUsersProvider
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;
        public UsersProvider(IMapper mapper, IUserRepository repository)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<StatusResponseDTO> AddUser(User user)
        {
            try
            {
                var newUserModel = new UserModel
                {
                    Email = user.Email,
                    Name = user.Name,
                    PrescriptionList = new List<PrescriptionModel>()
                };
                await _repository.AddUserAsync(newUserModel);
                return StatusResponseDTO.Ok(null);

            }
            catch(Exception e)
            {
                return StatusResponseDTO.GetError(e.InnerException != null ? e.InnerException.Message : e.Message);
            }
           
        }

        public async Task<StatusResponseDTO> DeleteUser(string Email)
        {
            try { 
                await _repository.DeleteUserByIdAsync(Email); 
                return StatusResponseDTO.Ok(null);

            }
            catch(Exception e)
            {
                return StatusResponseDTO.GetError(e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }

        public async Task<StatusResponseDTO> GetUserByEmail(string Email)
        {
            try {
                var user = await _repository.GetUserByIdAsync(Email);
                if (user == null)
                {
                    return StatusResponseDTO.NotFoundError();
                }
                var mapped = _mapper.Map<User>(user);
                return StatusResponseDTO.Ok(mapped);
            }
            catch(Exception e)
            {
                return StatusResponseDTO.GetError(e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }

        public StatusResponseDTO GetUsers()
        {
            try 
            {
                var users = _repository.GetAllUsers();
                var mapped = _mapper.Map<ICollection<User>>(users);
                return StatusResponseDTO.Ok(mapped);
            }
            catch(Exception e)
            {
                return StatusResponseDTO.GetError(e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }

        public async Task<StatusResponseDTO> UpdateUser(string Email, UserInputDto user)
        {
            try
            {
                var existingUser = await _repository.GetUserByIdAsync(Email);
                if (existingUser == null)
                    return StatusResponseDTO.NotFoundError();

                existingUser.Name = user.Name;
                await _repository.UpdateUserAsync(existingUser);
                var userEntity = _mapper.Map<User>(existingUser);
                return StatusResponseDTO.Ok(userEntity);
            }
            catch (Exception e)
            {
                return StatusResponseDTO.GetError(e.InnerException != null ? e.InnerException.Message : e.Message);
            }

        }
    }
}
