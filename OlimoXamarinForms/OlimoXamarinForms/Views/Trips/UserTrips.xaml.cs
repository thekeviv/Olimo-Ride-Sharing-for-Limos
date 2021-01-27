using OlimoXamarinForms.ViewModels;
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
    public partial class UserTrips : TabbedPage
    {
        UserTripsViewModel viewModel;

        public UserTrips()
        {
            InitializeComponent();
            viewModel = new UserTripsViewModel();
            
            Children.Add(new FutureTrips(viewModel));
            Children.Add(new PastTrips(viewModel));
        }
    }
}