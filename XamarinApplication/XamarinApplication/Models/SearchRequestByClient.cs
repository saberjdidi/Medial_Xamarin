using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApplication.Models
{
    public class SearchRequestByClient
    {
        #region Declaration
        private Client s_client;
        private DateTime s_fromDate;
        private DateTime s_toDate;
        #endregion

        #region Property
        public Client client
        {
            get { return s_client; }
            set { this.s_client = value; }
        }
        public DateTime fromDate
        {
            get { return s_fromDate; }
            set { this.s_fromDate = value; }
        }
        public DateTime toDate
        {
            get { return s_toDate; }
            set { this.s_toDate = value; }
        }
        #endregion

        #region CONSTRUCTOR
        public SearchRequestByClient()
        {
            s_fromDate = default(DateTime);
            s_toDate = default(DateTime);
        }
        #endregion
    }
}
