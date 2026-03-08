using Microsoft.AspNetCore.Mvc;
using Catalog.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Controllers;

[ApiController]
[Route("api/v1/products")]
public class ProductsController : ControllerBase
{
    /*private static readonly List<ProductDto> _products =
    [
        new(Guid.NewGuid(), "Notebook", 3.50m),
        new(Guid.NewGuid(), "Coffee", 5.99m)
    ];*/

    private readonly CatalogDbContext _db;

    public ProductsController(CatalogDbContext Db)
    {
        _db = Db;
    }

   /* [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll() => Ok(_Db.Products);

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductDto>> GetById(Guid id)
    {
        //var p = _products.FirstOrDefault(x => x.Id == id);
        var p = await _Db.Products.FirstOrDefault(x => x.Id == id);
        return p is null ? NotFound() : Ok(p);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> Create([FromBody] CreateProductRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return BadRequest("Name is required.");

        var product = new ProductDto(Guid.NewGuid(), request.Name, request.Price);
        _Db.Products.Add(product);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }*/

     [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll()
    {
        var products = await _db.Products
            .OrderBy(p => p.Name)
            .ToListAsync();

        var dto = products.Select(p => new ProductDto(p.Id, p.Name, p.Price));
        return Ok(dto);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductDto>> GetById(Guid id)
    {
        var p = await _db.Products.FirstOrDefaultAsync(x => x.Id == id);
        if (p is null) return NotFound();

        return Ok(new ProductDto(p.Id, p.Name, p.Price));
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> Create([FromBody] CreateProductRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return BadRequest("Name is required.");

        var entity = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Price = request.Price
        };

        _db.Products.Add(entity);
        await _db.SaveChangesAsync();

        var dto = new ProductDto(entity.Id, entity.Name, entity.Price);
        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }


    public record ProductDto(Guid Id, string Name, decimal Price);
    public record CreateProductRequest(string Name, decimal Price);
}