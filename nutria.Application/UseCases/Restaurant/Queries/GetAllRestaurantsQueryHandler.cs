using MediatR;
using Nutria.Domain.Interfaces;
using Nutria.Domain.Models;

namespace nutria.Application.UseCases.Restaurant.Queries;

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
        var dtos = await _restaurantRepository.GetActiveRestaurantsAsync();
        
        if (dtos == null) return new List<Nutria.Domain.Models.Restaurant>();

        var result = dtos.Select(dto => new Nutria.Domain.Models.Restaurant
        {

            Id = Guid.NewGuid(), 
            Name = dto.Name,
            Address = dto.Address,
            ContactPhone = dto.ContactPhone, 
            IsActive = dto.IsActive
        }).ToList();

        return result;
    }
}