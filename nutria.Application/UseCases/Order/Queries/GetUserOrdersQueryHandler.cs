using MediatR;
using Microsoft.EntityFrameworkCore;
using Nutria.Domain.Models;

namespace nutria.Application.UseCases.Order.Queries;

public record GetUserOrdersQuery(Guid UserId) : IRequest<List<Order>>;

public class GetUserOrdersQueryHandler
    : IRequestHandler<GetUserOrdersQuery, List<Order>>
{
    private readonly Context _context;

    public GetUserOrdersQueryHandler(Context context)
    {
        _context = context;
    }

    public async Task<List<Order>> Handle(
        GetUserOrdersQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Orders
            .AsNoTracking()
            .Where(x => x.UserId == request.UserId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}