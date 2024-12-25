using AutoMapper;
using filmio_api.BLL.DTO.Film;
using filmio_api.BLL.DTO.Film.Create;
using filmio_api.BLL.DTO.Film.Update;

namespace filmio_api.BLL.Mapping.Film;

public class FilmProfile : Profile
{
    public FilmProfile()
    {
        CreateMap<DAL.Entities.Film.Film, FilmResponseDto>();
        CreateMap<DAL.Entities.Film.Film, FilmCreateRequestRequestDto>().ReverseMap();
        CreateMap<FilmUpdateRequestDto, DAL.Entities.Film.Film>();
    }
}