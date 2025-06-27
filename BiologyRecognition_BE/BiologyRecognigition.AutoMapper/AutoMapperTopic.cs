using AutoMapper;
using BiologyRecognition.Domain.Entities;
using BiologyRecognition.DTOs.Chapter;
using BiologyRecognition.DTOs.Topic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognigition.AutoMapper
{
    public class AutoMapperTopic : Profile
    {
        public AutoMapperTopic()
        {
            CreateMap<Topic, TopicDTO>()
                .ForMember(dest => dest.ChapterName, opt => opt.MapFrom(src => src.Chapter.Name))
                  .ForMember(dest => dest.CreatedName, opt => opt.MapFrom(src => src.CreatedByNavigation.UserName))
            .ForMember(dest => dest.ModifiedName, opt => opt.MapFrom(src => src.ModifiedByNavigation.UserName));

            CreateMap<CreateTopicDTO, Topic>()
          .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.Now))
          .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.CreatedBy))
          .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.Now));
            CreateMap<UpdateChapterDTO, Topic>()
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
             .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Chapter, opt => opt.Ignore());
        }
    }
}
