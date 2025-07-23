
using AutoMapper;
using BiologyRecognition.Domain.Entities;
using BiologyRecognition.DTOs.Article;
using BiologyRecognition.DTOs.Artifact;
using BiologyRecognition.DTOs.Chapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognigition.AutoMapper
{
    public class AutoMapperArticle : Profile
    {
        public AutoMapperArticle()
        {
            CreateMap<Article, ArticleDTO>();
            CreateMap<Article, ArticleDetailsDTO>()
                  .ForMember(dest => dest.CreateName, opt => opt.MapFrom(src => src.CreatedByNavigation.UserName))
            .ForMember(dest => dest.ModifiedName, opt => opt.MapFrom(src => src.ModifiedByNavigation.UserName))
               .ForMember(dest => dest.ArtifactIds, opt => opt.MapFrom(src => src.Artifacts.Select(a => a.ArtifactId)));

            CreateMap<CreateArticleDTO, Article>()
        .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.Now))
        .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.CreatedBy))
        .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.Now))
        .ForMember(dest => dest.Artifacts, opt => opt.Ignore())
        ;
            CreateMap<UpdateArticleDTO, Article>()
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
             .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Artifacts, opt => opt.Ignore());
        }
    }
}
