using Microsoft.AspNetCore.Mvc;
using MediatR;
using nutria.Application.UseCases.Catalog.Queries;
using nutria.Application.UseCases.Restaurants.Queries;

namespace break_back.Controllers;

[ApiController]
[Route("[controller]")]
public class CatalogController : ControllerBase
{
    private readonly IMediator _mediator;

    public CatalogController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("restaurants")]
    public async Task<IActionResult> GetAllRestaurants(GetAllRestaurantsQuery query)
    {
        var res = await _mediator.Send(query);
        return Ok(res);
    }

    [HttpGet("restaurants/menu")]
    public async Task<IActionResult> GetMenu(GetMenuByRestaurantQuery query)
    {
        var menu = await _mediator.Send(query);
        return Ok(menu);
    }

    [HttpGet("meals/{query}")]
    public async Task<IActionResult> GetMeal(GetMealDetailsQuery query)
    {
        var meal = await _mediator.Send(query);
        return Ok(meal);
    }
}