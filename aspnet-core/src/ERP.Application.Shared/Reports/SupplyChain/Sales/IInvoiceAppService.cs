using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.SupplyChain.Sales
{
     public interface IInvoiceAppService : IApplicationService
    {
        List<InvoiceReport> GetData(int? tenantId,
           int fromDoc, int toDoc, DateTime fromDate, DateTime toDate);
        List<QuotationReport> GetQutationData(int? tenantId, int fromDoc, int toDoc);
        List<CostSheetReport> GetCostsheetData(int? tenantId, int fromDoc, int toDoc);
    }

    public class InvoiceReport
    {
        public int DocNo { get; set; }
        public string DocDate { get; set; }
        public string OGPNo { get; set; }
        public string DriverName { get; set; }
        public string VehicleNo { get; set; }
        public string CustomerName { get; set; }
        public string Narration { get; set; }
        public string ItemId { get; set; }
        public string Descp { get; set; }
        public string Unit { get; set; }
        public double? Qty { get; set; }
        public double? Rate { get; set; }
        public double? Disc { get; set; }
        public double? Amount { get; set; }
        public double? ExlTaxAmount { get; set; }
        public double? AdvIncTax { get; set; }
    
        public double? DeliveryCharges { get; set; }
        public double? SalesMargin { get; set; }
        public double? NetAmount { get; set; }
        public double? Tax { get; set; }
        public double? ValueExlSalesTax { get; set; }
        public double? SalesTaxAmt { get; set; }
        public double? AmtInclSalesTax { get; set; }
        public string TypeName { get; set; }
        public int? CustId { get; set; }
    }

    public class QuotationReport
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public int LocID { get; set; }
        public string LocName { get; set; }
        public int DocNo { get; set; }
        public DateTime DocDate { get; set; }
        public string MDocNo { get; set; }
        public DateTime MDocDate { get; set; }
        public string TypeID { get; set; }
        public string SalesCtrlAcc { get; set; }
        public int CustID { get; set; }
        public string CustomerName { get; set; }
        public string Narration { get; set; }
        public int TransType { get; set; }
        public string transDESC { get; set; }
        public string ItemID { get; set; }
        public string ITEM_NAME { get; set; }
        public string UNIT { get; set; }
        public string Conver { get; set; }
        public double? Qty { get; set; }
        public double? Rate { get; set; }
        public double? Disc { get; set; }
        public double? Amount { get; set; }
        public string TaxAuth { get; set; }
        public string CLASSDESC { get; set; }
        public string CLASSTYPE { get; set; }
        public string TaxClass { get; set; }
        public double? TaxRate { get; set; }
        public double? TaxAmt { get; set; }
        public double? NetAmount { get; set; }
        public string Remarks { get; set; }
        public string NoteText { get; set; }
        public string PayTerms { get; set; }
        public string DelvTerms { get; set; }
        public string ValidityTerms { get; set; }
        public string CreatedBy { get; set; }


        public string TaxAuth1 { get; set; }
        public string TaxClassDesc1 { get; set; }
        public string TaxClass1 { get; set; }
        public string TaxRate1 { get; set; }
        public string TaxAmt1 { get; set; }

        public string TaxAuth2 { get; set; }
        public string TaxClassDesc2 { get; set; }
        public string TaxClass2 { get; set; }
        public string TaxRate2 { get; set; }
        public string TaxAmt2 { get; set; }

        public string TaxAuth3 { get; set; }
        public string TaxClass3 { get; set; }
        public string TaxClassDesc3 { get; set; }
        public string TaxRate3 { get; set; }
        public string TaxAmt3 { get; set; }

        public string TaxAuth4 { get; set; }
        public string TaxClass4 { get; set; }
        public string TaxClassDesc4 { get; set; }
        public string TaxRate4 { get; set; }
        public string TaxAmt4 { get; set; }

        public string TaxAuth5 { get; set; }
        public string TaxClass5 { get; set; }
        public string TaxClassDesc5 { get; set; }
        public string TaxRate5 { get; set; }
        public string TaxAmt5 { get; set; }
        public double? Netamt { get; set; }
        public double? GrandAmt { get; set; }

    }
    public class CostSheetReport
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public int LocID { get; set; }
        public string LocName { get; set; }
        public int DocNo { get; set; }
        public int SaleDoc { get; set; }
        public DateTime DocDate { get; set; }
        public string MDocNo { get; set; }
        public DateTime MDocDate { get; set; }
        public string TypeID { get; set; }
        public string SalesCtrlAcc { get; set; }
        public int CustID { get; set; }
        public string CustomerName { get; set; }
        public string Narration { get; set; }
        public int TransType { get; set; }
        public string transDESC { get; set; }
        public string ItemID { get; set; }
        public string ITEM_NAME { get; set; }
        public string UNIT { get; set; }
        public string Conver { get; set; }
        public double? Qty { get; set; }
        public double? Rate { get; set; }
        public double? Disc { get; set; }
        public double? Amount { get; set; }
        public string TaxAuth { get; set; }
        public string CLASSDESC { get; set; }
        public string CLASSTYPE { get; set; }
        public string TaxClass { get; set; }
        public double? TaxRate { get; set; }
        public double? TaxAmt { get; set; }
        public string Remarks { get; set; }
        public string NoteText { get; set; }
        public string PayTerms { get; set; }
        public string DelvTerms { get; set; }
        public string ValidityTerms { get; set; }
        public string CreatedBy { get; set; }
        public string BasicStyle { get; set; }
        public string License { get; set; }
        public string PartyName { get; set; }
        public string ItemName { get; set; }
        public double? OrderQty { get; set; }
        public double? DirectCost { get; set; }
        public double? CommRate { get; set; }
        public double? CommAmt { get; set; }
        public double? OHRate { get; set; }
        public double? OHAmt { get; set; }
        public double? TaxRateD { get; set; }
        public double? TaxAmtD { get; set; }
        public double? TotalCost { get; set; }
        public double? ProfitRate { get; set; }
        public double? ProfitAmt { get; set; }
        public double? SalePrice { get; set; }
        public double? CostPP { get; set; }
        public double? SalePP { get; set; }
        public double? UsRate { get; set; }
        public double? SaleUS { get; set; }

        public double? Netamt { get; set; }
        public double? GrandAmt { get; set; }

    }
}
