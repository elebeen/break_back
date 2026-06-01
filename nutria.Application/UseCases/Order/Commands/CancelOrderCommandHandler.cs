using MediatR;
using Nutria.Domain.Interfaces;

namespace nutria.Application.UseCases.Order.Commands;

public record CancelOrderCommand(Guid OrderId)
    : IRequest<bool>;

public class CancelOrderCommandHandler
    : IRequestHandler<CancelOrderCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public CancelOrderCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(request.OrderId);

        if (order is null)
            return false;

        order.OrderStatus = "Cancelado";

        _unitOfWork.Orders.Update(order);

        await _unitOfWork.SaveChanges();

        return true;
    }
}