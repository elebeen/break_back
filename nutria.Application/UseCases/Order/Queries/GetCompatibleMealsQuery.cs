    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Nutria.Domain.Interfaces;
    using Nutria.Domain.Models;
    using Nutria.Infrastructure.Persistence.Context;

    namespace nutria.Application.UseCases.Order.Queries;

    public record GetCompatibleMealsQuery(Guid UserId)
        : IRequest<List<Meal>>;

    public class GetCompatibleMealsQueryHandler
        : IRequestHandler<GetCompatibleMealsQuery, List<Meal>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCompatibleMealsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Meal>> Handle(
            GetCompatibleMealsQuery request,
            CancellationToken cancellationToken)
        {
            return await _unitOfWork.Meals
                .GetCompatibleMealsAsync(request.UserId);
        }
    }