using OlimoXamarinForms.Models;
using OlimoXamarinForms.Services;
using OlimoXamarinForms.ViewModels;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
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
    public partial class EnterTripDetailsPage : PopupPage
    {
        IGoogleMapsApiService googleMapsApi = new GoogleMapsApiService();
        public HomePageViewModel viewModel;
        int i = 0;
        int j = 0;
        public EnterTripDetailsPage(HomePageViewModel _viewModel)
        {
            InitializeComponent();
            this.viewModel = _viewModel;
            BindingContext = viewModel;
            viewModel.MaxTripPassengers = 2;
            searchResultsDestination.IsVisible = false;
            searchResultsOrigin.IsVisible = false;
        }

        private async void closeAddressScreen(object sender, EventArgs e)
        {
            if (viewModel.PickupLocation == null || viewModel.DestinationLocation == null)
            {
                viewModel.focusOnCurrentLocation = true;
            }
            else
            {
                viewModel.focusOnCurrentLocation = false;
            }

            await Navigation.PopPopupAsync();
            await Shell.Current.Navigation.PushAsync(new HomePage(viewModel), false);
        }



        private void searchBar_TextChanged_Origin(object sender, TextChangedEventArgs e)
        {
            Xamarin.Forms.Entry bar = (Xamarin.Forms.Entry)sender;
            if (bar.Text.Length <= 2)
            {
                if(bar.Text.Length==0)
                {
                    viewModel._originLatitude = null;
                    viewModel._originLongitude = null;
                }
                searchResultsOrigin.IsVisible = false;
                viewModel.PickupLocation = bar.Text;
            }
            else
            {
                if(i%2!=0)
                {
                    searchResultsOrigin.IsVisible = true;
                    viewModel.GetAutoCompleteSuggestionsOrigin.Execute(bar.Text);
                    viewModel.PickupLocation = bar.Text;
                }
                i++;
            }

        }

        private void searchBar_TextChanged_Destination(object sender, TextChangedEventArgs e)
        {
            Xamarin.Forms.Entry bar = (Xamarin.Forms.Entry)sender;
            if (bar.Text.Length <= 2)
            {
                if(bar.Text.Length==0)
                {
                    viewModel._destinationLatitude = null;
                    viewModel._destinationLongitude = null;
                }
                searchResultsDestination.IsVisible = false;
                viewModel.DestinationLocation = bar.Text;
            }
            else
            {
                if(j%2!=0)
                {
                    searchResultsDestination.IsVisible = true;
                    viewModel.GetAutoCompleteSuggestionsDestination.Execute(bar.Text);
                    viewModel.DestinationLocation = bar.Text;
                }
                j++;
            }

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        // ### Methods for supporting animations in your popup page ###

        // Invoked before an animation appearing
        protected override void OnAppearingAnimationBegin()
        {
            base.OnAppearingAnimationBegin();
        }

        // Invoked after an animation appearing
        protected override void OnAppearingAnimationEnd()
        {
            base.OnAppearingAnimationEnd();
        }

        // Invoked before an animation disappearing
        protected override void OnDisappearingAnimationBegin()
        {
            base.OnDisappearingAnimationBegin();
        }

        // Invoked after an animation disappearing
        protected override void OnDisappearingAnimationEnd()
        {
            base.OnDisappearingAnimationEnd();
        }

        protected override Task OnAppearingAnimationBeginAsync()
        {
            return base.OnAppearingAnimationBeginAsync();
        }

        protected override Task OnAppearingAnimationEndAsync()
        {
            return base.OnAppearingAnimationEndAsync();
        }

        protected override Task OnDisappearingAnimationBeginAsync()
        {
            return base.OnDisappearingAnimationBeginAsync();
        }

        protected override Task OnDisappearingAnimationEndAsync()
        {
            return base.OnDisappearingAnimationEndAsync();
        }

        // ### Overrided methods which can prevent closing a popup page ###

        // Invoked when a hardware back button is pressed
        protected override bool OnBackButtonPressed()
        {
            // Return true if you don't want to close this popup page when a back button is pressed
            return base.OnBackButtonPressed();
        }

        // Invoked when background is clicked
        protected override bool OnBackgroundClicked()
        {
            // Return false if you don't want to close this popup page when a background of the popup page is clicked
            //return base.OnBackgroundClicked();
            return false;
        }

        private void searchResultsDestination_ItemSelected(object sender, SelectionChangedEventArgs e)
        {
            string destination = e.CurrentSelection.First().ToString();
            viewModel.DestinationLocation = destination;
            searchResultsDestination.IsVisible = false;
            for (int i = 0; i < viewModel.destinationPredictionsList.Count; i++)
            {
                if (viewModel.destinationPredictionsList[i].Description == destination)
                {
                    GooglePlace placeDetails = googleMapsApi.GetPlaceDetails(viewModel.destinationPredictionsList[i].PlaceId).Result;
                    viewModel._destinationLatitude = placeDetails.Latitude + "";
                    viewModel._destinationLongitude = placeDetails.Longitude + "";
                }
            }
        }

        private void searchResultsOrigin_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string origin = e.CurrentSelection.First().ToString();
            viewModel.PickupLocation = origin;
            searchResultsOrigin.IsVisible = false;
            for (int i = 0; i < viewModel.originPredictionsList.Count; i++)
            {
                if (viewModel.originPredictionsList[i].Description == origin)
                {
                    GooglePlace placeDetails = googleMapsApi.GetPlaceDetails(viewModel.originPredictionsList[i].PlaceId).Result;
                    viewModel._originLatitude = placeDetails.Latitude + "";
                    viewModel._originLongitude = placeDetails.Longitude + "";
                }
            }
        }

        private void stepperValueChanged(object sender, ValueChangedEventArgs e)
        {
            maxPassengers.Text = e.NewValue + "";
        }

        private void maxPassengers_TextChanged(object sender, TextChangedEventArgs e)
        {
            int number;
            if(e.NewTextValue.Length>0 && Int32.TryParse(e.NewTextValue, out number))
            {
                if(number>30)
                {
                    DisplayAlert("Oops", "We're sorry but we currently have vehicles available for upto 30 passengers max", "Ok");
                    viewModel.MaxTripPassengers = 30;
                }
                else
                {
                    viewModel.MaxTripPassengers = number;
                }
                
            }
            else if(e.NewTextValue.Length==0)
            {
                viewModel.MaxTripPassengers = 0;
            }
            else
            {
                viewModel.MaxTripPassengers = Int32.Parse(e.OldTextValue);
            }
            
        }
    }
}