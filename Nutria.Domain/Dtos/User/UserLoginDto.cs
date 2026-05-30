namespace Nutria.Domain.Dtos.User;

public class UserLoginDto
{
    public string FullName { get; set; } = null!;
    public string Password { get; set; } = null!;
}