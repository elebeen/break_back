using MediatR;
using Microsoft.EntityFrameworkCore;
using Nutria.Domain.Models;
using Nutria.Infrastructure.Persistence.Context;

namespace nutria.Application.UseCases.Order.Queries;

public record GetOrderByIdQuery(Guid OrderId) : IRequest<Nutria.Domain.Models.Order?>;

public class GetOrderByIdQueryHandler
    : IRequestHandler<GetOrderByIdQuery, Nutria.Domain.Models.Order?>
{
    private readonly AppdbContext _appdbContext;

    public GetOrderByIdQueryHandler(AppdbContext appdbContext)
    {
        _appdbContext = appdbContext;
    }

    public async Task<Nutria.Domain.Models.Order?> Handle(
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