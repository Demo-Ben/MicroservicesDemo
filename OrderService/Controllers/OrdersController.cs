using Contracts.Inventory;
using Contracts.Orders;
using Contracts.Payments;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace OrderService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IHttpClientFactory _clientFactory;

    public OrdersController(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    [HttpPost]
    public async Task<ActionResult<CreateOrderResponse>> CreateOrder(CreateOrderRequest request)
    {
        // different name httpFactory has different base address for each service,
        var _httpClientInventory = _clientFactory.CreateClient("Inventory");
        var _httpClientPayment = _clientFactory.CreateClient("Payment");

        // test request body
        //{
        //    "customerId": "22222222-2222-2222-2222-222222222222",
        //      "items": [
        //        {
        //           "productId": "11111111-1111-1111-1111-111111111111",
        //           "quantity": 2
        //        },
		//        {
        //           "productId": "22222222-2222-2222-2222-222222222222",
        //           "quantity": 10
        //        }
        //      ],
        //      "totalAmount": 150.00,
        //      "paymentMethod": "CreditCard"
        // }


        // 1. Reserve stock
        foreach (var item in request.Items)
        {
            var reserveReq = new ReserveStockRequest
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity
            };

            var reserveRes = await _httpClientInventory.PostAsJsonAsync("/api/inventory/reserve", reserveReq);
            var reserve = await reserveRes.Content.ReadFromJsonAsync<ReserveStockResponse>();

            if (reserve is null || !reserve.Success)
            {
                return Ok(new CreateOrderResponse
                {
                    Status = "Failed",
                    Message = reserve?.Message
                });
            }
        }

        // 2. Process payment
        var paymentReq = new ProcessPaymentRequest
        {
            CustomerId = request.CustomerId,
            Amount = request.TotalAmount,
            PaymentMethod = request.PaymentMethod
        };

        var paymentRes = await _httpClientPayment.PostAsJsonAsync("/api/payment/process", paymentReq);
        var paymentResult = await paymentRes.Content.ReadFromJsonAsync<ProcessPaymentResponse>();

        if (paymentResult is null || !paymentResult.Success)
        {
            return Ok(new CreateOrderResponse
            {
                Status = "Failed",
                Message = paymentResult?.Message
            });
        }

        // 3. Create order
        return Ok(new CreateOrderResponse
        {
            OrderId = Guid.NewGuid(),
            Status = "Confirmed",
            Message = $"Order confirmed. TransactionId: {paymentResult.TransactionId}"
        });
    }
}
