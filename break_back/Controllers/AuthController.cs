using MediatR;
using Microsoft.AspNetCore.Mvc;
using nutria.Application.UseCases.Auth.Commands;
using nutria.Application.UseCases.Auth.Queries;

namespace break_back.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
    {
        var token = await _mediator.Send(command);

        return Ok( new { message = "Login exitosamente" , token});
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var success = await _mediator.Send(command);

        if (!success)
            return BadRequest("El usuario ya existe o hubo un error.");

        return Ok(new { message = "Usuario registrado exitosamente" });
    }
}