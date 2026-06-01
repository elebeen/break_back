using MediatR;
using Microsoft.EntityFrameworkCore;
using Nutria.Domain.Dtos.Ingredient;
using Nutria.Domain.Dtos.Meal;
using Nutria.Domain.Dtos.NutritionalInfo;
using Nutria.Domain.Models;
using Nutria.Infrastructure.Persistence.Context;

namespace nutria.Application.UseCases.Catalog.Queries;

public record GetMealDetailsQuery(Guid MealId) : IRequest<MealDetailsDto?>;

internal sealed record GetMealDetailsQueryHandler : IRequestHandler<GetMealDetailsQuery, MealDetailsDto?>
{
    private readonly AppdbContext _appdbContext;
    public GetMealDetailsQueryHandler(AppdbContext appdbContext) => _appdbContext = appdbContext;

    public async Task<MealDetailsDto?> Handle(GetMealDetailsQuery request, CancellationToken cancellationToken)
    {
        return await _appdbContext.Meals
            .AsNoTracking()
            .Where(m => m.Id == request.MealId)
            .Select(m => new MealDetailsDto
            {
                Id = m.Id,
                RestaurantId = m.RestaurantId,
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
                } : null,
                Ingredients = m.Ingredients.Select(i => new IngredientDto
                {
                    Id = i.Id,
                    Name = i.Name,
                    IsAllergen = i.IsAllergen
                }).ToList()
            }).FirstOrDefaultAsync(cancellationToken);
    }
}