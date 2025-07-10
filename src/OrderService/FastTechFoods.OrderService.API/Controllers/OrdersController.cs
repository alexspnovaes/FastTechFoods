using FastTechFoods.OrderService.Application.Commands.CancelOrder;
using FastTechFoods.OrderService.Application.Commands.CreateOrder;
using FastTechFoods.OrderService.Application.Queries.GetOrderById;
using FastTechFoods.OrderService.Application.Queries.GetOrdersByClientId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FastTechFoods.OrderService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "client")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, null);
    }

    [HttpPost("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id, [FromBody] string reason)
    {
        await _mediator.Send(new CancelOrderCommand(id, reason));
        return NoContent();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var order = await _mediator.Send(new GetOrderByIdQuery(id));
        return order == null ? NotFound() : Ok(order);
    }

    [HttpGet("client/{clientId:guid}")]
    public async Task<IActionResult> GetByClient(Guid clientId)
    {
        var orders = await _mediator.Send(new GetOrdersByClientIdQuery(clientId));
        return Ok(orders);
    }
}
