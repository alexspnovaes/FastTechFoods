using MediatR;

namespace FastTechFoods.AuthService.Application.Commands.RegisterUser;

public record RegisterUserCommand(string Name, string Email, string? CPF, string Password, string Role) : IRequest<bool>;
