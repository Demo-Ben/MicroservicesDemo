using Contracts.Inventory;
using Microsoft.AspNetCore.Mvc;

namespace InventoryService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
    // In-memory product list for demonstration purposes
    private static readonly Dictionary<Guid, int> Stock = 
        new()
        {
            { Guid.Parse("11111111-1111-1111-1111-111111111111"), 100 }, 
            { Guid.Parse("22222222-2222-2222-2222-222222222222"), 50 }, 
            { Guid.Parse("33333333-3333-3333-3333-333333333333"), 75 }
        };

    // In-memory product list for demonstration purposes
    //private static readonly List<Product> _products = new()
    //{
    //    new Product { Id = 1, Name = "Keyboard", Price = 49.99m },
    //    new Product { Id = 2, Name = "Mouse", Price = 19.99m },
    //    new Product { Id = 3, Name = "Monitor", Price = 199.99m }
    //};


    [HttpPost("reserve")]
    public ActionResult<ReserveStockResponse> Reserve(ReserveStockRequest request)
    {
        if (!Stock.TryGetValue(request.ProductId, out var qty) || qty < request.Quantity)
        {
            return Ok(new ReserveStockResponse
            {
                Success = false,
                Message = "Insufficient stock"
            });
        }

        Stock[request.ProductId] -= request.Quantity;

        return Ok(new ReserveStockResponse
        {
            Success = true,
            Message = "Stock reserved " + request.ProductId,
        });
    }
}
