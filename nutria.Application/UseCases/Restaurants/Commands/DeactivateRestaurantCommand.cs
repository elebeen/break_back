using MediatR;
using Nutria.Domain.Interfaces;
using Nutria.Domain.Models;

namespace nutria.Application.UseCases.Restaurants.Commands;

public record DeactivateRestaurantCommand(Guid RestaurantId) : IRequest<bool>;

public class DeactivateRestaurantCommandHandler : IRequestHandler<DeactivateRestaurantCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeactivateRestaurantCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeactivateRestaurantCommand request, CancellationToken cancellationToken)
    {
        var restaurant = await _unitOfWork.Repository<Restaurant>().FindFirstAsync(r => r.Id == request.RestaurantId);
        
        if (restaurant == null) return false;

        restaurant.IsActive = false;
        
        await _unitOfWork.Repository<Restaurant>().UpdateAsync(restaurant);
        
        await _unitOfWork.SaveChanges();

        return true;
    }
}