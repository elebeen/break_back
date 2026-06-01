using MediatR;
using Nutria.Domain.Interfaces;


namespace nutria.Application.UseCases.Restaurant.Commands;

// 1. El comando solo necesita el ID del restaurante que queremos apagar
public record DeactivateRestaurantCommand(Guid RestaurantId) : IRequest<bool>;

// 2. El Handler que busca el restaurante y cambia su estado a inactivo
public class DeactivateRestaurantCommandHandler : IRequestHandler<DeactivateRestaurantCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeactivateRestaurantCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeactivateRestaurantCommand request, CancellationToken cancellationToken)
    {
        var restaurant = await _unitOfWork.Repository<Nutria.Domain.Models.Restaurant>()
            .FindFirstAsync(x => x.Id == request.RestaurantId);
        
        if (restaurant == null) return false;

        restaurant.IsActive = false;

        await _unitOfWork.Repository<Nutria.Domain.Models.Restaurant>().Update(restaurant);
        
        await _unitOfWork.SaveChanges();

        return true;
    }
}