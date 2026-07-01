using Nutria.Domain.Dtos.User;
using Nutria.Domain.Models;

namespace Nutria.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserWithHealthProfileAndConditionsAsync(Guid userId);
    Task<User?> GetUserWithConditionsAsync(Guid userId, int conditionId);
    Task<UserInfoDto?> GetUserInfoAsync(Guid userId);
}