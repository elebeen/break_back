using MediatR;
using Nutria.Domain.Interfaces.Repositories;
using Nutria.Domain.Models;

namespace nutria.Application.UseCases.Restaurants.Commands;

public record DeactivateRestaurantCommand(Guid RestaurantId) : IRequest<string>;

public class DeactivateRestaurantCommandHandler : IRequestHandler<DeactivateRestaurantCommand, string>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeactivateRestaurantCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<string> Handle(DeactivateRestaurantCommand request, CancellationToken cancellationToken)
    {
        var restaurant = await _unitOfWork.Repository<Restaurant>().FindFirstAsync(r => r.Id == request.RestaurantId);

        if (restaurant == null)
        {
            return "Restaurant not found";
        };

        if (restaurant.IsActive == false)
        {
            return "Restaurant is already inactive";
        };

        restaurant.IsActive = false;
        
        await _unitOfWork.Repository<Restaurant>().UpdateAsync(restaurant);
        
        await _unitOfWork.SaveChanges();

        return "Restaurant deactivated successfully";
    }
}