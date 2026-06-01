using MediatR;
using Microsoft.EntityFrameworkCore;
using Nutria.Domain.Interfaces;
using Nutria.Infrastructure.Persistence.Context;

namespace nutria.Application.UseCases.Order.Queries;

public record GetUserOrdersQuery(Guid UserId)
    : IRequest<List<Nutria.Domain.Models.Order>>;

public class GetUserOrdersQueryHandler
    : IRequestHandler<GetUserOrdersQuery, List<Nutria.Domain.Models.Order>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUserOrdersQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<Nutria.Domain.Models.Order>> Handle(
        GetUserOrdersQuery request,
        CancellationToken cancellationToken)
    {
        return await _unitOfWork.Orders
            .GetOrdersByUserAsync(request.UserId);
    }
}