using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Auth;
using OlimoXamarinForms.Services;
using Xamarin.Forms;
using OlimoXamarinForms.Droid.PlatformImplementations;
using Android.Gms.Extensions;
//using Firebase.Firestore;

[assembly: Dependency(typeof(FirebaseAuthenticator))]
namespace OlimoXamarinForms.Droid.PlatformImplementations
{
    class FirebaseAuthenticator : IFirebaseAuthenticator
    {
        public async Task<string> LoginWithEmailPassword(string email, string password)
        {
            var user = FirebaseAuth.Instance.CurrentUser;
            if(user == null)
            {
                var loginUser = await FirebaseAuth.Instance.
                        SignInWithEmailAndPasswordAsync(email.Trim(), password);
                var token = await loginUser.User.GetIdTokenAsync(false);
                App.UserEmail = loginUser.User.Email;
                await Xamarin.Essentials.SecureStorage.SetAsync("IsLogged", "1");
                return token.Token;
            }
            else
            {
                var aToken = await FirebaseAuth.Instance.GetAccessToken(false);
                Firebase.Auth.GetTokenResult token = (Firebase.Auth.GetTokenResult)aToken;
                App.UserEmail = FirebaseAuth.Instance.CurrentUser.Email;
                await Xamarin.Essentials.SecureStorage.SetAsync("IsLogged", "1");
                return token.Token;
            }
        }

        public async Task<string> SignUpWithEmailPassword(string email, string password, string name, bool isDriver)
        {
            var newUser = await FirebaseAuth.Instance.CreateUserWithEmailAndPasswordAsync(email.Trim(), password);
            App.UserEmail = newUser.User.Email;
            var token = await newUser.User.GetIdTokenAsync(false);
            await Xamarin.Essentials.SecureStorage.SetAsync("IsLogged", "1");
            return token.Token;
        }

        public async Task SendPasswordResetEmail(string email)
        {
            await FirebaseAuth.Instance.SendPasswordResetEmailAsync(email.Trim());
        }

        public void SignOut()
        {
            FirebaseAuth.Instance.SignOut();
        }
    }
}