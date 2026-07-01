using MediatR;
using Nutria.Domain.Dtos.Order;
using Nutria.Domain.Interfaces.Repositories;

namespace nutria.Application.UseCases.Orders.Queries;

public record GetUserOrdersQuery(Guid UserId) : IRequest<List<OrderResponse>>;

public class GetUserOrdersQueryHandler : IRequestHandler<GetUserOrdersQuery, List<OrderResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUserOrdersQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<OrderResponse>> Handle(GetUserOrdersQuery request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.Orders
            .GetOrdersByUserAsync(request.UserId);
    }
}