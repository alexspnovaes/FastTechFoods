using MediatR;

namespace FastTechFoods.MenuService.Application.Commands.DeleteProduct;

public record DeleteProductCommand(Guid Id) : IRequest;
