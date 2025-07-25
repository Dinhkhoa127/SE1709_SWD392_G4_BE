﻿using AutoMapper;
using BiologyRecognition.Domain.Entities;
using BiologyRecognition.DTOs.Subject;
using BiologyRecognition.DTOs.UserAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognigition.AutoMapper
{
    public class AutoMapperSubject : Profile
    {
        public AutoMapperSubject()
        {
            CreateMap<Subject, SubjectDTO>()
            .ForMember(dest => dest.CreatedName, opt => opt.MapFrom(src => src.CreatedByNavigation.UserName))
            .ForMember(dest => dest.ModifiedName, opt => opt.MapFrom(src => src.ModifiedByNavigation.UserName));

            CreateMap<CreateSubjectDTO, Subject>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<UpdateSubjectDTO, Subject>()
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore());
          

        }
    }
}
