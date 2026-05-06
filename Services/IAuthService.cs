using break_back.Models.Dtos.UserDtos;

namespace break_back.Services;

public interface IAuthService
{
    public bool ValidateUser(UserLoginDto user);
    public Task<bool> RegisterUser(UserRegisterDto user);
}