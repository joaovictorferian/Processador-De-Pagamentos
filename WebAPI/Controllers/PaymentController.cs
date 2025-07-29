using Microsoft.AspNetCore.Mvc;
using Application.UseCases.Pagamentos;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly PaymentUseCase _paymentUseCase;

    public PaymentController(PaymentUseCase paymentUseCase)
    {
        _paymentUseCase = paymentUseCase;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePayment([FromBody] decimal amount)
    {
        await _paymentUseCase.ProcessPaymentAsync(amount);
        return Accepted();
    }
}
