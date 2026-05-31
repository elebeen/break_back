using MediatR;
using Microsoft.EntityFrameworkCore;
using Nutria.Domain.Dtos.Restaurant;
using Nutria.Domain.Models;

namespace nutria.Application.UseCases.Catalog.Queries;

public record GetActiveRestaurantsQuery : IRequest<IEnumerable<RestaurantDto>>;

internal sealed record GetActiveRestaurantsQueryHandler : IRequestHandler<GetActiveRestaurantsQuery, IEnumerable<RestaurantDto>>
{
    private readonly AppdbContext _appdbContext;

    public GetActiveRestaurantsQueryHandler(AppdbContext appdbContext) => _appdbContext = appdbContext;

    public async Task<IEnumerable<RestaurantDto>> Handle(GetActiveRestaurantsQuery request, CancellationToken cancellationToken)
    {
        return await _appdbContext.Restaurants
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