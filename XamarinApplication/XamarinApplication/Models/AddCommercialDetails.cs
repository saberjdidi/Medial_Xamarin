using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApplication.Models
{
    public class AddCommercialDetails
    {
        public Reggion region { get; set; }
        public User agent { get; set; }
        public string headQuarter { get; set; }
        public string representant { get; set; }
        public string paymentCondition { get; set; }
    }
}
