using OlimoXamarinForms.Helpers;
using OlimoXamarinForms.Models;
using OlimoXamarinForms.ViewModels;
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
    public partial class RideOptions : ContentPage
    {
        HomePageViewModel viewModel;
        List<UserLocation> availableDrivers;
        public RideOptions(HomePageViewModel _viewModel)
        {
            this.viewModel = _viewModel;
            BindingContext = viewModel;
            viewModel.MaxTripPassengers = 12;
            GetDrivers(viewModel.MaxTripPassengers);
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
            InitializeComponent();
        }


        public async void GetDrivers(int maxPassengers)
        {
            viewModel.AddLimo(Constants.Sedan);
            viewModel.AddLimo(Constants.Suv);
            viewModel.AddLimo(Constants.UtilityVan);
            viewModel.AddLimo(Constants.DeluxeVan);
            viewModel.AddLimo(Constants.StretchVan);
            viewModel.AddLimo(Constants.Stretch);
            viewModel.AddLimo(Constants.LimoBus);
            viewModel.SelectedRideOption = viewModel.AvailableRideOptions[0];
            await DatabaseHelper.GetLimosInRadiusInTime(20, 10, Convert.ToDouble(viewModel._originLatitude), Convert.ToDouble(viewModel._originLongitude), maxPassengers);
            while (DatabaseHelper.driversInPastMin == null)
            { }
            this.availableDrivers = DatabaseHelper.driversInPastMin;

        }
        private void ridesCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            viewModel.SelectedRideOption = e.CurrentSelection.FirstOrDefault() as OlimoXamarinForms.Models.RideOption;
            OnPropertyChanged();
        }

        private void openRideSummaryPage(object sender, EventArgs e)
        {
            Shell.Current.Navigation.PushAsync(new RideSummary(viewModel));
        }

        private void SelectPreviousRideOption(object sender, EventArgs e)
        {
            RideOption currentSelection = viewModel.SelectedRideOption;
            int lengthOfRideOptions = viewModel.AvailableRideOptions.Count;
            int currentIndex = viewModel.AvailableRideOptions.IndexOf(currentSelection);
            if(currentIndex - 1 >= 0)
            {
                viewModel.SelectedRideOption = viewModel.AvailableRideOptions[currentIndex - 1];
            }
        }

        private void SelectNextRideOption(object sender, EventArgs e)
        {
            RideOption currentSelection = viewModel.SelectedRideOption;
            int lengthOfRideOptions = viewModel.AvailableRideOptions.Count;
            int currentIndex = viewModel.AvailableRideOptions.IndexOf(currentSelection);
            if (currentIndex + 1 < lengthOfRideOptions)
            {
                viewModel.SelectedRideOption = viewModel.AvailableRideOptions[currentIndex + 1];
            }
        }
    }
}