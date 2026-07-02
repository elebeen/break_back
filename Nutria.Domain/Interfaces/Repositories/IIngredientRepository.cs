using Nutria.Domain.Dtos.Ingredient;
using Nutria.Domain.Models;

namespace Nutria.Domain.Interfaces.Repositories;

public interface IIngredientRepository : IRepository<Ingredient>
{
    Task<List<IngredientDto>> GetIngredientsByIdsAsync(List<int> ingredientIds);
}