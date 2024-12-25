using AutoMapper;
using filmio_api.BLL.DTO.Film;
using filmio_api.BLL.DTO.Film.Create;
using filmio_api.BLL.DTO.Film.Update;

namespace filmio_api.BLL.Mapping.Film;

public class FilmProfile : Profile
{
    public FilmProfile()
    {
        CreateMap<DAL.Entities.Film.Film, FilmDto>();
        CreateMap<DAL.Entities.Film.Film, FilmCreateDto>().ReverseMap();
        CreateMap<FilmUpdateDto, DAL.Entities.Film.Film>();
    }
}