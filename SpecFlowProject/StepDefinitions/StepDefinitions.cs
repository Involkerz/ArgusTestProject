using CheckoutEndpoint.Endpoint;
using CheckoutEndpoint.Endpoint.Entities;
using FluentAssertions;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace SpecFlowProject.StepDefinitions;

[Binding]
public class StepDefinitions
{
    private readonly List<OrderItem> _currentOrders = [];
    private double _currentBill;

    [StepDefinition(@"Visitors place an order at '([^']*)'")]
    public void VisitorsPlaceTheOrder(TimeSpan orderTime, Table table)
    {
        ValidateOrderTime(orderTime);

        var (startersCount, mainsCount, drinksCount) =
            table.CreateInstance<(int startersCount, int mainsCount, int drinksCount)>();

        for (var i = 0; i < startersCount; i++) _currentOrders.Add(new OrderItem(OrderTypeEnum.Starter, orderTime));
        for (var i = 0; i < mainsCount; i++) _currentOrders.Add(new OrderItem(OrderTypeEnum.Main, orderTime));
        for (var i = 0; i < drinksCount; i++) _currentOrders.Add(new OrderItem(OrderTypeEnum.Drink, orderTime));
    }

    [When(@"Visitor cancels the order from '([^']*)'")]
    public void VisitorCancelsTheOrder(TimeSpan orderTime, Table table)
    {
        ValidateOrderTime(orderTime);
        var (startersCount, mainsCount, drinksCount) =
            table.CreateInstance<(int startersCount, int mainsCount, int drinksCount)>();

        var ordersToRemove = new List<OrderItem>();

        for (var i = 0; i < startersCount; i++) ordersToRemove.Add(new OrderItem(OrderTypeEnum.Starter, orderTime));
        for (var i = 0; i < mainsCount; i++) ordersToRemove.Add(new OrderItem(OrderTypeEnum.Main, orderTime));
        for (var i = 0; i < drinksCount; i++) ordersToRemove.Add(new OrderItem(OrderTypeEnum.Drink, orderTime));

        foreach (var itemToRemove in ordersToRemove.Select(order =>
                     _currentOrders.FindLast(o => o.OrderType == order.OrderType && o.OrderTime == order.OrderTime)))
        {
            if (itemToRemove is null)
                throw new InvalidOperationException("Can't remove an order as no matching existing orders found!");

            _currentOrders.Remove(itemToRemove);
        }
    }


    [When(@"Visitors request the final bill")]
    public void VisitorsRequestTheFinalBill()
    {
        _currentBill = OrderCalculationEndpoint.GetFinalBillSum(_currentOrders);
    }

    [Then(@"The bill is calculated and is '(.*)'£")]
    public void TheBillIsCalculatedAndIs(double expectedBill)
    {
        _currentBill.Should().Be(expectedBill);
    }

    private static void ValidateOrderTime(TimeSpan orderTime)
    {
        if (orderTime >= new TimeSpan(24, 0, 0)) throw new ArgumentException();
    }
}