using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace CicdLab.Tests;

public class ApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ApiTests(WebApplicationFactory<Program> factory) => _client = factory.CreateClient();

    [Fact]
    public async Task Health_ReturnsOk()
    {
        var response = await _client.GetAsync("/health");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Discount_ReturnsFinalPrice()
    {
        var json = await _client.GetStringAsync("/api/discount?price=100&percent=10");
        using var doc = JsonDocument.Parse(json);
        Assert.Equal(90m, doc.RootElement.GetProperty("final").GetDecimal());
    }

    [Fact]
    public async Task Discount_InvalidPercent_ReturnsBadRequest()
    {
        var response = await _client.GetAsync("/api/discount?price=100&percent=150");
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
