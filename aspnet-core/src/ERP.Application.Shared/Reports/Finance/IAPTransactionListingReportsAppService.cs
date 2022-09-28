using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.Finance
{
    public interface IAPTransactionListingReportsAppService
    {
        List<TransactionListDto> GetData(DateTime FromDate, DateTime ToDate, string Book, string User, int tenantId, string status, int locId);
    }
    public class TransactionListDto
    {
        public string ChequeNo { get; set; }
        public int Id { get; set; }
        public int? SubAccId { get; set; }
        public string AccountId { get; set; }
        public string BookConfigMerge { get; set; }
        public int DetId { get; set; }
        public string BookId { get; set; }
        public string BookName { get; set; }
        public int ConfigID { get; set; }
        public string AccountName { get; set; }
        public string Narration { get; set; }
        public double Debit { get; set; }
        public double Credit { get; set; }
        public bool Posted { get; set; }
        public int Doc { get; set; }
        public string Date { get; set; }
        public string User { get; set; }
        public DateTime? AudtTime { get; set; }
        public string SubAccName { get; set; }
        public string PostedBy { get; set; }
        public DateTime PostedDate { get; set; }
        public int LocId { get; set; }
        public string LocDesc { get; set; }
        public string LedgerDesc { get; set; }
        public string Status { get; set; }
    }

}
