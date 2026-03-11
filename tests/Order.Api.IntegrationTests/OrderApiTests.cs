using System.Net;
using System.Net.Http.Json;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Order.Api.IntegrationTests;

public class OrderApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    public OrderApiTests(WebApplicationFactory<Program> factory)
    {
      _client = factory.CreateClient();
    }

    [Fact]
    public async Task Create_Order_then_VerifyOrderId_with_Catalog()
    { 
        // This product ID must already exist in Catalog DB
        var productId = Guid.Parse("7beeaffd-0c83-49c4-86b9-278dbea4a86e");

        var createRequest = new
        {
            Items = new[]
            {
                new
                {
                    ProductId = productId,
                    Quantity = 2
                }
            }
        };

        var postResponse = await _client.PostAsJsonAsync("/api/v1/orders", createRequest);
        postResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var postContent = await postResponse.Content.ReadAsStringAsync();
        postContent.Should().NotBeNullOrWhiteSpace();

        using var postJson = JsonDocument.Parse(postContent);
        var orderId = postJson.RootElement.GetProperty("id").GetGuid();

        var getOrderResponse = await _client.GetAsync($"/api/v1/orders/{orderId}");
        getOrderResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var getOrderContent = await getOrderResponse.Content.ReadAsStringAsync();
        getOrderContent.Should().NotBeNullOrWhiteSpace();
        
    }

    [Fact]
    public async Task GetOrderById()
    {
        var orderId = Guid.Parse("a777bcce-e8e0-4ee5-aca7-65bd4923c302");
        var response = await _client.GetAsync($"/api/v1/orders/{orderId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var getOrderId = await response.Content.ReadAsStringAsync();
        getOrderId.Should().NotBeNullOrWhiteSpace();
    }
}