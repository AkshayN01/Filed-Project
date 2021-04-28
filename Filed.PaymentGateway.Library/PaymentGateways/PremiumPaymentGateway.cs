using Filed.PaymentGateway.Contracts.PaymentModel;
using Filed.PaymentGateway.Interfaces.Gateway;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Filed.PaymentGateway.Library.PaymentGateways
{
    public class PremiumPaymentGateway : IPaymentGateway
    {
        private readonly List<String> PaymentStatuses = new List<string>() { "Failed", "Failed" };   //For test purpose

        public async Task<String> GetPaymentStatus(PaymentDetails paymentDetails)
        {
            Int32 failureCount = 0;
            String response = String.Empty;


            //Payment gateway integration code goes over here

            response = await PaymentGatewayAPI(paymentDetails);

            if (response == "Failed")
            {
                while (failureCount < 3 && response == "Failed")
                {
                    response = await PaymentGatewayAPI(paymentDetails);
                    failureCount++;
                }
            }
            
            return response;
        }

        //Actual Implementation goes over here
        private async Task<String> PaymentGatewayAPI(PaymentDetails paymentDetails)
        {
            String response = String.Empty;
            Random random = new Random();
            await Task.Delay(500);
            response = PaymentStatuses[random.Next(0, PaymentStatuses.Count)];
            return response;
        }
    }
}
