using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OlimoXamarinForms.Models;
using Stripe;

namespace OlimoXamarinForms.Helpers
{
    public class PaymentHelper
    {

        public static string CreateNewCustomer(string email)
        {
            StripeConfiguration.ApiKey = Constants.StripeKey;
            var customerOptions = new CustomerCreateOptions
            {
                Email = email
            };
            var customerService = new CustomerService();
            Customer customer = customerService.Create(customerOptions);
            return customer.Id;
        }

        public static void AddNewPaymentOption(string cardNumber, int expiryMonth, int expiryYear, string cvc)
        {
            StripeConfiguration.ApiKey = Constants.StripeKey;
            var options = new PaymentMethodCreateOptions
            {
                Type = "card",
                Card = new PaymentMethodCardCreateOptions
                {
                    Number = cardNumber,
                    ExpMonth = expiryMonth,
                    ExpYear = expiryYear,
                    Cvc = cvc
                }
            };
            var service = new PaymentMethodService();
            PaymentMethod pmMethod =  service.Create(options);
            var attachOptions = new PaymentMethodAttachOptions
            {
                Customer = App.PaymentId
            };
            var attachService = new PaymentMethodService();
            attachService.Attach(pmMethod.Id, attachOptions);
        }

        public static void DeletePaymentOption(string customerId, string cardId)
        {
            StripeConfiguration.ApiKey = Constants.StripeKey;

            var service = new CardService();
            service.Delete(
              customerId,
              cardId
            );
        }

        public static async Task<List<PaymentMethod>> GetPaymentMethods()
        {
            StripeConfiguration.ApiKey = Constants.StripeKey;
            if (App.PaymentId == null || App.PaymentId.Length == 0)
            {
                FirebaseUser userDetails = await FirebaseFirestoreHelper.GetUserInfo(App.UserEmail);
                App.PaymentId = userDetails.PaymentId;
            }
            var options = new PaymentMethodListOptions
            {
                Customer = App.PaymentId,
                Type = "card"
            };
            var service = new PaymentMethodService();
            List<PaymentMethod> paymentMethods =  service.List(options).Data;
            return paymentMethods;
        }

        public static void CreateCharge(string customerId, int amount)
        {
            StripeConfiguration.ApiKey = Constants.StripeKey;
            var chargeOptions = new ChargeCreateOptions
            {
                Amount = amount,
                Currency = "cad",
                Customer = customerId,
            };
            var chargeService = new ChargeService();
            Charge charge = chargeService.Create(chargeOptions);
        }        
    }
}
