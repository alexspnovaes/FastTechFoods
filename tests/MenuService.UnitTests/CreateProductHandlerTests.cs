using FastTechFoods.MenuService.Application.Commands.CreateProduct;
using FastTechFoods.MenuService.Domain.Entities;
using FastTechFoods.MenuService.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace MenuService.IntegrationTests;

public class CreateProductHandlerTests
{
    private readonly Mock<IProductRepository> _repo;
    private readonly CreateProductHandler _handler;

    public CreateProductHandlerTests()
    {
        _repo = new Mock<IProductRepository>();
        _handler = new CreateProductHandler(_repo.Object);
    }

    [Fact]
    public async Task Handle_ShouldAddProductAndReturnId()
    {
        // Arrange
        CreateProductCommand cmd = new(
            Name: "Pizza",
            Description: "Delicious",
            Price: 25.0m,
            Category: "Main"
        );
        _repo
            .Setup(r => r.AddAsync(It.IsAny<Product>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        var result = await _handler.Handle(cmd, CancellationToken.None);

        // Assert
        result.Should().NotBe(Guid.Empty);
        _repo.Verify(r => r.AddAsync(It.Is<Product>(p =>
            p.Name == "Pizza" &&
            p.Description == "Delicious" &&
            p.Price == 25.0m &&
            p.Category == "Main" &&
            p.IsAvailable
        )), Times.Once);
    }
}
