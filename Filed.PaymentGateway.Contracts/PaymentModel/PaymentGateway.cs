using Filed.PaymentGateway.Contracts.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Filed.PaymentGateway.Contracts.PaymentModel
{
    public class PaymentDetails
    {
        public String CardHolderName { get; set; }
        public String CreditCardNumber { get; set; }
        public String ExpirationDate { get; set; }
        public String SecurityCode { get; set; }
        public Double Amount { get; set; }
    }

    #region " EFCore Model "

    public class Person : BaseEntity
    {
        public Person()
        {
            CardInformation = new List<CardInformation>();
        }

        [Required, MaxLength(50)]                 //Adding Maxlength attribute to avoid nvarchar-max for performance issues.
        public String FullName { get; set; }
        
        public bool IsDeleted { get; set; } = false;

        public List<CardInformation> CardInformation { get; set; }
    }

    public class CardInformation : BaseEntity
    {
        public CardInformation()
        {
            Transactions = new List<Transactions>();
        }

        [MaxLength(20)]
        public String SecurityCode { get; set; }

        [Required]
        public String CreditCardNumber { get; set; }

        [Required]
        public DateTime ExpirationDate { get; set; }
        
        public bool IsDeleted { get; set; } = false;

        public List<Transactions> Transactions { get; set; }
    }

    public class Transactions : BaseEntity
    {
        [Required, MaxLength(15)]
        public String PaymentState { get; set; } = "Processing"; // Adding default value
    }

    #endregion
}
