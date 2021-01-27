using OlimoXamarinForms.Helpers;
using OlimoXamarinForms.Services;
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
    public partial class NewUserSignUpPage : ContentPage
    {
        public NewUserSignUpPage()
        {
            InitializeComponent();
        }

        private async void registerNewAccount(object sender, EventArgs e)
        {
            if(name.Text?.Length>0 && email.Text?.Length > 0 && password.Text?.Length>0)
            {
                try
                {
                    string token = await DependencyService.Get<IFirebaseAuthenticator>()
                        .SignUpWithEmailPassword(email.Text, password.Text, name.Text, isDriver.IsChecked);
                    string paymentId = PaymentHelper.CreateNewCustomer(email.Text.Trim());
                    await FirebaseFirestoreHelper.AddUserInfo(name.Text.Trim(), email.Text.Trim().ToLower(), isDriver.IsChecked, paymentId);
                    await Xamarin.Essentials.SecureStorage.SetAsync("isLogged", "1");
                    await Xamarin.Essentials.SecureStorage.SetAsync("UserEmail", email.Text.Trim());
                    App.UserEmail = email.Text.Trim();
                    await Xamarin.Essentials.SecureStorage.SetAsync("PaymentId", paymentId);
                    App.PaymentId = paymentId;
                    await Xamarin.Essentials.SecureStorage.SetAsync("UserName", name.Text.Trim());
                    App.UserName = name.Text.Trim();
                    Xamarin.Forms.Application.Current.MainPage = new AppShell();
                    await Shell.Current.GoToAsync("//main");
                }
                catch (Exception ex)
                {
                    if (ex.GetType().ToString() == "Firebase.Auth.FirebaseAuthInvalidCredentialsException")
                    {
                        await DisplayAlert("Oops", "Please check the Email is correct", "Ok");
                    }
                    else if (ex.GetType().ToString() == "Firebase.Auth.FirebaseAuthWeakPasswordException")
                    {
                        await DisplayAlert("Oops", "The password doesn't meet the minimum security requirements. Please re-enter a stronger Password", "Ok");
                    }
                    else if (ex.GetType().ToString() == "Firebase.Auth.FirebaseAuthUserCollisionException")
                    {
                        await DisplayAlert("Oops", "The entered Email has already been used before. Try logging in or resetting the password.", "Ok");
                    }
                    else
                    {
                        await DisplayAlert("Oops", "Something went wrong. Please try again.", "Ok");
                    }
                }
            }
            else
            {
                await DisplayAlert("Oops", "Some of the fields have not been filled in.", "Ok");
            }
        }
    }
}