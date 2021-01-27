using Microsoft.Azure.Documents.Spatial;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OlimoXamarinForms.Models
{
	public class UserLocation
	{
		[JsonProperty(PropertyName = "location")]
		public Point Location { get; set; }

		[JsonProperty(PropertyName = "time")]
		public DateTime Time { get; set; }

		[JsonProperty(PropertyName = "useremail")]
		public string UserEmail { get; set; }

		[JsonProperty(PropertyName = "username")]
		public string UserName { get; set; }

		[JsonProperty(PropertyName = "devicemanufacturer")]
		public string DeviceManufacturer { get; set; }

		[JsonProperty(PropertyName = "devicemodel")]
		public string DeviceModel { get; set; }

		[JsonProperty(PropertyName = "devicetype")]
		public string DeviceType { get; set; }

		[JsonProperty(PropertyName = "maxpassengers")]
		public int MaxPassengers { get; set; }

		[JsonProperty(PropertyName = "operatingsystem")]
		public string OperatingSystem { get; set; }

		[JsonProperty(PropertyName = "OperatingSystemVersion")]
		public string OperatingSystemVersion { get; set; }

		[JsonProperty(PropertyName = "vehicletype")]
		public string VehicleType { get; set; }

	}
}
