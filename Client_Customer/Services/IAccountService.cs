using Client_Customer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client_Customer.Services
{
    /// <summary>
    /// Used for dependency inversion of the account services
    /// </summary>
    interface IAccountService
    {
        Task<string> PostNewClientAsync(Client newOrder, string Url);

        Task<Client> GetClientByIDAndHashPassAsync(Uri pathUri);

        Task<string> PutUpdatedClientAsync(Order updatedOrder, string Url);
    }
}
