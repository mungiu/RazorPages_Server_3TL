using System.Net.Http;
using System.Threading.Tasks;

namespace Client_Customer.Services
{
    interface IThisHttpClient
    {
        Task<HttpClient> GetClient();
    }
}
