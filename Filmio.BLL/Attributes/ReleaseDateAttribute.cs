using System.ComponentModel.DataAnnotations;

namespace Filmio.BLL.Attributes;

public class ReleaseDateAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
        {
            return new ValidationResult("Input parameter cannot be null");
        }

        if (value is DateTime releaseDate)
        {
            if (releaseDate > DateTime.UtcNow)
            {
                return new ValidationResult("Release date cannot be in the future");
            }
        }

        return ValidationResult.Success;
    }
}