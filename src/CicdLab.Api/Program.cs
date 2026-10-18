using CicdLab.Api;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/health", () => Results.Ok(new
{
    status = "Healthy",
    version = Environment.GetEnvironmentVariable("APP_VERSION") ?? "dev"
}));

app.MapGet("/api/discount", (decimal price, decimal percent) =>
{
    try
    {
        return Results.Ok(new { price, percent, final = DiscountCalculator.Apply(price, percent) });
    }
    catch (ArgumentOutOfRangeException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});

app.Run();

// Needed so the test project can use WebApplicationFactory<Program>.
public partial class Program { }
