using MediatR;
using Microsoft.EntityFrameworkCore;
using Nutria.Domain.Models;
using Nutria.Infrastructure.Persistence.Context;

namespace nutria.Application.UseCases.Order.Queries;

public record GetUserOrdersQuery(Guid UserId) : IRequest<List<Nutria.Domain.Models.Order>>;

public class GetUserOrdersQueryHandler
    : IRequestHandler<GetUserOrdersQuery, List<Nutria.Domain.Models.Order>>
{
    private readonly AppdbContext _appdbContext;

    public GetUserOrdersQueryHandler(AppdbContext appdbContext)
    {
        _appdbContext = appdbContext;
    }

    public async Task<List<Nutria.Domain.Models.Order>> Handle(
        GetUserOrdersQuery request,
        CancellationToken cancellationToken)
    {
        return await _appdbContext.Orders
            .AsNoTracking()
            .Where(x => x.UserId == request.UserId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}