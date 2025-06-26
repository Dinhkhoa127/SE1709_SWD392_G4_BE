using AutoMapper;
using BiologyRecognition.DTOs.UserAccount;
using BiologyRecognition.Domain.Entities;
using BiologyRecognigition.AutoMapper;
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

            CreateMap<CreateAccountDTO, UserAccount>()
      .ForMember(dest => dest.UserName, opt => opt.MapFrom<EmailToUsernameResolver>())
      .ForMember(dest => dest.Password, opt => opt.MapFrom(src => "123@"))
      .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.Now))
      .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
      .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow));



            CreateMap<UpdateAccountStudentDTO, UserAccount>()
             .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.Now))
             .ForMember(dest => dest.UserAccountId, opt => opt.Ignore())
             .ForMember(dest => dest.RoleId, opt => opt.Ignore())
             .ForMember(dest => dest.IsActive, opt => opt.Ignore())
             .ForMember(dest => dest.CreatedDate, opt => opt.Ignore());

            CreateMap<UpdateAccountAdminDTO, UserAccount>()
           .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<UserAccount, UpdateAccountAdminDTO>()
                .ForMember(dest => dest.Password, opt => opt.Ignore());

            CreateMap<RegisterDTO, UserAccount>();

            CreateMap<UserAccount, AccountResponseDTO>()
            .ForMember(dest => dest.AccessToken, opt => opt.Ignore());

        }
    }
}
