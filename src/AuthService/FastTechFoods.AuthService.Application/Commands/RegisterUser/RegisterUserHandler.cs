using System.Security.Cryptography;
using System.Text;
using FastTechFoods.AuthService.Domain.Entities;
using FastTechFoods.AuthService.Domain.Interfaces;
using MediatR;

namespace FastTechFoods.AuthService.Application.Commands.RegisterUser;

public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, bool>
{
    private readonly IUserRepository _repo;

    public RegisterUserHandler(IUserRepository repo)
    {
        _repo = repo;
    }

    public async Task<bool> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existing = request.Email != null
            ? await _repo.GetByEmailAsync(request.Email)
            : request.CPF != null ? await _repo.GetByCpfAsync(request.CPF) : null;

        if (existing != null)
            return false;

        var passwordHash = ComputeSha256Hash(request.Password);

        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            CPF = request.CPF,
            PasswordHash = passwordHash,
            Role = request.Role
        };

        await _repo.AddAsync(user);
        return true;
    }

    private string ComputeSha256Hash(string rawData)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
        return Convert.ToBase64String(bytes);
    }
}
