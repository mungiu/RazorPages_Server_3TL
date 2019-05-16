using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Client_Customer.Models
{
    public class Client
    {

        public int clientID { get; set; }
        public string companyName { get; set; }
        public Address address { get; set; }
        public string email { get; set; }
        public string telephoneNumber { get; set; }
        public string clientType { get; set; }
        public string passwordHashW { get; set; }
    }
}
