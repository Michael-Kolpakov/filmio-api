namespace Filmio.BLL.DTO.Authentication.Login;

public class LoginResponseDto
{
    public UserShortDto User { get; set; }

    public string AccessToken { get; set; }
}