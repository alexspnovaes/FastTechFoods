using FastTechFoods.MenuService.Domain.Interfaces;
using MediatR;

namespace FastTechFoods.MenuService.Application.Commands.UpdateProduct;

public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, bool>
{
    private readonly IProductRepository _repo;

    public UpdateProductHandler(IProductRepository repo)
    {
        _repo = repo;
    }

    public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _repo.GetByIdAsync(request.Id);
        if (product is null) return false;

        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        product.Category = request.Category;
        product.IsAvailable = request.IsAvailable;

        await _repo.UpdateAsync(product);
        return true;
    }
}
