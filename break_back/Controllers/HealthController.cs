using break_back.Services;
using Microsoft.AspNetCore.Mvc;
using Nutria.Domain.Dtos.HealthProfile;
using Nutria.Domain.Dtos.User;

namespace break_back.Controllers;

[ApiController]
[Route("[controller]")]
public class HealthController : ControllerBase
{
    private readonly IHealthService _healthService;

    public HealthController(IHealthService healthService)
    {
        _healthService = healthService;
    }

    [HttpGet("/profile/{userId}")]
    public async Task<UserHealthProfileDto> GetProfile(Guid userId)
    {
        return await _healthService.GetProfile(userId);
    }
    
    [HttpPost]
    public async Task<IActionResult> UpdateProfile(HealthProfileCreateDto profile)
    {
        var result = await _healthService.UpdateProfile(profile.UserId, profile);
        return Ok(result);
    }

    [HttpPost("conditions/{conditionId}")]
    public async Task<IActionResult> AddCondition(Guid userId, int conditionId)
    {
        await _healthService.AddConditionToUser(userId, conditionId);
        return Ok(new { message = "Condición añadida correctamente" });
    }

    [HttpDelete("/{userId}/")]
    public async Task<IActionResult> RemoveCondition(Guid userId, int conditionId)
    {
        await _healthService.RemoveConditionFromUser(userId, conditionId);
        return Ok( new { message = "Condición eliminda correctamente"});
    }
}