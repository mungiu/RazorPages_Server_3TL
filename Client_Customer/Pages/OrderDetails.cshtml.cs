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
    public class OrderDetailsModel : PageModel
    {
        private OrderService orderService;
        private string targetUrlGetOrder;
        private string targetUrlTakeOrder;
        private string targetUrlLateDelivery;
        private Uri targetUriGetOrder;
        public Order orderModel;
        public string clientType;

        public void OnGet(string orderNumber)
        {
            // getting saved identity token
            var identityToken = Task.Run(() => HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken)).Result;

            // writing out user claims
            foreach (var claim in User.Claims)
            {
                if (claim.Type.Equals("role"))
                    clientType = claim.Value;
            }

            targetUrlGetOrder = "http://localhost:8080/server_war_exploded/root/api/order/";
            targetUriGetOrder = new Uri(new Uri(targetUrlGetOrder), orderNumber);
            orderService = new OrderService();
            orderModel = Task.Run(() => orderService.GetOrderByOrderNumberAsync(targetUriGetOrder)).Result;
        }

        public void OnPostTakeOrder(string orderNumber)
        {
            string currentUserID = null;

            // getting saved identity token
            var identityToken = Task.Run(() => HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken)).Result;

            // writing out user claims
            foreach (var claim in User.Claims)
            {
                if (claim.Type.Equals("sub"))
                    currentUserID = claim.Value;
            }

            targetUrlTakeOrder = "http://localhost:8080/server_war_exploded/root/api/acceptorder/";
            orderService = new OrderService();
            Task<string> response = orderService.PutAssignedContractorToOrderAsync(orderNumber, currentUserID, targetUrlTakeOrder);

            targetUrlGetOrder = "http://localhost:8080/server_war_exploded/root/api/order/";
            targetUriGetOrder = new Uri(new Uri(targetUrlGetOrder), orderNumber);
            orderModel = Task.Run(() => orderService.GetOrderByOrderNumberAsync(targetUriGetOrder)).Result;
        }

        public void OnPostLateOrder(string orderNumber)
        {
            /// TODO: REQUIRES REAL URL
            string currentUserID = null;

            // getting saved identity token
            var identityToken = Task.Run(() => HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken)).Result;

            // writing out user claims
            foreach (var claim in User.Claims)
            {
                if (claim.Type.Equals("sub"))
                    currentUserID = claim.Value;
            }

            targetUrlLateDelivery = "http://localhost:8080/server_war_exploded/root/api/lateDelivery/";
            orderService = new OrderService();
            Task<string> response = orderService.PutAssignedContractorToOrderAsync(orderNumber, currentUserID, targetUrlLateDelivery);

            targetUrlGetOrder = "http://localhost:8080/server_war_exploded/root/api/order/";
            targetUriGetOrder = new Uri(new Uri(targetUrlGetOrder), orderNumber);
            orderModel = Task.Run(() => orderService.GetOrderByOrderNumberAsync(targetUriGetOrder)).Result;
        }
    }
}