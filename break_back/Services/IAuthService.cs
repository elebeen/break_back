using Nutria.Domain.Dtos.User;

namespace break_back.Services;

public interface IMediator
{
    public bool ValidateUser(UserLoginDto user);
    public Task<bool> RegisterUser(UserRegisterDto user);
}