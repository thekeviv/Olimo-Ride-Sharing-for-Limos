using OlimoXamarinForms.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OlimoXamarinForms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddNewCard : ContentPage
    {
        public AddNewCard()
        {
            InitializeComponent();
            BindingContext = this;
        }


        private void AddNewPaymentMethod(object sender, EventArgs e)
        {
            string userId = PaymentHelper.CreateNewCustomer(App.UserEmail);

            if(cardNumber.Text=="" || expiryMonth.Text=="" || expiryYear.Text=="" ||
                cvv.Text=="")
            {
                DisplayAlert("Oops", "One or more fields have not been filled in", "Ok");
            }
            else
            {
                try
                {
                    PaymentHelper.AddNewPaymentOption(cardNumber.Text, Int32.Parse(expiryMonth.Text), Int32.Parse(expiryYear.Text), cvv.Text);
                }
                catch (Stripe.StripeException ex)
                {
                    DisplayAlert("Oops", ex.Message, "Ok");
                }
            }
        }
    }
}