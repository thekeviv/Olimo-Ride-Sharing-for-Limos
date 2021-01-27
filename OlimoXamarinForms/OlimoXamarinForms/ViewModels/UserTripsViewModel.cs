using OlimoXamarinForms.Helpers;
using OlimoXamarinForms.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlimoXamarinForms.ViewModels
{
    public class UserTripsViewModel : INotifyPropertyChanged
    {
        private List<UserTrip> pastTrips;
        public List<UserTrip> PastTrips
        {
            get { return pastTrips; }
            set 
            {
                this.pastTrips = value;
                if(pastTrips.Count>0)
                {
                    PastRidesFound = true;
                    PastRidesTextEnable = false;
                }
                else
                {
                    PastRidesFound = false;
                    PastRidesTextEnable = true;
                }
                OnPropertyChanged("PastTrips");
            }
        }

        private List<UserTrip> futureTrips;
        public List<UserTrip> FutureTrips
        {
            get { return futureTrips; }
            set
            {
                this.futureTrips = value;
                if (futureTrips.Count > 0)
                {
                    FutureRidesFound = true;
                    FutureRidesTextEnable = false;
                }
                else
                {
                    FutureRidesFound = false;
                    FutureRidesTextEnable = true;
                }
                OnPropertyChanged("FutureTrips");
            }
        }

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

        private bool pastRidesFound;

        public bool PastRidesFound
        {
            get { return pastRidesFound; }
            set
            {
                pastRidesFound = value;
                OnPropertyChanged("PastRidesFound");
            }
        }

        private bool pastRidesTextEnable;

        public bool PastRidesTextEnable
        {
            get { return pastRidesTextEnable; }
            set
            {
                pastRidesTextEnable = value;
                OnPropertyChanged("PastRidesTextEnable");
            }
        }

        private bool futureRidesFound = false;

        public bool FutureRidesFound
        {
            get { return futureRidesFound; }
            set
            {
                futureRidesFound = value;
                OnPropertyChanged("FutureRidesFound");
            }
        }

        private bool futureRidesTextEnable = true;

        public bool FutureRidesTextEnable
        {
            get { return futureRidesTextEnable; }
            set
            {
                futureRidesTextEnable = value;
                OnPropertyChanged("FutureRidesTextEnable");
            }
        }

        public UserTripsViewModel()
        {
            PastTrips = new List<UserTrip>();
            FutureTrips = new List<UserTrip>();
            this.IsBusy = true;
            GetTrips();
            this.IsBusy = false;
        }

        public async void GetTrips()
        {
            
            List<UserTrip> allTrips = await GetTripsAsync();
            var pastTrips = from pastTrip in allTrips where pastTrip.BookingTime < DateTime.UtcNow.AddMinutes(-30) select pastTrip;
            List<UserTrip> pTrips = new List<UserTrip>();
            List<UserTrip> fTrips = new List<UserTrip>();
            foreach (UserTrip _pastTrip in pastTrips)
            {
                pTrips.Add(_pastTrip);
            }
            PastTrips = pTrips;
            var futureTrips = from futureTrip in allTrips where futureTrip.BookingTime > DateTime.UtcNow.AddMinutes(-30) select futureTrip;
            foreach (UserTrip _futureTrip in futureTrips)
            {
                fTrips.Add(_futureTrip);
            }
            FutureTrips = fTrips;
        }

        Task<List<UserTrip>> GetTripsAsync()
        {
            return FirebaseFirestoreHelper.GetTrips();              
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
