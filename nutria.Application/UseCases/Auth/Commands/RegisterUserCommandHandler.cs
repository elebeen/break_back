using MediatR;
using Nutria.Domain.Dtos.User;
using Nutria.Domain.Interfaces;
using Nutria.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace nutria.Application.UseCases.Auth.Commands;

public record RegisterUserCommand(UserRegisterDto UserRegister) : IRequest<bool>;

internal sealed record RegisterUserCommandHandler
    : IRequestHandler<RegisterUserCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserCommandHandler(IUnitOfWork unitOfWork)   
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _unitOfWork
            .Repository<User>()
            .Query()
            .FirstOrDefaultAsync(x => x.Email == request.UserRegister.Email, cancellationToken);

        if (existingUser != null)
            return false;

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.UserRegister.Password);

        var newUser = new User
        {
            FullName = request.UserRegister.FullName,
            Email = request.UserRegister.Email,
            PasswordHash = passwordHash
        };

        await _unitOfWork.Repository<User>().AddAsync(newUser);
        await _unitOfWork.SaveChanges();

        return true;
    }
}