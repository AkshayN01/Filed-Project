using Filed.PaymentGateway.Interfaces.Gateway;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Filed.PaymentGateway.API.Controllers
{
    public class BaseController : ControllerBase
    {
        public BaseController(IPaymentGateway paymentGateway)
        {

        }
    }
}
