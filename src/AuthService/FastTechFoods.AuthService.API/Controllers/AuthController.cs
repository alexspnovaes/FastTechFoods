using FastTechFoods.AuthService.Application.Commands.Login;
using FastTechFoods.AuthService.Application.Commands.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FastTechFoods.AuthService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
    {
        var success = await _mediator.Send(command);
        if (!success) return Conflict("User already exists.");
        return Ok("User registered successfully.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var token = await _mediator.Send(command);
        if (token is null) return Unauthorized("Invalid credentials.");
        return Ok(new { token });
    }
}
