using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.Finance
{
    public interface ITrialBalanceAppService
    {
        List<TrialBalanceDto> GetData(int TenantId, string FromDate, string ToDate, string FromAcc, string ToAcc, int status, int locId, bool includeZeroBalance, string curRate);
    }
    public class TrialBalanceDto
    {
        public string Seg1 { get; set; }
        public string Seg2 { get; set; }
        public string Seg3 { get; set; }
        public string AccountId { get; set; }
        public string Seg1Name { get; set; }
        public string Seg2Name { get; set; }
        public string Seg3Name { get; set; }
        public string AccountName { get; set; }
        public string Family { get; set; }
        public double? Debit { get; set; }
        public double? Credit { get; set; }
        public double? OpeningDebit { get; set; }
        public double? OpeningCredit { get; set; }
        public bool Sl { get; set; }
        public int LocId { get; set; }
        public string LocDesc { get; set; }
    }
}
