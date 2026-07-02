using MediatR;
using Nutria.Domain.Dtos.User;
using Nutria.Domain.Interfaces.Repositories;
using Nutria.Domain.Models;

namespace nutria.Application.UseCases.Users.Commands;

public record EditUserInfoCommand(UserInfoDto UserInfo) : IRequest<string>;

internal sealed record EditUserInfoCommandHandler : IRequestHandler<EditUserInfoCommand, string>
{
    private readonly IUnitOfWork _unitOfWork;

    public EditUserInfoCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<string> Handle(EditUserInfoCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Repository<User>().FindFirstAsync(u => u.Id == request.UserInfo.Id);

        if (user == null)
        {
            throw new ArgumentException("User not found");
        }

        // 2. Modificamos únicamente los campos deseados sobre el usuario recuperado
        user.Id = request.UserInfo.Id;
        user.FullName = request.UserInfo.FullName;
        user.Email = request.UserInfo.Email;

        // 3. Le indicamos al repositorio que actualice la entidad (opcional en EF si tiene tracking, pero buena práctica con tu UnitOfWork)
        await _unitOfWork.Repository<User>().UpdateAsync(user);

        // 4. Guardamos los cambios. EF Core ejecutará el UPDATE manteniendo el PasswordHash y Role intactos
        await _unitOfWork.SaveChanges();
    
        return "Profile updated successfully";
    }
}