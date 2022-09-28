using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.GeneralLedger.Transaction.Dtos
{
    public class AccountsPostingListDto
    {
        public string BookId;
        public string BookName;
        public int ConfigId;
        public int DocNo;
        public DateTime DocDate;
        public string UserId;
        public bool Posted;
        public int? DetailId;

        public string LocDesc;
        public string NARRATION;
        public decimal? Amount;
    }
}
