namespace Filmio.BLL.DTO.Authentication.Register;

public class RegisterResponseDto
{
    public UserShortDto User { get; set; }

    public string AccessToken { get; set; }
}