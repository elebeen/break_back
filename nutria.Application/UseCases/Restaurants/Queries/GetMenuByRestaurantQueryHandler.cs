using MediatR;
using Nutria.Domain.Dtos.Meal;
using Nutria.Domain.Interfaces.Repositories;
using Nutria.Domain.Models;

namespace nutria.Application.UseCases.Restaurants.Queries;

public record GetMenuByRestaurantQuery(Guid RestaurantId) : IRequest<IEnumerable<MealDto>>;

internal sealed record GetMenuByRestaurantQueryHandler : IRequestHandler<GetMenuByRestaurantQuery, IEnumerable<MealDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetMenuByRestaurantQueryHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<IEnumerable<MealDto>> Handle(GetMenuByRestaurantQuery request, CancellationToken cancellationToken)
    {
        var res = await _unitOfWork.Repository<Restaurant>().FindFirstAsync(r => r.IsActive == false);

        var restaurant = await _unitOfWork.Repository<Restaurant>()
            .FindFirstAsync(r => r.Id == request.RestaurantId);

        if (restaurant == null)
        {
            throw new ArgumentException("Restaurant not found.");
        }

        // 2. Si el restaurante está desactivado, lanzamos la excepción con el string
        if (restaurant.IsActive == false)
        {
            throw new ArgumentException("Restaurant is deactivated.");
        }
        
        return await _unitOfWork.Meals.GetMealsByRestaurantAsync(request.RestaurantId);
    }
}