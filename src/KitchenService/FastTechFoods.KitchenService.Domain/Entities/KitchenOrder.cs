namespace FastTechFoods.KitchenService.Domain.Entities;

public class KitchenOrder
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid OriginalOrderId { get; set; }
    public string ClientName { get; set; } = null!;
    public List<KitchenOrderItem> Items { get; set; } = new();
    public string Status { get; set; } = "Aguardando"; // Aguardando, Aceito, Rejeitado, Pronto
    public string? RejectionReason { get; set; }
    public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
    public string DeliveryMethod { get; set; }
}
