using MediatR;
using Microsoft.EntityFrameworkCore;
using Nutria.Domain.Models;

namespace nutria.Application.UseCases.Order.Queries;

public record GetOrderByIdQuery(Guid OrderId) : IRequest<Order?>;

public class GetOrderByIdQueryHandler
    : IRequestHandler<GetOrderByIdQuery, Order?>
{
    private readonly AppdbContext _appdbContext;

    public GetOrderByIdQueryHandler(AppdbContext appdbContext)
    {
        _appdbContext = appdbContext;
    }

    public async Task<Order?> Handle(
        GetOrderByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _appdbContext.Orders
            .Include(x => x.OrderItems)
            .FirstOrDefaultAsync(
                x => x.Id == request.OrderId,
                cancellationToken);
    }
}