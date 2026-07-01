using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using nutria.Application.UseCases.Catalog.Queries;
using nutria.Application.UseCases.Restaurants.Queries;

namespace break_back.Controllers;

[Authorize]
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
    
    [HttpGet("meals")]
    public async Task<IActionResult> GetMeals(GetAllMealsQuery query)
    {
        var meals = await _mediator.Send(query);
        return Ok(meals);
    }
}