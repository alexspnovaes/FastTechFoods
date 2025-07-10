using FastTechFoods.KitchenService.Application.Commands.AcceptOrder;
using FastTechFoods.KitchenService.Application.Commands.MarkOrderReady;
using FastTechFoods.KitchenService.Application.Commands.RejectOrder;
using FastTechFoods.KitchenService.Application.Queries.GetKitchenOrderById;
using FastTechFoods.KitchenService.Application.Queries.GetPendingKitchenOrders;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FastTechFoods.KitchenService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "kitchen")] 
public class KitchenController : ControllerBase
{
    private readonly IMediator _mediator;

    public KitchenController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("pending")]
    public async Task<IActionResult> GetPending()
    {
        var result = await _mediator.Send(new GetPendingKitchenOrdersQuery());
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetKitchenOrderByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("{id:guid}/accept")]
    public async Task<IActionResult> Accept(Guid id)
    {
        await _mediator.Send(new AcceptOrderCommand(id));
        return NoContent();
    }

    [HttpPost("{id:guid}/reject")]
    public async Task<IActionResult> Reject(Guid id, [FromBody] string reason)
    {
        await _mediator.Send(new RejectOrderCommand(id, reason));
        return NoContent();
    }

    [HttpPost("{id:guid}/ready")]
    public async Task<IActionResult> MarkReady(Guid id)
    {
        await _mediator.Send(new MarkOrderReadyCommand(id));
        return NoContent();
    }
}
