using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Client_Customer.Services
{
    /// <summary>
    /// A class used for creating one HttpClient that will be used throughout the application.
    /// </summary>
    public class ThisHttpClient : IThisHttpClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private HttpClient _httpClient = new HttpClient();
        /// <summary>
        /// Constructor accepting a context accessor
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public ThisHttpClient(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        /// <summary>
        /// Return the current http client with a base address http://localhost:1601/ and with a json header
        /// </summary>
        /// <returns></returns>
        public async Task<HttpClient> GetClient()
        {
            _httpClient.BaseAddress = new Uri("http://localhost:1601/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            return _httpClient;
        }
    }
}
