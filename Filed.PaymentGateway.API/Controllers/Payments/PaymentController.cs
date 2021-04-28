using Filed.PaymentGateway.Contracts.PaymentModel;
using Filed.PaymentGateway.DataAccess.Common;
using Filed.PaymentGateway.Interfaces.Gateway;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Filed.PaymentGateway.API.Controllers.Payments
{
    [Route("api/[controller]")]
    public class PaymentController : BaseController
    {
        private readonly Blanket.Payment.Payment _PaymentContext;

        public PaymentController(IPaymentGateway paymentGateway, PaymentContext paymentContext) : base(paymentGateway)
        {
            _PaymentContext = new Blanket.Payment.Payment(paymentGateway, paymentContext);
        }

        [HttpPost]
        [Route("make-payment")]
        public async Task<IActionResult> MakePayment(PaymentDetails paymentDetails, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                String response = await _PaymentContext.MakePayment(paymentDetails);
                return Ok(response);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
