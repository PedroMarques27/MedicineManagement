using Process.DTOs;
using Process.DTOs.Entities;

namespace Process.Providers
{
    public interface IMedicineProvider
    {
        StatusResponseDTO GetAll();
        Task<StatusResponseDTO> GetByNameAsync(string name);
        Task<StatusResponseDTO> AddMedicine(Medicine medicine);
        Task<StatusResponseDTO> UpdateMedicine(string name, Medicine medicine);
        Task<StatusResponseDTO> DeleteMedicine(string Name);
    }
}
