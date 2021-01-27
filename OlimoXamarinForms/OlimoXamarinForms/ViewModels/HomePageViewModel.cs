using OlimoXamarinForms.Models;
using OlimoXamarinForms.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace OlimoXamarinForms.ViewModels
{
    public class HomePageViewModel : INotifyPropertyChanged
    {

        //IList<RideOptions> CarOptions;
        IGoogleMapsApiService googleMapsApi = new GoogleMapsApiService();
        public bool focusOnCurrentLocation = true;
        private ObservableCollection<RideOption> availableRideOptions;
        public ObservableCollection<RideOption> AvailableRideOptions
        {
            get { return availableRideOptions; }
            set
            {
                if (availableRideOptions != value)
                {
                    availableRideOptions = value;
                    OnPropertyChanged("AvailableRideOptions");
                }
            }
        }
        RideOption selectedRideOption;
        public RideOption SelectedRideOption
        {
            get { return selectedRideOption; }
            set
            {
                if (selectedRideOption != value)
                {
                    selectedRideOption = value;
                    OnPropertyChanged("SelectedRideOption");
                }
            }
        }
        #region GoogleAutoCompleteSuggestion Origin
        public ICommand GetAutoCompleteSuggestionsOrigin => new Command<string>((string query) =>
        {
            List<string> searchResults = new List<string>();

            if (query.Length > 2)
            {
                GooglePlaceAutoCompleteResult originPredictions = googleMapsApi.GetPlaces(query).Result;
                originPredictionsList = originPredictions.AutoCompletePlaces.ToList();
                for (int i = 0; i < originPredictionsList.Count; i++)
                {
                    searchResults.Add(originPredictionsList[i].Description);
                }
                AutoCompletePredictionsListOrigin = searchResults;
            }
            else
            {
                return;
            }
        });
        public List<GooglePlaceAutoCompletePrediction> originPredictionsList { get; set; }
        List<string> autoCompletePredictionsListOrigin = null;
        public List<string> AutoCompletePredictionsListOrigin
        {
            get
            {
                return autoCompletePredictionsListOrigin;
            }
            set
            {
                autoCompletePredictionsListOrigin = value;
                OnPropertyChanged("AutoCompletePredictionsListOrigin");
            }
        }
        #endregion

        #region GoogleAutoCompleteSuggestion Destination
        public ICommand GetAutoCompleteSuggestionsDestination => new Command<string>((string query) =>
        {
            List<string> searchResults = new List<string>();

            if (query.Length > 2)
            {
                GooglePlaceAutoCompleteResult destinationPredictions = googleMapsApi.GetPlaces(query).Result;
                destinationPredictionsList = destinationPredictions.AutoCompletePlaces.ToList();
                for (int i = 0; i < destinationPredictionsList.Count; i++)
                {
                    searchResults.Add(destinationPredictionsList[i].Description);
                }
                AutoCompletePredictionsListDestination = searchResults;
            }
            else
            {
                return;
            }
        });

        private bool isBusy;

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged("IsBusy");
            }
        }

        public List<GooglePlaceAutoCompletePrediction> destinationPredictionsList { get; set; }
        List<string> autoCompletePredictionsListDestination = null;
        public List<string> AutoCompletePredictionsListDestination
        {
            get
            {
                return autoCompletePredictionsListDestination;
            }
            set
            {
                autoCompletePredictionsListDestination = value;
                OnPropertyChanged("AutoCompletePredictionsListDestination");
            }
        }
        #endregion

        public ICommand GetUserLocationCommand { get; set; }
        public ICommand GetLocationNameCommand { get; set; }
        public ICommand CenterMapCommand { get; set; }

        public string _originLatitude;
        public string _originLongitude;
        public string _destinationLatitude;
        public string _destinationLongitude;
        private string _pickupLocation;
        string _destinationLocation;
        public string messageForDriver;
        public string PickupLocation
        {
            get { return _pickupLocation; }
            set
            {
                _pickupLocation = value;
                if (!string.IsNullOrEmpty(_pickupLocation))
                {
                    //GetPlacesCommand.Execute(_pickupLocation);
                }
                OnPropertyChanged("PickupLocation");
            }
        }
        public string DestinationLocation
        {
            get
            {
                return _destinationLocation;
            }
            set
            {
                _destinationLocation = value;
                if (!string.IsNullOrEmpty(_destinationLocation))
                {
                    // GetPlacesCommand.Execute(_destinationLocation);
                }
                OnPropertyChanged("DestinationLocation");
            }
        }
        private DateTime _tripDateAndTime;
        public DateTime TripDateAndTime
        {
            get { return _tripDateAndTime; }
            set
            {
                _tripDateAndTime = value;
                OnPropertyChanged("TripDateAndTime");
            }
        }

        private TimeSpan _tripTimeSpan;

        public TimeSpan TripTime
        {
            get { return _tripTimeSpan; }
            set 
            {
                _tripTimeSpan = value;
                DateTime newDateTime = new DateTime(_tripDateAndTime.Year, _tripDateAndTime.Month,
                    _tripDateAndTime.Day, _tripTimeSpan.Hours, _tripTimeSpan.Minutes, 
                    _tripTimeSpan.Seconds, DateTimeKind.Utc
                    );
                _tripDateAndTime = newDateTime;
                OnPropertyChanged("TripDateAndTime");
                OnPropertyChanged("TripTime");
            }
        }

        private int _maxTripPassengers;
        public int MaxTripPassengers
        {
            get { return _maxTripPassengers; }
            set
            {
                _maxTripPassengers = value;
                OnPropertyChanged("MaxTripPassengers");
            }
        }

        private string username = "Good Morning";
        public string UserName
        {
            get { return username; }
            set
            {
                username = username + " " + value;
                OnPropertyChanged("UserName");
            }
        }
        public HomePageViewModel()
        {
            GetUserLocationCommand = new Command(async () => await GetActualUserLocation());
            GetLocationNameCommand = new Command<Location>(async (param) => await GetLocationName(param));
            GetUserLocationCommand.Execute(null);
            AvailableRideOptions = new ObservableCollection<RideOption>();
            TripDateAndTime = DateTime.Now;
            TripTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        }


        async Task GetActualUserLocation()
        {
            try
            {
                // await Task.Yield();
                var request = new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(5000));
                Location location = await Geolocation.GetLocationAsync(request);
                //TODO
                //This needs to be checked as here, a race condition seems to be happening as a result of which sometimes
                //the location isn't centered
                while (location == null) { }
                if (location != null)
                {
                    _originLatitude = location.Latitude + "";
                    _originLongitude = location.Longitude + "";
                    CenterMapCommand.Execute(location);
                    GetLocationNameCommand.Execute(location);
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                //await UserDialogs.Instance.AlertAsync("Error", "Unable to get actual location", "Ok");
            }
        }

        //Get place 
        public async Task GetLocationName(Location position)
        {
            try
            {
                var placemarks = await Geocoding.GetPlacemarksAsync(position.Latitude, position.Longitude);
                PickupLocation = placemarks?.FirstOrDefault()?.FeatureName + " " + placemarks?.FirstOrDefault()?.Thoroughfare;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
        public void AddVehicleOptions(string option)
        {
            AddLimo(option);
        }
        public void AddLimo(string limoType)
        {
            if (limoType == "SEDAN")
            {
                AvailableRideOptions.Add(new RideOption(Constants.Sedan, Constants.SedanImage, Constants.SedanCost, Constants.SedanMaxPassengers));
            }
            else if (limoType == "SUV")
            {
                AvailableRideOptions.Add(new RideOption(Constants.Suv, Constants.SuvImage, Constants.SuvCost, Constants.SuvMaxPassengers));
            }
            else if (limoType == "UTILITYVAN")
            {
                AvailableRideOptions.Add(new RideOption(Constants.UtilityVan, Constants.UtilityVanImage, Constants.UtilityVanCost, Constants.UtilityVanMaxPassengers));
            }
            else if (limoType == "DELUXEVAN")
            {
                AvailableRideOptions.Add(new RideOption(Constants.DeluxeVan, Constants.DeluxeVanImage, Constants.DeluxeVanCost, Constants.DeluxeVanMaxPassengers));
            }
            else if (limoType == "STRETCH")
            {
                AvailableRideOptions.Add(new RideOption(Constants.Stretch, Constants.StretchImage, Constants.StretchCost, Constants.StretchMaxPassengers));
            }
            else if (limoType == "STRETCHVAN")
            {
                AvailableRideOptions.Add(new RideOption(Constants.StretchVan, Constants.StretchVanImage, Constants.StretchVanCost, Constants.StretchVanMaxPassengers));
            }
            else if (limoType == "LIMOBUS")
            {
                AvailableRideOptions.Add(new RideOption(Constants.LimoBus, Constants.LimoBusImage, Constants.LimoBusCost, Constants.LimoBusMaxPassengers));
            }
            else
            {
                AvailableRideOptions.Add(new RideOption("OTHER", Constants.SedanImage, Constants.SedanCost, Constants.SedanMaxPassengers));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
