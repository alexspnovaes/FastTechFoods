using FastTechFoods.KitchenService.Application.Commands.AcceptOrder;
using FastTechFoods.KitchenService.Application.Commands.MarkOrderReady;
using FastTechFoods.KitchenService.Application.Commands.RejectOrder;
using FastTechFoods.KitchenService.Application.Queries.GetKitchenOrderById;
using FastTechFoods.KitchenService.Application.Queries.GetPendingKitchenOrders;
using FastTechFoods.KitchenService.Domain.Entities;
using FastTechFoods.KitchenService.Domain.Interfaces;
using FluentAssertions;
using Moq;
namespace KitchenService.UnitTests;

public class KitchenHandlersTests
{
    [Fact]
    public async Task AcceptOrder_InvokesAcceptAsync()
    {
        var id = Guid.NewGuid();
        var repo = new Mock<IKitchenOrderRepository>();
        var handler = new AcceptOrderHandler(repo.Object);

        await handler.Handle(new AcceptOrderCommand(id), CancellationToken.None);

        repo.Verify(x => x.AcceptAsync(id), Times.Once);
    }

    [Fact]
    public async Task RejectOrder_InvokesRejectAsync()
    {
        var id = Guid.NewGuid();
        var reason = "Faltou item";
        var repo = new Mock<IKitchenOrderRepository>();
        var handler = new RejectOrderHandler(repo.Object);

        await handler.Handle(new RejectOrderCommand(id, reason), CancellationToken.None);

        repo.Verify(x => x.RejectAsync(id, reason), Times.Once);
    }

    [Fact]
    public async Task MarkOrderReady_InvokesMarkReadyAsync()
    {
        var id = Guid.NewGuid();
        var repo = new Mock<IKitchenOrderRepository>();
        var handler = new MarkOrderReadyHandler(repo.Object);

        await handler.Handle(new MarkOrderReadyCommand(id), CancellationToken.None);

        repo.Verify(x => x.MarkReadyAsync(id), Times.Once);
    }

    [Fact]
    public async Task GetPendingKitchenOrders_ReturnsRepoResult()
    {
        var list = new List<KitchenOrder>
            {
                new KitchenOrder { Id = Guid.NewGuid() }
            };
        var repo = new Mock<IKitchenOrderRepository>();
        repo.Setup(x => x.GetPendingAsync())
            .ReturnsAsync(list);

        var handler = new GetPendingKitchenOrdersHandler(repo.Object);
        var result = await handler.Handle(new GetPendingKitchenOrdersQuery(), CancellationToken.None);

        result.Should().BeEquivalentTo(list);
    }

    [Fact]
    public async Task GetKitchenOrderById_ReturnsRepoResultOrNull()
    {
        var id = Guid.NewGuid();
        var order = new KitchenOrder { Id = id };
        var repo = new Mock<IKitchenOrderRepository>();
        repo.Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync(order);

        var handler = new GetKitchenOrderByIdHandler(repo.Object);
        var found = await handler.Handle(new GetKitchenOrderByIdQuery(id), CancellationToken.None);
        found.Should().Be(order);

        repo.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((KitchenOrder?)null);
        var none = await handler.Handle(new GetKitchenOrderByIdQuery(Guid.NewGuid()), CancellationToken.None);
        none.Should().BeNull();
    }
}