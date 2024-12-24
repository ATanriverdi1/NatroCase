using MediatR;
using Microsoft.AspNetCore.Mvc;
using NatroCase.Api.Models.User.Request;
using NatroCase.Application.Common.Models;
using NatroCase.Application.User.Queries;
using NatroCase.Domain.User;
using NatroCase.Domain.User.Entities;

namespace NatroCase.Api.Controllers;

[Route("users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        await _mediator.Send(request.ToCommand());
        return CreatedAtAction("Create", null);
    }

    [HttpPost("auth")]
    [ProducesResponseType(typeof(UserAuthToken), StatusCodes.Status201Created)]
    public async Task<IActionResult> Auth([FromBody] AutUserRequest request)
    {
        var token = await _mediator.Send(request.ToCommand());
        return CreatedAtAction("Auth", token);
    }

    [HttpPatch("{id}/add-favorite")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> AddFavorite([FromRoute] Guid id, [FromBody] AddUserFavoriteRequest request)
    {
        await _mediator.Send(request.ToCommand(id));
        return NoContent();
    }
    
    [HttpPatch("{id}/remove-favorite")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RemoveFavorite([FromRoute] Guid id, [FromBody] RemoveUserFavoriteRequest request)
    {
        await _mediator.Send(request.ToCommand(id));
        return NoContent();
    }

    [HttpGet("{id}/favorites")]
    [ProducesResponseType(typeof(Nextable<Favorite>) ,StatusCodes.Status200OK)]
    public async Task<IActionResult> GetFavorites([FromRoute] Guid id, [FromQuery] string? domainName, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
    {
        if (pageIndex < 1) pageIndex = 1;
        if (pageSize < 1) pageSize = 10;
        var nextable = await _mediator.Send(new UserFavoritesByIdQuery(id, domainName, pageIndex, pageSize));
        return Ok(nextable);
    }
}