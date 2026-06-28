using MediatR;
using Microsoft.AspNetCore.Mvc;
using nutria.Application.UseCases.Recommendation.Queries;

namespace break_back.Controllers;

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
}