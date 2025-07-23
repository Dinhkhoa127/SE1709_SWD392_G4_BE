using AutoMapper;
using BiologyRecognition.Domain.Entities;
using BiologyRecognition.DTOs.ArtifactType;
using BiologyRecognition.DTOs.Chapter;
using BiologyRecognition.DTOs.Topic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognigition.AutoMapper
{
    public class AutoMapperArtifactType:Profile
    {
        public AutoMapperArtifactType()
        {
            CreateMap<ArtifactType, ArtifactTypeDTO>()
                .ForMember(dest => dest.TopicName, opt => opt.MapFrom(src => src.Topic.Name));

            CreateMap<CreateArtifactTypeDTO, ArtifactType>();
            CreateMap<UpdateArtifactTypeDTO, ArtifactType>();
        }
    }
}
