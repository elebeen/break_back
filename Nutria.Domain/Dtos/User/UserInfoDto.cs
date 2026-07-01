namespace Nutria.Domain.Dtos.User;

public class UserInfoDto
{
    public Guid Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;
}