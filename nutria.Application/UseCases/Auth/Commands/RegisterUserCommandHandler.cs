using MediatR;
using Nutria.Domain.Dtos.User;
using Nutria.Domain.Interfaces;
using Nutria.Domain.Models;

namespace nutria.Application.UseCases.Auth.Commands;

public record RegisterUserCommand(UserRegisterDto UserRegister) : IRequest<bool>;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = _unitOfWork.Repository<User>().FindByName(request.UserRegister.Email);
        if (existingUser != null) return false;

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.UserRegister.Password);

        var newUser = new User
        {
            FullName = request.UserRegister.FullName,
            Email = request.UserRegister.Email,
            PasswordHash = passwordHash,
        };

        _unitOfWork.Repository<User>().Add(newUser);
        await _unitOfWork.SaveChanges();

        return true;
    }
}