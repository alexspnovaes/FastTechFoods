using FastTechFoods.MenuService.Application.Commands.UpdateProduct;
using FastTechFoods.MenuService.Domain.Entities;
using FastTechFoods.MenuService.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace MenuService.IntegrationTests;

public class UpdateProductHandlerTests
{
    private readonly Mock<IProductRepository> _repo;
    private readonly UpdateProductHandler _handler;

    public UpdateProductHandlerTests()
    {
        _repo = new Mock<IProductRepository>();
        _handler = new UpdateProductHandler(_repo.Object);
    }

    [Fact]
    public async Task Handle_WhenProductNotFound_ReturnsFalse()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Product?)null);

        // Act
        var result = await _handler.Handle(new UpdateProductCommand(
            Id: id, Name: "", Description: "", Price: 0m, Category: "", IsAvailable: false
        ), CancellationToken.None);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_WhenProductExists_UpdatesAndReturnsTrue()
    {
        // Arrange
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = "Old",
            Description = "Desc",
            Price = 1m,
            Category = "Cat",
            IsAvailable = true
        };
        _repo.Setup(r => r.GetByIdAsync(product.Id)).ReturnsAsync(product);
        _repo.Setup(r => r.UpdateAsync(product)).Returns(Task.CompletedTask).Verifiable();

        var cmd = new UpdateProductCommand(
            Id: product.Id,
            Name: "New",
            Description: "NewDesc",
            Price: 10m,
            Category: "NewCat",
            IsAvailable: false
        );

        // Act
        var result = await _handler.Handle(cmd, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        product.Name.Should().Be("New");
        product.Description.Should().Be("NewDesc");
        product.Price.Should().Be(10m);
        product.Category.Should().Be("NewCat");
        product.IsAvailable.Should().BeFalse();
        _repo.Verify(r => r.UpdateAsync(product), Times.Once);
    }
}
