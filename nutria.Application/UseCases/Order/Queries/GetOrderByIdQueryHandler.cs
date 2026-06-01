using MediatR;
using Microsoft.EntityFrameworkCore;
using Nutria.Domain.Interfaces;
using Nutria.Domain.Models;
using Nutria.Infrastructure.Persistence.Context;

namespace nutria.Application.UseCases.Order.Queries;

public record GetOrderByIdQuery(Guid OrderId)
    : IRequest<Nutria.Domain.Models.Order?>;

public class GetOrderByIdQueryHandler
    : IRequestHandler<GetOrderByIdQuery, Nutria.Domain.Models.Order?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetOrderByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Nutria.Domain.Models.Order?> Handle(
        GetOrderByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _unitOfWork.Orders
            .GetOrderDetailsAsync(request.OrderId);
    }
}