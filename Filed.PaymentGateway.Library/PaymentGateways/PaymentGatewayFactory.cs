using Filed.PaymentGateway.Interfaces.Gateway;
using System;
using System.Collections.Generic;
using System.Text;

namespace Filed.PaymentGateway.Library.PaymentGateways
{
    public class PaymentGatewayFactory
    {
        public IPaymentGateway GetPaymentGateway(Double Amount)
        {
            if(Amount <= 20)
            {
                return new CheapPaymentGateway();
            }
            else if(Amount > 20 && Amount <= 500)
            {
                return new ExpensivePaymentGateway();
            }
            else
            {
                return new PremiumPaymentGateway();
            }
        }
    }
}
