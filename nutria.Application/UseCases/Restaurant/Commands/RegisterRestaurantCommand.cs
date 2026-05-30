using MediatR;
using Nutria.Domain.Interfaces;

namespace nutria.Application.UseCases.Restaurant.Commands;

public record RegisterRestaurantCommand(
    string Name,
    string Address,
    string Phone
) : IRequest<Guid>;

public class RegisterRestaurantCommandHandler
    : IRequestHandler<RegisterRestaurantCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public RegisterRestaurantCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(
        RegisterRestaurantCommand request,
        CancellationToken cancellationToken)
    {
        var restaurant = new Restaurant
        {
            Name = request.Name,
            Address = request.Address,
            ContactPhone = request.Phone,
            IsActive = true
        };

        _unitOfWork.Repository<Restaurant>().Add(restaurant);

        await _unitOfWork.SaveChanges();

        return restaurant.Id;
    }
}