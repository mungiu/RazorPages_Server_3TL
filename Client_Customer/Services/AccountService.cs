using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Client_Customer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;

namespace Client_Customer.Services
{
    /// <summary>
    /// Manages GET, POST, PUT, CANCEL, DELETE requests regarding a user account with the API
    /// </summary>
    public class AccountService : IAccountService
    {
        /// <summary>
        /// GET request of client by ID and hasshed password
        /// </summary>
        /// <param name="clientUri"></param>
        /// <returns></returns>
        public async Task<Client> GetClientByIDAndHashPassAsync(Uri clientUri)
        {
            Client client = null;

            string jsonString = await GetJsonStringAsync(clientUri);
            client = JsonConvert.DeserializeObject<Client>(jsonString);

            return client;
        }
        /// <summary>
        /// POST request of newwely created client
        /// </summary>
        /// <param name="newClient"></param>
        /// <param name="targetUrl"></param>
        /// <returns></returns>
        public async Task<string> PostNewClientAsync(Client newClient, string targetUrl)
        {
            JsonSerializer jsonSerializer = new JsonSerializer();
            string orderAsJSON = JsonConvert.SerializeObject(newClient);/*, new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
                //NOTE: Indenting files increases their size by ~ 20%
                //Formatting = Formatting.Indented
            });*/

            StringContent jsonAsHttpContent = new StringContent(orderAsJSON, Encoding.UTF8, "application/json");

            string response = await PostJsonToUrlAsync(targetUrl, jsonAsHttpContent);
            return response;
        }
        /// <summary>
        /// PUT request with updated client information
        /// </summary>
        /// <param name="updatedClient"></param>
        /// <param name="Url"></param>
        /// <returns></returns>
        public async Task<string> PutUpdatedClientAsync(Order updatedClient, string Url)
        {
            // Serializing the object into JSON
            JsonSerializer jsonSerializer = new JsonSerializer();
            string orderAsJSON = JsonConvert.SerializeObject(updatedClient, new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
                //NOTE: Indenting files increases their size by ~ 20%
                //Formatting = Formatting.Indented
            });

            StringContent jsonAsHttpContent = new StringContent(orderAsJSON, Encoding.UTF8, "application/json");

            string response = await PostJsonToUrlAsync(Url, jsonAsHttpContent);

            return response;
        }
        /// <summary>
        /// GET request for any type or JSON at provided URL
        /// </summary>
        /// <param name="pathUri"></param>
        /// <returns></returns>
        public async Task<string> GetJsonStringAsync(Uri pathUri)
        {
            string jsonString = null;
            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage httpResponse = await httpClient.GetAsync(pathUri);
                if (httpResponse.IsSuccessStatusCode)
                {
                    jsonString = await httpResponse.Content.ReadAsStringAsync();
                }
            }
            return jsonString;
        }
        /// <summary>
        /// POST request for any type of JSON a provided URL
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="jsonAsHttpContent"></param>
        /// <returns></returns>
        public async Task<string> PostJsonToUrlAsync(string Url, StringContent jsonAsHttpContent)
        {
            string httpResponseAsString = null;
            // Sending the JSON and getting status response
            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage httpResponse = await httpClient.PostAsync(Url, jsonAsHttpContent);
                httpResponseAsString = httpResponse.StatusCode.ToString();
            }

            if (httpResponseAsString == null)
                return "Server did not reply";
            else
                return httpResponseAsString;
        }
    }
}
