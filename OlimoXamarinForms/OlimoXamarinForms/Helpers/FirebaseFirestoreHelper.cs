using OlimoXamarinForms.Models;
using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OlimoXamarinForms.Helpers
{
    public class FirebaseFirestoreHelper
    {

        public static async Task UpdateTipAmount()
        {

        }

        //TODO
        public static async Task UpdateDriver()
        {

        }

        public static async Task<List<UserTrip>> GetTrips()
        {
            List<UserTrip> trips = new List<UserTrip>();
            var query = await CrossCloudFirestore.Current
                                     .Instance
                                     .GetCollection("trips")
                                     .WhereEqualsTo("CustomerEmail", App.UserEmail.Trim().ToLower())
                                     .GetDocumentsAsync();

            var yourModels = query.ToObjects<UserTrip>();
            foreach(UserTrip trip in yourModels)
            {
                trips.Add(trip);
            }
            return trips;
        }

        public static async Task CreateTrip(string customerEmail, string customerName, string driverEmail, string startLocation, string destinationLocation, DateTime timeOfBooking, DateTime lastEdited, double basePrice, string driverMessage, string vehicleClass)
        {
            await CrossCloudFirestore.Current
                         .Instance
                         .GetCollection("trips")
                         .AddDocumentAsync(new { CustomerEmail = customerEmail, CustomerName = customerName, 
                         DriverEmail = driverEmail, StartLocation = startLocation, DestinationLocation = destinationLocation, 
                         TimeOfBooking = timeOfBooking, LastEdited = lastEdited, BasePrice = basePrice, DriverMessage = driverMessage,
                         VehicleClass = vehicleClass});
        }

        public static async Task GetLimosInRadiusInTime(double distance, double minutesPast, double latitude, double longitude, int maxPassengers)
        {

        }

        public static async Task AddUserInfo(string name, string email, bool isDriver, string paymenytId)
        {
            await CrossCloudFirestore.Current.Instance.
                GetCollection("users").GetDocument(email.ToLower().Trim()).SetDataAsync(new { Username = name, IsDriver = isDriver, PaymentId = paymenytId });
        }

        public static async Task<FirebaseUser> GetUserInfo(string email)
        {
            var query = await CrossCloudFirestore.Current
                                     .Instance
                                     .GetCollection("users")
                                     .WhereEqualsTo(FieldPath.DocumentId, email)
                                     .GetDocumentsAsync();
            var a = query.ToObjects<FirebaseUser>();
            foreach(FirebaseUser user in a)
            {
                return user;
            }
            return null;
        }

        public static async Task AddPaymentIdToUser(string paymentId)
        {
            await CrossCloudFirestore.Current
                         .Instance
                         .GetCollection("users")
                         .GetDocument(App.UserEmail)
                         .UpdateDataAsync(new { PaymentId = paymentId });
        }
    }
}
