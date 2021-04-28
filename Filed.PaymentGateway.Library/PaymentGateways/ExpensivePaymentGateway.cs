using Filed.PaymentGateway.Contracts.PaymentModel;
using Filed.PaymentGateway.Interfaces.Gateway;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Filed.PaymentGateway.Library.PaymentGateways
{
    public class ExpensivePaymentGateway : IPaymentGateway
    {
        private readonly List<String> PaymentStatuses = new List<string>() { "Failed", "Success" };   //For test purpose

        public async Task<String> GetPaymentStatus(PaymentDetails paymentDetails)
        {
            String response = String.Empty;
            Random random = new Random();
            await Task.Delay(500);


            //Payment gateway integration code goes over here


            response = PaymentStatuses[random.Next(0, PaymentStatuses.Count)];

            if(response == "Failed")
            {
                CheapPaymentGateway cheapPaymentGateway = new CheapPaymentGateway();
                response = await cheapPaymentGateway.GetPaymentStatus(paymentDetails);
            }

            return response;
        }
    }
}
