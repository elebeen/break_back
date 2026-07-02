using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nutria.Application.UseCases.Meals.Commands;

namespace break_back.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class MealsController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public MealsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> CreateMeal([FromBody] CreateMealCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(new { message = result });
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateMeal([FromBody] UpdateMealCommand command )
    {
        var res = await _mediator.Send(command);
        return Ok(res);
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteMeal([FromQuery] DeactivateMealCommand command)
    {
        var res = await _mediator.Send(command);
        return Ok(res);
    }
}