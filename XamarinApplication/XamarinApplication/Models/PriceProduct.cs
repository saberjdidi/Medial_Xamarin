using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApplication.Models
{
    public class PriceProduct
    {
        public Supplier supplier { get; set; }
        //public object productDTOs { get; set; }
        public List<ProductDTO> productDTOs { get; set; }
    }
}
