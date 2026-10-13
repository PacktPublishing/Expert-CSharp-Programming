// Source code for: Expert CSharp Programming.
// Author: Christian Nagel.
// Licensed under the MIT License.

using Microsoft.Extensions.Logging;

sealed class PaymentService(ILogger<PaymentService> logger)
{
    public void Charge(int orderId, decimal amount)
    {
        logger.LogInformation("PaymentService: charging {Amount:C} for order {OrderId}", amount, orderId);
        // payment gateway call...
        logger.LogInformation("PaymentService: payment authorised for order {OrderId}", orderId);
    }
}
