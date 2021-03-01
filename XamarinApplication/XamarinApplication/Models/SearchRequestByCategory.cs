using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApplication.Models
{
    public class SearchRequestByCategory
    {
        #region Declaration
        private string s_code;
        private string s_description;
        private Category s_category;
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
        public Category category
        {
            get { return s_category; }
            set
            {
                this.s_category = value;
            }
        }
        #endregion
    }
}
