using OlimoXamarinForms.Helpers;
using Stripe;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace OlimoXamarinForms.ViewModels
{
    class PaymentViewModel : INotifyPropertyChanged
    {
        private List<PaymentMethod> paymentMethods;
        public List<PaymentMethod> PaymentMethods
        {
            get { return paymentMethods; }
            set
            {
                this.paymentMethods = value;
                PaymentMethodsAvailable = (paymentMethods.Count > 0) ? true : false;
                OnPropertyChanged("PaymentMethods");
            }
        }

        private bool paymentMethodsAvailable;

        public bool PaymentMethodsAvailable
        {
            get { return paymentMethodsAvailable; }
            set
            {
                paymentMethodsAvailable = value;
                OnPropertyChanged("PaymentMethodsAvailable");
            }
        }
        private bool isBusy;

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged("IsBusy");
            }
        }

        public PaymentViewModel()
        {
            this.IsBusy = true;
            GetPaymentOptions();
            this.IsBusy = false;
        }
        private async void GetPaymentOptions()
        {
            PaymentMethods = await PaymentHelper.GetPaymentMethods();
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
