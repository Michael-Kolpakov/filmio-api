using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace filmio_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
    private IMediator? _mediator;

    protected IMediator? Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

    protected ActionResult HandleResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
        {
            return result.Value == null
                ? NotFound()
                : Ok(result.Value);
        }

        return BadRequest(result.Reasons);
    }
}