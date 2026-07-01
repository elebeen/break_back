using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nutria.Application.UseCases.Restaurants.Commands;
using nutria.Application.UseCases.Restaurants.Queries;
using Nutria.Domain.Models;

namespace break_back.Controllers;

[Authorize]
[ApiController]
public class RestaurantController : ControllerBase
{
    private readonly IMediator _mediator;

    public RestaurantController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("profile")]
    public async Task<ActionResult> Get(GetRestaurantInfoQuery  query)
    {
        var res = await _mediator.Send(query);
        return Ok(res);
    }
    
    [HttpPost("create")]
    public async Task<ActionResult> Create([FromBody] RegisterRestaurantCommand command)
    {
        var res = await  _mediator.Send(command);
        return Ok(res);
    }

    [HttpPost("update")]
    public async Task<ActionResult> Update([FromBody] RegisterRestaurantCommand command)
    {
        var res = await  _mediator.Send(command);
        return Ok(res);
    }

    [HttpPost("delete")]
    public async Task<ActionResult> Delete([FromQuery] RegisterRestaurantCommand command)
    {
        var res = await  _mediator.Send(command);
        return Ok(res);
    }
}