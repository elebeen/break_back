using MediatR;
using Nutria.Domain.Interfaces;

namespace nutria.Application.UseCases.Restaurant.Queries;

// Cambiamos el tipo de retorno a List<Nutria.Domain.Models.Restaurant> para que coincida exactamente con la interfaz
public record GetAllRestaurantsQuery() : IRequest<List<Nutria.Domain.Models.Restaurant>>;

public class GetAllRestaurantsQueryHandler : IRequestHandler<GetAllRestaurantsQuery, List<Nutria.Domain.Models.Restaurant>>
{
    private readonly IRestaurantRepository _restaurantRepository;

    public GetAllRestaurantsQueryHandler(IRestaurantRepository restaurantRepository)
    {
        _restaurantRepository = restaurantRepository;
    }

    public async Task<List<Nutria.Domain.Models.Restaurant>> Handle(GetAllRestaurantsQuery request, CancellationToken cancellationToken)
    {

        var result = await _restaurantRepository.GetActiveRestaurantsAsync();
        return result;
    }
}