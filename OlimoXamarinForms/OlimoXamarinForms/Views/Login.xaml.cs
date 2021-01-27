using Microsoft.Graph;
using Microsoft.Identity.Client;
using OlimoXamarinForms.Helpers;
using OlimoXamarinForms.Models;
using OlimoXamarinForms.Services;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OlimoXamarinForms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
        }

        private async void SignIn_Clicked(object sender, EventArgs e)
        {
            if(email.Text?.Length > 0 && password.Text?.Length > 0)
            {
                IsBusy.IsVisible = true;
                IsBusy.IsRunning = true;
                try
                {
                    App.FirebaseToken = await DependencyService.Get<IFirebaseAuthenticator>().LoginWithEmailPassword(email.Text.Trim().ToLower(), password.Text);
                    FirebaseUser userInfo = await FirebaseFirestoreHelper.GetUserInfo(email.Text.Trim().ToLower());
                    await Xamarin.Essentials.SecureStorage.SetAsync("isLogged", "1");
                    await Xamarin.Essentials.SecureStorage.SetAsync("UserEmail", email.Text.Trim().ToLower());
                    App.UserEmail = email.Text.Trim();
                    await Xamarin.Essentials.SecureStorage.SetAsync("PaymentId", userInfo.PaymentId);
                    App.PaymentId = userInfo.PaymentId;
                    await Xamarin.Essentials.SecureStorage.SetAsync("UserName", userInfo.Username);
                    App.UserName = userInfo.Username;
                    IsBusy.IsVisible = false;
                    IsBusy.IsRunning = false;
                    Xamarin.Forms.Application.Current.MainPage = new AppShell();
                    await Shell.Current.GoToAsync("//main");   
                }
                catch (Exception ex)
                {
                    IsBusy.IsVisible = false;
                    IsBusy.IsRunning = false;
                    DependencyService.Get<IFirebaseAuthenticator>().SignOut();
                    Xamarin.Essentials.SecureStorage.RemoveAll(); 
                    if (ex.GetType().ToString() == "Firebase.Auth.FirebaseAuthInvalidUserException")
                    {
                        await DisplayAlert("Oops", "We didn't find any user with that Email. Please register for a new Account", "Ok");
                    }
                    else if (ex.GetType().ToString() == "Firebase.Auth.FirebaseAuthInvalidCredentialsException")
                    {
                        await DisplayAlert("Oops", "Invalid Email/Password Combination. Please try again", "Ok");
                    }
                    else
                    {
                        await DisplayAlert("Oops", "Something went wrong. Please try again.", "Ok");
                    }
                }  
            }
            else
            {
                await DisplayAlert("Oops", "Please enter your Email and Password", "Ok");
            }
        }

        private async void newUserSignUp(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewUserSignUpPage());
        }

        private async void forgotPasswordClicked(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new PasswordResetPage());
        }
    }
}