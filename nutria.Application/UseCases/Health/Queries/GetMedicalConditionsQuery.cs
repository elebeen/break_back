using MediatR;
using Microsoft.EntityFrameworkCore;
using Nutria.Domain.Models;

namespace nutria.Application.UseCases.Health.Queries;

public record GetMedicalConditionsQuery()
    : IRequest<List<MedicalCondition>>;

public class GetMedicalConditionsQueryHandler
    : IRequestHandler<GetMedicalConditionsQuery,List<MedicalCondition>>
{
    private readonly AppdbContext _appdbContext;

    public GetMedicalConditionsQueryHandler(AppdbContext appdbContext)
    {
        _appdbContext = appdbContext;
    }

    public async Task<List<MedicalCondition>> Handle(
        GetMedicalConditionsQuery request,
        CancellationToken cancellationToken)
    {
        return await _appdbContext.MedicalConditions
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}