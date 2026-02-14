using Contracts.Inventory;
using Microsoft.AspNetCore.Mvc;

namespace InventoryService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
    private static readonly Dictionary<Guid, int> Stock = new()
    {
        { Guid.Parse("11111111-1111-1111-1111-111111111111"), 100 }
    };

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
            Message = "Stock reserved"
        });
    }
}
