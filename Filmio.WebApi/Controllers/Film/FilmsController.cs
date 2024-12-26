using Filmio.BLL.DTO.Film;
using Filmio.BLL.DTO.Film.Create;
using Filmio.BLL.DTO.Film.Update;
using Filmio.BLL.MediatR.Film.Create;
using Filmio.BLL.MediatR.Film.Delete;
using Filmio.BLL.MediatR.Film.GetAll;
using Filmio.BLL.MediatR.Film.GetById;
using Filmio.BLL.MediatR.Film.Update;
using Microsoft.AspNetCore.Mvc;

namespace Filmio.WebApi.Controllers.Film;

public class FilmsController : BaseApiController
{
    /// <summary>
    /// Allows you to receive all films or paginated films depending on the transmitted parameters
    /// </summary>
    /// <param name="page">The page number we need to get</param>
    /// <param name="pageSize">Number of elements per page</param>
    /// <returns>Returns movies that have passed the selection</returns>
    [HttpGet("get-all")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetAllFilmsResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll([FromQuery] ushort? page, [FromQuery] ushort? pageSize)
    {
        return HandleResult(await Mediator!.Send(new GetAllFilmsQuery(page, pageSize)));
    }

    /// <summary>
    /// Allows you to get a movie by Id
    /// </summary>
    /// <param name="id">Id using which we want to receive the film</param>
    /// <returns>Returns the corresponding film</returns>
    [HttpGet("get-by-id/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FilmResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        return HandleResult(await Mediator!.Send(new GetFilmByIdQuery(id)));
    }

    /// <summary>
    /// Allows you to create a new film
    /// </summary>
    /// <param name="film">The film object to be added to the database</param>
    /// <returns>Returns the added film object</returns>
    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(FilmResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] FilmCreateRequestDto film)
    {
        return HandleResult(await Mediator!.Send(new CreateFilmCommand(film)));
    }

    /// <summary>
    /// Allows you to modify an existing film
    /// </summary>
    /// <param name="film">New film object to change old one</param>
    /// <returns>Returns the modified film object</returns>
    [HttpPut("update")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FilmResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromBody] FilmUpdateRequestDto film)
    {
        return HandleResult(await Mediator!.Send(new UpdateFilmCommand(film)));
    }

    /// <summary>
    /// Allows you to delete an existing film
    /// </summary>
    /// <param name="id">Id using which the film object to be deleted will be found</param>
    /// <returns>Returns the deleted film object</returns>
    [HttpDelete("delete/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FilmResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        return HandleResult(await Mediator!.Send(new DeleteFilmCommand(id)));
    }
}