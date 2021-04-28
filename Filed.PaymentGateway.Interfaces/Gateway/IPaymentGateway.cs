using Filed.PaymentGateway.Contracts.PaymentModel;
using System;
using System.Threading.Tasks;

namespace Filed.PaymentGateway.Interfaces.Gateway
{
    public interface IPaymentGateway
    { 
        Task<String> GetPaymentStatus(PaymentDetails paymentDetails);
    }
}
