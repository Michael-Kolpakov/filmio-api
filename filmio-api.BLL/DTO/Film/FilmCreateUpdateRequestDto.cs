using System.ComponentModel.DataAnnotations;
using filmio_api.BLL.Attributes;

namespace filmio_api.BLL.DTO.Film;

public class FilmCreateUpdateRequestDto
{
    [Required(ErrorMessage = "{0} can't be empty or null")]
    [StringLength(40, ErrorMessage = "{0} should be less than {1} characters")]
    public string? Title { get; set; }

    [Required(ErrorMessage = "{0} can't be empty or null")]
    [StringLength(40, ErrorMessage = "{0} should be less than {1} characters")]
    [RegularExpression(@"^[a-zA-Z| ]*$", ErrorMessage = "{0} can only contain latin letters, spaces and the '|' character")]
    public string? Genre { get; set; }

    [Required(ErrorMessage = "{0} can't be empty or null")]
    [StringLength(25, ErrorMessage = "{0} should be less than {1} characters")]
    [RegularExpression(@"^[a-zA-Z' ]*$", ErrorMessage = "{0} can only contain latin letters and apostrophes")]
    public string? Director { get; set; }

    [Display(Name = "Release date")]
    [Required(ErrorMessage = "{0} can't be empty or null")]
    [ReleaseDate]
    public DateTime ReleaseDate { get; set; }

    [Range(0, 10, ErrorMessage = "{0} should be between {1} and {2}")]
    public float? Rating { get; set; }

    [StringLength(500, ErrorMessage = "{0} should be less than {1} characters")]
    public string? Description { get; set; }
}