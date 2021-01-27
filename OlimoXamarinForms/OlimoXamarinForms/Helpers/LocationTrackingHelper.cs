using Microsoft.Azure.Documents.Client;
using OlimoXamarinForms.Models;
using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace OlimoXamarinForms.Helpers
{
    public class LocationTrackingHelper
    {
        public static Location LastKnownLocation = null;

        private static string databaseId = "Olimo";

        public static DocumentClient client;

        public static Uri collectionLink = UriFactory.CreateDocumentCollectionUri(databaseId, "UserLocations");

        public static void StartLocationTracking()
        {
            Device.StartTimer(TimeSpan.FromSeconds(10.0), () => TrackLocation(true));
        }


        private static bool TrackLocation(bool continueTimer)
        {
            if (continueTimer)
            {
                var request = new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(5000));
                Location location = null;
                while (true)
                {
                    location = Task.Run(async () => await Geolocation.GetLastKnownLocationAsync()).Result;
                    if (location != null)
                    {
                        break;
                    }
                }
                if (location != LastKnownLocation)
                {
                    LastKnownLocation = location;
                    client = new DocumentClient(new System.Uri(Constants.EndpointUri), Constants.ReadWritePrimaryKey);
                    UserLocation newLocation = new UserLocation
                    {
                        Location = new Microsoft.Azure.Documents.Spatial.Point(location.Longitude, location.Latitude),
                        Time = DateTime.UtcNow,
                        UserEmail = App.UserEmail,
                        UserName = App.UserName,
                        DeviceManufacturer = DeviceInfo.Manufacturer,
                        DeviceModel = DeviceInfo.Model,
                        OperatingSystem = DeviceInfo.Platform.ToString(),
                        OperatingSystemVersion = DeviceInfo.VersionString,
                        DeviceType = DeviceInfo.Idiom.ToString(),
                        VehicleType = Constants.LimoBus,
                        MaxPassengers = 12
                    };
                    CrossCloudFirestore.Current
                         .Instance
                         .GetCollection("userlocations")
                         .AddDocumentAsync(newLocation);
                    client.CreateDocumentAsync(collectionLink, newLocation);

                    
                }
                return true;
            }
            else
            {
                //else stop the timer
                return false;
            }

        }
    }
}
