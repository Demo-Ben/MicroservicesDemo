using Contracts.Payments;
using Microsoft.AspNetCore.Mvc;

namespace PaymentService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    [HttpPost("process")]
    public ActionResult<ProcessPaymentResponse> Process(ProcessPaymentRequest request)
    {
        if (request.Amount <= 0)
        {
            return Ok(new ProcessPaymentResponse
            {
                Success = false,
                Message = "Invalid amount"
            });
        }

        return Ok(new ProcessPaymentResponse
        {
            Success = true,
            TransactionId = Guid.NewGuid().ToString("N"),
            Message = "Payment successful"
        });
    }
}
