using Process.DTOs;

namespace Process.Providers
{
    public interface IPrescriptionProvider
    {
        Task<StatusResponseDTO> CreatePrescriptionAsync(string Email, ICollection<string> medicines);
        Task<StatusResponseDTO> UpdatePrescriptionAsync(Guid id, ICollection<string> medicines);
        StatusResponseDTO GetPrescriptionById(Guid Id);
        Task<StatusResponseDTO> GetPrescriptionByEmailAsync(string Email);
        StatusResponseDTO GetAllPrescriptions();
        Task<StatusResponseDTO> DeletePrescriptionByIdAsync(Guid Id);
    }
}
