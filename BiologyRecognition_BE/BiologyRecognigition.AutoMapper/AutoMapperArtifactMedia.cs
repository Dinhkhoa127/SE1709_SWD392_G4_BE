using AutoMapper;
using BiologyRecognition.Domain.Entities;
using BiologyRecognition.DTOs.ArtifactMedia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognigition.AutoMapper
{
    public class AutoMapperArtifactMedia : Profile
    {
        public AutoMapperArtifactMedia()
        {
            
                
                CreateMap<ArtifactMedia, ArtifactMediaDTO>()
                    .ForMember(dest => dest.ArtifactName, opt => opt.MapFrom(src => src.Artifact.Name));

              
                CreateMap<CreateArtifactMediaDTO, ArtifactMedia>();

                
                CreateMap<UpdateArtifactMediaDTO, ArtifactMedia>();


            
        }
    }
}
