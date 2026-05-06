using Microsoft.AspNetCore.Mvc;
using break_back.Models.Dtos.HealthProfileDtos;
using break_back.Services;

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

    [HttpPost("profile")]
    public async Task<IActionResult> UpdateProfile(HealthProfileCreateDto profile)
    {
        // En una app real, el UserId vendría del Token JWT (User.Claims)
        // Por ahora lo simulamos o lo recibes en el body
        var result = await _healthService.UpsertProfile(profile.UserId, profile);
        return Ok(result);
    }

    [HttpPost("conditions/{conditionId}")]
    public async Task<IActionResult> AddCondition(Guid userId, int conditionId)
    {
        await _healthService.AddConditionToUser(userId, conditionId);
        return Ok(new { message = "Condición añadida correctamente" });
    }
}