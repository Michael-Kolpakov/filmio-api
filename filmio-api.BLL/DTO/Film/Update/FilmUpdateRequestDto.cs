using System.ComponentModel.DataAnnotations;

namespace filmio_api.BLL.DTO.Film.Update;

public class FilmUpdateRequestDto : FilmCreateUpdateRequestDto
{
    [Required(ErrorMessage = "{0} can't be empty or null")]
    public int Id { get; set; }
}