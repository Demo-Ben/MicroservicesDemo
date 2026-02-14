namespace Contracts.Orders;

public class CreateOrderRequest
{
    public Guid CustomerId { get; set; }
    public List<OrderItemDto> Items { get; set; }
    public decimal TotalAmount { get; set; }
    public string PaymentMethod { get; set; }
}

public class OrderItemDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}

public class CreateOrderResponse
{
    public Guid OrderId { get; set; }
    public string Status { get; set; }
    public string Message { get; set; }
}
