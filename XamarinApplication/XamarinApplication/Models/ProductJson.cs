using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApplication.Models
{
    public class ProductJson
    {
        public Product product { get; set; }
        public string username { get; set; }
        //public Price prices { get; set; }
        public PackagingCost eubasicpcost { get; set; }
        public PackagingCost euddpcost { get; set; }
        //public int piecesPerContainer { get; set; }
    }
}
