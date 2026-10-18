using CicdLab.Api;
using Xunit;

namespace CicdLab.Tests;

public class DiscountCalculatorTests
{
    [Theory]
    [InlineData(100, 10, 90)]
    [InlineData(200, 0, 200)]
    [InlineData(50, 100, 0)]
    [InlineData(19.99, 15, 16.99)]
    public void Apply_ReturnsDiscountedPrice(decimal price, decimal percent, decimal expected)
        => Assert.Equal(expected, DiscountCalculator.Apply(price, percent));

    [Theory]
    [InlineData(-1, 10)]
    [InlineData(100, -5)]
    [InlineData(100, 150)]
    public void Apply_InvalidInput_Throws(decimal price, decimal percent)
        => Assert.Throws<ArgumentOutOfRangeException>(() => DiscountCalculator.Apply(price, percent));
}
