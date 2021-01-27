using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using OlimoXamarinForms.Models;
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
    public partial class FutureTrips : ContentPage
    {
        private UserTripsViewModel viewModel;
        public FutureTrips(UserTripsViewModel _viewModel)
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