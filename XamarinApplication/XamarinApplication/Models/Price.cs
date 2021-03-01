using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApplication.Models
{
    public class Price
    {
        public long id { get; set; }
        public DateTime? validationTime { get; set; }
        public Cost cost { get; set; }
        public User user { get; set; }
    }
}
