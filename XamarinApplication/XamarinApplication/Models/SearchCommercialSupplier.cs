using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApplication.Models
{
   public class SearchCommercialSupplier
    {
        public int maxResult { get; set; }
        public int supplierId { get; set; }
        public string order { get; set; }
        public string sortedBy { get; set; }
        public string criteria { get; set; }
        public Double exchangeRate { get; set; }
        public string format { get; set; }
    }
}
