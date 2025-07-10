namespace FastTechFoods.BuildingBlocks.Messaging.Events;

public class OrderCreatedEvent
{
    public Guid OrderId { get; set; }
    public Guid ClientId { get; set; }
    public string ClientName { get; set; } = null!;
    public string DeliveryMethod { get; set; } = null!;
    public List<PedidoCriadoItem> Items { get; set; } = new();
}

public class PedidoCriadoItem
{
    public string ProductName { get; set; } = null!;
    public int Quantity { get; set; }
}
