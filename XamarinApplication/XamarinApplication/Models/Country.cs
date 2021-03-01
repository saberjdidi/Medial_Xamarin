using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApplication.Models
{
    public class Country
    {
        public long id { get; set; }
        public string iso { get; set; }
        public string name { get; set; }
        public string niceName { get; set; }
        public string iso3 { get; set; }
        public int numCode { get; set; }
        public int phoneCode { get; set; }
    }
}
