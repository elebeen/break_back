using MediatR;
using Microsoft.EntityFrameworkCore;
using Nutria.Domain.Dtos.Restaurant;
using Nutria.Domain.Models;

namespace nutria.Application.UseCases.Catalog.Queries;

public record GetActiveRestaurantsQuery : IRequest<IEnumerable<RestaurantDto>>;

internal sealed record GetActiveRestaurantsQueryHandler : IRequestHandler<GetActiveRestaurantsQuery, IEnumerable<RestaurantDto>>
{
    private readonly Context _context;

    public GetActiveRestaurantsQueryHandler(Context context) => _context = context;

    public async Task<IEnumerable<RestaurantDto>> Handle(GetActiveRestaurantsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Restaurants
            .AsNoTracking()
            .Where(r => r.IsActive == true)
            .Select(r => new RestaurantDto
            {
                Name = r.Name,
                Address = r.Address,
                ContactPhone = r.ContactPhone,
                IsActive = r.IsActive
            }).ToListAsync(cancellationToken);
    }
}