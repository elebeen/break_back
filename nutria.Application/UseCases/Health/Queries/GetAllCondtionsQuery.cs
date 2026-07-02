using MediatR;
using Nutria.Domain.Dtos.MedicalCondition;
using Nutria.Domain.Interfaces.Repositories;

namespace nutria.Application.UseCases.Health.Queries;

public record GetAllCondtionsQuery() : IRequest<List<MedicalConditionGetDto>>;

internal sealed class GetAllCondtionsQueryHandler : IRequestHandler<GetAllCondtionsQuery, List<MedicalConditionGetDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllCondtionsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<List<MedicalConditionGetDto>> Handle(GetAllCondtionsQuery request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.MedicalConditions.GetAllConditions();
    }
}

