using CheckoutEndpoint.Configuration;
using CheckoutEndpoint.Endpoint.Entities;

namespace CheckoutEndpoint.Endpoint;

public static class OrderCalculationEndpoint
{
    public static double GetFinalBillSum(List<OrderItem> orders)
    {
        var sum = orders.Sum(order => order.OrderType switch
        {
            OrderTypeEnum.Starter => CalculateStarterPrice(order),
            OrderTypeEnum.Main => CalculateMainPrice(order),
            OrderTypeEnum.Drink => CalculateDrinkPrice(order),
            _ => throw new ArgumentOutOfRangeException()
        });

        return Math.Round(sum, 2);
    }

    private static double CalculateStarterPrice(OrderItem orderItem)
    {
        if (orderItem.OrderType != OrderTypeEnum.Starter)
            throw new ArgumentException(
                $"Invalid order type provided. Expected {OrderTypeEnum.Starter} while provided {orderItem.OrderType}");

        return MenuPrices.StarterPrice * (1 + MenuPrices.FoodServiceCharge);
    }

    private static double CalculateMainPrice(OrderItem orderItem)
    {
        if (orderItem.OrderType != OrderTypeEnum.Main)
            throw new ArgumentException(
                $"Invalid order type provided. Expected {OrderTypeEnum.Main} while provided {orderItem.OrderType}");

        return MenuPrices.MainPrice * (1 + MenuPrices.FoodServiceCharge);
    }

    private static double CalculateDrinkPrice(OrderItem orderItem)
    {
        if (orderItem.OrderType != OrderTypeEnum.Drink)
            throw new ArgumentException(
                $"Invalid order type provided. Expected {OrderTypeEnum.Drink} while provided {orderItem.OrderType}");

        if (orderItem.OrderTime >= MenuPrices.DiscountTime)
            return MenuPrices.DrinkPrice;

        if (MenuPrices.DrinkDiscount > 1)
            throw new ArgumentException(
                "The provided drinks discount if more than 100%. Please check your configuration!");

        return MenuPrices.DrinkPrice * (1 - MenuPrices.DrinkDiscount);
    }
}