using break_back.Models.Dtos;
using break_back.Services;
using Microsoft.AspNetCore.Mvc;

namespace break_back.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    public IActionResult Login([FromBody] UserLoginDto userLoginDto)
    {
        if (!_authService.ValidateUser(userLoginDto))
        {
            return Unauthorized();
        }

        return Ok();
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var success = await _authService.RegisterUser(userRegisterDto);

        if (!success)
            return BadRequest("El usuario ya existe o hubo un error.");

        return Ok(new { message = "Usuario registrado exitosamente" });
    }
}