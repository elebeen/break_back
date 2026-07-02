using Microsoft.EntityFrameworkCore;
using Nutria.Domain.Dtos.Ingredient;
using Nutria.Domain.Interfaces.Repositories;
using Nutria.Domain.Models;
using Nutria.Infrastructure.Persistence.Context;

namespace Nutria.Infrastructure.Repositories;

public class IngredientRepository : Repository<Ingredient>, IIngredientRepository
{
    public IngredientRepository(AppdbContext context) : base(context) { }
    

    public async Task<List<IngredientDto>> GetIngredientsByIdsAsync(List<int> ingredientIds)
    {
        return await _context.Ingredients
            .AsNoTracking()
            .Where(i => ingredientIds.Contains(i.Id))
            .Select(i => new IngredientDto
            {
                Id = i.Id,
                Name = i.Name,
                IsAllergen = i.IsAllergen
            })
            .ToListAsync();
    }
}