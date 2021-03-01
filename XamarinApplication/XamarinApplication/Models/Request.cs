using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApplication.Models
{
    public class Request
    {
        public long id { get; set; }
        public string code { get; set; }
        public DateTime? creationDate { get; set; }
        public DateTime? validationDate { get; set; }
        public string groupId { get; set; }
        public DateTime? checkDate { get; set; }
        public Branch branch { get; set; }
        public Patient patient { get; set; }
        public Nomenclatura nomenclatura { get; set; }
        public Client client { get; set; }
        public Status status { get; set; }
        public bool isGroup { get; set; }
    }
}
