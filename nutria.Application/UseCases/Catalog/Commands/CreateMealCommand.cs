using MediatR;
using Nutria.Domain.Interfaces.Repositories;
using Nutria.Domain.Models;

namespace nutria.Application.UseCases.Catalog.Commands;

public record CreateMealCommand(
    string Name,
    string Description,
    decimal Price,
    Guid RestaurantId,
    int Calories,
    int SodiumMg,
    int SugarG
) : IRequest<bool>;

public class CreateMealCommandHandler : IRequestHandler<CreateMealCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateMealCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(CreateMealCommand request, CancellationToken cancellationToken)
    {
        var newMeal = new Meal
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            RestaurantId = request.RestaurantId,
            NutritionalInfo = new NutritionalInfo
            {
                Id = Guid.NewGuid(),
                Calories = request.Calories,
                SodiumMg = request.SodiumMg,
                SugarG = request.SugarG
            }
        };

        await _unitOfWork.Repository<Meal>().AddAsync(newMeal);
        
        await _unitOfWork.SaveChanges();

        return true;
    }
}