using MediatR;
using Nutria.Domain.Dtos.Restaurant;
using Nutria.Domain.Interfaces.Repositories;

namespace nutria.Application.UseCases.Restaurants.Queries;

public record GetAllRestaurantsQuery() : IRequest<List<RestaurantDto>>;

public class GetAllRestaurantsQueryHandler : IRequestHandler<GetAllRestaurantsQuery, List<RestaurantDto>>
{
    private readonly IRestaurantRepository _restaurantRepository;

    public GetAllRestaurantsQueryHandler(IRestaurantRepository restaurantRepository)
    {
        _restaurantRepository = restaurantRepository;
    }

    public async Task<List<RestaurantDto>> Handle(GetAllRestaurantsQuery request, CancellationToken cancellationToken)
    {

        var result = await _restaurantRepository.GetActiveRestaurantsAsync();
        return result;
    }
}