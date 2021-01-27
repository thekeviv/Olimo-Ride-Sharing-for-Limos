using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.Documents.Spatial;
using OlimoXamarinForms.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OlimoXamarinForms.Helpers
{
    public class DatabaseHelper
    {
        private static string databaseId = "Olimo";

        public static DocumentClient client;

        public static Uri userLocationsCollectionLink = UriFactory.CreateDocumentCollectionUri(databaseId, "UserLocations");

        public static Uri tripsCollectionLink = UriFactory.CreateDocumentCollectionUri(databaseId, "Trips");



        public static Collection<string> vehicleClassesToLoad;
        public static List<UserLocation> driversInPastMin;
        public const int EarthRadius = 6371;
        public DatabaseHelper()
        {
            
        }

        private static double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        private static double RadianToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
        }

        //https://coderwall.com/p/otkscg/geographic-searches-within-a-certain-distance
        public static async Task GetLimosInRadiusInTime(double distance, double minutesPast, double latitude, double longitude, int maxPassengers)
        {
            /*
            double maxLatitude = latitude + (distance / EarthRadius);
            double minLatitude = latitude - (distance / EarthRadius);
            double maxLongitude = longitude + RadianToDegree(distance/(EarthRadius/(Math.Cos(DegreeToRadian(latitude)))));
            double minLongitude = longitude - RadianToDegree(distance / (EarthRadius / (Math.Cos(DegreeToRadian(latitude)))));
            DateTime minutesDifference = DateTime.UtcNow.Add(new TimeSpan(0, Convert.ToInt32(-1 * minutesPast), 0));
            dbClient = LocationTrackingHelper.dbClient;
            var table = Table.LoadTable(dbClient, "DriverLocations");
            ScanFilter scanFilter = new ScanFilter();
            scanFilter.AddCondition("Time", ScanOperator.Between, minutesDifference.ToString("o"), DateTime.UtcNow.ToString("o"));
            scanFilter.AddCondition("Latitude", ScanOperator.Between, minLatitude, maxLatitude);
            scanFilter.AddCondition("Longitude", ScanOperator.Between, minLongitude, maxLongitude);
            //scanFilter.AddCondition("MaxPassengers", ScanOperator.LessThanOrEqual, maxPassengers);
            ScanOperationConfig scanConfig = new ScanOperationConfig
            {
                Filter = scanFilter,
                Limit = 1,
            };
            Search queryFilterSearch = table.Scan(scanConfig);

            List<Document> allDocs = await queryFilterSearch.GetRemainingAsync();
            vehicleClassesToLoad = new Collection<string>();
            driversInPastMin = new Dictionary<string, string>();
            for(int i=0; i<allDocs.Count; i++)
            {
                string jsonResponse = allDocs[i].ToJson();
                var data = JObject.Parse(jsonResponse);
                string email = data["UserEmail"].ToString();
                string vehicleType = data["VehicleType"].ToString();
                if(!vehicleClassesToLoad.Contains(vehicleType))
                {
                    vehicleClassesToLoad.Add(vehicleType);
                }
                string time = data["Time"].ToString();
                if(!driversInPastMin.ContainsKey(email))
                {
                    driversInPastMin.Add(email, vehicleType);
                }
                
            }
            */
            var userPoint = new Point(longitude, latitude);
            client = LocationTrackingHelper.client;
            var feedOptions = new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true };
            var query = client.CreateDocumentQuery<UserLocation>(userLocationsCollectionLink, feedOptions).Where(driver => userPoint.Distance(driver.Location) < 10).AsDocumentQuery();

            List<UserLocation> locations = new List<UserLocation>();
            vehicleClassesToLoad = new Collection<string>();
            while (query.HasMoreResults)
            {
                locations.AddRange(await query.ExecuteNextAsync<UserLocation>());
            }
            driversInPastMin = locations;
            for (int i = 0; i < locations.Count; i++)
            {
                UserLocation a = locations[i];
                string vehicleType = a.VehicleType;
                if (vehicleClassesToLoad.Contains(vehicleType))
                {

                }
                else
                {
                    vehicleClassesToLoad.Add(vehicleType);
                }
            }
        }


        public static async Task CreateTrip(string customerEmail, string customerName, string driverEmail, string startLocation, string destinationLocation, DateTime timeOfBooking, DateTime lastEdited, double basePrice, string driverMessage, string vehicleClass)
        {
            UserTrip newTrip = new UserTrip
            {
                UserEmail = customerEmail,
                UserName = customerName,
                DriverEmail = driverEmail,
                StartLocation = startLocation,
                DestinationLocation = destinationLocation,
                BookingTime = timeOfBooking,
                LastEditedTime = lastEdited,
                TripBasePrice = basePrice,
                DriverMessage = driverMessage,
                TripTipAmount = 0,
                VehicleClass = vehicleClass
            };

            await client.CreateDocumentAsync(tripsCollectionLink, newTrip);

            /*
            dbClient = LocationTrackingHelper.dbClient;
            var table = Table.LoadTable(dbClient, "Trips");
            var item = new Document();
            item["UserEmail"] = customerEmail;
            item["CustomerName"] = customerName;
            item["StartLocation"] = startLocation;
            item["DestinationLocation"] = destinationLocation;
            item["BookingTime"] = timeOfBooking;
            item["Time"] = lastEdited;
            item["TripBasePrice"] = basePrice;
            item["MessageForDriver"] = driverMessage;
            await table.PutItemAsync(item);
            */
        }

        public static async Task<List<UserTrip>> GetTrips(bool isPastRides)
        {
            List<UserTrip> trips = new List<UserTrip>();
            var feedOptions = new FeedOptions { EnableCrossPartitionQuery = true };
            client = new DocumentClient(new System.Uri(Constants.EndpointUri), Constants.ReadWritePrimaryKey);
            //TODO
            //Fix this
            var query = isPastRides? client.CreateDocumentQuery<UserTrip>(tripsCollectionLink, feedOptions).Where(trip => trip.UserEmail == App.UserEmail).AsDocumentQuery() :
                client.CreateDocumentQuery<UserTrip>(tripsCollectionLink, feedOptions).Where(trip => trip.UserEmail == App.UserEmail).AsDocumentQuery();
            while (query.HasMoreResults)
            {
                trips.AddRange(await query.ExecuteNextAsync<UserTrip>());
            }
            return trips;
        }

        //TODO
        public static async Task UpdateTipAmount()
        {

        }

        //TODO
        public static async Task UpdateDriver()
        {

        }

    }
}
