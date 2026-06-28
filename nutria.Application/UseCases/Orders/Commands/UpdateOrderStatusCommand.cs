using MediatR;
using Nutria.Domain.Interfaces;
using Nutria.Domain.Interfaces.Repositories;
using Nutria.Domain.Models;

namespace nutria.Application.UseCases.Orders.Commands;

public record UpdateOrderStatusCommand(
    Guid OrderId,
    string Status
) : IRequest<bool>;

public class UpdateOrderStatusCommandHandler
    : IRequestHandler<UpdateOrderStatusCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateOrderStatusCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(
        UpdateOrderStatusCommand request,
        CancellationToken cancellationToken)
    {
        var order = await _unitOfWork.Repository<Order>().FindFirstAsync(u => u.Id == request.OrderId);

        if (order is null)
            return false;

        order.OrderStatus = request.Status;

        await _unitOfWork.Orders.UpdateAsync(order);

        await _unitOfWork.SaveChanges();

        return true;
    }
}