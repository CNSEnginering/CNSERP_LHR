using System;
using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Sales.OECSH.Dtos
{
    public class OECSHDto : EntityDto
    {
        public int LocID { get; set; }

        public int DocNo { get; set; }

        public DateTime? DocDate { get; set; }

        public string SaleDoc { get; set; }

        public string MDocNo { get; set; }

        public DateTime? MDocDate { get; set; }

        public string TypeID { get; set; }

        public string SalesCtrlAcc { get; set; }

        public int? CustID { get; set; }

        public string Narration { get; set; }

        public string NoteText { get; set; }

        public string PayTerms { get; set; }

        public string DelvTerms { get; set; }

        public string ValidityTerms { get; set; }

        public bool Approved { get; set; }

        public string ApprovedBy { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public bool Active { get; set; }

        public string AudtUser { get; set; }

        public DateTime? AudtDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }

        public string BasicStyle { get; set; }

        public string License { get; set; }

        public double? DirectCost { get; set; }

        public double? CommRate { get; set; }

        public double? CommAmt { get; set; }

        public double? OHRate { get; set; }

        public double? OHAmt { get; set; }

        public double? TaxRate { get; set; }

        public double? TaxAmt { get; set; }

        public double? TotalCost { get; set; }

        public double? ProfRate { get; set; }

        public double? ProfAmt { get; set; }

        public double? SalePrice { get; set; }

        public double? CostPP { get; set; }

        public double? SalePP { get; set; }

        public double? USRate { get; set; }
        public double? SaleUS { get; set; }

    }
}