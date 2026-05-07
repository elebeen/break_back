using break_back.Models.Dtos.User;

namespace break_back.Repositories;

public interface IHealthRepository
{
    public Task<UserHealthProfileDto?> GetUserHealthData(Guid userId);
}