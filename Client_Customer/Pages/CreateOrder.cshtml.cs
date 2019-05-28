using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Client_Customer.Models;
using Client_Customer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Client_Customer.Pages
{
    public class CreateOrderModel : PageModel
    {
        private OrderService orderService;
        private string targetUrl;

        [BindProperty]
        public Order Order { get; set; }
        public string currentUserID;
        public string clientType;

        public void OnGet()
        {
            foreach (var claim in User.Claims)
            {
                if (claim.Type.Equals("sub"))
                    currentUserID = claim.Value;
                if (claim.Type.Equals("role"))
                    clientType = claim.Value;
            }
        }
        public void OnPost()
        {
            foreach (var claim in User.Claims)
            {
                if (claim.Type.Equals("sub"))
                    Order.companyID = claim.Value;
                if (claim.Type.Equals("role"))
                    clientType = claim.Value;
            }

            targetUrl = "http://localhost:8080/server_war_exploded/root/api/order";
            orderService = new OrderService();
            Task<string> response = orderService.PostNewOrderAsync(Order, targetUrl);

            //Redirect("http://localhost:8080/server_war_exploded/root/api/orders");
        }
    }
}