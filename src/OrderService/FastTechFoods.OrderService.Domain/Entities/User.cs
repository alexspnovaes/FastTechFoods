namespace FastTechFoods.OrderService.Domain.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? CPF { get; set; }
    public string PasswordHash { get; set; } = null!;
    public string Role { get; set; } = "client";
}
