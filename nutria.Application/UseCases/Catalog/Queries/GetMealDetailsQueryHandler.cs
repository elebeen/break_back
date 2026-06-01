using MediatR;
using Nutria.Domain.Dtos.Meal;
using Nutria.Domain.Interfaces;

namespace nutria.Application.UseCases.Catalog.Queries;

public record GetMealDetailsQuery(Guid MealId) : IRequest<MealDetailsDto?>;

internal sealed record GetMealDetailsQueryHandler : IRequestHandler<GetMealDetailsQuery, MealDetailsDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetMealDetailsQueryHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<MealDetailsDto?> Handle(GetMealDetailsQuery request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Meals.GetMealDetails(request.MealId);
    }
}