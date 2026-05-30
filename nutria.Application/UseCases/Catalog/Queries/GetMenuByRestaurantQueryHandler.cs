using MediatR;
using Microsoft.EntityFrameworkCore;
using Nutria.Domain.Dtos.Meal;
using Nutria.Domain.Dtos.NutritionalInfo;
using Nutria.Domain.Models;

namespace nutria.Application.UseCases.Catalog.Queries;

public record GetMenuByRestaurantQuery(Guid RestaurantId) : IRequest<IEnumerable<MealDto>>;

public class GetMenuByRestaurantQueryHandler : IRequestHandler<GetMenuByRestaurantQuery, IEnumerable<MealDto>>
{
    private readonly Context _context;
    public GetMenuByRestaurantQueryHandler(Context context) => _context = context;

    public async Task<IEnumerable<MealDto>> Handle(GetMenuByRestaurantQuery request, CancellationToken cancellationToken)
    {
        return await _context.Meals
            .AsNoTracking()
            .Where(m => m.RestaurantId == request.RestaurantId && m.IsActive == true)
            .Select(m => new MealDto
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                Price = m.Price,
                ImageUrl = m.ImageUrl,
                IsActive = m.IsActive,
                NutritionalInfo = m.NutritionalInfo != null ? new NutritionalInfoDto
                {
                    Calories = m.NutritionalInfo.Calories,
                    ProteinG = m.NutritionalInfo.ProteinG,
                    CarbsG = m.NutritionalInfo.CarbsG,
                    FatsG = m.NutritionalInfo.FatsG,
                    SodiumMg = m.NutritionalInfo.SodiumMg,
                    SugarG = m.NutritionalInfo.SugarG,
                    FiberG = m.NutritionalInfo.FiberG
                } : null
            }).ToListAsync(cancellationToken);
    }
}