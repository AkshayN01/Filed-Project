using Filed.PaymentGateway.Contracts.PaymentModel;
using Filed.PaymentGateway.DataAccess.Common;
using Filed.PaymentGateway.DataAccess.Repository;
using Filed.PaymentGateway.Interfaces.Gateway;
using Filed.PaymentGateway.Interfaces.Repository;
using Filed.PaymentGateway.Library.Encryption;
using Filed.PaymentGateway.Library.PaymentGateways;
using Filed.PaymentGateway.Library.Utility;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Filed.PaymentGateway.Blanket.Payment
{
    public class Payment
    {
        private PaymentContext _PaymentContext;
        private IPaymentGateway _PaymentGateway;
        private readonly RSAEncryption _Encryption;
        private readonly IBaseRepository<Person> _PeopleRepo;
        private readonly IBaseRepository<CardInformation> _CardInfoRepo;
        private readonly IBaseRepository<Transactions> _TransactionRepo;

        private readonly PaymentGatewayFactory _PaymentGatewayFactory;

        public Payment(IPaymentGateway paymentGateway, PaymentContext paymentContext)
        {
            _PaymentContext = paymentContext;
            _Encryption = new RSAEncryption();
            _PaymentGatewayFactory = new PaymentGatewayFactory();
            _PeopleRepo = new GenericRepository<Person>(paymentGateway, paymentContext);
            _CardInfoRepo = new GenericRepository<CardInformation>(paymentGateway, paymentContext);
            _TransactionRepo = new GenericRepository<Transactions>(paymentGateway, paymentContext);
        }

        public async Task<String> MakePayment(PaymentDetails paymentDetails)
        {
            String response = String.Empty;

            try
            {
                //Using Custom Credit Card Validation Instead of CreditCardAttribute
                if (!String.IsNullOrEmpty(paymentDetails?.CreditCardNumber) && Validation.IsCardNumberValid(paymentDetails.CreditCardNumber))
                {
                    if(paymentDetails.Amount > 0 && Validation.IsExpirationDateValid(paymentDetails.ExpirationDate))
                    {
                        //String encryptedCardNumber = _Encryption.EncryptAndSave(paymentDetails.CreditCardNumber);
                        String encryptedCardNumber = paymentDetails.CreditCardNumber;

                        //Checking for existing user.
                        var existing_user = _PaymentContext.People.Include(x => x.CardInformation)
                            .Where(x => x.FullName == paymentDetails.CardHolderName)
                            .FirstOrDefault();
                        if(existing_user is null){
                            // if the user doesn't exist , insert into the database
                            existing_user = AddNewUser(paymentDetails, encryptedCardNumber);
                        }
                        else
                        {
                            // checking whether the card exists or not
                            var card = existing_user.CardInformation.Where(x => x.CreditCardNumber == encryptedCardNumber).FirstOrDefault();
                            if(card is null)
                            {
                                CardInformation cardInformation = new CardInformation()
                                {
                                    CreditCardNumber = encryptedCardNumber,
                                    SecurityCode = paymentDetails.SecurityCode
                                };
                                existing_user.CardInformation.Add(cardInformation);
                                existing_user = _PeopleRepo.UpdateAndGet(existing_user);
                            }
                        }

                        //Creating a new transaction
                        Transactions NewTransaction = new Transactions();
                        _TransactionRepo.Insert(NewTransaction);
                        _TransactionRepo.Save();
                        existing_user.CardInformation.Where(x => x.CreditCardNumber == encryptedCardNumber).FirstOrDefault().Transactions.Add(NewTransaction);
                        existing_user = _PeopleRepo.UpdateAndGet(existing_user);

                        //Payment Gateway Call
                        _PaymentGateway = _PaymentGatewayFactory.GetPaymentGateway(paymentDetails.Amount);
                        response = await _PaymentGateway.GetPaymentStatus(paymentDetails);

                        //Updating the response
                        NewTransaction.PaymentState = response;
                        _TransactionRepo.Update(NewTransaction);
                        _TransactionRepo.Save();
                    }
                }
                else
                {
                    response = "Invalid Card Number";
                }
            }
            catch(Exception ex)
            {
                response = ex.Message;
            }

            return response;
        }
        
        private Person AddNewUser(PaymentDetails paymentDetails, string encryptedCardNumber)
        {
            Person person = new Person();
            try
            {   
                Person new_user = new Person() { FullName = paymentDetails.CardHolderName };
                CardInformation cardInformation = new CardInformation()
                {
                    CreditCardNumber = encryptedCardNumber,
                    SecurityCode = paymentDetails.SecurityCode
                };
                new_user.CardInformation.Add(cardInformation);
                person = _PeopleRepo.InsertAndGet(new_user);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return person;
        }
    }
}
