using Microsoft.EntityFrameworkCore;
using Nutria.Domain.Dtos.User;
using Nutria.Domain.Interfaces.Repositories;
using Nutria.Domain.Models;
using Nutria.Infrastructure.Persistence.Context;

namespace Nutria.Infrastructure.Repositories;

public class UserRepository(AppdbContext context) : Repository<User>(context), IUserRepository
{
    public async Task<User?> GetUserWithHealthProfileAndConditionsAsync(Guid userId)
    {
        return await _context.Users
            .Include(u => u.HealthProfile)
            .Include(u => u.Conditions)
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<User?> GetUserWithConditionsAsync(Guid userId, int conditionId )
    {
        return await _context.Users
            .Include(u => u.Conditions.Where(c => c.Id == conditionId))
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<UserInfoDto?> GetUserInfoAsync(Guid userId)
    {
        return await _context.Users
            .AsNoTracking()
            .Where(u => u.Id == userId)
            .Select(o => new UserInfoDto
                {
                    Id = o.Id,
                    Email = o.Email,
                    FullName = o.FullName
                }).FirstOrDefaultAsync();
    }
}