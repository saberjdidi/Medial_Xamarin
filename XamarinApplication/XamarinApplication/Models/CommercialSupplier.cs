using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApplication.Models
{
   public class CommercialSupplier
    {
        public Product product { get; set; }
        public List<Price> prices { get; set; }
        public Cost eubasicpcost { get; set; }
        public Cost euddpcost { get; set; }
    }
}
