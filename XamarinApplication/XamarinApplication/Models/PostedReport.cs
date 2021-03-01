using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApplication.Models
{
    public class PostedReport
    {
        public long id { get; set; }
        public string sendingDate { get; set; }
        public string flState { get; set; }
        public string note { get; set; }
    }
}
