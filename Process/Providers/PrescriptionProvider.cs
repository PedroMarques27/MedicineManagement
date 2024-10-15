using AutoMapper;
using Database.Models;
using Database.Repositories;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using NuGet.Protocol.Core.Types;
using Process.DTOs;
using Process.DTOs.Entities;

namespace Process.Providers
{
    public class PrescriptionProvider : IPrescriptionProvider
    {
        private readonly IPrescriptionRepository _prescriptionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMedicineRepository _medicineRepository;
        private readonly IMapper _mapper;
        public PrescriptionProvider(IMapper mapper, IUserRepository userRepository, IMedicineRepository medicineRepository, IPrescriptionRepository prescriptionRepository) 
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _prescriptionRepository = prescriptionRepository;
            _medicineRepository = medicineRepository;
        }

        public async Task<StatusResponseDTO> CreatePrescriptionAsync(string Email, ICollection<string> medicines)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(Email);
                if (user == null) return StatusResponseDTO.NotFoundError();

                var prescription = new PrescriptionModel
                {
                    Id = Guid.NewGuid(),
                    CreationDate = DateTime.Now,
                    UserEmail = Email,
                    MedicineList = new List<MedicineModel>()
                };
                foreach(var medicine in medicines)
                {
                    var medicineModel = await _medicineRepository.GetMedicineByNameAsync(medicine);
                    if (medicineModel != null) prescription.MedicineList.Add(medicineModel);
                }
                await _prescriptionRepository.AddPrescriptionAsync(prescription);
                return StatusResponseDTO.Ok(null);

            }
            catch (Exception e)
            {
                return StatusResponseDTO.GetError(e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }

        public async Task<StatusResponseDTO> DeletePrescriptionByIdAsync(Guid Id)
        {
            try
            {
                
                await _prescriptionRepository.DeletePrescriptionByIdAsync(Id);
                return StatusResponseDTO.Ok(null);

            }
            catch (Exception e)
            {
                return StatusResponseDTO.GetError(e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }

        public StatusResponseDTO GetAllPrescriptions()
        {
            try
            {
                var prescriptionsModelList = _prescriptionRepository.GetAllPrescriptions();
                var prescriptions = _mapper.Map<ICollection<Prescription>>(prescriptionsModelList);
                return StatusResponseDTO.Ok(prescriptions);

            }
            catch (Exception e)
            {
                return StatusResponseDTO.GetError(e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }

        public async Task<StatusResponseDTO> GetPrescriptionByEmailAsync(string Email)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(Email);
                if (user == null) return StatusResponseDTO.NotFoundError();
                var prescriptions = _mapper.Map<ICollection<Prescription>>(user.PrescriptionList);
                return StatusResponseDTO.Ok(prescriptions);

            }
            catch (Exception e)
            {
                return StatusResponseDTO.GetError(e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }

        public StatusResponseDTO GetPrescriptionById(Guid Id)
        {
            try
            {
                var prescription = _prescriptionRepository.GetPrescriptionById(Id);
                if (prescription == null) return StatusResponseDTO.NotFoundError();
                var prescriptionsEntity = _mapper.Map<Prescription>(prescription);
                return StatusResponseDTO.Ok(prescriptionsEntity);

            }
            catch (Exception e)
            {
                return StatusResponseDTO.GetError(e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }

        public async Task<StatusResponseDTO> UpdatePrescriptionAsync(Guid Id, ICollection<string> medicines)
        {

            try
            {
                var prescription = _prescriptionRepository.GetPrescriptionById(Id);
                if (prescription == null) return StatusResponseDTO.NotFoundError();
                prescription.MedicineList.Clear();
                foreach (var medicine in medicines)
                {
                    var medicineModel = await _medicineRepository.GetMedicineByNameAsync(medicine);
                    if (medicineModel != null) prescription.MedicineList.Add(medicineModel);
                }
                await _prescriptionRepository.UpdatePrescriptionAsync(prescription);
                return StatusResponseDTO.Ok(null);

            }
            catch (Exception e)
            {
                return StatusResponseDTO.GetError(e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }
    }
}
