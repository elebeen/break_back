using MediatR;
using Nutria.Domain.Dtos.Restaurant;
using Nutria.Domain.Interfaces.Repositories;
using Nutria.Domain.Models;

namespace nutria.Application.UseCases.Restaurants.Commands;

public record RegisterRestaurantCommand(RestaurantDto RestaurantDto) : IRequest<string>;

public class RegisterRestaurantCommandHandler : IRequestHandler<RegisterRestaurantCommand, string>
{
    private readonly IUnitOfWork _unitOfWork;

    public RegisterRestaurantCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<string> Handle(RegisterRestaurantCommand request, CancellationToken cancellationToken)
    {
        var existingRes = _unitOfWork.Repository<Restaurant>().FindFirstAsync(u => u.Name == request.RestaurantDto.Name);

        if (existingRes != null)
        {
            throw new ArgumentException("Restaurant already exists");
        }
        
        var restaurant = new Restaurant
        {
            Name = request.RestaurantDto.Name,
            Address = request.RestaurantDto.Address,
            ContactPhone = request.RestaurantDto.ContactPhone,
            IsActive = true
        };

        await _unitOfWork.Repository<Restaurant>().AddAsync(restaurant);
        
        await _unitOfWork.SaveChanges();

        return "Restaurant registered successfully";
    }
}