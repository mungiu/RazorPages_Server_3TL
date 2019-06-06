using Client_Customer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client_Customer.Services
{
    /// <summary>
    /// Interface used for OrderService dependency inversion
    /// </summary>
    public interface IOrderService
    {
        Task<string> PostNewOrderAsync(Order newOrder, string Url);

        Task<OrderList> GetOrderListAsync(Uri pathUri);

        Task<Order> GetOrderByOrderNumberAsync(Uri pathUri);

        Task<string> PutUpdatedOrderAsync(Order updatedOrder, string Url);

        Task<string> PostAssignedContractorToOrderAsync(string orderID, string contractorID, string targetUrl);
    }
}
