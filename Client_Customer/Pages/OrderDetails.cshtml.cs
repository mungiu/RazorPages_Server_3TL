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
    /// <summary>
    /// Model class for Order Details, responsible for handling all get/post requests directly from the User.
    /// </summary>
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

        /// <summary>
        /// Handles any unspecified OnGet requests from user. Currently returns all details about the current order using both the Java API and the Google API in series. NOT in paralel.
        /// </summary>
        /// <param name="orderNumber"></param>
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
        /// <summary>
        /// Hnadles specific Post requests for taking an order as a contractor.
        /// </summary>
        /// <param name="orderNumber"></param>
        /// <returns></returns>
        public IActionResult OnPostTakeOrder(string orderNumber)
        {
            orderService = new OrderService();
            UpdateUserClaimsFromIdentityServer4();

            urlTakeOrder = "http://localhost:8080/server_war_exploded/root/api/acceptorder/";
            Task<string> response = orderService.PostAssignedContractorToOrderAsync(orderNumber, currentUserID, urlTakeOrder);

            return RedirectToPage("MyOrders");
        }
        /// <summary>
        /// Allows the contractor thee notify that he will be late.
        /// </summary>
        /// <param name="orderNumber"></param>
        /// <returns></returns>
        public IActionResult OnPostLateOrder(string orderNumber)
        {
            orderService = new OrderService();
            updateType = "lateOrder";
            UpdateUserClaimsFromIdentityServer4();

            urlUpdateOrderTracking = "http://localhost:8080/server_war_exploded/root/api/updatestatus";
            Task<string> response = orderService.PostUpdateOrderTrackingAsync(orderNumber, currentUserID, updateType, urlUpdateOrderTracking);

            return RedirectToPage("MyOrders");
        }
        /// <summary>
        /// Allows the contractor to notify that he has picked up the order.
        /// </summary>
        /// <param name="orderNumber"></param>
        /// <returns></returns>
        public IActionResult OnPostPickedUp(string orderNumber)
        {
            orderService = new OrderService();
            updateType = "orderPickedUp";
            UpdateUserClaimsFromIdentityServer4();

            SetUpdateOrderTrackingUrl();
            Task<string> response = orderService.PostUpdateOrderTrackingAsync(orderNumber, currentUserID, updateType, urlUpdateOrderTracking);

            return RedirectToPage("MyOrders");
        }
        /// <summary>
        /// Allows the contractor to notify that he has delivered the order.
        /// </summary>
        /// <param name="orderNumber"></param>
        /// <returns></returns>
        public IActionResult OnPostDelivered(string orderNumber)
        {
            orderService = new OrderService();
            updateType = "orderDelivered";
            UpdateUserClaimsFromIdentityServer4();

            SetUpdateOrderTrackingUrl();
            Task<string> response = orderService.PostUpdateOrderTrackingAsync(orderNumber, currentUserID, updateType, urlUpdateOrderTracking);

            return RedirectToPage("MyOrders");
        }
        /// <summary>
        /// Allows the contractor to cancel his order.
        /// </summary>
        /// <param name="orderNumber"></param>
        /// <returns></returns>
        public IActionResult OnPostCancelOrderAsContractor(string orderNumber)
        {
            orderService = new OrderService();
            UpdateUserClaimsFromIdentityServer4();

            string urlCancelAsContractor = "http://localhost:8080/server_war_exploded/root/api/refusetaken/" + orderNumber;
            Task<string> response = orderService.CancelOrderAsContractorAsync(urlCancelAsContractor);

            return RedirectToPage("MyOrders");
        }
        /// <summary>
        /// Allows the customer to delete his order.
        /// </summary>
        /// <param name="orderNumber"></param>
        /// <returns></returns>
        public IActionResult OnPostDeleteOrderAsCustomer(string orderNumber)
        {
            orderService = new OrderService();
            UpdateUserClaimsFromIdentityServer4();

            string deleteOrderUrl = "http://localhost:8080/server_war_exploded/root/api/deleteunsigned/" + orderNumber;
            Task<string> response = orderService.DeleteOrderAsync(deleteOrderUrl);

            return RedirectToPage("MyOrders");

            /*
            urlGetOrder = "http://localhost:8080/server_war_exploded/root/api/order/";
            UriGetOrder = new Uri(new Uri(urlGetOrder), orderNumber);
            Order = Task.Run(() => orderService.GetOrderByOrderNumberAsync(UriGetOrder)).Result;*/
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
        /// <summary>
        /// Sets the URL for updating order tracking details.
        /// </summary>
        [NonHandler]
        public void SetUpdateOrderTrackingUrl()
        {
            urlUpdateOrderTracking = "http://localhost:8080/server_war_exploded/root/api/updatestatus";
        }
    }
}