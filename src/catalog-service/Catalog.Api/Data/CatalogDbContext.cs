using Microsoft.EntityFrameworkCore;
namespace Catalog.Api.Data;

public class CatalogDbContext : DbContext
{
    public CatalogDbContext(DbContextOptions<CatalogDbContext> option):base(option){}

    public DbSet<Product> Products => Set<Product>();
}

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public decimal Price { get; set; }
}