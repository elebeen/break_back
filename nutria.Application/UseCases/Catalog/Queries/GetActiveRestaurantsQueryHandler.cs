using MediatR;
using Nutria.Domain.Dtos.Restaurant;
using Nutria.Domain.Interfaces;
using Nutria.Domain.Interfaces.Repositories;

namespace nutria.Application.UseCases.Catalog.Queries;

public record GetActiveRestaurantsQuery : IRequest<IEnumerable<RestaurantDto>>;

internal sealed record GetActiveRestaurantsQueryHandler : IRequestHandler<GetActiveRestaurantsQuery, IEnumerable<RestaurantDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetActiveRestaurantsQueryHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<IEnumerable<RestaurantDto>> Handle(GetActiveRestaurantsQuery request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Restaurants.GetActiveRestaurantsAsync();
    }
}