using Plugin.CloudFirestore.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace OlimoXamarinForms.Models
{
    public class FirebaseUser
    {
        [MapTo("Username")]
        public string Username { get; set; }

        [MapTo("IsDriver")]
        public bool IsDriver { get; set; }
        [MapTo("PaymentId")]
        public string PaymentId { get; set; }

    }
}
