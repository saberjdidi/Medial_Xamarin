using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApplication.Models
{
    public class Nomenclatura
    {
        public long id { get; set; }
        public string code { get; set; }
        public string descrEsameProf { get; set; }
        public DateTime? startValidation { get; set; }
        public DateTime? endValidation { get; set; }
    }
}
