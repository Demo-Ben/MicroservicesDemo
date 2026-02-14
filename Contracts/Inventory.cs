namespace Contracts.Inventory;

public class ReserveStockRequest
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}

public class ReserveStockResponse
{
    public bool Success { get; set; }
    public string Message { get; set; }
}
