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
    public class OrderService : IOrderService
    {
        public async Task<string> PostNewOrderAsync(Order newOrder, string targetUrl)
        {
            JsonSerializer jsonSerializer = new JsonSerializer();
            string orderAsJSON = JsonConvert.SerializeObject(newOrder);

            StringContent jsonAsHttpContent = new StringContent(orderAsJSON, Encoding.UTF8, "application/json");

            string response = await PostJsonToUrlAsync(targetUrl, jsonAsHttpContent);
            return response;
        }

        public async Task<string> PostRegisterClientAsync(Client newClient, string targetUrlRegisterClient)
        {
            JsonSerializer jsonSerializer = new JsonSerializer();
            string clientAsJSON = JsonConvert.SerializeObject(newClient);

            StringContent jsonAsHttpContent = new StringContent(clientAsJSON, Encoding.UTF8, "application/json");

            string response = await PostJsonToUrlAsync(targetUrlRegisterClient, jsonAsHttpContent);
            return response;
        }

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

        public async Task<Order> GetOrderByOrderNumberAsync(Uri orderUri)
        {
            Order order = null;

            string jsonString = await GetJsonStringAsync(orderUri);
            order = JsonConvert.DeserializeObject<Order>(jsonString);

            return order;
        }

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

        public async Task<string> PostUpdateOrderTrackingAsync(string orderNumber, string companyID, string updateType, string urlUpdateOrderTracking)
        {
            string jsonData = $"{{'orderNumber': {orderNumber}, 'companyId': {companyID}, 'updateType': {updateType}}}";
            StringContent jsonAsHttpContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

            string response = await PostJsonToUrlAsync(urlUpdateOrderTracking, jsonAsHttpContent);
            return response;
        }

        public async Task<string> PostAssignedContractorToOrderAsync(string orderNumber, string companyID, string targetUrl)
        {
            // "{{" = "{" but is used for escaping character bacause f "$"
            string jsonData = $"{{'orderNumber': {orderNumber}, 'companyId': {companyID}}}";
            StringContent jsonAsHttpContent = new StringContent(jsonData, Encoding.UTF8, "application/json");


            string response = await PostJsonToUrlAsync(targetUrl, jsonAsHttpContent);
            return response;
        }

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

        public async Task<string> DeleteCancelOrderAsContractorAsync(string urlCancelAsContractor)
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
        //public async Task<string> PutJsonToUrlAsync(string Url, StringContent jsonAsHttpContent)
        //{
        //    string httpResponseAsString = null;
        //    // Sending the JSON and getting status response
        //    using (HttpClient httpClient = new HttpClient())
        //    {
        //        HttpResponseMessage httpResponse = await httpClient.PutAsync(Url, jsonAsHttpContent);
        //        httpResponseAsString = httpResponse.StatusCode.ToString();
        //    }

        //    if (httpResponseAsString == null)
        //        return "Server did not reply";
        //    else
        //        return httpResponseAsString;
        //}
    }
}
