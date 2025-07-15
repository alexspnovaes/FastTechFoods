using FastTechFoods.OrderService.Application.Commands.CancelOrder;
using FastTechFoods.OrderService.Domain.Entities;
using FastTechFoods.OrderService.Domain.Interfaces;
using FluentAssertions;
using Moq;
namespace OrderService.UnitTests;
public class CancelOrderHandlerTests
{
    private readonly Mock<IOrderRepository> _repo = new();
    private readonly CancelOrderHandler _handler;

    public CancelOrderHandlerTests()
    {
        _handler = new CancelOrderHandler(_repo.Object);
    }

    [Fact]
    public async Task Handle_WithStatusRecebido_ShouldCancelAndSave()
    {
        // arrange
        var order = new Order
        {
            Id = Guid.NewGuid(),
            ClientId = Guid.NewGuid(),
            DeliveryMethod = "Delivery",
            Status = "Recebido",
            Items =
            [
                new OrderItem
                {
                    ProductId   = Guid.NewGuid(),
                    ProductName = "Produto Teste",
                    UnitPrice   = 10.50m,
                    Quantity    = 2
                }
            ]
        };

        _repo
          .Setup(x => x.GetByIdAsync(order.Id))
          .ReturnsAsync(order);

        // act
        await _handler.Handle(
            new CancelOrderCommand(order.Id, "Cliente desistiu"),
            CancellationToken.None
        );

        // assert
        order.Status.Should().Be("Cancelado");
        order.CancelReason.Should().Be("Cliente desistiu");
        _repo.Verify(x => x.UpdateAsync(order), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenNotRecebido_ShouldThrowInvalidOperation()
    {
        // arrange
        var order = new Order
        {
            Id = Guid.NewGuid(),
            Status = "Pronto"
        };

        _repo
          .Setup(x => x.GetByIdAsync(order.Id))
          .ReturnsAsync(order);

        // act
        Func<Task> act = () => _handler.Handle(
            new CancelOrderCommand(order.Id, "Tarde demais"),
            CancellationToken.None
        );

        // assert
        await act
            .Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("*antes de iniciado o preparo*");
    }
}
