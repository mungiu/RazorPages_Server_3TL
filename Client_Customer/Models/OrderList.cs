using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client_Customer.Models
{
    public class OrderList
    {
        public List<Order> Orders { get; set; }

        //public OrderList()
        //{
        //    // creating order simulation
        //    //Orders = new List<Order>() {
        //    //new Order("4613516313","Germany, Berlin",DateTime.Now,"USA, Washington",DateTime.Now,"something golden","medium",123),
        //    //new Order("1631896113","Netherlands, Amsterdam",DateTime.Now,"Denmark, Copenhagen",DateTime.Now,"something silverish","small",123)
        //    //};
        //    // updating order simulation
        //}
    }
}
