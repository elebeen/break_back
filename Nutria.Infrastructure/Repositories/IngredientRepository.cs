using Microsoft.EntityFrameworkCore;
using Nutria.Domain.Dtos.Ingredient;
using Nutria.Domain.Interfaces.Repositories;
using Nutria.Domain.Models;
using Nutria.Infrastructure.Persistence.Context;

namespace Nutria.Infrastructure.Repositories;

public class IngredientRepository : Repository<Ingredient>, IIngredientRepository
{
    public IngredientRepository(AppdbContext context) : base(context) { }
    

    public async Task<List<Ingredient>> GetIngredientsByIdsAsync(List<int> ingredientIds)
    {
        return await _context.Ingredients
            .Where(i => ingredientIds.Contains(i.Id))
            .ToListAsync();
    }
}