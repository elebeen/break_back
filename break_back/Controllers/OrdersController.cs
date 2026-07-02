using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nutria.Application.UseCases.Orders.Commands;
using nutria.Application.UseCases.Orders.Queries;

namespace break_back.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public OrdersController(IMediator mediator)
    {
        this._mediator = mediator;
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetOrdersHistory([FromQuery] GetUserOrdersQuery query)
    {
        var res = await _mediator.Send(query);
        return Ok(res);
    }

    [HttpGet("get/{id}")]
    public async Task<IActionResult> GetOrderById([FromRoute] GetOrderByIdQuery query)
    {
        var res = await _mediator.Send(query);
        return Ok(res);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command)
    {
        var res = await _mediator.Send(command);
        return Ok(res);
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrderStatusCommand command)
    {
        var res = await _mediator.Send(command);
        return Ok(res);
    }
    
}