using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client_Customer.Models
{
    /// <summary>
    /// Represents the top level class used for deserializing data retrieved from the Distance Matrix API provided by google.
    /// </summary>
    public class GoogleRoute
    {
        public string[] destination_addresses { get; set; }
        public string[] origin_addresses { get; set; }
        public Row[] rows { get; set; }
        public string status { get; set; }
    }
    /// <summary>
    /// A list of grouped elements from the Distance Matrix API
    /// </summary>
    public class Row
    {
        public Element[] elements { get; set; }
    }
    /// <summary>
    /// Groups all elements of the Distance Matrix API response into one object
    /// </summary>
    public class Element
    {
        public Distance distance { get; set; }
        public Duration duration { get; set; }
        public string status { get; set; }
    }
    /// <summary>
    /// Represents the distance received from the google api
    /// </summary>
    public class Distance
    {
        public string text { get; set; }
        public int value { get; set; }
    }
    /// <summary>
    /// Represents the duration received from the google api
    /// </summary>
    public class Duration
    {
        public string text { get; set; }
        public int value { get; set; }
    }

}
