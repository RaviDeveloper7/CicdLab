using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using CicdLab.Api;


namespace CicdLab.Tests
{
    public static class ShippingCalculatorTests
    {
        // create tests for the ShippingCalculator class in CicdLab.Api namespace using xUnit framework.
        // The tests should cover various scenarios including valid inputs, invalid inputs, and edge cases.
        // Use [Theory] and[InlineData] attributes to provide multiple test cases for each method.
        // Ensure that the tests assert the expected outcomes and handle exceptions appropriately.

        [Theory]
        [InlineData(0.5, "local", false, 5)]
        [InlineData(3, "national", true, 30)]
        [InlineData(10, "international", false, 90)]
        [InlineData(25, "local", true, 75)]
        public static void Calculate_ValidInputs_ReturnsExpected(decimal weightKg, string zone, bool express, decimal expected)
        {
            var result = ShippingCalculator.Calculate(weightKg, zone, express);
            Assert.Equal(expected, result);
        }




    }
}
