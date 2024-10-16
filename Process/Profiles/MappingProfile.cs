using AutoMapper;
using Database.Models;
using Microsoft.OpenApi.Extensions;
using Process.DTOs.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Process.Profiles
{
    [ExcludeFromCodeCoverage]
    public class MappingProfile: Profile
    {
        public MappingProfile() 
        {
            CreateMap<User, UserModel>()
                 .ForMember(dest => dest.PrescriptionList, opt => opt.MapFrom(src => src.PrescriptionList))
                 .ReverseMap();

            CreateMap<Prescription, PrescriptionModel>()
                .ForMember(dest => dest.MedicineList, opt => opt.MapFrom(src=> src.MedicineList));

            CreateMap<Medicine, MedicineModel>()
                .ForMember(dest => dest.PrescriptionMedicines, opt => opt.Ignore())
                .ReverseMap();


            CreateMap<PrescriptionModel, Prescription>()
                .ForMember(dest => dest.MedicineList,
                           opt => opt.MapFrom(src => src.MedicineList.Select(pm => pm.MedicineName)));
        }
    }
}
