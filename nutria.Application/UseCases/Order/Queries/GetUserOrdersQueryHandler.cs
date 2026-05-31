using MediatR;
using Microsoft.EntityFrameworkCore;
using Nutria.Domain.Models;

namespace nutria.Application.UseCases.Order.Queries;

public record GetUserOrdersQuery(Guid UserId) : IRequest<List<Order>>;

public class GetUserOrdersQueryHandler
    : IRequestHandler<GetUserOrdersQuery, List<Order>>
{
    private readonly AppdbContext _appdbContext;

    public GetUserOrdersQueryHandler(AppdbContext appdbContext)
    {
        _appdbContext = appdbContext;
    }

    public async Task<List<Order>> Handle(
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