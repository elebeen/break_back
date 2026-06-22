namespace Nutria.Domain.Interfaces;

public interface IJwtService
{
    //public string GenerateJwtToken(string userId, string userName, string role);
    public string GenerateJwtToken(string userId, string userName);
}