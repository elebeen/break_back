using MediatR;
using Microsoft.EntityFrameworkCore;
using Nutria.Domain.Models;
using Nutria.Infrastructure.Persistence.Context;

namespace nutria.Application.UseCases.Order.Queries;

public record GetCompatibleMealsQuery(Guid UserId) : IRequest<List<Meal>>;

public class GetCompatibleMealsQueryHandler
    : IRequestHandler<GetCompatibleMealsQuery,List<Meal>>
{
    private readonly AppdbContext _appdbContext;

    public GetCompatibleMealsQueryHandler(AppdbContext appdbContext)
    {
        _appdbContext = appdbContext;
    }

    public async Task<List<Meal>> Handle(
        GetCompatibleMealsQuery request,
        CancellationToken cancellationToken)
    {
        var profile = await _appdbContext.HealthProfiles
            .FirstOrDefaultAsync(
                x => x.UserId == request.UserId,
                cancellationToken);

        return await _appdbContext.Meals
            .Include(x => x.NutritionalInfo)
            .Where(x =>
                x.NutritionalInfo!.Calories <= profile!.DailyCalorieTarget)
            .ToListAsync(cancellationToken);
    }
}