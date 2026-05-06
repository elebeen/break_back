using break_back.Models.Dtos.UserDtos;
using break_back.Repositories;
using break_back.Models;
using Microsoft.AspNetCore.Authentication;

namespace break_back.Services.Implements;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    
    public AuthService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public bool ValidateUser(UserLoginDto user)
    {
        var existingUser = _unitOfWork.Repository<User>().FindByName(user.FullName);
        
        if (user == null)
        {
            return false;
        }
        
        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(user.Password, existingUser.PasswordHash);
        
        return isPasswordValid;
    }
    
    public async Task<bool> RegisterUser(UserRegisterDto userloginDto)
    {
        var existingUser = _unitOfWork.Repository<User>().FindByName(userloginDto.Email);
        if (existingUser != null) return false;

        // 2. Aquí deberías encriptar la contraseña (ej. BCrypt)
        userloginDto.Password = BCrypt.Net.BCrypt.HashPassword(userloginDto.Password);

        var newUser = new User()
        {
            FullName =  userloginDto.FullName,
            Email = userloginDto.Email,
            PasswordHash = userloginDto.Password,
        };
        
        // 3. Guardar
        _unitOfWork.Repository<User>().Add(newUser);
        await _unitOfWork.SaveChanges();
        
        return true;
    }
}