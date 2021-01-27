 using OlimoXamarinForms.Helpers;
using OlimoXamarinForms.Models;
using OlimoXamarinForms.Services;
using OlimoXamarinForms.ViewModels;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace OlimoXamarinForms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        #region Bindable properties
        public static readonly BindableProperty CenterMapCommandProperty =
            BindableProperty.Create(nameof(CenterMapCommand), typeof(ICommand), typeof(HomePage), null, BindingMode.TwoWay);

        public ICommand CenterMapCommand
        {
            get { return (ICommand)GetValue(CenterMapCommandProperty); }
            set { SetValue(CenterMapCommandProperty, value); }
        }

        public static readonly BindableProperty DrawRouteCommandProperty =
            BindableProperty.Create(nameof(DrawRouteCommand), typeof(ICommand), typeof(HomePage), null, BindingMode.TwoWay);

        public ICommand DrawRouteCommand
        {
            get { return (ICommand)GetValue(DrawRouteCommandProperty); }
            set { SetValue(DrawRouteCommandProperty, value); }
        }

        public static readonly BindableProperty UpdateCommandProperty =
          BindableProperty.Create(nameof(UpdateCommand), typeof(ICommand), typeof(HomePage), null, BindingMode.TwoWay);


        public ICommand UpdateCommand
        {
            get { return (ICommand)GetValue(UpdateCommandProperty); }
            set { SetValue(UpdateCommandProperty, value); }
        }

        public static readonly BindableProperty CleanPolylineCommandProperty =
          BindableProperty.Create(nameof(CleanPolylineCommand), typeof(ICommand), typeof(HomePage), null, BindingMode.TwoWay);


        public ICommand CleanPolylineCommand
        {
            get { return (ICommand)GetValue(CleanPolylineCommandProperty); }
            set { SetValue(CleanPolylineCommandProperty, value); }
        }


        public static readonly BindableProperty GetActualLocationCommandProperty =
            BindableProperty.Create(nameof(GetActualLocationCommand), typeof(ICommand), typeof(HomePage), null, BindingMode.TwoWay);

        public ICommand GetActualLocationCommand
        {
            get { return (ICommand)GetValue(GetActualLocationCommandProperty); }
            set { SetValue(GetActualLocationCommandProperty, value); }
        }
        #endregion
        HomePageViewModel viewModel;
        IGoogleMapsApiService googleMapsApi = new GoogleMapsApiService();

        //this constructor was created as when we come back from the EnterTripDetails, we need
        //to keep the same constructor. If we simply close the EnterTripDetails, the constructor
        //doesn't execute and we cant draw on the map. To do so, we use this constructor. It is
        //also useful when starting a new session after a trip has been booked as then a new viewmodel
        //can be passed. This constructor doesn't restart the timers for location tracking and for
        //dynamodb refresh token.
        public HomePage(HomePageViewModel _viewModel)
        {
            viewModel = _viewModel;
            
            GetActualLocationCommand = new Command(async () => await GetActualLocation());
            //if ((viewModel._originLatitude != null && viewModel._originLongitude != null) || (viewModel._destinationLatitude != null && viewModel._destinationLongitude != null))
            //{
            //    GetActualLocationCommand.Execute(null);
            //}
            InitializeComponent();
            //map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(49.282730, -123.120735),
            //                                Distance.FromKilometers(100)));
            if(viewModel.PickupLocation!=null && viewModel.PickupLocation!="" 
                && viewModel.DestinationLocation!=null && viewModel.DestinationLocation!="")
            {
                map.MoveToRegion(MapSpan.FromBounds(new Xamarin.Forms.GoogleMaps.Bounds(new Position(Double.Parse(viewModel._originLatitude),
                    Double.Parse(viewModel._originLongitude)), new Position(Double.Parse(viewModel._destinationLatitude),
                    Double.Parse(viewModel._destinationLongitude)))));
            }
            else
            {
                map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(49.282730, -123.120735),
                                             Distance.FromKilometers(100)));
            }
            BindingContext = viewModel;
            UpdateCommand = new Command<Xamarin.Forms.GoogleMaps.Position>(Update);
            CenterMapCommand = new Command<Location>(OnCenterMap);
            CleanPolylineCommand = new Command(CleanPolyline);
            AddMapStyle();
            map.MyLocationEnabled = true;
            map.UiSettings.MyLocationButtonEnabled = true;
            map.UiSettings.ZoomControlsEnabled = false;
            Shell.SetNavBarIsVisible(this, false);
            if ((viewModel._originLatitude != null && viewModel._originLongitude != null) &&
                (viewModel._destinationLatitude != null && viewModel._destinationLongitude != null))
            {
                Position start = new Position(Convert.ToDouble(viewModel._originLatitude), 
                    Convert.ToDouble(viewModel._originLongitude));
                Position end = new Position(Convert.ToDouble(viewModel._destinationLatitude), 
                    Convert.ToDouble(viewModel._destinationLongitude));
                DrawRoute(start, end);
            }
        }
        //this is the main constructor, the one above was created to keep the same view model 
        //when the code comes back from the EnterTripDetails class as then, we need to execute
        //the constructor to draw on the map
        public HomePage()
        {
            InitializeComponent();          
            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(49.282730, -123.120735), Distance.FromKilometers(100)));
            GetActualLocationCommand = new Command(async () => await GetActualLocation());
            viewModel = new HomePageViewModel();
            viewModel.UserName = App.UserName;
            ICommand updateFirebaseToken = new Command(async () => await UpdateFirebaseToken().ContinueWith(t => { Console.WriteLine("Firebase token updated"); }));
            updateFirebaseToken.Execute("");
            LocationTrackingHelper.StartLocationTracking();
            BindingContext = viewModel;
            UpdateCommand = new Command<Xamarin.Forms.GoogleMaps.Position>(Update);
            CenterMapCommand = new Command<Location>(OnCenterMap);
            CleanPolylineCommand = new Command(CleanPolyline);
            AddMapStyle();
            map.MyLocationEnabled = true;
            map.UiSettings.MyLocationButtonEnabled = true;
            map.UiSettings.ZoomControlsEnabled = false;
            //Shell.SetTabBarBackgroundColor(this, Color.Transparent);
            Shell.SetNavBarIsVisible(this, false);
            //DrawRouteCommand = new Command<List<Xamarin.Forms.GoogleMaps.Position>>(DrawRoute);

        }

        async Task UpdateFirebaseToken()
        {
            App.FirebaseToken = await DependencyService.Get<IFirebaseAuthenticator>().LoginWithEmailPassword("", "");                                                                                                                                                                                                                                                                                                                                                                                                                                
        }
        void AddMapStyle()
        {
            var assembly = typeof(App).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream($"OlimoXamarinForms.MapStyle.json");
            string styleFile;
            using (var reader = new System.IO.StreamReader(stream))
            {
                styleFile = reader.ReadToEnd();
            }

            map.MapStyle = MapStyle.FromJson(styleFile);
        }

        async void Update(Xamarin.Forms.GoogleMaps.Position position)
        {
            if (map.Pins.Count == 1 && map.Polylines != null && map.Polylines?.Count > 1)
                return;

            var cPin = map.Pins.FirstOrDefault();

            if (cPin != null)
            {
                cPin.Position = new Position(position.Latitude, position.Longitude);
                //cPin.Icon = (Device.RuntimePlatform == Device.Android) ? BitmapDescriptorFactory.FromBundle("ic_taxi.png") : BitmapDescriptorFactory.FromView(new Image() { Source = "ic_taxi.png", WidthRequest = 25, HeightRequest = 25 });

                await map.MoveCamera(CameraUpdateFactory.NewPosition(new Position(position.Latitude, position.Longitude)));
                var previousPosition = map?.Polylines?.FirstOrDefault()?.Positions?.FirstOrDefault();
                map.Polylines?.FirstOrDefault()?.Positions?.Remove(previousPosition.Value);
            }
            else
            {
                //END TRIP
                map.Polylines?.FirstOrDefault()?.Positions?.Clear();
            }

        }

        void CleanPolyline()
        {
            map.Polylines.Clear();
        }

        async void DrawRoute(Position start, Position end)
        {
            GoogleDirection googleDirection = await googleMapsApi.GetDirections(start.Latitude + "", start.Longitude + "",
                    end.Latitude + "", end.Longitude + "");
            if (googleDirection.Routes != null && googleDirection.Routes.Count > 0)
            {
                var positions = (Enumerable.ToList(PolylineHelper.Decode(googleDirection.Routes.First().OverviewPolyline.Points)));
                map.Polylines.Clear();
                var polyline = new Xamarin.Forms.GoogleMaps.Polyline();
                polyline.StrokeColor = Color.Black;
                polyline.StrokeWidth = 3;
                foreach (var p in positions)
                {
                    polyline.Positions.Add(p);

                }
                map.Polylines.Add(polyline);
                map.MoveToRegion(MapSpan.FromBounds(new Xamarin.Forms.GoogleMaps.Bounds(new Position(polyline.Positions[0].Latitude,
                    polyline.Positions[0].Longitude-0.008), new Position(polyline.Positions[polyline.Positions.Count - 1].Latitude,
                    polyline.Positions[polyline.Positions.Count - 1].Longitude+0.008))), true);
            }
        }


        void OnCenterMap(Location location)
        {
            map.MoveToRegion(MapSpan.FromCenterAndRadius(
                new Position(location.Latitude, location.Longitude), Distance.FromMiles(2)));
        }

        void LoadNearCars(Location location)
        {
            map.Polylines.Clear();
            map.Pins.Clear();
            for (int i = 0; i < 7; i++)
            {
                var random = new Random();

                map.Pins.Add(new Xamarin.Forms.GoogleMaps.Pin
                {
                    Type = PinType.Place,
                    Position = new Position(location.Latitude + (random.NextDouble() * 0.008), location.Longitude + (random.NextDouble() * 0.008)),
                    Label = "Car",
                    //Icon = (Device.RuntimePlatform == Device.Android) ? BitmapDescriptorFactory.FromBundle("ic_car.png") : BitmapDescriptorFactory.FromView(new Image() { Source = "ic_car.png", WidthRequest = 25, HeightRequest = 25 }),
                    Tag = string.Empty
                });
            }
        }

        //Desactivate pin tap
        void Map_PinClicked(object sender, PinClickedEventArgs e)
        {
            e.Handled = true;
        }

        public void HandleSearchContentView(bool show)
        {
            //searchContentView.IsVisible = show;
        }

        public void Handle_Tapped(object sender, EventArgs e)
        {

            //CustomMasterDetailPage.Current.IsPresented = true;
        }

        public void Handle_CameraIdled(object sender, CameraIdledEventArgs e)
        {
            //chooseLocationButton?.Command.Execute(map.CameraPosition.Target);
        }

        public void OnDoneClicked(object sender, EventArgs e)
        {
            //headerSearch.FocusDestination();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var safeInsets = On<Xamarin.Forms.PlatformConfiguration.iOS>().SafeAreaInsets();
            base.OnAppearing();
            if (viewModel.focusOnCurrentLocation)
            {
                GetActualLocationCommand.Execute(null);
            }
            viewModel.focusOnCurrentLocation = true;
        }


        private async void openAddressScreen(object sender, FocusEventArgs e)
        {
            aEntry.Unfocus();
            await Navigation.PushPopupAsync(new EnterTripDetailsPage(viewModel));
        }

        private void showRideOptions(object sender, EventArgs e)
        {
            if(viewModel.DestinationLocation!=null && viewModel.PickupLocation!=null)
            {
                Shell.Current.Navigation.PushAsync(new RideOptions(viewModel));
            }
            else
            {
                DisplayAlert("Oops", "Please select a start and end location", "Ok");
            }
        }
        async Task GetActualLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.High);
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    map.MoveToRegion(MapSpan.FromCenterAndRadius(
                        new Position(location.Latitude, location.Longitude), Distance.FromMiles(0.3)));

                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Unable to get actual location", "Ok");
            }
        }


    }
}