using Microsoft.Identity.Client;
using OlimoXamarinForms.Services;
using OlimoXamarinForms.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OlimoXamarinForms
{
    public partial class App : Xamarin.Forms.Application
    {
        public static string FirebaseToken { get; set; }
        public static string UserName { get; set; }
        public static string UserEmail { get; set; }
        public static int isDriver { get; set; }
        public static double UserRating { get; set; }
        public static string UserPhone { get; set; }

        public static string PaymentId { get; set; }

        public static IPublicClientApplication AuthenticationClient { get; private set; }

        public static object UIParent { get; set; } = null;
        public static string AccessToken { get; set; }

        public static IPublicClientApplication PCA = null;
        public static string AppId = "[APP_ID_HERE]";
        public App()
        {
            InitializeComponent();
            PCA = PublicClientApplicationBuilder.Create(Constants.ClientId)
                .WithRedirectUri($"msal{Constants.ClientId}://auth")
                .WithIosKeychainSecurityGroup("com.microsoft.adalcache")
                .Build();

            AuthenticationClient = PublicClientApplicationBuilder.Create(Constants.ClientId)
                .WithIosKeychainSecurityGroup(Constants.IosKeychainSecurityGroups)
                .WithB2CAuthority(Constants.AuthoritySignin)
                .WithRedirectUri($"msal{Constants.ClientId}://auth")
                .Build();
            GoogleMapsApiService.Initialize(Constants.GoogleMapsApiKey);
            IGoogleMapsApiService googleMapsApi = new GoogleMapsApiService();
            //string refreshToken = Xamarin.Essentials.SecureStorage.GetAsync("RefreshToken").Result;
            var isLogged = Xamarin.Essentials.SecureStorage.GetAsync("IsLogged").Result;
            //FirebaseToken = DependencyService.Get<IFirebaseAuthenticator>().IsLoggedIn().Result;
            if (isLogged == "1")
            {
                UserName = Xamarin.Essentials.SecureStorage.GetAsync("UserName").Result;
                UserEmail = Xamarin.Essentials.SecureStorage.GetAsync("UserEmail").Result;
                PaymentId = Xamarin.Essentials.SecureStorage.GetAsync("PaymentId").Result;
                MainPage = new AppShell();
            }
            else
            {
                MainPage = new NavigationPage(new Login());
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
