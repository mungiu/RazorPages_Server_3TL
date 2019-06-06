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
    /// <summary>
    /// Model class for CreateOrder page, responsible for handling all get/post requests directly from the User.
    /// </summary>
    public class CreateOrderModel : PageModel
    {
        private OrderService orderService;
        private string targetUrl;

        [BindProperty]
        public Order Order { get; set; }
        public string currentUserID;
        public string clientType;
        /// <summary>
        /// Handles any unspecified OnGet requests from user. Currently retrievs user information for further needs such as order binding to customerID.
        /// </summary>
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
        /// <summary>
        /// Handles any unspecified OnPost requests from user. Currently posts a new order to the second tier of this application after gatherin information about the currently logged in user.
        /// </summary>
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
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

                return RedirectToPage("MyOrders", currentUserID);
            }
            else
            {
                return RedirectToPage("CreateOrder");
            }
            //Redirect("http://localhost:8080/server_war_exploded/root/api/orders");
        }
    }
}