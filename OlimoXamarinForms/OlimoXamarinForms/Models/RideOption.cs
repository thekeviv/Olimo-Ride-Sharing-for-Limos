using System;
using System.Collections.Generic;
using System.Text;

namespace OlimoXamarinForms.Models
{
    public class RideOption
    {
        public string RideName { get; set; }
        public string ImageName { get; set; }
        public double RideCost { get; set; }
        public int MaxNumberOfPassengers { get; set; }

        public RideOption(string RideName, string ImageName, double RideCost, int MaxNumberOfPassengers)
        {
            this.RideName = RideName;
            this.ImageName = ImageName;
            this.RideCost = RideCost;
            this.MaxNumberOfPassengers = MaxNumberOfPassengers;
        }
    }
}
