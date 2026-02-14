using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Orders;

public record CreateOrderRequest(
    Guid CustomerId,
    List<OrderItemDto> Items,
    decimal TotalAmount,
    string PaymentMethod
);

public record OrderItemDto(
    Guid ProductId,
    int Quantity
);

public record CreateOrderResponse(
    Guid OrderId,
    string Status,
    string Message
);
