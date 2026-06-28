using Microsoft.EntityFrameworkCore;
using Nutria.Domain.Interfaces;
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

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _context.Users
            .AnyAsync(u => u.Email == email);
    }
}