using System.ComponentModel.DataAnnotations;

namespace Filmio.DAL.Entities.Users;

public class User
{
    [Key]
    public int Id { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? PasswordHash { get; set; }
}