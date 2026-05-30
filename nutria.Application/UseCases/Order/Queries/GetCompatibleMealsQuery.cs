using MediatR;
using Microsoft.EntityFrameworkCore;
using Nutria.Domain.Models;

namespace nutria.Application.UseCases.Order.Queries;

public record GetCompatibleMealsQuery(Guid UserId)
    : IRequest<List<Meal>>;

public class GetCompatibleMealsQueryHandler
    : IRequestHandler<GetCompatibleMealsQuery,List<Meal>>
{
    private readonly Context _context;

    public GetCompatibleMealsQueryHandler(Context context)
    {
        _context = context;
    }

    public async Task<List<Meal>> Handle(
        GetCompatibleMealsQuery request,
        CancellationToken cancellationToken)
    {
        var profile = await _context.HealthProfiles
            .FirstOrDefaultAsync(
                x => x.UserId == request.UserId,
                cancellationToken);

        return await _context.Meals
            .Include(x => x.NutritionalInfo)
            .Where(x =>
                x.NutritionalInfo.Calories <= profile.DailyCalorieTarget)
            .ToListAsync(cancellationToken);
    }
}