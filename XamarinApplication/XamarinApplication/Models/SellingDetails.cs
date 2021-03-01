using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApplication.Models
{
    public class SellingDetails
    {
        public long id { get; set; }
        public Product product { get; set; }
        public Client client { get; set; }
        public int quantity { get; set; }
        public decimal totalPrice { get; set; }
        public DateTime sellingDate { get; set; }
        public string billingKey { get; set; }
        public int billingRow { get; set; }
    }
}
