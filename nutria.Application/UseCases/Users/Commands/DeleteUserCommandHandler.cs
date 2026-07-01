using MediatR;
using Nutria.Domain.Interfaces.Repositories;
using Nutria.Domain.Models;

namespace nutria.Application.UseCases.Users.Commands;

public record DeleteUserCommand(Guid UserId) : IRequest<string>;

internal sealed class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, string>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<string> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Repository<User>().FindFirstAsync(u => u.Id == request.UserId);

        if (user == null)
        {
            throw new ArgumentException("User not found.");
        }

        // 2. SOFT DELETE: En lugar de usar .Delete(), modificamos su propiedad de estado.
        // Opción A (Si añadiste el campo IsActive):
        // user.IsActive = false; 

        // Opción B (Si prefieres usar los campos actuales sin modificar la BD):
        user.Role = "Eliminado";

        // 3. Marcamos la entidad como modificada en el repositorio
        await _unitOfWork.Repository<User>().UpdateAsync(user); 

        // 4. Confirmamos los cambios de manera asíncrona en la base de datos
        await _unitOfWork.SaveChanges();

        return "User deleted successfully.";
    }
}