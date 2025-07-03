namespace FastTechFoods.OrderService.Domain.Entities;

public class Order
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ClientId { get; set; }
    public List<OrderItem> Items { get; set; } = new();
    public decimal Total => Items.Sum(i => i.Quantity * i.UnitPrice);
    public string DeliveryMethod { get; set; } = null!; // "Balcão", "Drive-thru", "Delivery"
    public string Status { get; set; } = "Recebido"; // Ex: Recebido, Aceito, Pronto, Cancelado
    public string? CancelReason { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
