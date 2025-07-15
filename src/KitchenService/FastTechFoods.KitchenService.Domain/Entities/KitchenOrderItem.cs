namespace FastTechFoods.KitchenService.Domain.Entities;

public class KitchenOrderItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string ProductName { get; set; } = null!;
    public int Quantity { get; set; }
}
