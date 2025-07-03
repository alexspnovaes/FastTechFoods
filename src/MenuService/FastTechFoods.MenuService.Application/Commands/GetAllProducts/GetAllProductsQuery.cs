using FastTechFoods.MenuService.Domain.Entities;
using MediatR;

namespace FastTechFoods.MenuService.Application.Queries.GetAllProducts;

public class GetAllProductsQuery : IRequest<IEnumerable<Product>> { }
