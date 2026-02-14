var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// "Name" http clients for Inventory and Payment services,
// which will be used in the OrderController to make HTTP requests to these services.
// Add HttpClient for Inventory and Payment services
builder.Services.AddHttpClient("Inventory", c =>
                {
                    c.BaseAddress = new Uri("https://localhost:6003");
                });

builder.Services.AddHttpClient("Payment", c =>
                {
                    c.BaseAddress = new Uri("https://localhost:6002");
                });


builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
