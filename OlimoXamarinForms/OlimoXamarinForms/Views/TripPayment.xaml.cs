using OlimoXamarinForms.Helpers;
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
    public partial class TripPayment : PopupPage
    {
        public HomePageViewModel viewModel;
        PaymentViewModel paymentsViewModel;
        public TripPayment(HomePageViewModel _viewModel)
        {
            InitializeComponent();
            this.viewModel = _viewModel;
            //this.paymentsViewModel = new PaymentViewModel();
            BindingContext = viewModel;
            //paymentsList.BindingContext = paymentsViewModel;
            GetTripMethods();
        }

        public void GetTripMethods()
        {
            
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
            return base.OnBackgroundClicked();
        }

        private async void TripBookedSuccess(object sender, EventArgs e)
        {
            await FirebaseFirestoreHelper.CreateTrip(App.UserEmail, App.UserName, "", viewModel.PickupLocation, viewModel.DestinationLocation, viewModel.TripDateAndTime.ToUniversalTime(), DateTime.UtcNow, 200, viewModel.messageForDriver, viewModel.SelectedRideOption.RideName);
            await Navigation.PopPopupAsync();
            await Shell.Current.Navigation.PushAsync(new TripBookedSuccess());
        }

        private async void AddNewPaymentOption(object sender, EventArgs e)
        {
            await Navigation.PopPopupAsync();
            Shell.Current.Navigation.PushAsync(new AddNewCard());
        }
    }
}