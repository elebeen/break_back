using MediatR;
using Nutria.Domain.Interfaces;
using Nutria.Domain.Interfaces.Repositories;
using Nutria.Domain.Models;

namespace nutria.Application.UseCases.Orders.Queries;

public record GetOrderByIdQuery(Guid OrderId)
    : IRequest<Order?>;

public class GetOrderByIdQueryHandler
    : IRequestHandler<GetOrderByIdQuery, Order?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetOrderByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Order?> Handle(
        GetOrderByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _unitOfWork.Orders
            .GetOrderDetailsAsync(request.OrderId);
    }
}