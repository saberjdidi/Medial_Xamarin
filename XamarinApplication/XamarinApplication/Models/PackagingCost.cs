using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApplication.Models
{
    public class PackagingCost
    {
        public long id { get; set; }
        public Currency currency { get; set; }
        public decimal value { get; set; }
    }
}
