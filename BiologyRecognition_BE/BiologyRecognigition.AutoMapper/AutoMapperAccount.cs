using AutoMapper;
using BiologyRecognition.DTOs.UserAccount;
using BiologyRecognition.Domain.Entities;
namespace BiologyRecognition.AutoMapper
{
    public class AutoMapperAccount : Profile
    {
        public AutoMapperAccount()
        {
              CreateMap<UserAccount, UserAccountDTO>();

              CreateMap<RegisterDTO, UserAccount>()
             .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.Now))
             .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));

              CreateMap<UpdateAccountStudentDTO, UserAccount>()
             .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.Now))
             .ForMember(dest => dest.UserAccountId, opt => opt.Ignore())
             .ForMember(dest => dest.RoleId, opt => opt.Ignore()) 
             .ForMember(dest => dest.IsActive, opt => opt.Ignore())
             .ForMember(dest => dest.RequestCode, opt => opt.Ignore())
             .ForMember(dest => dest.ApplicationCode, opt => opt.Ignore())
             .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
             .ForMember(dest => dest.CreatedDate, opt => opt.Ignore());

            CreateMap<UpdateAccountAdminDTO, UserAccount>()
           .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow));
        }
    }
}
