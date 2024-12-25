using filmio_api.BLL.DTO.Film;
using filmio_api.BLL.DTO.Film.Create;
using filmio_api.BLL.DTO.Film.Update;
using filmio_api.BLL.MediatR.Film.Create;
using filmio_api.BLL.MediatR.Film.Delete;
using filmio_api.BLL.MediatR.Film.GetAll;
using filmio_api.BLL.MediatR.Film.GetById;
using filmio_api.BLL.MediatR.Film.Update;
using Microsoft.AspNetCore.Mvc;

namespace filmio_api.Controllers.Film;

public class FilmsController : BaseApiController
{
    [HttpGet("get-all")]
    [Produces("application/json", Type = typeof(GetAllFilmsResponseDto))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetAllFilmsResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll([FromQuery] ushort? page, [FromQuery] ushort? pageSize)
    {
        return HandleResult(await Mediator!.Send(new GetAllFilmsQuery(page, pageSize)));
    }

    [HttpGet("get-by-id/{id:int}")]
    [Produces("application/json", Type = typeof(FilmResponseDto))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FilmResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        return HandleResult(await Mediator!.Send(new GetFilmByIdQuery(id)));
    }

    [HttpPost("create")]
    [Produces("application/json", Type = typeof(FilmResponseDto))]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(FilmResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] FilmCreateRequestDto film)
    {
        return HandleResult(await Mediator!.Send(new CreateFilmCommand(film)));
    }

    [HttpPut("update")]
    [Produces("application/json", Type = typeof(FilmResponseDto))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FilmResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromBody] FilmUpdateRequestDto film)
    {
        return HandleResult(await Mediator!.Send(new UpdateFilmCommand(film)));
    }

    [HttpDelete("delete/{id:int}")]
    [Produces("application/json", Type = typeof(FilmResponseDto))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FilmResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        return HandleResult(await Mediator!.Send(new DeleteFilmCommand(id)));
    }
}