using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApplication.Models
{
    public class Patient
    {
        public long id { get; set; }
        public string title { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string fullName { get; set; }
        public string birthDate { get; set; }
        public string fiscalCode { get; set; }
        public string phone { get; set; }
        public string cellPhone { get; set; }
        public string email { get; set; }
        public string note { get; set; }
        public string mpiCode { get; set; }
        public DateTime? deleteDate { get; set; }
        
    }
}
