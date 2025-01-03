using AutoMapper;
using Filmio.BLL.DTO.Authentication;
using Filmio.BLL.DTO.Authentication.Register;

namespace Filmio.BLL.Mapping.Authentication.Register;

public class RegisterProfile : Profile
{
    public RegisterProfile()
    {
        CreateMap<DAL.Entities.Users.User, UserShortDto>();
        CreateMap<DAL.Entities.Users.User, RegisterResponseDto>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src));
        CreateMap<RegisterRequestDto, DAL.Entities.Users.User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));
    }
}