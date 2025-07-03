using FastTechFoods.MenuService.Domain.Interfaces;
using MediatR;

namespace FastTechFoods.MenuService.Application.Commands.DeleteProduct;

public class DeleteProductHandler : IRequestHandler<DeleteProductCommand>
{
    private readonly IProductRepository _repo;

    public DeleteProductHandler(IProductRepository repo)
    {
        _repo = repo;
    }

    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        await _repo.DeleteAsync(request.Id);
    }
}
