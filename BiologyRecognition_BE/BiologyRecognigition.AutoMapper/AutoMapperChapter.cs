using AutoMapper;
using BiologyRecognition.Domain.Entities;
using BiologyRecognition.DTOs.Chapter;
using BiologyRecognition.DTOs.Subject;

namespace BiologyRecognition.AutoMapper
{
    public class AutoMapperChapter : Profile
    {
        public AutoMapperChapter()
        {
            CreateMap<Chapter, ChapterDTO>()
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Subject.Name))
                  .ForMember(dest => dest.CreatedName, opt => opt.MapFrom(src => src.CreatedByNavigation.UserName))
            .ForMember(dest => dest.ModifiedName, opt => opt.MapFrom(src => src.ModifiedByNavigation.UserName));

            CreateMap<CreateChapterDTO, Chapter>()
          .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.Now))
          .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.CreatedBy))
          .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.Now));
            CreateMap<UpdateChapterDTO, Chapter>()
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
             .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Subject, opt => opt.Ignore());
        }
    }
}
