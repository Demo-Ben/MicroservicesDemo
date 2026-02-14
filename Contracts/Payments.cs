using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Payments;

public record ProcessPaymentRequest(
    Guid CustomerId,
    decimal Amount,
    string PaymentMethod
);

public record ProcessPaymentResponse(
    bool Success,
    string TransactionId,
    string Message
);
