using AutoMapper;
using BiologyRecognition.Domain.Entities;
using BiologyRecognition.DTOs.Artifact;
using BiologyRecognition.DTOs.ArtifactMedia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognigition.AutoMapper
{
    public class AutoMapperArtifact : Profile
    {
       public AutoMapperArtifact() {
            
            CreateMap<Artifact, ArtifactDTO>()
                 .ForMember(dest => dest.ArtifactName,
            opt => opt.MapFrom(src => src.Name))
        .ForMember(dest => dest.ArtifactTypeName,
            opt => opt.MapFrom(src => src.ArtifactType.Name))
        .ForMember(dest => dest.TopicId,
            opt => opt.MapFrom(src => src.ArtifactType.Topic.TopicId))
        .ForMember(dest => dest.TopicName,
            opt => opt.MapFrom(src => src.ArtifactType.Topic.Name));

            CreateMap<Artifact, ArtifactDetailsDTO>()
    .ForMember(dest => dest.ArtifactName,
        opt => opt.MapFrom(src => src.Name))
    .ForMember(dest => dest.ArtifactTypeName,
        opt => opt.MapFrom(src => src.ArtifactType.Name))
    .ForMember(dest => dest.TopicId,
        opt => opt.MapFrom(src => src.ArtifactType.Topic.TopicId))
    .ForMember(dest => dest.TopicName,
        opt => opt.MapFrom(src => src.ArtifactType.Topic.Name));

            CreateMap<UpdateArtifactDTO, Artifact>()
    .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.Now))
    .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
    .ForMember(dest => dest.CreatedDate, opt => opt.Ignore());


            CreateMap<CreateArtifactDTO, Artifact>()
     .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.Now))
          .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.CreatedBy))
          .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<Artifact, ArtifactWithMediaArticleDTO>()
                  .ForMember(dest => dest.ArtifactName,
            opt => opt.MapFrom(src => src.Name))
        .ForMember(dest => dest.ArtifactTypeName,
            opt => opt.MapFrom(src => src.ArtifactType.Name))
        .ForMember(dest => dest.TopicId,
            opt => opt.MapFrom(src => src.ArtifactType.Topic.TopicId))
        .ForMember(dest => dest.TopicName,
            opt => opt.MapFrom(src => src.ArtifactType.Topic.Name));

            CreateMap<ArtifactMedia, ArtifactMediaWithArtifactDTO>()
               .ForMember(dest => dest.DescriptionMedia,
            opt => opt.MapFrom(src => src.Description));
        }
    }
}
