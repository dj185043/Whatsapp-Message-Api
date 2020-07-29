using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SendMessage.Models
{
    public class Prediction
    {
        public int Period { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string SKU { get; set; }
        public string Type { get; set; }
    }
}