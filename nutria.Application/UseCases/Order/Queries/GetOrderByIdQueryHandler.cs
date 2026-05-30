using MediatR;
using Microsoft.EntityFrameworkCore;
using Nutria.Domain.Models;

namespace nutria.Application.UseCases.Order.Queries;

public record GetOrderByIdQuery(Guid OrderId) : IRequest<Order?>;

public class GetOrderByIdQueryHandler
    : IRequestHandler<GetOrderByIdQuery, Order?>
{
    private readonly Context _context;

    public GetOrderByIdQueryHandler(Context context)
    {
        _context = context;
    }

    public async Task<Order?> Handle(
        GetOrderByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Orders
            .Include(x => x.OrderItems)
            .FirstOrDefaultAsync(
                x => x.Id == request.OrderId,
                cancellationToken);
    }
}