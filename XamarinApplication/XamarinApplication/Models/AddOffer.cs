using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApplication.Models
{
    public class AddOffer
    {
        public Client client { get; set; }
        //public User createdBy { get; set; }
        public string supplyCondition { get; set; }
        public string paymentCondition { get; set; }
        public string note { get; set; }
        public DateTime date { get; set; }
    }
}
