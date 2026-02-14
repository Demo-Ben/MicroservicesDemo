using System.Net.Http.Json;
using Contracts.Inventory;
using Contracts.Orders;
using Contracts.Payments;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure HttpClients for other services
builder.Services.AddHttpClient("InventoryService", client =>
{
    client.BaseAddress = new Uri("https://localhost:5003");
});

builder.Services.AddHttpClient("PaymentService", client =>
{
    client.BaseAddress = new Uri("https://localhost:5002");
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// In-memory orders store
var orders = new Dictionary<Guid, string>();

app.MapPost("/api/orders", async (
    CreateOrderRequest request,
    IHttpClientFactory httpClientFactory) =>
{
    var inventoryClient = httpClientFactory.CreateClient("InventoryService");
    var paymentClient = httpClientFactory.CreateClient("PaymentService");

    // 1. Reserve stock for each item
    foreach (var item in request.Items)
    {
        var reserveReq = new ReserveStockRequest(item.ProductId, item.Quantity);
        var reserveResponse = await inventoryClient
            .PostAsJsonAsync("/api/inventory/reserve", reserveReq);

        var reserveContent = await reserveResponse.Content
            .ReadFromJsonAsync<ReserveStockResponse>();

        if (reserveContent is null || !reserveContent.Success)
        {
            return Results.Ok(new CreateOrderResponse(
                Guid.Empty,
                "Failed",
                $"Stock reservation failed: {reserveContent?.Message}"
            ));
        }
    }

    // 2. Process payment
    var paymentReq = new ProcessPaymentRequest(
        request.CustomerId,
        request.TotalAmount,
        request.PaymentMethod
    );

    var paymentResponse = await paymentClient
        .PostAsJsonAsync("/api/payments/process", paymentReq);

    var paymentContent = await paymentResponse.Content
        .ReadFromJsonAsync<ProcessPaymentResponse>();

    if (paymentContent is null || !paymentContent.Success)
    {
        return Results.Ok(new CreateOrderResponse(
            Guid.Empty,
            "Failed",
            $"Payment failed: {paymentContent?.Message}"
        ));
    }

    // 3. Create order
    var orderId = Guid.NewGuid();
    orders[orderId] = "Confirmed";

    return Results.Ok(new CreateOrderResponse(
        orderId,
        "Confirmed",
        $"Order confirmed. TransactionId: {paymentContent.TransactionId}"
    ));
});

app.Run();


//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();
