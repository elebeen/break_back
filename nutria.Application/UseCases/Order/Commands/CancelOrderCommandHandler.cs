using MediatR;
using Nutria.Domain.Interfaces;
using Nutria.Domain.Models;

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
        var order = await _unitOfWork
            .Repository<Nutria.Domain.Models.Order>()
            .GetByIdAsync(request.OrderId);

        if (order is null)
            return false;

        // Mejor práctica: lógica de dominio (si luego lo quieres mejorar)
        order.OrderStatus = "Cancelado";

        _unitOfWork.Repository<Nutria.Domain.Models.Order>().Update(order);

        await _unitOfWork.SaveChanges();

        return true;
    }
}