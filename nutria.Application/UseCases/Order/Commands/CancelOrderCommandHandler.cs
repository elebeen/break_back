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
        var order = await _unitOfWork.Repository<Nutria.Domain.Models.Order>()
            .FindFirstAsync(x => x.Id == request.OrderId);

        if (order == null)
            return false;

        order.OrderStatus = "Cancelado";

        await _unitOfWork.Repository<Nutria.Domain.Models.Order>().Update(order);

        await _unitOfWork.SaveChanges();

        return true;
    }
}