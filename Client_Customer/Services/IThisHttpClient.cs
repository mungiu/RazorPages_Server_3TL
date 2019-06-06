using System.Net.Http;
using System.Threading.Tasks;

namespace Client_Customer.Services
{
    /// <summary>
    /// Interface used for HttpClient dependency inversion
    /// </summary>
    interface IThisHttpClient
    {
        Task<HttpClient> GetClient();
    }
}
