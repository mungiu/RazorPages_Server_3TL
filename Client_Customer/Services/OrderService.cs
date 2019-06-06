using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Client_Customer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Client_Customer.Services
{
    /// <summary>
    /// Contains all methods used for facilitating http requests regarding an Order. Communicates with a RESTful type of API and has all methods defined as asynchronious tasks.
    /// </summary>
    public class OrderService : IOrderService
    {
        /// <summary>
        /// Posts a new order to the second tier.
        /// </summary>
        /// <param name="newOrder">The order to be posted</param>
        /// <param name="targetUrl">The API url for the POST request</param>
        /// <returns></returns>
        public async Task<string> PostNewOrderAsync(Order newOrder, string targetUrl)
        {
            JsonSerializer jsonSerializer = new JsonSerializer();
            string orderAsJSON = JsonConvert.SerializeObject(newOrder);

            StringContent jsonAsHttpContent = new StringContent(orderAsJSON, Encoding.UTF8, "application/json");

            string response = await PostJsonToUrlAsync(targetUrl, jsonAsHttpContent);
            return response;
        }
        /// <summary>
        /// Posts a new client to the second tier
        /// </summary>
        /// <param name="newClient">The actual client</param>
        /// <param name="targetUrlRegisterClient">The API url for posting new clients</param>
        /// <returns></returns>
        public async Task<string> PostRegisterClientAsync(Client newClient, string targetUrlRegisterClient)
        {
            JsonSerializer jsonSerializer = new JsonSerializer();
            string clientAsJSON = JsonConvert.SerializeObject(newClient);

            StringContent jsonAsHttpContent = new StringContent(clientAsJSON, Encoding.UTF8, "application/json");

            string response = await PostJsonToUrlAsync(targetUrlRegisterClient, jsonAsHttpContent);
            return response;
        }
        /// <summary>
        /// Gets a list of all available orders from the second Tier
        /// </summary>
        /// <param name="pathUri">The Uri of the RESTful API</param>
        /// <returns></returns>
        public async Task<OrderList> GetOrderListAsync(Uri pathUri)
        {
            OrderList orderList = null;

            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage httpResponse = await httpClient.GetAsync(pathUri);
                if (httpResponse.IsSuccessStatusCode)
                {
                    string jsonString = await httpResponse.Content.ReadAsStringAsync();
                    orderList = JsonConvert.DeserializeObject<OrderList>(jsonString);
                }

                return orderList;
            }
        }
        /// <summary>
        /// Gets the details of a specific order by order number
        /// </summary>
        /// <param name="orderUri">The URI to the API for getting and order details by ID</param>
        /// <returns></returns>
        public async Task<Order> GetOrderByOrderNumberAsync(Uri orderUri)
        {
            Order order = null;

            string jsonString = await GetJsonStringAsync(orderUri);
            order = JsonConvert.DeserializeObject<Order>(jsonString);

            return order;
        }
        /// <summary>
        /// Gets a JSON string from a specified URI path
        /// </summary>
        /// <param name="pathUri">JSON string request location enpoint</param>
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
        /// Updates orders details via a put request towards the second tier API
        /// </summary>
        /// <param name="updatedOrder">Patrially filled order containing only details o be updated</param>
        /// <param name="Url">API url for this type of PUT requests</param>
        /// <returns></returns>
        public async Task<string> PutUpdatedOrderAsync(Order updatedOrder, string Url)
        {
            JsonSerializer jsonSerializer = new JsonSerializer();
            string orderAsJSON = JsonConvert.SerializeObject(updatedOrder, new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
                //NOTE: Indenting files increases their size by ~ 20%
                //Formatting = Formatting.Indented
            });

            StringContent jsonAsHttpContent = new StringContent(orderAsJSON, Encoding.UTF8, "application/json");

            string response = await PutJsonToUrlAsync(Url, jsonAsHttpContent);
            return response;
        }
        /// <summary>
        /// Updates order tracking via a Post request to the API
        /// </summary>
        /// <param name="orderNumber">order number to update</param>
        /// <param name="companyID">companyID that updates</param>
        /// <param name="updateType">type of update ("lateOrder", "orderPickedUp", "orderDelivered")</param>
        /// <param name="urlUpdateOrderTracking">URL used for orderTrackingupdate as POST requests to API</param>
        /// <returns></returns>
        public async Task<string> PostUpdateOrderTrackingAsync(string orderNumber, string companyID, string updateType, string urlUpdateOrderTracking)
        {
            string jsonData = $"{{'orderNumber': {orderNumber}, 'companyId': {companyID}, 'updateType': {updateType}}}";
            StringContent jsonAsHttpContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            string response = await PostJsonToUrlAsync(urlUpdateOrderTracking, jsonAsHttpContent);
            return response;
        }
        /// <summary>
        /// POST newely assigned contractor to current order
        /// </summary>
        /// <param name="orderNumber">Order number contractor is assigned to</param>
        /// <param name="companyID">Contractor company ID</param>
        /// <param name="targetUrl">API URL for such POST request</param>
        /// <returns></returns>
        public async Task<string> PostAssignedContractorToOrderAsync(string orderNumber, string companyID, string targetUrl)
        {
            // "{{" = "{" but is used for escaping character bacause f "$"
            string jsonData = $"{{'orderNumber': {orderNumber}, 'companyId': {companyID}}}";
            StringContent jsonAsHttpContent = new StringContent(jsonData, Encoding.UTF8, "application/json");


            string response = await PostJsonToUrlAsync(targetUrl, jsonAsHttpContent);
            return response;
        }
        /// <summary>
        /// PUT request to any specified URL with StringContent type of object
        /// </summary>
        /// <param name="Url">The API URL for such PUT requests</param>
        /// <param name="jsonAsHttpContent">The string containing content</param>
        /// <returns></returns>
        public async Task<string> PutJsonToUrlAsync(string Url, StringContent jsonAsHttpContent)
        {
            string httpResponseAsString = null;
            // Sending the JSON and getting status response
            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage httpResponse = await httpClient.PutAsync(Url, jsonAsHttpContent);
                httpResponseAsString = httpResponse.StatusCode.ToString();
            }

            if (httpResponseAsString == null)
                return "Server did not reply";
            else
                return httpResponseAsString;
        }
        /// <summary>
        /// POST request to any specified URL with StringContent object
        /// </summary>
        /// <param name="Url">API url for such request</param>
        /// <param name="jsonAsHttpContent">The actual string containing JSON content</param>
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
        /// <summary>
        /// CANCEL type of request, where contractor cancels an order he was previously assigned to
        /// </summary>
        /// <param name="urlCancelAsContractor">API url for this type of CANCEL requests</param>
        /// <returns></returns>
        public async Task<string> CancelOrderAsContractorAsync(string urlCancelAsContractor)
        {
            string httpResponseAsString = null;
            // Sending the JSON and getting status response
            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage httpResponse = await httpClient.DeleteAsync(urlCancelAsContractor);
                httpResponseAsString = httpResponse.StatusCode.ToString();
            }

            if (httpResponseAsString == null)
                return "Server did not reply";
            else
                return httpResponseAsString;
        }
        /// <summary>
        /// DELETE type of request for deleting an existing order not yet assigned to a contractor
        /// </summary>
        /// <param name="deleteOrderUrl">The URL for such type of DELETE request</param>
        /// <returns></returns>
        public async Task<string> DeleteOrderAsync(string deleteOrderUrl)
        {
            string httpResponseAsString = null;
            // Sending the JSON and getting status response
            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage httpResponse = await httpClient.DeleteAsync(deleteOrderUrl);
                httpResponseAsString = httpResponse.StatusCode.ToString();
            }

            if (httpResponseAsString == null)
                return "Server did not reply";
            else
                return httpResponseAsString;
        }
        /// <summary>
        /// GET request to the Google Maps Distance Matrix API returning distance and travel time information from specified point A to B
        /// </summary>
        /// <param name="urlWithAPIKey">Google API fully formed URL with request details</param>
        /// <returns></returns>
        public async Task<GoogleRoute> GetRouteInformationFromGoogleAPI(string urlWithAPIKey)
        {
            Uri uri = new Uri(urlWithAPIKey);
            GoogleRoute googleRoute = null;

            string jsonString = await GetJsonStringAsync(uri);
            googleRoute = JsonConvert.DeserializeObject<GoogleRoute>(jsonString);

            return googleRoute;
        }
    }
}
