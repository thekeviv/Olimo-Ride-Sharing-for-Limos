using Microsoft.Identity.Client;
using OlimoXamarinForms.Services;
using OlimoXamarinForms.ViewModels;
using OlimoXamarinForms.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OlimoXamarinForms
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : Shell
    {
        private ShellViewModel viewModel;
        public AppShell()
        {
            InitializeComponent();
            viewModel = new ShellViewModel();
            this.BindingContext = viewModel;
            logoutCommand.BindingContext = this;
        }

        public ICommand LogoutCommand => new Command(() => this.LogOutAsync());

        public void LogOutAsync()
        {
            DependencyService.Get<IFirebaseAuthenticator>().SignOut();
            Device.BeginInvokeOnMainThread(() =>
            {
                Xamarin.Essentials.SecureStorage.RemoveAll();
                Application.Current.MainPage = new NavigationPage(new Login());
            });
        }
    }
}