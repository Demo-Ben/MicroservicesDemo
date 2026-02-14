using Contracts.Inventory;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Simple in-memory stock
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var stock = new Dictionary<Guid, int>();

// Seed some stock
var product1 = Guid.Parse("11111111-1111-1111-1111-111111111111");
stock[product1] = 100;

app.MapPost("/api/inventory/reserve", (ReserveStockRequest request) =>
{
    if (!stock.TryGetValue(request.ProductId, out var currentQty) || currentQty < request.Quantity)
    {
        return Results.Ok(new ReserveStockResponse(false, "Insufficient stock"));
    }

    stock[request.ProductId] = currentQty - request.Quantity;
    return Results.Ok(new ReserveStockResponse(true, "Stock reserved"));
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
