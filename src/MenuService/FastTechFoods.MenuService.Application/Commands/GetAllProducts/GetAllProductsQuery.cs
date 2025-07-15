using FastTechFoods.MenuService.Domain.Entities;
using MediatR;

namespace FastTechFoods.MenuService.Application.Queries.GetAllProducts;

public record GetAllProductsQuery(string? Category) : IRequest<IEnumerable<Product>> { }
