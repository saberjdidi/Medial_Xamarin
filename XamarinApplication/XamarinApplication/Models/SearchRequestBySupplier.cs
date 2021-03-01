using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApplication.Models
{
    public class SearchRequestBySupplier
    {
        #region Declaration
        private string s_code;
        private string s_description;
        private Supplier s_supplier;
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
        public Supplier supplier
        {
            get { return s_supplier; }
            set
            {
                this.s_supplier = value;
            }
        }
        #endregion
    }
}
