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
        private string urlGetOrder;
        private string urlTakeOrder;
        private string targetUrlPickedUpOrder;
        private string targetUrlDeliveredOrder;
        private string urlGoogleDistanceCalculation;
        private string urlUpdateOrderTracking;
        private string updateType;
        private Uri UriGetOrder;
        public Order Order { get; set; }
        public string currentUserID;
        public string clientType;
        public GoogleRoute GoogleRoute { get; set; }

        public void OnGet(string orderNumber)
        {
            //// getting saved identity token
            //var identityToken = Task.Run(() => HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken)).Result;
            UpdateUserClaimsFromIdentityServer4();

            urlGetOrder = "http://localhost:8080/server_war_exploded/root/api/order/";
            UriGetOrder = new Uri(new Uri(urlGetOrder), orderNumber);
            orderService = new OrderService();

            Task<Order> taskOrder = orderService.GetOrderByOrderNumberAsync(UriGetOrder);
            taskOrder.Wait();
            Order = taskOrder.Result;

            urlGoogleDistanceCalculation = $"https://maps.googleapis.com/maps/api/distancematrix/json?units=imperial" +
                "&origins=" +
                $"{Order.pickUpAddress.city.Replace(" ", "+")}," +
                $"{Order.pickUpAddress.zipCode.Replace(" ", "+")}," +
                $"{Order.pickUpAddress.country.Replace(" ", "+")}," +
                $"{Order.pickUpAddress.street.Replace(" ", "+")}" +
                "&destinations=" +
                $"{Order.dropOffAddress.city.Replace(" ", "+")}," +
                $"{Order.dropOffAddress.zipCode.Replace(" ", "+")}," +
                $"{Order.dropOffAddress.country.Replace(" ", "+")}," +
                $"{Order.dropOffAddress.street.Replace(" ", "+")}" +
                "&key=AIzaSyCbrS4DNvvkVu_i4iEAWM5T_5K2H0Ck3_Y&";

            Task<GoogleRoute> taskGoogleRoute = orderService.GetRouteInformationFromGoogleAPI(urlGoogleDistanceCalculation);
            taskGoogleRoute.Wait();
            GoogleRoute = taskGoogleRoute.Result;
        }

        public void OnPostTakeOrder(string orderNumber)
        {
            orderService = new OrderService();
            UpdateUserClaimsFromIdentityServer4();

            urlTakeOrder = "http://localhost:8080/server_war_exploded/root/api/acceptorder/";
            Task<string> response = orderService.PostAssignedContractorToOrderAsync(orderNumber, currentUserID, urlTakeOrder);

            urlGetOrder = "http://localhost:8080/server_war_exploded/root/api/order/";
            UriGetOrder = new Uri(new Uri(urlGetOrder), orderNumber);
            Order = Task.Run(() => orderService.GetOrderByOrderNumberAsync(UriGetOrder)).Result;
        }

        public void OnPostLateOrder(string orderNumber)
        {
            orderService = new OrderService();
            updateType = "lateOrder";
            UpdateUserClaimsFromIdentityServer4();

            urlUpdateOrderTracking = "http://localhost:8080/server_war_exploded/root/api/updatestatus";
            Task<string> response = orderService.PostUpdateOrderTrackingAsync(orderNumber, currentUserID, updateType, urlUpdateOrderTracking);

            urlGetOrder = "http://localhost:8080/server_war_exploded/root/api/order/";
            UriGetOrder = new Uri(new Uri(urlGetOrder), orderNumber);
            Order = Task.Run(() => orderService.GetOrderByOrderNumberAsync(UriGetOrder)).Result;
        }

        public void OnPostPickedUp(string orderNumber)
        {
            orderService = new OrderService();
            updateType = "orderPickedUp";
            UpdateUserClaimsFromIdentityServer4();

            SetUpdateOrderTrackingUrl();
            Task<string> response = orderService.PostUpdateOrderTrackingAsync(orderNumber, currentUserID, updateType, urlUpdateOrderTracking);

            urlGetOrder = "http://localhost:8080/server_war_exploded/root/api/order/";
            UriGetOrder = new Uri(new Uri(urlGetOrder), orderNumber);
            Order = Task.Run(() => orderService.GetOrderByOrderNumberAsync(UriGetOrder)).Result;
        }

        public void OnPostDelivered(string orderNumber)
        {
            orderService = new OrderService();
            updateType = "orderDelivered";
            UpdateUserClaimsFromIdentityServer4();

            SetUpdateOrderTrackingUrl();
            Task<string> response = orderService.PostUpdateOrderTrackingAsync(orderNumber, currentUserID, updateType, urlUpdateOrderTracking);

            urlGetOrder = "http://localhost:8080/server_war_exploded/root/api/order/";
            UriGetOrder = new Uri(new Uri(urlGetOrder), orderNumber);
            Order = Task.Run(() => orderService.GetOrderByOrderNumberAsync(UriGetOrder)).Result;
        }

        public void OnPostCancelOrderAsContractor(string orderNumber)
        {
            orderService = new OrderService();
            UpdateUserClaimsFromIdentityServer4();

            string urlCancelAsContractor = "http://localhost:8080/server_war_exploded/root/api/refusetaken/" + orderNumber;
            Task<string> response = orderService.CancelOrderAsContractorAsync(urlCancelAsContractor);

            urlGetOrder = "http://localhost:8080/server_war_exploded/root/api/order/";
            UriGetOrder = new Uri(new Uri(urlGetOrder), orderNumber);
            Order = Task.Run(() => orderService.GetOrderByOrderNumberAsync(UriGetOrder)).Result;
        }

        public void OnPostDeleteOrderAsCustomer(string orderNumber)
        {
            orderService = new OrderService();
            UpdateUserClaimsFromIdentityServer4();

            string deleteOrderUrl = "http://localhost:8080/server_war_exploded/root/api/deleteunsigned/" + orderNumber;
            Task<string> response = orderService.DeleteOrderAsync(deleteOrderUrl);

            /*
            urlGetOrder = "http://localhost:8080/server_war_exploded/root/api/order/";
            UriGetOrder = new Uri(new Uri(urlGetOrder), orderNumber);
            Order = Task.Run(() => orderService.GetOrderByOrderNumberAsync(UriGetOrder)).Result;*/
        }

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

        [NonHandler]
        public void SetUpdateOrderTrackingUrl()
        {
            urlUpdateOrderTracking = "http://localhost:8080/server_war_exploded/root/api/updatestatus";
        }
    }
}