using OlimoXamarinForms.ViewModels;
using OlimoXamarinForms.Views;
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
    public partial class TripBookedSuccess : ContentPage
    {
        public TripBookedSuccess()
        {
            InitializeComponent();
            Shell.SetNavBarIsVisible(this, false);
        }

        private void OpenHomePage(object sender, EventArgs e)
        {
            Shell.Current.Navigation.PushAsync(new HomePage());
        }

        private void OpenRides(object sender, EventArgs e)
        {
            Shell.Current.Navigation.PushAsync(new UserTrips());
        }
    }
}