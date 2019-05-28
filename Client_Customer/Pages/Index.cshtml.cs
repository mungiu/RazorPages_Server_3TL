using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Client_Customer.Models;
using Client_Customer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Diagnostics;

namespace Client_Customer.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private OrderService orderService;
        private Uri targetUri;
        public OrderList orderList;

        public void OnGet()
        {
            targetUri = new Uri("http://localhost:8080/server_war_exploded/root/api/allOrders");
            orderService = new OrderService();
            orderList = Task.Run(() => orderService.GetOrderListAsync(targetUri)).Result;


            //// FOR DEBUG PURPOSE getting saved identity token
            //var identityToken = Task.Run(() => HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken)).Result;
            //Debug.WriteLine($"Identity token: {identityToken}");
            //// writing out user claims
            //foreach(var claim in User.Claims)
            //{
            //    Debug.WriteLine($"Claim type: {claim.Type} - Claim value: {claim.Value}");
            //}

            //// How to search for an order
            //Predicate<Order> searchOrderPredicate = p => { return p.OrderNumber == "fakeSearchedForOrderNumber"; };
            //Order order = orderList.Result.Orders.Find(searchOrderPredicate);
        }
    }
}