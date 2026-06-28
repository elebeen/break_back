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
    public async Task<IActionResult> GetAllRestaurants()
    {
        var res = await _mediator.Send(new  GetAllRestaurantsQuery());
        return Ok(res);
    }

    [HttpGet("restaurants/menu")]
    public async Task<IActionResult> GetMenu([FromQuery] GetMenuByRestaurantQuery query)
    {
        var menu = await _mediator.Send(query);
        return Ok(menu);
    }

    [HttpGet("meal")]
    public async Task<IActionResult> GetMeal([FromQuery] GetMealDetailsQuery query)
    {
        var meal = await _mediator.Send(query);
        return Ok(meal);
    }
}