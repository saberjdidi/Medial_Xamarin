using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApplication.Models
{
    public class Event
    {
        public long id { get; set; }
        public string title { get; set; }
        public double startsAt { get; set; }
        public double endsAt { get; set; }
        public ColorEvent color { get; set; }
        public bool draggable { get; set; } 
        public bool resizable { get; set; }
    }
}
