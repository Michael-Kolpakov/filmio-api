using System.ComponentModel.DataAnnotations;

namespace Filmio.BLL.DTO.Authentication.Register;

public class RegisterRequestDto
{
    [Display(Name = "Full name")]
    [Required(ErrorMessage = "{0} can't be empty or null")]
    [StringLength(30, ErrorMessage = "{0} should be less than {1} characters")]
    [RegularExpression(@"^[a-zA-Z\s']*$", ErrorMessage = "{0} can only contain latin letters, spaces and apostrophes")]
    public string? FullName { get; set; }

    [Required(ErrorMessage = "{0} can't be empty or null")]
    [EmailAddress(ErrorMessage = "{0} must be in a valid format")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "{0} can't be empty or null")]
    [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "{0} can only contain numbers and latin letters")]
    public string? Password { get; set; }
}