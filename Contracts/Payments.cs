namespace Contracts.Payments;

public class ProcessPaymentRequest
{
    public Guid CustomerId { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; }
}

public class ProcessPaymentResponse
{
    public bool Success { get; set; }
    public string TransactionId { get; set; }
    public string Message { get; set; }
}
