using Microsoft.AspNetCore.Mvc;
using Process.DTOs;
using Process.DTOs.Entities;

namespace Process.Providers
{
    public interface IUsersProvider
    {
        StatusResponseDTO GetUsers();
        Task<StatusResponseDTO> GetUserByEmail(string Email);
        Task<StatusResponseDTO> AddUser(User user);
        Task<StatusResponseDTO> UpdateUser(string Email, User user);
        Task<StatusResponseDTO> DeleteUser(string Email);


    }
}
