using AutoMapper;
using Filmio.BLL.DTO.Authentication;
using Filmio.BLL.DTO.Authentication.Login;

namespace Filmio.BLL.Mapping.Authentication.Login;

public class LoginProfile : Profile
{
    public LoginProfile()
    {
        CreateMap<DAL.Entities.Users.User, UserShortDto>();
        CreateMap<DAL.Entities.Users.User, LoginResponseDto>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src));
        CreateMap<LoginRequestDto, DAL.Entities.Users.User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));
    }
}