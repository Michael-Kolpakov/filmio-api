namespace Filmio.BLL.DTO.Film;

public class GetAllFilmsResponseDto
{
    public int TotalAmount { get; set; }

    public IEnumerable<FilmResponseDto> Films { get; set; } = new List<FilmResponseDto>();
}