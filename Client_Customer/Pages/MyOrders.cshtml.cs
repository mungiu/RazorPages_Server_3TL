using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client_Customer.Models;
using Client_Customer.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Client_Customer.Pages
{   /// <summary>
    /// Model class for MyOrders, responsible for handling all get/post requests directly from the User.
    /// </summary>
    public class MyOrdersModel : PageModel
    {
        private OrderService orderService;
        private Uri targetUri;
        public OrderList orderList;
        public string currentUserID;
        public string clientType;
        /// <summary>
        /// Handles any unspecified OnGet requests from user. Currently returns a list of all orders related to the logged in user.
        /// </summary>
        public void OnGet()
        {
            string currentUserID = null;

            UpdateUserClaimsFromIdentityServer4();

            orderService = new OrderService();
            targetUri = new Uri("http://localhost:8080/server_war_exploded/root/api/myorders/" + currentUserID);
            orderList = Task.Run(() => orderService.GetOrderListAsync(targetUri)).Result;
        }
        /// <summary>
        /// Gets all orders filtered by status
        /// </summary>
        public void OnGetByStatus()
        {
            string currentUserID = null;

            UpdateUserClaimsFromIdentityServer4();

            orderService = new OrderService();
            targetUri = new Uri("http://localhost:8080/server_war_exploded/root/api/myorders/orderbystatus" + currentUserID);
            orderList = Task.Run(() => orderService.GetOrderListAsync(targetUri)).Result;
        }
        /// <summary>
        /// Gets all orders filtered by deadline
        /// </summary>
        public void OnGetByDeadline()
        {
            string currentUserID = null;

            UpdateUserClaimsFromIdentityServer4();

            orderService = new OrderService();
            targetUri = new Uri("http://localhost:8080/server_war_exploded/root/api/myorders/orderbydeadline" + currentUserID);
            orderList = Task.Run(() => orderService.GetOrderListAsync(targetUri)).Result;
        }

        /// <summary>
        /// Updates information about who a user claims to be in the current session.
        /// </summary>
        [NonHandler]
        public void UpdateUserClaimsFromIdentityServer4()
        {
            foreach (var claim in User.Claims)
            {
                if (claim.Type.Equals("sub"))
                    currentUserID = claim.Value;
                if (claim.Type.Equals("role"))
                    clientType = claim.Value;
            }
        }
    }
}