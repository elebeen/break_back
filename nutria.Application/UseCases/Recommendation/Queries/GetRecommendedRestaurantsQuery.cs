using MediatR;
using Microsoft.EntityFrameworkCore;
using Nutria.Domain.Models;

namespace nutria.Application.UseCases.Recommendation.Queries;

public record GetRecommendedRestaurantsQuery(Guid UserId)
    : IRequest<List<Restaurant>>;

public class GetRecommendedRestaurantsQueryHandler
    : IRequestHandler<GetRecommendedRestaurantsQuery,List<Restaurant>>
{
    private readonly AppdbContext _appdbContext;

    public GetRecommendedRestaurantsQueryHandler(AppdbContext appdbContext)
    {
        _appdbContext = appdbContext;
    }

    public async Task<List<Restaurant>> Handle(
        GetRecommendedRestaurantsQuery request,
        CancellationToken cancellationToken)
    {
        return await _appdbContext.Restaurants
            .Where(r => r.IsActive == true)
            .OrderBy(r => r.Name)
            .ToListAsync(cancellationToken);
    }
}