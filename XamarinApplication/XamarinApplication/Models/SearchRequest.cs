using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApplication.Models
{
    public class SearchRequest
    {
        #region Declaration
        private string s_code;
        private string s_description;
        #endregion

        #region Property
        public string code
        {
            get { return s_code; }
            set { this.s_code = value; }
        }
        public string description
        {
            get { return s_description; }
            set { this.s_description = value; }
        }
        public string order { get; set; }
        public string sortedBy { get; set; }
        #endregion
    }
}
