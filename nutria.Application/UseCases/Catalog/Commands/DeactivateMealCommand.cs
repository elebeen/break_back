using MediatR;
using Nutria.Domain.Interfaces;
using Nutria.Domain.Models;

namespace nutria.Application.UseCases.Catalog.Commands;

public record DeactivateMealCommand(Guid MealId) : IRequest<bool>;

public class DeactivateMealCommandHandler : IRequestHandler<DeactivateMealCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeactivateMealCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeactivateMealCommand request, CancellationToken cancellationToken)
    {
        // Buscamos el plato en el repositorio genérico usando su ID
        var meal = await _unitOfWork.Repository<Meal>().GetByIdAsync(request.MealId);
        
        if (meal == null) return false;

        _unitOfWork.Repository<Meal>().Update(meal);
        
        await _unitOfWork.SaveChanges();

        return true;
    }
}