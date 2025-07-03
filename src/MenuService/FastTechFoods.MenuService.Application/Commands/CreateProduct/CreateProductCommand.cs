using MediatR;

namespace FastTechFoods.MenuService.Application.Commands.CreateProduct;

public record CreateProductCommand(string Name, string Description, decimal Price, string Category) : IRequest<Guid>;
