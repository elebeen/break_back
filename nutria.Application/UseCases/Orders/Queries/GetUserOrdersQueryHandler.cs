using MediatR;
using Nutria.Domain.Interfaces;
using Nutria.Domain.Interfaces.Repositories;
using Nutria.Domain.Models;

namespace nutria.Application.UseCases.Orders.Queries;

public record GetUserOrdersQuery(Guid UserId)
    : IRequest<List<Order>>;

public class GetUserOrdersQueryHandler
    : IRequestHandler<GetUserOrdersQuery, List<Order>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUserOrdersQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<Order>> Handle(
        GetUserOrdersQuery request,
        CancellationToken cancellationToken)
    {
        return await _unitOfWork.Orders
            .GetOrdersByUserAsync(request.UserId);
    }
}