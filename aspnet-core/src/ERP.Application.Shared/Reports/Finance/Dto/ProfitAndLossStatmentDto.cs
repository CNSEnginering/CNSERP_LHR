using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.Finance.Dto
{
    public class ProfitAndLossStatmentDto
    {
       // public int Year { get; set; }

        public string HeadingText { get; set; }

        //public string AccountID { get; set; }
        public string TypeId { get; set; }
        public double? Amount { get; set; }

        //public int PrevYear { get; set; }

        //public string PrevHeadingText { get; set; }

        //public string PrevAccountID { get; set; }

        public double? PrevAmount { get; set; }
        public double? GLPLCtGId { get; set; }
        public string Type { get; set; }
    }
}
