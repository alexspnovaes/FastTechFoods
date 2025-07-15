using MediatR;

namespace FastTechFoods.AuthService.Application.Commands.Login;

public record LoginCommand(string EmailOrCpf, string Password) : IRequest<string?>;
