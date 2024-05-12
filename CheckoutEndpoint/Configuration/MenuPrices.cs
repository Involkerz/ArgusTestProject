using System;

namespace CheckoutEndpoint.Configuration;

/// <summary>
///     Contains prices and discounts/extra charges for food and drinks
/// </summary>
public static class MenuPrices
{
    public const double StarterPrice = 4.00;
    public const double MainPrice = 7.00;
    public const double DrinkPrice = 2.50;

    public const double DrinkDiscount = 0.3; // 30% for orders before 19:00
    public const double FoodServiceCharge = 0.1; // 10% extra service charge on mains and starters
    public static TimeSpan DiscountTime = new(19, 0, 0);
}