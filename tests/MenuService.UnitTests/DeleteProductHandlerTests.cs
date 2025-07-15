using FastTechFoods.MenuService.Application.Commands.DeleteProduct;
using FastTechFoods.MenuService.Domain.Interfaces;
using Moq;

namespace MenuService.IntegrationTests;

public class DeleteProductHandlerTests
{
    private readonly Mock<IProductRepository> _repo;
    private readonly DeleteProductHandler _handler;

    public DeleteProductHandlerTests()
    {
        _repo = new Mock<IProductRepository>();
        _handler = new DeleteProductHandler(_repo.Object);
    }

    [Fact]
    public async Task Handle_ShouldCallDeleteAsync()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repo.Setup(r => r.DeleteAsync(id)).Returns(Task.CompletedTask).Verifiable();

        // Act
        await _handler.Handle(new DeleteProductCommand(id), CancellationToken.None);

        // Assert
        _repo.Verify(r => r.DeleteAsync(id), Times.Once);
    }
}
