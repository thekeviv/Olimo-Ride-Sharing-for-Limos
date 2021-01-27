using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OlimoXamarinForms.Models
{
	public class UserTrip
	{
		[JsonProperty(PropertyName = "useremail")]
		public string UserEmail { get; set; }

		[JsonProperty(PropertyName = "username")]
		public string UserName { get; set; }

		[JsonProperty(PropertyName = "drivername")]
		public string DriverEmail { get; set; }

		[JsonProperty(PropertyName = "startlocation")]
		public string StartLocation { get; set; }

		[JsonProperty(PropertyName = "destinationlocation")]
		public string DestinationLocation { get; set; }

		[JsonProperty(PropertyName = "bookingtime")]
		public DateTime BookingTime { get; set; }

		[JsonProperty(PropertyName = "lasteditedtime")]
		public DateTime LastEditedTime { get; set; }

		[JsonProperty(PropertyName = "tripbaseprice")]
		public double TripBasePrice { get; set; }

		[JsonProperty(PropertyName = "triptipamount")]
		public double TripTipAmount { get; set; }

		[JsonProperty(PropertyName = "drivermessage")]
		public string DriverMessage { get; set; }

		[JsonProperty(PropertyName = "vehicleclass")]
		public string VehicleClass { get; set; }

	}
}
