using System;
using System.Collections.Generic;
using System.Text;

namespace OlimoXamarinForms
{
    public class Constants
    {
        #region GoogleMapsKeys
        public static string GoogleMapsApiKey = "Add Google Maps Key Here";

        #endregion

        #region VehicleNames
        public static string Sedan = "SEDAN";
        public static string Suv = "SUV";
        public static string UtilityVan = "UTILITYVAN";
        public static string DeluxeVan = "DELUXEVAN";
        public static string Stretch = "STRETCH";
        public static string StretchVan = "STRETCHVAN";
        public static string LimoBus = "LIMOBUS";

        #endregion

        #region VehicleClassImagePaths

        public static string SedanImage = "sedan.jpg";
        public static string SuvImage = "suv.jpg";
        public static string UtilityVanImage = "van.jpg";
        public static string DeluxeVanImage = "deluxevan.jpg";
        public static string StretchImage = "stretch.jpg";
        public static string StretchVanImage = "stretchvan.png";
        public static string LimoBusImage = "limobus.png";

        #endregion

        #region VehicleClassCosts

        public static double SedanCost = 85;
        public static double SuvCost = 95;
        public static double UtilityVanCost = 110;
        public static double DeluxeVanCost = 130;
        public static double StretchCost = 140;
        public static double StretchVanCost = 160;
        public static double LimoBusCost = 160;

        #endregion

        #region VehicleClassMaxPassengers

        public static int SedanMaxPassengers = 4;
        public static int SuvMaxPassengers = 7;
        public static int UtilityVanMaxPassengers = 12;
        public static int DeluxeVanMaxPassengers = 12;
        public static int StretchMaxPassengers = 12;
        public static int StretchVanMaxPassengers = 12;
        public static int LimoBusMaxPassengers = 22;

        #endregion

        #region CosmosDb secrets

        // The Azure Cosmos DB endpoint for running this sample.
        public static readonly string EndpointUri = "";
        // The primary key for the Azure Cosmos account.
        public static readonly string ReadWritePrimaryKey = "";

        public static readonly string ReadOnlyPrimaryKey = "";

        #endregion

        #region AzureAD B2C secrets

        public static string Tenant = "";

        public static string PolicySignUpAndIn = "";

        public static string AuthorityBase = $"";
        public static string Authority = $"{AuthorityBase}{PolicySignUpAndIn}";

        public static string[] scopes = new string[] { "" };

        public static string[] scopeUserReadWrite = new string[] { "" };
        //public static string[] AppScopes = { "User.Read" };
        public static string iosKeychainSecurityGroup = "";
        public static readonly string TenantName = "";
        public static readonly string TenantId = "";
        public static string clientId = "";
        static readonly string policySignin = "";
        static readonly string policyPassword = "";
        static readonly string authorityBase = $"";
        public static string ClientId
        {
            get
            {
                return clientId;
            }
        }
        public static string AuthoritySignin
        {
            get
            {
                return $"{authorityBase}{policySignin}";
            }
        }
        public static string AuthorityPasswordReset
        {
            get
            {
                return $"{authorityBase}{policyPassword}";
            }
        }
        public static string[] Scopes
        {
            get
            {
                return scopes;
            }
        }
        public static string IosKeychainSecurityGroups
        {
            get
            {
                return iosKeychainSecurityGroup;
            }
        }
        #endregion

        #region AzureAD B2C Graph secrets
        public static string aadInstance = "";
        public static string aadGraphResourceId = "";
        public static string aadGraphEndpoint = "https://graph.windows.net/";
        public static string aadGraphSuffix = "";
        public static string aadGraphVersion = "api-version=1.6";
        #endregion

        #region Stripe Constants

        public static string StripeKey = "";

        #endregion


    }
}
