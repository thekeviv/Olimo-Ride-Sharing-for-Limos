using OlimoXamarinForms.Services;
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
    public partial class PasswordResetPage : PopupPage
    {
        public PasswordResetPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if(email.Text?.Length>0)
            {
                try
                {
                    await DependencyService.Get<IFirebaseAuthenticator>().SendPasswordResetEmail(email.Text.Trim());
                    await DisplayAlert("Done", "The password reset Email was successfully sent. Please follow the instructions in the Email to finish resetting the password.", "Ok");
                }
                catch(Exception ex)
                {
                    if (ex.GetType().ToString() == "Firebase.Auth.FirebaseAuthInvalidUserException")
                    {
                        await DisplayAlert("Oops", "We didn't find any user with that Email. Please register for a new Account or contact us at kcstechnologies@outlook.com", "Ok");
                    }
                    else
                    {
                        await DisplayAlert("Oops", "Something went wrong. Please try again.", "Ok");
                    }
                }

            }
            else
            {
                await DisplayAlert("Oops", "Please enter a valid Email", "Ok");
            }
        }
    }
}