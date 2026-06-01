using MediatR;
using Nutria.Domain.Dtos.User;
using Nutria.Domain.Interfaces;
using Nutria.Domain.Models;

namespace nutria.Application.UseCases.Health.Queries;

public record GetDailyNutritionSummaryQuery(Guid UserId)
    : IRequest<DailyNutritionSummaryDto>;

public class GetDailyNutritionSummaryQueryHandler
    : IRequestHandler<GetDailyNutritionSummaryQuery, DailyNutritionSummaryDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetDailyNutritionSummaryQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task<DailyNutritionSummaryDto> Handle(
        GetDailyNutritionSummaryQuery request,
        CancellationToken cancellationToken)
    {
        var today = DateTime.UtcNow.Date;

        var orders = _unitOfWork
            .Repository<Order>()
            .Query()
            .Where(x =>
                x.UserId == request.UserId &&
                x.CreatedAt.HasValue &&
                x.CreatedAt.Value.Date == today)
            .ToList();

        var summary = new DailyNutritionSummaryDto();

        foreach (var order in orders)
        {
            foreach (var item in order.OrderItems)
            {
                if (item.Meal?.NutritionalInfo == null)
                    continue;

                summary.Calories += item.Meal.NutritionalInfo.Calories * item.Quantity;
                summary.Protein += item.Meal.NutritionalInfo.ProteinG * item.Quantity;
                summary.Carbs += item.Meal.NutritionalInfo.CarbsG * item.Quantity;
                summary.Fats += item.Meal.NutritionalInfo.FatsG * item.Quantity;
            }
        }

        return Task.FromResult(summary);
    }
}