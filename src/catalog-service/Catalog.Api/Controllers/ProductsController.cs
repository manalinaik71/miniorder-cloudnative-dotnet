using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers;

[ApiController]
[Route("api/v1/products")]
public class ProductsController : ControllerBase
{
    private static readonly List<ProductDto> _products =
    [
        new(Guid.NewGuid(), "Notebook", 3.50m),
        new(Guid.NewGuid(), "Coffee", 5.99m)
    ];

    [HttpGet]
    public ActionResult<IEnumerable<ProductDto>> GetAll() => Ok(_products);

    [HttpGet("{id:guid}")]
    public ActionResult<ProductDto> GetById(Guid id)
    {
        var p = _products.FirstOrDefault(x => x.Id == id);
        return p is null ? NotFound() : Ok(p);
    }

    [HttpPost]
    public ActionResult<ProductDto> Create([FromBody] CreateProductRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return BadRequest("Name is required.");

        var product = new ProductDto(Guid.NewGuid(), request.Name, request.Price);
        _products.Add(product);

        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    public record ProductDto(Guid Id, string Name, decimal Price);
    public record CreateProductRequest(string Name, decimal Price);
}