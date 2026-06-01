using Nutria.Domain.Models;

namespace Nutria.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);

    Task<User?> GetUserWithHealthProfileAsync(Guid userId);

    Task<User?> GetUserWithConditionsAsync(Guid userId, int conditionId);

    Task<bool> ExistsByEmailAsync(string email);
}