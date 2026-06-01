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

    [HttpGet("/profile/{userId}")]
    public async Task<UserHealthProfileDto> GetProfile(GetUserProfileQuery query)
    {
        return await _mediator.Send(query);
    }
    
    [HttpPost]
    public async Task<IActionResult> UpdateProfile(UpdateHealthProfileCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("conditions/{conditionId}")]
    public async Task<IActionResult> AddCondition(AddMedicalConditionCommand command)
    {
        await _mediator.Send(command);
        return Ok(new { message = "Condición añadida correctamente" });
    }

    [HttpDelete("/{userId}/")]
    public async Task<IActionResult> RemoveCondition(RemoveMedicalConditionCommand command)
    {
        await _mediator.Send(command);
        return Ok( new { message = "Condición eliminda correctamente"});
    }
}