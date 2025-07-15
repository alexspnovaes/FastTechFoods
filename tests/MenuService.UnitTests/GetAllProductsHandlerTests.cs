using FastTechFoods.MenuService.Application.Queries.GetAllProducts;
using FastTechFoods.MenuService.Domain.Entities;
using FastTechFoods.MenuService.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace MenuService.IntegrationTests;

public class GetAllProductsHandlerTests
{
    private readonly Mock<IProductRepository> _repo;
    private readonly GetAllProductsHandler _handler;

    public GetAllProductsHandlerTests()
    {
        _repo = new Mock<IProductRepository>();
        _handler = new GetAllProductsHandler(_repo.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnAllFromRepository()
    {
        // Arrange
        var list = new List<Product>
        {
            new() { Id = Guid.NewGuid(), Name = "A", Price = 5m, Category = "X" },
            new() { Id = Guid.NewGuid(), Name = "B", Price = 7m, Category = "X" }
        };
        _repo.Setup(r => r.GetAllAsync("X")).ReturnsAsync(list);

        // Act
        var result = await _handler.Handle(new GetAllProductsQuery("X"), CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(list);
    }
}