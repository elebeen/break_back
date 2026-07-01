using MediatR;
using Nutria.Domain.Dtos.User;
using Nutria.Domain.Models;
using Nutria.Domain.Interfaces.Repositories;
using Nutria.Domain.Interfaces.Services;

namespace nutria.Application.UseCases.Auth.Queries;

public record LoginUserCommand(UserLoginDto UserLogin) : IRequest<string>;

internal sealed record LoginUserQueryHandler : IRequestHandler<LoginUserCommand, string>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtService _jwtService;

    public LoginUserQueryHandler(IUnitOfWork unitOfWork, IJwtService jwtService)
    {
        _unitOfWork = unitOfWork;
        _jwtService = jwtService;
    }

    public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _unitOfWork
            .Repository<User>()
            .FindFirstAsync(u => u.Email == request.UserLogin.Email);

        if (existingUser == null ||
            !BCrypt.Net.BCrypt.Verify(request.UserLogin.Password, existingUser.PasswordHash))
        {
            //return "Credentials do not match";
            throw new ArgumentException("Invalid credentials");
        }

        if (existingUser.Role == "Eliminado")
        {
            throw new ArgumentException("User Not Found");
        }

        var token = _jwtService.GenerateJwtToken(
            existingUser.Id.ToString(),
            existingUser.FullName);

        return token;
    }
}