using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client_Customer.Models
{
    /// <summary>
    /// Used for representing a list of clients in one object.
    /// </summary>
    public class ClientList
    {
        public List<Client> clients { get; set; }
    }
}
