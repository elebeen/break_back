using Microsoft.AspNetCore.Mvc;
using break_back.Services;

namespace break_back.Controllers;

[ApiController]
[Route("[controller]")]
public class CatalogController : ControllerBase
{
    private readonly ICatalogService _catalogService;

    public CatalogController(ICatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    [HttpGet("restaurants")]
    public async Task<IActionResult> GetRestaurants()
    {
        var restaurants = await _catalogService.GetActiveRestaurants();
        return Ok(restaurants);
    }

    [HttpGet("restaurants/{id}/menu")]
    public async Task<IActionResult> GetMenu(Guid id)
    {
        var menu = await _catalogService.GetMenuByRestaurant(id);
        return Ok(menu);
    }

    [HttpGet("meals/{id}")]
    public async Task<IActionResult> GetMeal(Guid id)
    {
        var meal = await _catalogService.GetMealDetails(id);
        if (meal == null) return NotFound();
        return Ok(meal);
    }
}