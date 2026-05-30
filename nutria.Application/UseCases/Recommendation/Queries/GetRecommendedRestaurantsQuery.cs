using MediatR;
using Microsoft.EntityFrameworkCore;
using Nutria.Domain.Models;

namespace nutria.Application.UseCases.Recommendation.Queries;

public record GetRecommendedRestaurantsQuery(Guid UserId)
    : IRequest<List<Restaurant>>;

public class GetRecommendedRestaurantsQueryHandler
    : IRequestHandler<GetRecommendedRestaurantsQuery,List<Restaurant>>
{
    private readonly Context _context;

    public GetRecommendedRestaurantsQueryHandler(Context context)
    {
        _context = context;
    }

    public async Task<List<Restaurant>> Handle(
        GetRecommendedRestaurantsQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Restaurants
            .Where(r => r.IsActive == true)
            .OrderBy(r => r.Name)
            .ToListAsync(cancellationToken);
    }
}