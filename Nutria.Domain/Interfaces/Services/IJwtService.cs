namespace Nutria.Domain.Interfaces.Services;

public interface IJwtService
{
    public string GenerateJwtToken(string userId, string userName);
}