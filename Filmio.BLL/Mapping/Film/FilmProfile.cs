using AutoMapper;
using Filmio.BLL.DTO.Film;
using Filmio.BLL.DTO.Film.Create;
using Filmio.BLL.DTO.Film.Update;

namespace Filmio.BLL.Mapping.Film;

public class FilmProfile : Profile
{
    public FilmProfile()
    {
        CreateMap<DAL.Entities.Films.Film, FilmResponseDto>();
        CreateMap<FilmCreateRequestDto, DAL.Entities.Films.Film>();
        CreateMap<FilmUpdateRequestDto, DAL.Entities.Films.Film>();
    }
}