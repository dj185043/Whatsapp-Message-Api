using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SendMessage.Models
{
    public class Transaction
    {
        public string TransactionID { get; set; }
        public string TransactionDateTime { get; set; }
        public string SKU { get; set; }
        public string CustomerID { get; set; }
        public decimal NetPrice { get; set; }
        public int Quantity { get; set; }
        public string State { get; set; }
    }
}