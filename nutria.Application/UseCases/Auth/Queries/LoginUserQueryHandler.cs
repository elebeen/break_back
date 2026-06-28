using MediatR;
using Nutria.Domain.Dtos.User;
using Nutria.Domain.Interfaces;
using Nutria.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Nutria.Domain.Interfaces.Repositories;


namespace nutria.Application.UseCases.Auth.Queries;

public record LoginUserCommand(UserLoginDto UserLogin) : IRequest<bool>;

internal sealed record LoginUserQueryHandler : IRequestHandler<LoginUserCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public LoginUserQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _unitOfWork.Repository<User>().FindFirstAsync(u => u.Email == request.UserLogin.Email);

        if (existingUser == null)
            return false;

        return BCrypt.Net.BCrypt.Verify(
            request.UserLogin.Password,
            existingUser.PasswordHash
        );
    }
}