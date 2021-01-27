using OlimoXamarinForms.ViewModels;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace OlimoXamarinForms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RideSummary : ContentPage
    {
        HomePageViewModel viewModel;
        public RideSummary(HomePageViewModel _viewModel)
        {
            InitializeComponent();
            Shell.SetNavBarIsVisible(this, false);
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
            this.viewModel = _viewModel;
            BindingContext = viewModel;
        }

        private async void SelectPaymentMethod(object sender, EventArgs e)
        {
            viewModel.messageForDriver = DriverMessage.Text;
            await Navigation.PushPopupAsync(new TripPayment(this.viewModel));
        }
    }
}