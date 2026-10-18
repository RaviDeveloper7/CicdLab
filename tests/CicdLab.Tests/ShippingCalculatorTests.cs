using CicdLab.Api;
using Xunit;

namespace CicdLab.Tests;

public class ShippingCalculatorTests
{
    [Theory]
    [InlineData(0.5, "local", false, 5)]
    [InlineData(3, "national", true, 33)]
    [InlineData(10, "international", false, 90)]
    [InlineData(25, "local", true, 75)]
    public void Calculate_ValidInputs_ReturnsExpected(decimal weightKg, string zone, bool express, decimal expected)
    {
        var result = ShippingCalculator.Calculate(weightKg, zone, express);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void Calculate_InvalidWeight_Throws(decimal weightKg)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => ShippingCalculator.Calculate(weightKg, "local", false));
    }

    [Fact]
    public void Calculate_UnknownZone_Throws()
    {
        Assert.Throws<ArgumentException>(() => ShippingCalculator.Calculate(5, "unknown", false));
    }
}
