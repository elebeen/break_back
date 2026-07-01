using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nutria.Application.UseCases.Health.Queries;
using nutria.Application.UseCases.Users.Commands;

namespace break_back.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("Profile/")]
    public async Task<IActionResult> Get([FromQuery] GetUserProfileQuery query)
    {
        var res = await _mediator.Send(query);
        return Ok(res);
    }

    [HttpPost("Profile/Edit")]
    public async Task<IActionResult> Edit([FromBody] EditUserInfoCommand command)
    {
        return Ok(await _mediator.Send(command));
    }
    
    [HttpDelete("Profile/Delete")]
    public async Task<IActionResult> Delete([FromQuery] DeleteUserCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(new { message = result });
    }
}