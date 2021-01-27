using Microsoft.Graph;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace OlimoXamarinForms.Helpers
{
    public class UserProfileUpdatesHelper
    {
        private static AuthenticationContext authContext;
        private static Microsoft.IdentityModel.Clients.ActiveDirectory.ClientCredential credential;
        private string clientId { get; set; }
        private string clientSecret { get; set; }
        private static string tenant { get; set; }
        public UserProfileUpdatesHelper(string clientId, string clientSecret, string tenant)
        {
            //this.clientId = clientId;
            //this.clientSecret = clientSecret;
            //this.tenant = tenant;

            //this.authContext = new AuthenticationContext("https://login.microsoftonline.com/" + tenant);
            //this.credential = ConfidentialClientApplicationBuilder.Create(clientId).WithAuthority(Constants.Authority).Build();
        }

        public static async Task<string> GetUserByObjectId(string objectId)
        {
            return await SendGraphGetRequest("/users/" + objectId, null);
        }

        public static async Task<string> UpdateUser(string objectId, string json)
        {
            return await SendGraphPatchRequest("/users/" + objectId, json);
        }

        private static async Task<string> SendGraphPatchRequest(string api, string json)
        {
            // NOTE: This client uses ADAL v2, not ADAL v4
            //AuthenticationResult result = authContext.AcquireToken(Constants.aadGraphResourceId, credential);
            HttpClient http = new HttpClient();
            string url = Constants.aadGraphEndpoint + tenant + api + "?" + Constants.aadGraphVersion;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("PATCH " + url);
            Console.WriteLine("Authorization: Bearer " + App.AccessToken.Substring(0, 80) + "...");
            Console.WriteLine("Content-Type: application/json");
            Console.WriteLine("");
            Console.WriteLine(json);
            Console.WriteLine("");

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", App.AccessToken);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                object formatted = JsonConvert.DeserializeObject(error);
                throw new WebException("Error Calling the Graph API: \n" + JsonConvert.SerializeObject(formatted, Formatting.Indented));
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine((int)response.StatusCode + ": " + response.ReasonPhrase);
            Console.WriteLine("");

            return await response.Content.ReadAsStringAsync();
        }

        public static async Task<string> SendGraphGetRequest(string api, string query)
        {
            //Microsoft.IdentityModel.Clients.ActiveDirectory.IClientAssertionCertificate cert = Certi;
            // First, use ADAL to acquire a token using the app's identity (the credential)
            // The first parameter is the resource we want an access_token for; in this case, the Graph API.
            //Microsoft.Identity.Client.AuthenticationResult result = authContext.AcquireTokenAsync("https://graph.windows.net", credential);

            // For B2C user managment, be sure to use the 1.6 Graph API version.
            string tenant = "kcstechnologiesb2c.onmicrosoft.com";
            string clientId = "46fb6136-4e8c-4b24-89dd-e5200808ac9e";
            string clientSecret = "Oc198PyN+HyApvpcZQsu5w/8JzP6PQ9h0HvGOKX3kMM=";
            authContext = new AuthenticationContext("https://login.microsoftonline.com/" + tenant);
            credential = new Microsoft.IdentityModel.Clients.ActiveDirectory.ClientCredential(clientId, clientSecret);

            HttpClient http = new HttpClient();
            string url = "https://graph.windows.net/" + tenant + api + "?" + Constants.aadGraphVersion;
            if (!string.IsNullOrEmpty(query))
            {
                url += "&" + query;
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("GET " + url);
            Console.WriteLine("Authorization: Bearer " + App.AccessToken.Substring(0, 80) + "...");
            Console.WriteLine("");

            // Append the access token for the Graph API to the Authorization header of the request, using the Bearer scheme.
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", App.AccessToken);
            HttpResponseMessage response = await http.SendAsync(request);
            
            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                object formatted = JsonConvert.DeserializeObject(error);
                throw new WebException("Error Calling the Graph API: \n" + JsonConvert.SerializeObject(formatted, Formatting.Indented));
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine((int)response.StatusCode + ": " + response.ReasonPhrase);
            Console.WriteLine("");

            return await response.Content.ReadAsStringAsync();
        }

    }
}
