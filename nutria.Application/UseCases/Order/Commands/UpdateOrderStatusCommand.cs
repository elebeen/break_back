using MediatR;
using Nutria.Domain.Interfaces;

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
        var order = _unitOfWork.Repository<Order>()
            .FindbyGuid(request.OrderId);

        if (order == null)
            return false;

        order.OrderStatus = request.Status;

        await _unitOfWork.SaveChanges();

        return true;
    }
}