using MediatR;
using Microsoft.EntityFrameworkCore;
using Nutria.Domain.Models;
using Nutria.Infrastructure.Persistence.Context;


namespace nutria.Application.UseCases.Recommendation.Queries;

public record GetRecommendedRestaurantsQuery(Guid UserId)
    : IRequest<List<Nutria.Domain.Models.Restaurant>>;

public class GetRecommendedRestaurantsQueryHandler
    : IRequestHandler<GetRecommendedRestaurantsQuery,List<Nutria.Domain.Models.Restaurant>>
{
    private readonly AppdbContext _appdbContext;

    public GetRecommendedRestaurantsQueryHandler(AppdbContext appdbContext)
    {
        _appdbContext = appdbContext;
    }

    public async Task<List<Nutria.Domain.Models.Restaurant>> Handle(
        GetRecommendedRestaurantsQuery request,
        CancellationToken cancellationToken)
    {
        return await _appdbContext.Restaurants
            .Where(r => r.IsActive == true)
            .OrderBy(r => r.Name)
            .ToListAsync(cancellationToken);
    }
}