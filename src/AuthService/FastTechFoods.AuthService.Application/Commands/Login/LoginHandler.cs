using System.Security.Cryptography;
using System.Text;
using FastTechFoods.AuthService.Application.Services;
using FastTechFoods.AuthService.Domain.Interfaces;
using MediatR;

namespace FastTechFoods.AuthService.Application.Commands.Login;

public class LoginHandler : IRequestHandler<LoginCommand, string?>
{
    private readonly IUserRepository _repo;
    private readonly ITokenService _tokenService;

    public LoginHandler(IUserRepository repo, ITokenService tokenService)
    {
        _repo = repo;
        _tokenService = tokenService;
    }

    public async Task<string?> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = request.EmailOrCpf.Contains("@")
            ? await _repo.GetByEmailAsync(request.EmailOrCpf)
            : await _repo.GetByCpfAsync(request.EmailOrCpf);

        if (user == null) return null;

        var hash = ComputeSha256Hash(request.Password);
        if (user.PasswordHash != hash) return null;

        return _tokenService.GenerateToken(user);
    }

    private string ComputeSha256Hash(string rawData)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
        return Convert.ToBase64String(bytes);
    }
}
