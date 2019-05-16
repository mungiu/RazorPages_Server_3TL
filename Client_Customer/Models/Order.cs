using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Client_Customer.Models
{
    public class Order
    {
        public string orderNumber { get; set; }
        public string companyID { get; set; }
        public Address pickUpAddress { get; set; }
        public string pickUpDeadline { get; set; }
        public Address dropOffAddress { get; set; }
        public string dropOffDeadline { get; set; }
        public string contentDescription { get; set; }
        public string containerSize { get; set; }
        public float weight { get; set; }
        public string size { get; set; }
        public float price { get; set; }
        public bool awaitingPickUp { get; set; }
        public bool pickedUp { get; set; }
        public bool delivered { get; set; }
    }
}
