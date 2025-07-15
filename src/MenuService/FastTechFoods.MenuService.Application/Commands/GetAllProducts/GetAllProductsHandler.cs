using FastTechFoods.MenuService.Domain.Entities;
using FastTechFoods.MenuService.Domain.Interfaces;
using MediatR;

namespace FastTechFoods.MenuService.Application.Queries.GetAllProducts;

public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<Product>>
{
    private readonly IProductRepository _repo;

    public GetAllProductsHandler(IProductRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        return await _repo.GetAllAsync(request.Category);
    }
}
