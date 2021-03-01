using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApplication.Models
{
    public class SearchModel
    {
        #region Declaration
        private long s_id1;
        private long s_id2;
        private long s_id3;
        private long s_nomenclatureId;
        private string s_criteria0;
        private string s_criteria1;
        private string s_criteria2;
        private string s_criteria3;
        private string s_criteria4;
        private string s_criteria5;
        private string s_status;
        private DateTime s_date;
        private DateTime s_date1;
        private long s_ambulatoireService;
        private string s_check1;
        private string s_positive;
        private int s_downloadStatus;
        private int s_offset;
        private int s_maxResult;
        private string s_order;
        private string s_sortedBy;

        #endregion

        #region Property

        public long id1 
        {
            get { return s_id1; }
            set { this.s_id1 = value; }
        }
        public long id2
        {
            get { return s_id2; }
            set { this.s_id2 = value; }
        }
        public long id3
        {
            get { return s_id3; }
            set { this.s_id3 = value; }
        }
        public long nomenclatureId
        {
            get { return s_nomenclatureId; }
            set { this.s_nomenclatureId = value; }
        }
        public string criteria0
        {
            get { return s_criteria0; }
            set { this.s_criteria0 = value; }
        }
        public string criteria1
        {
            get { return s_criteria1; }
            set { this.s_criteria1 = value; }
        }
        public string criteria2
        {
            get { return s_criteria2; }
            set { this.s_criteria2 = value; }
        }
        public string criteria3
        {
            get { return s_criteria3; }
            set { this.s_criteria3 = value; }
        }
        public string criteria4
        {
            get { return s_criteria4; }
            set { this.s_criteria4 = value; }
        }
        public string criteria5
        {
            get { return s_criteria5; }
            set { this.s_criteria5 = value; }
        }
        public string status
        {
            get { return s_status; }
            set { this.s_status = value; }
        }
        public DateTime date
        {
            get { return s_date; }
            set { this.s_date = value; }
        }
        public DateTime date1
        {
            get { return s_date1; }
            set { this.s_date1 = value; }
        }
        public long ambulatoireService
        {
            get { return s_ambulatoireService; }
            set { this.s_ambulatoireService = value; }
        }
        public string check1
        {
            get { return s_check1; }
            set { this.s_check1 = value; }
        }
        public string positive
        {
            get { return s_positive; }
            set { this.s_positive = value; }
        }
        public int downloadStatus
        {
            get { return s_downloadStatus; }
            set { this.s_downloadStatus = value; }
        }
        public int offset
        {
            get { return s_offset; }
            set { this.s_offset = value; }
        }
        public int maxResult
        {
            get { return s_maxResult; }
            set { this.s_maxResult = value; }
        }
        public string order
        {
            get { return s_order; }
            set { this.s_order = value; }
        }
        public string sortedBy
        {
            get { return s_sortedBy; }
            set { this.s_sortedBy = value; }
        }

        #endregion

        #region Constructor
        public SearchModel()
        {

            s_id1 = -1;
            s_id2 = -1;
            s_id3 = -1;
            s_nomenclatureId = -1;
            s_criteria0 = string.Empty;
            s_criteria1 = string.Empty;
            s_criteria2 = string.Empty;
            s_criteria3 = string.Empty;
            s_criteria4 = string.Empty;
            s_criteria5 = string.Empty;
            //s_status = string.Empty;
            s_date = default(DateTime);
            s_date1 = default(DateTime);
            s_ambulatoireService = -1;
            s_check1 = null;
            s_positive = null;
            s_downloadStatus = 0;
            s_offset = 0;
    }
    #endregion
}
}
