using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApplication.Models
{
    public class SearchProductComponent
    {
        public bool composed { get; set; }
        public bool isActive { get; set; }
        public int maxResult { get; set; }
        public string order { get; set; }
        public string sortedBy { get; set; }
    }
}
