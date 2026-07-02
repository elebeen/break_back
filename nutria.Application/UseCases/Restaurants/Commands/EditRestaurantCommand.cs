using MediatR;
using Nutria.Domain.Dtos.Restaurant;
using Nutria.Domain.Interfaces.Repositories;
using Nutria.Domain.Models;

namespace nutria.Application.UseCases.Restaurants.Commands;

public record EditRestaurantCommand(RestaurantDto Restaurant) : IRequest<string>;

internal sealed record EditRestaurantCommandHandler : IRequestHandler<EditRestaurantCommand, string>
{
    private readonly IUnitOfWork _unitOfWork;

    public EditRestaurantCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<string> Handle(EditRestaurantCommand request, CancellationToken cancellationToken)
    {
        var res = await _unitOfWork.Repository<Restaurant>().FindFirstAsync(u => u.Id == request.Restaurant.Id);

        if (res == null)
        {
            throw new ArgumentException("User not found");
        }
        
        res.Name = request.Restaurant.Name;
        res.ContactPhone = request.Restaurant.ContactPhone;
        res.Address = request.Restaurant.Address;
        res.IsActive = request.Restaurant.IsActive;
        
        await _unitOfWork.Repository<Restaurant>().UpdateAsync(res);

        await _unitOfWork.SaveChanges();
        
        return "Restaurant updated successfully";
    }
}

