using MediatR;
using Microsoft.EntityFrameworkCore;
using Nutria.Domain.Models;

namespace nutria.Application.UseCases.Catalog.Queries;

public record SearchMealsQuery(string Name)
    : IRequest<List<Meal>>;

public class SearchMealsQueryHandler
    : IRequestHandler<SearchMealsQuery,List<Meal>>
{
    private readonly Context _context;

    public SearchMealsQueryHandler(Context context)
    {
        _context = context;
    }

    public async Task<List<Meal>> Handle(
        SearchMealsQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Meals
            .AsNoTracking()
            .Where(x => x.Name.ToLower()
                .Contains(request.Name.ToLower()))
            .ToListAsync(cancellationToken);
    }
}