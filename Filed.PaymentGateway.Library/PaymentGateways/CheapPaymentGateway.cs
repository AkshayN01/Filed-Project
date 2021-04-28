using Filed.PaymentGateway.Contracts.PaymentModel;
using Filed.PaymentGateway.Interfaces.Gateway;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Filed.PaymentGateway.Library.PaymentGateways
{
    public class CheapPaymentGateway : IPaymentGateway
    {
        private readonly List<String> PaymentStatuses = new List<string>() { "Failed", "Success" };   //For test purpose

        public async Task<String> GetPaymentStatus(PaymentDetails paymentDetails)
        {
            Random random = new Random();
            await Task.Delay(500);

            //Payment gateway integration code goes over here


            return PaymentStatuses[random.Next(0,PaymentStatuses.Count)];
        }
    }
}
