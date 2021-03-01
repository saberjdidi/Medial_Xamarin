using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApplication.Models
{
    public class AddEvent
    {
        public string title { get; set; }
        public string note { get; set; }
        public TaskStatuss status { get; set; }
        public TaskType type { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string colorPrimary { get; set; }
    }
}
