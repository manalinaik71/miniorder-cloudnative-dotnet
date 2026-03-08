using Microsoft.EntityFrameworkCore;

namespace Order.Api.Data;
public class OrderDbContext : DbContext
{
    public OrderDbContext(DbContextOptions<OrderDbContext> option) : base(option)
    {
        
    }
    
    public DbSet<OrderEntity> Orders => Set<OrderEntity>();
    public DbSet<OrderItemEntity> OrderItems => Set<OrderItemEntity>();
}

public class OrderEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAtUTC { get; set; }
    public List<OrderItemEntity> Items { get; set; } = new();
}

public class OrderItemEntity
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }

    public Guid OrderEntityId { get; set; }
    public OrderEntity Order { get; set; } = default!;
}
