using AutoMapper;
using BiologyRecognition.Domain.Entities;
using BiologyRecognition.DTOs.Recognition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognigition.AutoMapper
{
    public class AutoMapperRecognition : Profile
    {
        public AutoMapperRecognition()
        {
            CreateMap<Recognition, RecognitionDTO>()
    .ForMember(dest => dest.ArtifactName, opt => opt.MapFrom(src => src.Artifact.Name));

        }
    }
}
