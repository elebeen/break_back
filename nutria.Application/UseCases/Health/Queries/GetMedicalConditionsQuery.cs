using MediatR;
using Microsoft.EntityFrameworkCore;
using Nutria.Domain.Models;

namespace nutria.Application.UseCases.Health.Queries;

public record GetMedicalConditionsQuery()
    : IRequest<List<MedicalCondition>>;

public class GetMedicalConditionsQueryHandler
    : IRequestHandler<GetMedicalConditionsQuery,List<MedicalCondition>>
{
    private readonly Context _context;

    public GetMedicalConditionsQueryHandler(Context context)
    {
        _context = context;
    }

    public async Task<List<MedicalCondition>> Handle(
        GetMedicalConditionsQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.MedicalConditions
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}