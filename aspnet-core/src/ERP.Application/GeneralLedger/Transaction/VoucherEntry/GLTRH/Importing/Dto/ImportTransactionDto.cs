using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH.Importing
{
    public class ImportTransactionDto
    {
        public  int Id { get ; set; }

        //public int? VouchNo { get; set; }
        public  string BookID { get; set; }

        
        public  int ConfigID { get; set; }

        
        public  int DocNo { get; set; }

        public  string FmtDocNo { get; set; }

        
        public  int DocMonth { get; set; }

        
        public  DateTime DocDate { get; set; }

        public  string NARRATION { get; set; }

        
        public  bool Posted { get; set; }

        public  string PostedBy { get; set; }

        public  DateTime? PostedDate { get; set; }

        public  bool Approved { get; set; }

        public  string AprovedBy { get; set; }

        public  DateTime? AprovedDate { get; set; }

        public  string AuditUser { get; set; }

        public  DateTime? AuditTime { get; set; }

        public  string OldCode { get; set; }

        public  string CURID { get; set; }

        public  double CURRATE { get; set; }

        public  string CreatedBy { get; set; }

        public  DateTime? CreatedOn { get; set; }
        public  byte? ChType { get; set; }
        public  string ChNumber { get; set; }
        public  decimal? Amount { get; set; }

        public  int LocId { get; set; }

        public string Reference { get; set; }

        public int TenantId { get; set; }

        public List<ImportTransactionDetailDto> importTransactionDetailDtos { get; set; }

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
