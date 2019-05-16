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
        OrderService orderService;
        Uri targetUri;
        public OrderList orderList;

        public void OnGet()
        {
            targetUri = new Uri("http://localhost:8080/server_war_exploded/root/api/orders"/* insert RESTful URL to GET User related orders, includes current user ID*/);
            orderService = new OrderService();
            orderList = Task.Run(() => orderService.GetOrderListAsync(targetUri)).Result;


            var identityToken = Task.Run(() => HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken)).Result;

            //// FOR DEBUG PURPOSE
            //// writing it out
            //Debug.WriteLine($"Identity token: {identityToken}");
            //// writing out user claims
            //foreach (var claim in User.Claims)
            //{
            //    Debug.WriteLine($"Claim type: {claim.Type} - Claim value: {claim.Value}");
            //}
        }
    }
}