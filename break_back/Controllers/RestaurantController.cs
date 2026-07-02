using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nutria.Application.UseCases.Restaurants.Commands;
using nutria.Application.UseCases.Restaurants.Queries;

namespace break_back.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class RestaurantController : ControllerBase
{
    private readonly IMediator _mediator;

    public RestaurantController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("profile")]
    public async Task<ActionResult> Get([FromQuery] GetRestaurantInfoQuery  query)
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

    [HttpPut("update")]
    public async Task<ActionResult> Update([FromBody] EditRestaurantCommand command)
    {
        var res = await  _mediator.Send(command);
        return Ok(res);
    }

    [HttpDelete("delete")]
    public async Task<ActionResult> Delete([FromQuery] DeactivateRestaurantCommand command)
    {
        var res = await  _mediator.Send(command);
        return Ok(res);
    }
}