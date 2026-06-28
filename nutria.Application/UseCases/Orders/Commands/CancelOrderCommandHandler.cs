using MediatR;
using Nutria.Domain.Interfaces;
using Nutria.Domain.Interfaces.Repositories;
using Nutria.Domain.Models;

namespace nutria.Application.UseCases.Orders.Commands;

public record CancelOrderCommand(Guid OrderId)
    : IRequest<bool>;

public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public CancelOrderCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _unitOfWork.Repository<Order>().FindFirstAsync(u =>  u.Id == request.OrderId);

        if (order is null)
            return false;

        order.OrderStatus = "Cancelado";

        await _unitOfWork.Orders.UpdateAsync(order);

        await _unitOfWork.SaveChanges();

        return true;
    }
}