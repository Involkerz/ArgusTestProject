namespace CheckoutEndpoint.Endpoint.Entities;

public class OrderItem(OrderTypeEnum orderType, TimeSpan orderTime)
{
    public OrderTypeEnum OrderType { get; set; } = orderType;
    public TimeSpan OrderTime { get; set; } = orderTime;
}