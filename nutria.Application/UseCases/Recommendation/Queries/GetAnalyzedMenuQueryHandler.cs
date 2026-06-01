using MediatR;
using Nutria.Domain.Dtos.Meal;
using Nutria.Domain.Interfaces;

namespace nutria.Application.UseCases.Recommendation.Queries;

public abstract record GetAnalyzedMenuQuery(Guid UserId) : IRequest<List<MealWithIndicatorsDto>>;

internal sealed record GetAnalyzedMenuQueryHandler : IRequestHandler<GetAnalyzedMenuQuery, List<MealWithIndicatorsDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAnalyzedMenuQueryHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<List<MealWithIndicatorsDto>> Handle(GetAnalyzedMenuQuery request, CancellationToken cancellationToken)
    {
       return await _unitOfWork.Meals.GetCompatibleMealsAsync(request.UserId);
    }
}