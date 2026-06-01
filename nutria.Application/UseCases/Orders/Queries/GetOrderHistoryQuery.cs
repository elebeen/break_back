using MediatR;
using Nutria.Domain.Interfaces;
using Nutria.Domain.Models;

namespace nutria.Application.UseCases.Orders.Queries;

public record GetOrderHistoryQuery(Guid UserId) : IRequest<IEnumerable<Order>>;

public class GetOrderHistoryQueryHandler
    : IRequestHandler<GetOrderHistoryQuery, IEnumerable<Order>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetOrderHistoryQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task<IEnumerable<Order>> Handle(
        GetOrderHistoryQuery request,
        CancellationToken cancellationToken)
    {
        var orders = _unitOfWork
            .Repository<Order>()
            .Query()
            .Where(o => o.UserId == request.UserId)
            .OrderByDescending(o => o.CreatedAt)
            .AsEnumerable();

        return Task.FromResult(orders);
    }
}