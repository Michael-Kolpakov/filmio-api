namespace Filmio.BLL.DTO.Film;

public class FilmResponseDto
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Genre { get; set; }

    public string? Director { get; set; }

    public DateTime ReleaseDate { get; set; }

    public float? Rating { get; set; }

    public string? Description { get; set; }
}