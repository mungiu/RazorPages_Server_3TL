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
        OrderService orderService;
        string targetUrl;

        [BindProperty]
        public Order Order { get; set; }

        //public string PickUpAddress { get; set; }
        //public DateTime PickUpDeadline { get; set; }
        //public string DropOffAddress { get; set; }
        //public DateTime DropOffDeadline { get; set; }
        //public string PackageContentDescription { get; set; }
        //public string ContainerSize { get; set; }
        //public int PackageWeight { get; set; }

        public void OnPost()
        {
            // TODO: make real value and not hardcoded value
            Order.companyID = "1111";
            targetUrl = "http://localhost:8080/server_war_exploded/root/api/order";
            orderService = new OrderService();
            Task<string> response = orderService.PostNewOrderAsync(Order, targetUrl);
        }
    }
}