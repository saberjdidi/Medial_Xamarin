using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApplication.Models
{
    public class AddCurrency
    {
        public string entity { get; set; }
        public string currency { get; set; }
        public string alphabeticCode { get; set; }
        public string numericCode { get; set; }
        public string minorUnit { get; set; }
    }
}
