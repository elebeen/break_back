using MediatR;
using Microsoft.EntityFrameworkCore;
using Nutria.Domain.Dtos.Meal;
using Nutria.Domain.Models;
using Nutria.Infrastructure.Persistence.Context;


namespace nutria.Application.UseCases.Recommendation.Queries;

public abstract record GetAnalyzedMenuQuery(Guid UserId) : IRequest<List<MealWithIndicatorsDto>>;

internal sealed record GetAnalyzedMenuQueryHandler : IRequestHandler<GetAnalyzedMenuQuery, List<MealWithIndicatorsDto>>
{
    private readonly AppdbContext _appdbContext;
    public GetAnalyzedMenuQueryHandler(AppdbContext appdbContext) => _appdbContext = appdbContext;

    public async Task<List<MealWithIndicatorsDto>> Handle(GetAnalyzedMenuQuery request, CancellationToken cancellationToken)
    {
        var user = await _appdbContext.Users
            .AsNoTracking()
            .Include(u => u.HealthProfile)
            .Include(u => u.Conditions)
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user == null) return new List<MealWithIndicatorsDto>();

        var mealData = await _appdbContext.Meals
            .AsNoTracking()
            .Where(m => m.IsActive == true)
            .Select(m => new
            {
                m.Id,
                m.Name,
                m.Price,
                m.RestaurantId,
                RestaurantName = m.Restaurant.Name,
                NutritionalInfo = m.NutritionalInfo != null ? new
                {
                    m.NutritionalInfo.Calories,
                    m.NutritionalInfo.SugarG,
                    m.NutritionalInfo.SodiumMg
                } : null,
                Allergens = m.Ingredients
                    .Where(i => i.IsAllergen)
                    .Select(i => i.Name.ToLower())
                    .ToList()
            }).ToListAsync(cancellationToken);

        var result = new List<MealWithIndicatorsDto>(mealData.Count);
        var userConditions = user.Conditions.Select(c => c.Name.ToLower()).ToList();

        var dailyCalorieTarget = user.HealthProfile?.DailyCalorieTarget ?? 0;
        var dailySugarLimit = user.HealthProfile?.DailySugarLimitG ?? 0;
        var dailySodiumLimit = user.HealthProfile?.DailySodiumLimitMg ?? 0;

        foreach (var m in mealData)
        {
            var dto = new MealWithIndicatorsDto
            {
                Id = m.Id,
                Name = m.Name,
                Price = m.Price,
                RestaurantId = m.RestaurantId,
                RestaurantName = m.RestaurantName,
                SpecificWarnings = new List<string>()
            };

            if (m.NutritionalInfo != null)
            {
                dto.ExceedsCalorieLimit = dailyCalorieTarget > 0 && m.NutritionalInfo.Calories > dailyCalorieTarget;
                dto.ExceedsSugarLimit = dailySugarLimit > 0 && m.NutritionalInfo.SugarG > dailySugarLimit;
                dto.ExceedsSodiumLimit = dailySodiumLimit > 0 && m.NutritionalInfo.SodiumMg > dailySodiumLimit;
            }

            foreach (var condition in userConditions)
            {
                if (m.Allergens.Any(a => condition.Contains(a) || a.Contains(condition)))
                {
                    dto.HasAllergenWarning = true;
                    dto.SpecificWarnings.Add($"Contiene ingredientes relacionados con: {condition}");
                }
            }
            result.Add(dto);
        }

        return result;
    }
}