using Catalog.Api.Data;
using Catalog.Api.Domain;
using Catalog.Api.Contracts;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Default");

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CatalogDbContext>(options => options.UseNpgsql(connectionString));

var app = builder.Build();

using(var scope = app.Services.CreateScope()){
    var db = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
    db.Database.Migrate();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

//create a product
/*app.MapPost("/api/v1/products", async(CatalogDbContext db, Product product) =>
{
    db.Products.Add(product);
    await db.SaveChangesAsync();
    return Results.Created($"/api/v1/products/{product.Id}", product);
});*/

app.MapPost("/api/v1/products",async(CatalogDbContext db, CreateProductRequest request) =>
{
    var product = new Product
    {
        Name = request.name,
        Price = request.price
    };
    db.Products.Add(product);
    await db.SaveChangesAsync();
    return Results.Created($"/api/v1/products/{product.Id}", product);
});

//Read a product
app.MapGet("/api/v1/products",async (CatalogDbContext db) =>
{
    var products = await db.Products.AsNoTracking().ToListAsync();
    return Results.Ok(products);

});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
