namespace CicdLab.Api;

public static class ShippingCalculator
{
    public static decimal Calculate(decimal weightKg, string zone, bool express)
    {
        if (weightKg <= 0)
            throw new ArgumentOutOfRangeException(nameof(weightKg), "Weight must be positive.");

        decimal baseRate;
        if (weightKg <= 1)
        {
            baseRate = 5m;
        }
        else if (weightKg <= 5)
        {
            baseRate = 12m;
        }
        else if (weightKg <= 20)
        {
            baseRate = 30m;
        }
        else
        {
            baseRate = 60m;
        }

        decimal zoneMultiplier;
        if (zone == "local") zoneMultiplier = 1m;
        else if (zone == "national") zoneMultiplier = 1.5m;
        else if (zone == "international") zoneMultiplier = 3m;
        else throw new ArgumentException("Unknown zone.", nameof(zone));

        var total = baseRate * zoneMultiplier;
        if (express)
        {
            total += 15m;
        }

        return total;
    }
}