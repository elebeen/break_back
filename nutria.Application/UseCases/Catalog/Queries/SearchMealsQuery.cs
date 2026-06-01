using MediatR;
using Microsoft.EntityFrameworkCore;
using Nutria.Domain.Models;
using Nutria.Infrastructure.Persistence.Context;

namespace nutria.Application.UseCases.Catalog.Queries;

public record SearchMealsQuery(string Name)
    : IRequest<List<Meal>>;

public class SearchMealsQueryHandler
    : IRequestHandler<SearchMealsQuery,List<Meal>>
{
    private readonly AppdbContext _appdbContext;

    public SearchMealsQueryHandler(AppdbContext appdbContext)
    {
        _appdbContext = appdbContext;
    }

    public async Task<List<Meal>> Handle(
        SearchMealsQuery request,
        CancellationToken cancellationToken)
    {
        return await _appdbContext.Meals
            .AsNoTracking()
            .Where(x => x.Name.ToLower()
                .Contains(request.Name.ToLower()))
            .ToListAsync(cancellationToken);
    }
}