using MediatR;
using Microsoft.EntityFrameworkCore;
using Nutria.Domain.Dtos.MedicalCondition;
using Nutria.Domain.Interfaces;
using Nutria.Domain.Models;
using Nutria.Infrastructure.Persistence.Context;

namespace nutria.Application.UseCases.Health.Queries;

public record GetMedicalConditionsQuery(Guid UserId) : IRequest<List<MedicalConditionGetDto>>;

public class GetMedicalConditionsQueryHandler : IRequestHandler<GetMedicalConditionsQuery,List<MedicalConditionGetDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetMedicalConditionsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<MedicalConditionGetDto>> Handle(GetMedicalConditionsQuery request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.MedicalConditions.GetConditionsByUserId(request.UserId);
    }
}