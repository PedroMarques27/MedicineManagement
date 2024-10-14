﻿using AutoMapper;
using Database.Models;
using Microsoft.OpenApi.Extensions;
using Process.DTOs.Entities;

namespace Process.Profiles
{
    public class MappingProfile: Profile
    {
        public MappingProfile() 
        {
            CreateMap<User, UserModel>()
                 .ForMember(dest => dest.PrescriptionList, opt => opt.MapFrom(src => src.PrescriptionList))
                 .ReverseMap();

            CreateMap<Prescription, PrescriptionModel>()
                .ForMember(dest => dest.MedicineList, opt => opt.MapFrom(src=> src.MedicineList))
                .ReverseMap();

            CreateMap<MedicineModel, Medicine>()
                .ReverseMap();
        }
    }
}
