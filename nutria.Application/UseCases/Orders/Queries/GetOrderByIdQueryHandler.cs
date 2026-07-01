using MediatR;
using Nutria.Domain.Dtos.Order;
using Nutria.Domain.Interfaces.Repositories;

namespace nutria.Application.UseCases.Orders.Queries;

public record GetOrderByIdQuery(Guid OrderId) : IRequest<OrderResponse?>;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderResponse?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetOrderByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OrderResponse?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Orders.GetOrderDetailsByIdAsync(request.OrderId);
    }
}