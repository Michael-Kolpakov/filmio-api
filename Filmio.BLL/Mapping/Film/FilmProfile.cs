using AutoMapper;
using Filmio.BLL.DTO.Film;
using Filmio.BLL.DTO.Film.Create;
using Filmio.BLL.DTO.Film.Update;

namespace Filmio.BLL.Mapping.Film;

public class FilmProfile : Profile
{
    public FilmProfile()
    {
        CreateMap<DAL.Entities.Film.Film, FilmResponseDto>();
        CreateMap<DAL.Entities.Film.Film, FilmCreateRequestDto>().ReverseMap();
        CreateMap<FilmUpdateRequestDto, DAL.Entities.Film.Film>();
    }
}