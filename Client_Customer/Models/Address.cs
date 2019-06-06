using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client_Customer.Models
{
    /// <summary>
    /// Represents the address as a Model when address information processing happens.
    /// </summary>
    public class Address
    {
        public string street { get; set; }
        public string city { get; set; }
        public string zipCode { get; set; }
        public string country { get; set; }
    }
}
