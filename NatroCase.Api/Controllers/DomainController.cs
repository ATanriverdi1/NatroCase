using MediatR;
using Microsoft.AspNetCore.Mvc;
using NatroCase.Application.External.Rdap.Models.Response;
using NatroCase.Application.External.Rdap.Queries;

namespace NatroCase.Api.Controllers;

[Route("domains")]
[ApiController]
public class DomainController : ControllerBase
{
    private readonly IMediator _mediator;

    public DomainController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{domain}/check")]
    [ProducesResponseType(typeof(CheckDomainNameResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> CheckDomain([FromRoute] string domain)
    {
        var response = await _mediator.Send(new RdapCheckDomainNameQuery(domain));
        return Ok(response);
    }
}