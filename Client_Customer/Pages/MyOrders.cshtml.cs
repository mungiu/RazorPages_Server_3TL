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
{
    public class MyOrdersModel : PageModel
    {
        private OrderService orderService;
        private Uri targetUri;
        public OrderList orderList;

        public void OnGet()
        {
            string currentUserID = null;

            foreach (var claim in User.Claims)
            {
                if (claim.Type.Equals("sub"))
                    currentUserID = claim.Value;
            }

            orderService = new OrderService();
            targetUri = new Uri("http://localhost:8080/server_war_exploded/root/api/myorders/" + currentUserID);
            orderList = Task.Run(() => orderService.GetOrderListAsync(targetUri)).Result;
        }
    }
}