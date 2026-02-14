using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Inventory;

public record ReserveStockRequest(
    Guid ProductId,
    int Quantity
);

public record ReserveStockResponse(
    bool Success,
    string Message
);
