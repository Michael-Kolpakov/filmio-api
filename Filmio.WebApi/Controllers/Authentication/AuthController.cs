using System.Security.Claims;
using Filmio.BLL.DTO.Authentication.Login;
using Filmio.BLL.DTO.Authentication.Register;
using Filmio.BLL.MediatR.Authentication.DeleteAccount;
using Filmio.BLL.MediatR.Authentication.Login;
using Filmio.BLL.MediatR.Authentication.Register;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Filmio.WebApi.Controllers.Authentication;

public class AuthController : BaseApiController
{
    [HttpPost("registration")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
    {
        return HandleResult(await Mediator!.Send(new RegisterCommand(registerRequestDto)));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
    {
        return HandleResult(await Mediator!.Send(new LoginQuery(loginRequestDto)));
    }

    [HttpDelete("delete")]
    [Authorize]
    public async Task<IActionResult> DeleteAccount()
    {
        var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value);

        return HandleResult(await Mediator!.Send(new DeleteAccountCommand(userId)));
    }
}