using Nutria.Domain.Dtos.Meal;

namespace break_back.Services;

public interface IRecommendationService
{ 
    Task<List<MealWithIndicatorsDto>> GetAnalyzedMenu(Guid userId);
}