using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nutria.Application.UseCases.Meals.Queries;
using nutria.Application.UseCases.Recommendation.Queries;

namespace break_back.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class RecommendationController : ControllerBase
{
    private readonly IMediator _mediator;

    public RecommendationController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetSmartMenu([FromQuery] GetAnalyzedMenuQuery query)
    {
        var menu = await _mediator.Send(query);
        return Ok(menu);
    }
    
    [HttpGet("meals")]
    public async Task<IActionResult> GetMeals(GetAllMealsQuery query)
    {
        var meals = await _mediator.Send(query);
        return Ok(meals);
    }
}