using Microsoft.AspNetCore.Mvc;
using Order.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Order.Api.Controllers;

[ApiController]
[Route("api/v1/orders")]
public class OrdersController : ControllerBase
{
    private readonly OrderDbContext _db;
    public OrdersController(OrderDbContext db)
    {
        _db = db;
    }

   [HttpPost]
   public async Task<ActionResult<OrderDto>> Create([FromBody]CreateOrderRequest request)
    {
        if(request.Items == null || request.Items.Count == 0)
        {
            return BadRequest("Atleast one item should be created");
        }

        var orderEntity = new OrderEntity
        {
            Id = Guid.NewGuid(),
            CreatedAtUTC = DateTime.UtcNow,
            Items = request.Items.Select(i => new OrderItemEntity
            {
                Id = Guid.NewGuid(),
                ProductId = i.ProductId,
                Quantity = i.Quantity
            }).ToList()
        };

        _db.Orders.Add(orderEntity);
       await  _db.SaveChangesAsync();

        var dto = new OrderDto(
            orderEntity.Id,
            orderEntity.CreatedAtUTC,
            orderEntity.Items.Select(i=>new OrderItemDto(i.ProductId ,i.Quantity)).ToList()
        );

        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    } 


   [HttpGet("{id:guid}")]
   public async Task<ActionResult<OrderDto>> GetById(Guid id)
    {
        var order = await _db.Orders
                     .Include(o=>o.Items)
                     .FirstOrDefaultAsync(i=>i.Id == id);

        if(order is null) 
        return NotFound();

          var dto = new OrderDto
          (
            order.Id,
            order.CreatedAtUTC,
            order.Items.Select(i=>new OrderItemDto(i.ProductId,i.Quantity)).ToList()
          );

        return Ok(dto);

    }

   [HttpGet]
   public async Task<ActionResult<IEnumerable<OrderDto>>> GetAll()
    {
        var order = await _db.Orders
                     .Include(o=>o.Items)
                     .OrderByDescending(i=>i.CreatedAtUTC)
                     .ToListAsync();

       

         var dtos = order.Select(o =>
            new OrderDto(
                o.Id,
                o.CreatedAtUTC,
                o.Items.Select(i => new OrderItemDto(i.ProductId, i.Quantity)).ToList()
            )
        );

        return Ok(dtos);
    }


    public record CreateOrderRequest(List<CreateOrderItemRequest> Items);
    public record CreateOrderItemRequest(Guid ProductId, int Quantity);

    public record OrderDto(Guid Id, DateTime CreatedAtUTC, List<OrderItemDto> Items);
    public record OrderItemDto(Guid ProductId, int Quantity);
}