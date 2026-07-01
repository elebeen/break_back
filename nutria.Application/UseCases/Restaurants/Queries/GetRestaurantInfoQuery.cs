using MediatR;
using Nutria.Domain.Dtos.Restaurant;
using Nutria.Domain.Interfaces.Repositories;
using Nutria.Domain.Models;

namespace nutria.Application.UseCases.Restaurants.Queries;

public record GetRestaurantInfoQuery(Guid RestaurantId) : IRequest<RestaurantDto>;

internal sealed record GetRestaurantInfoQueryHandler : IRequestHandler<GetRestaurantInfoQuery, RestaurantDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetRestaurantInfoQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<RestaurantDto> Handle(GetRestaurantInfoQuery request, CancellationToken cancellationToken)
    {
        var res = await _unitOfWork.Repository<Restaurant>().FindFirstAsync(u => u.Id == request.RestaurantId);

        if (res == null)
        {
            throw new ArgumentException("Restaurant not found");
        }
        
        var info = await _unitOfWork.Restaurants.GetRestaurantByIdAsync(request.RestaurantId);
        
        return info;
    }
}

