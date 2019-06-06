using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Client_Customer.Models
{
    /// <summary>
    /// Represents the Order object which is used for Serializing and Deserializing JSON strings carrying Order information.
    /// </summary>
    public class Order
    {
        public string orderNumber { get; set; }
        public string companyID { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public Address pickUpAddress { get; set; }
        [Required]
        public string pickUpDeadline { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public Address dropOffAddress { get; set; }
        [Required]
        public string dropOffDeadline { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string contentDescription { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string containerSize { get; set; }
        [Required]
        [Range(0, 99999.99)]
        public float weight { get; set; }
        [Required]
        public string size { get; set; }
        [Required]
        [Range(0, 999999.99)]
        public float price { get; set; }
        public string responsibleCompany { get; set; }
        public bool awaitingPickUp { get; set; }
        public bool pickedUp { get; set; }
        public bool lateDelivery { get; set; }
        public bool delivered { get; set; }
        public float distance { get; set; }
    }
}
