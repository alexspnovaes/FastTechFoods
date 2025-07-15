using FastTechFoods.MenuService.Application.Commands.CreateProduct;
using FastTechFoods.MenuService.Application.Commands.DeleteProduct;
using FastTechFoods.MenuService.Application.Commands.UpdateProduct;
using FastTechFoods.MenuService.Application.Queries.GetAllProducts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FastTechFoods.MenuService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? category) =>
            Ok(await _mediator.Send(new GetAllProductsQuery(category)));

        [HttpPost]
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetAll), new { id }, null);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductCommand command)
        {
            if (id != command.Id) return BadRequest();
            var updated = await _mediator.Send(command);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteProductCommand(id));
            return NoContent();
        }
    }
}
