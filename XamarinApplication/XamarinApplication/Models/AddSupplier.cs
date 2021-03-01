using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApplication.Models
{
    public class AddSupplier
    {
        public string code { get; set; }
        public string description { get; set; }
        public Country country { get; set; }
        public string note { get; set; }
        public Containner container { get; set; }
        public CustomsDuty customsDuty { get; set; }
        public bool european { get; set; }
        public bool exporter { get; set; }
        public bool dutyFree { get; set; }
    }
}
