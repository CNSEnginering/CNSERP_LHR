using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.Finance.Dto
{
    public class BankReconcilationReportDto
    {
        public double Opening { get; set; }
        public double ClearedPayments { get; set; }
        public int ClearedItems { get; set; }
        public int ClearedDepositItems { get; set; }
        public double ClearedDepositPayments { get; set; }
        public double UnClearedPayments { get; set; }
        public int UnClearedItems { get; set; }
        public double UnClearedDeposits { get; set; }
        public double UnClearedDepositItems { get; set; }   
    }

    public class BankReconcilationReportDetailDto
    {
        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public string SubType { get; set; }
        public string Type { get; set; }
        public int Id { get; set; }
        public string Reference { get; set; }
        public string Narration { get; set; }
    }
}
