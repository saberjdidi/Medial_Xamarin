using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApplication.Models
{
    public class AddClient
    {
        public string code { get; set; }
        public string description { get; set; }
        public string district { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string fiscalCode { get; set; }
        public string iva { get; set; }
        public string note { get; set; }
        public string clientTaskLog { get; set; }
        public Country country { get; set; }
        public Category category { get; set; }
        public Groupe groupe { get; set; }
        public Province province { get; set; }
        public AddCommercialDetails commercialDetails { get; set; }
    }
}
