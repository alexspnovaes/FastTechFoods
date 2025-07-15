using MediatR;

namespace FastTechFoods.MenuService.Application.Commands.UpdateProduct;

public record UpdateProductCommand(Guid Id, string Name, string Description, decimal Price, string Category, bool IsAvailable) : IRequest<bool>;
