using MediatR;
using Microsoft.AspNetCore.Mvc;
using nutria.Application.UseCases.Health.Commands;
using nutria.Application.UseCases.Health.Queries;
using Nutria.Domain.Dtos.User;

namespace break_back.Controllers;

[ApiController]
[Route("[controller]")]
public class HealthController : ControllerBase
{
    private readonly IMediator _mediator;

    public HealthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("Profile/")]
    public async Task<UserHealthProfileDto> GetProfile([FromQuery] GetUserProfileQuery query)
    {
        return await _mediator.Send(query);
    }
    
    [HttpPost("Profile/Update")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateHealthProfileCommand command)
    {
        var res = await _mediator.Send(command);

        return Ok( new { message = res } );
    }

    [HttpPost("Condition/Add")]
    public async Task<IActionResult> AddCondition([FromBody] AddMedicalConditionCommand command)
    {
        var res = await _mediator.Send(command);
        return Ok(new { message = res });
    }

    [HttpDelete("Condition/Delete")]
    public async Task<IActionResult> RemoveCondition([FromQuery] RemoveMedicalConditionCommand command)
    {
        var res = await _mediator.Send(command);
        return Ok( new { message = res });
    }
}