namespace CicdLab.Api;

public static class DiscountCalculator
{
    public static decimal Apply(decimal price, decimal percent)
    {
        if (price < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(price), "Price must be zero or greater.");
        }
        if (percent is < 0 or > 100) 
        { 
            throw new ArgumentOutOfRangeException(nameof(percent), "Percent must be between 0 and 100.");
        }

        return Math.Round(price - price * percent / 100m, 2);
    }
}
