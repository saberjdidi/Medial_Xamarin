using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApplication.Models
{
    public class CommercialDetails
    {
        public long id { get; set; }
        public Reggion region { get; set; }
        public User agent { get; set; }
        public string headQuarter { get; set; }
        public string representant { get; set; }
        public string paymentCondition { get; set; }
    }
}
