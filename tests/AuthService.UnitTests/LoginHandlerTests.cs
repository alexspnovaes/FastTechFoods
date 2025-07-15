using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FastTechFoods.AuthService.Application.Commands.Login;
using FastTechFoods.AuthService.Domain.Entities;
using FastTechFoods.AuthService.Domain.Interfaces;
using FastTechFoods.AuthService.Application.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace AuthService.UnitTests;

public class LoginHandlerTests
{
    private readonly Mock<IUserRepository> _repo = new();
    private readonly Mock<ITokenService> _tokenSvc = new();
    private readonly LoginHandler _handler;

    public LoginHandlerTests()
    {
        _handler = new LoginHandler(_repo.Object, _tokenSvc.Object);
    }

    [Fact]
    public async Task Handle_ValidCredentials_ReturnsToken()
    {
        // 1) cria o hash de "secret"
        string ComputeHash(string raw)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(raw));
            return Convert.ToBase64String(bytes);
        }

        var password = "secret";
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "Alice",
            Role = "client",
            PasswordHash = ComputeHash(password)
        };

        // 2) mocka o repo para devolver o user ao buscar por email
        _repo
          .Setup(x => x.GetByEmailAsync("alice@example.com"))
          .ReturnsAsync(user);

        // 3) mocka o token service
        _tokenSvc
          .Setup(x => x.GenerateToken(user))
          .Returns("jwt-token-123");

        // 4) executa
        var command = new LoginCommand(
            EmailOrCpf: "alice@example.com",
            Password: password
        );
        var token = await _handler.Handle(command, CancellationToken.None);

        token.Should().Be("jwt-token-123");
    }

    [Fact]
    public async Task Handle_InvalidPassword_ReturnsNull()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "Alice",
            Role = "client",
            PasswordHash = "someOtherHash"
        };
        _repo
          .Setup(x => x.GetByEmailAsync("alice@example.com"))
          .ReturnsAsync(user);

        var command = new LoginCommand(
            EmailOrCpf: "alice@example.com",
            Password: "wrong"
        );
        var token = await _handler.Handle(command, CancellationToken.None);

        token.Should().BeNull();
    }

    [Fact]
    public async Task Handle_UserNotFound_ReturnsNull()
    {
        _repo
          .Setup(x => x.GetByEmailAsync("bob@example.com"))
          .ReturnsAsync((User?)null);

        var command = new LoginCommand(
            EmailOrCpf: "bob@example.com",
            Password: "any"
        );
        var token = await _handler.Handle(command, CancellationToken.None);

        token.Should().BeNull();
    }
}
