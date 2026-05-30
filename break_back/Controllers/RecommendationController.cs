using break_back.Services;
using Microsoft.AspNetCore.Mvc;

namespace break_back.Controllers;

[ApiController]
[Route("[controller]")]
public class RecommendationController : ControllerBase
{
    private readonly IRecommendationService _recommendationService;

    public RecommendationController(IRecommendationService recommendationService)
    {
        _recommendationService = recommendationService;
    }
    
    [HttpGet("/{userId}/")]
    public async Task<IActionResult> GetSmartMenu(Guid userId)
    {
        var menu = await _recommendationService.GetAnalyzedMenu(userId);
        return Ok(menu);
    }
}