using System.Net;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Catalog.Api.IntegrationTests;

public class CatalogApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    public CatalogApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_Catalog_Endpoint_Should_Return_Success()
    {
        var response = await _client.GetAsync("api/v1/products");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNullOrWhiteSpace();

        using var js = JsonDocument.Parse(content);
        js.RootElement.ValueKind.Should().NotBe(JsonValueKind.Undefined);
    }
}
