using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH.Importing
{
    public class ImportTransactionDetailDto
    {
        public  int Id { get; set; }

        public  int DetID { get; set; }

        public  string AccountID { get; set; }

        public  int? SubAccID { get; set; }

        public  string Narration { get; set; }

        public  double? Amount { get; set; }

        public  string ChequeNo { get; set; }

        public  bool IsAuto { get; set; }

        public  int LocId { get; set; }

        public int TenantId { get; set; }

        /// <summary>
        /// Can be set when reading data from excel or when importing Control Detail
        /// </summary>
        public string Exception { get; set; }

        public bool CanBeImported()
        {
            return string.IsNullOrEmpty(Exception);
        }
    }
}
