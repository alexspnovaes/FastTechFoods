using FastTechFoods.AuthService.Application.Commands.RegisterUser;
using FastTechFoods.AuthService.Domain.Entities;
using FastTechFoods.AuthService.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace AuthService.UnitTests;

public class RegisterUserHandlerTests
{
    private readonly Mock<IUserRepository> _repo = new();
    private readonly RegisterUserHandler _handler;

    public RegisterUserHandlerTests()
    {
        _handler = new RegisterUserHandler(_repo.Object);
    }

    [Fact]
    public async Task Handle_NewUser_ReturnsTrue()
    {      
        _repo.Setup(x => x.AddAsync(It.IsAny<User>()))
             .Returns(Task.CompletedTask);

        var command = new RegisterUserCommand(
            Name: "Test User",
            Email: "a@b.com",
            CPF: "12345678900",
            Password: "Pwd@123",
            Role: "client"
        );

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().BeTrue();
        _repo.Verify(x => x.AddAsync(It.Is<User>(u => u.Email == "a@b.com")), Times.Once);
    }

    [Fact]
    public async Task Handle_ExistingUser_ReturnsFalse()
    {
        var existing = new User { Id = Guid.NewGuid(), Email = "a@b.com" };
        _repo
          .Setup(x => x.GetByEmailAsync("a@b.com"))
          .ReturnsAsync(existing);

        var command = new RegisterUserCommand(
            Name: "Test User",
            Email: "a@b.com",
            CPF: null,
            Password: "Pwd@123",
            Role: "client"
        );

        // act
        var result = await _handler.Handle(command, CancellationToken.None);

        // assert
        result.Should().BeFalse();
        _repo.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Never);
    }

}
