/*using MediatR;
using Nutria.Domain.Interfaces;
using Nutria.Domain.Models;

namespace nutria.Application.UseCases.Orders.Queries;

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
    }*/
    
// nota
// la clase GetAnalyzedMenuQueryHandler hace lo mismo