using MediatR;
using Nutria.Domain.Interfaces;

namespace nutria.Application.UseCases.Order.Commands;

public record CancelOrderCommand(Guid OrderId) : IRequest<bool>;

public class CancelOrderCommandHandler
    : IRequestHandler<CancelOrderCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public CancelOrderCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(
        CancelOrderCommand request,
        CancellationToken cancellationToken)
    {
        var order = _unitOfWork.Repository<Order>()
            .FindbyGuid(request.OrderId);

        if (order == null)
            return false;

        order.OrderStatus = "Cancelado";

        await _unitOfWork.SaveChanges();

        return true;
    }
}