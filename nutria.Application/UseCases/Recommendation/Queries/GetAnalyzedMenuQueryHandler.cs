using MediatR;
using Nutria.Domain.Dtos.Meal;
using Nutria.Domain.Interfaces;
using Nutria.Domain.Interfaces.Repositories;

namespace nutria.Application.UseCases.Recommendation.Queries;

public record GetAnalyzedMenuQuery(Guid UserId) : IRequest<List<MealWithIndicatorsDto>>;

internal sealed record GetAnalyzedMenuQueryHandler : IRequestHandler<GetAnalyzedMenuQuery, List<MealWithIndicatorsDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAnalyzedMenuQueryHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<List<MealWithIndicatorsDto>> Handle(GetAnalyzedMenuQuery request, CancellationToken cancellationToken)
    {
       var res = await _unitOfWork.Meals.GetMealsByUserId(request.UserId);

       if (res == null)
           throw new ArgumentException("No related meals found");
       
       return res;
    }
}