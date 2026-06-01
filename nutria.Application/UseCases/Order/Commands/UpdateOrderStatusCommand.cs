using MediatR;
using Nutria.Domain.Interfaces;
using Nutria.Domain.Models;

namespace nutria.Application.UseCases.Order.Commands;

public record UpdateOrderStatusCommand(Guid OrderId, string Status) : IRequest<bool>;

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
        var order = await _unitOfWork
            .Repository<Nutria.Domain.Models.Order>()
            .GetByIdAsync(request.OrderId);

        if (order is null)
            return false;

        order.OrderStatus = request.Status;

        _unitOfWork.Repository<Nutria.Domain.Models.Order>().Update(order);

        await _unitOfWork.SaveChanges();

        return true;
    }
}