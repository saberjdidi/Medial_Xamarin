using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApplication.Models
{
    public class ProductDTO
    {
        public Product product { get; set; }
        public List<Price> prices { get; set; }
    }
}
