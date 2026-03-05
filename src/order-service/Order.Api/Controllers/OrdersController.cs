using Microsoft.AspNetCore.Mvc;

namespace Order.Api.Controllers;

[ApiController]
[Route("api/v1/orders")]
public class OrdersController : ControllerBase
{
    private static readonly List<OrderDto> _orders = [];

    [HttpPost]
    public ActionResult<OrderDto> Create([FromBody] CreateOrderRequest request)
    {
        if (request.Items is null || request.Items.Count == 0)
            return BadRequest("At least one item is required.");

        var order = new OrderDto(
            Id: Guid.NewGuid(),
            CreatedAtUtc: DateTime.UtcNow,
            Items: request.Items.Select(i => new OrderItemDto(i.ProductId, i.Quantity)).ToList()
        );

        _orders.Add(order);
        return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
    }

    [HttpGet("{id:guid}")]
    public ActionResult<OrderDto> GetById(Guid id)
    {
        var order = _orders.FirstOrDefault(o => o.Id == id);
        return order is null ? NotFound() : Ok(order);
    }

    [HttpGet]
    public ActionResult<IEnumerable<OrderDto>> GetAll()
    {
        return Ok(_orders);
    }

    public record CreateOrderRequest(List<CreateOrderItemRequest> Items);
    public record CreateOrderItemRequest(Guid ProductId, int Quantity);

    public record OrderDto(Guid Id, DateTime CreatedAtUtc, List<OrderItemDto> Items);
    public record OrderItemDto(Guid ProductId, int Quantity);
}