using System;
using System.Collections.Generic;
using System.Text;

namespace OlimoXamarinForms.Models
{
    public class User
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Market { get; set; }
        public string Phone { get; set; }
        public double Rating { get; set; }
        public int userType { get; set; }
        public string carCategory { get; set; }
    }
}
