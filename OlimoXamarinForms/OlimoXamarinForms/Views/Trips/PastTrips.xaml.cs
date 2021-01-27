using OlimoXamarinForms.ViewModels;
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
    public partial class PastTrips : ContentPage
    {
        UserTripsViewModel viewModel;
        public PastTrips(UserTripsViewModel _viewModel)
        {
            InitializeComponent();
            viewModel = _viewModel;
            BindingContext = viewModel;
        }

        private void OpenHomePage(object sender, EventArgs e)
        {
            Shell.Current.Navigation.PushAsync(new HomePage());
        }
    }
}