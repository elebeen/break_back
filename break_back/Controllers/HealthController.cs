using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using nutria.Application.UseCases.Health.Commands;
using nutria.Application.UseCases.Health.Queries;

namespace break_back.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class HealthController : ControllerBase
{
    private readonly IMediator _mediator;

    public HealthController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("Profile/Update")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateHealthProfileCommand command)
    {
        var res = await _mediator.Send(command);

        return Ok( new { message = res });
    }

    [HttpGet("MedicalConditions")]
    public async Task<IActionResult> GetMedicalConditions([FromQuery] GetMedicalConditionsQuery query)
    {
        var res = await _mediator.Send(query);
        return Ok( res );
    }

    [HttpPost("Condition/Add")]
    public async Task<IActionResult> AddCondition([FromBody] AddMedicalConditionCommand command)
    {
        var res = await _mediator.Send(command);
        return Ok( new { message = res });
    }

    [HttpDelete("Condition/Delete")]
    public async Task<IActionResult> RemoveCondition([FromQuery] RemoveMedicalConditionCommand command)
    {
        var res = await _mediator.Send(command);
        return Ok( new { message = res });
    }
}