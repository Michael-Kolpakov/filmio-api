using System.ComponentModel.DataAnnotations;

namespace Filmio.BLL.DTO.Authentication.Login;

public class LoginRequestDto
{
    [Required(ErrorMessage = "{0} can't be empty or null")]
    [EmailAddress(ErrorMessage = "{0} must be in a valid format")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "{0} can't be empty or null")]
    [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "{0} can only contain numbers and latin letters")]
    public string? Password { get; set; }
}