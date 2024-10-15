using AutoMapper;
using Database.Models;
using Database.Repositories;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using NuGet.Protocol.Core.Types;
using Process.DTOs;
using Process.DTOs.Entities;

namespace Process.Providers
{
    public class MedicineProvider : IMedicineProvider
    {
        private readonly IMedicineRepository _repository;
        private readonly IMapper _mapper;

        public MedicineProvider(IMedicineRepository repository, IMapper mapper) 
        {
            this._mapper = mapper;
            _repository = repository;
        }
        public async Task<StatusResponseDTO> AddMedicine(Medicine medicine)
        {
            try
            {
                var newMedicine = _mapper.Map<MedicineModel>(medicine);
                newMedicine.CreationDate = DateTime.Now;
                await _repository.AddMedicineAsync(newMedicine);
                return StatusResponseDTO.Ok(newMedicine);

            }
            catch (Exception e)
            {
                return StatusResponseDTO.GetError(e.InnerException != null ? e.InnerException.Message : e.Message);
            }

        }

        public async Task<StatusResponseDTO> DeleteMedicine(string Name)
        {
            try
            {
                await _repository.DeleteMedicineByNameAsync(Name);
                return StatusResponseDTO.Ok(null);

            }
            catch (Exception e)
            {
                return StatusResponseDTO.GetError(e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }

        public StatusResponseDTO GetAll()
        {
            try
            {
                var medicines = _repository.GetAllMedicines();
                var mapped = _mapper.Map<ICollection<Medicine>>(medicines);
                return StatusResponseDTO.Ok(mapped);
            }
            catch (Exception e)
            {
                return StatusResponseDTO.GetError(e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }

        public async Task<StatusResponseDTO> GetByNameAsync(string name)
        {
            try
            {
                var medicine = await _repository.GetMedicineByNameAsync(name);
                if (medicine == null)
                {
                    return StatusResponseDTO.NotFoundError();
                }
                var mapped = _mapper.Map<Medicine>(medicine);
                return StatusResponseDTO.Ok(mapped);
            }
            catch (Exception e)
            {
                return StatusResponseDTO.GetError(e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }

        public async Task<StatusResponseDTO> UpdateMedicine(string name, Medicine medicine)
        {
            try
            {
                var existingMedicine = await _repository.GetMedicineByNameAsync(name);
                if (existingMedicine == null)
                    return StatusResponseDTO.NotFoundError();

                existingMedicine.Quantity = medicine.Quantity;
                await _repository.UpdateMedicineAsync(existingMedicine);
                return StatusResponseDTO.Ok(existingMedicine);
            }
            catch (Exception e)
            {
                return StatusResponseDTO.GetError(e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }
    }
}
