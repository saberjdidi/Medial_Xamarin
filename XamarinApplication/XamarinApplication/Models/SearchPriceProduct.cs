using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApplication.Models
{
    public class SearchPriceProduct
    {
         public List<int> supplierList { get; set; }
        // public object supplierList { get; set; }
        public DateTime validationTime { get; set; }
        public string format { get; set; }

        public SearchPriceProduct()
        {
           // supplierList = new List<Supplier>();
        }
    }
}
