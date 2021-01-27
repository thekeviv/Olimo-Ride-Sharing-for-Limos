using OlimoXamarinForms.Helpers;
using OlimoXamarinForms.ViewModels;
using Stripe;
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
    public partial class PaymentOptions : ContentPage
    {
        List<PaymentMethod> PaymentMethods = new List<PaymentMethod>();

        private PaymentViewModel viewModel;
        
        public PaymentOptions()
        {
            InitializeComponent();
            this.viewModel = new PaymentViewModel();
            paymentMethodsAvailable.IsVisible = viewModel.PaymentMethodsAvailable ? false : true;
            BindingContext = viewModel;
        }
        

        private void AddNewPaymentMethod(object sender, EventArgs e)
        {
            Shell.Current.Navigation.PushAsync(new AddNewCard());
        }
    }
}