using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.SupplyChain.Inventory.Dtos
{
    public class ItemListingDto
    {
        public string Seg1 { get; set; }
        public string Seg1Desc { get; set; }
        public string Seg2 { get; set; }
        public string Seg2Desc { get; set; }
        public string Seg3 { get; set; }
        public string Seg3Desc { get; set; }
        public string ItemId { get; set; }
        public string Descp { get; set; }
        public string Unit { get; set; }
        public string Tax { get; set; }
        public int? ItemType { get; set; }

        public string CreationDate { get; set; }
    }
    public class AgeingInvoiceListingDto
    {

        public string ACCOUNTID { get; set; }
        public string ACCOUNTNAME { get; set; }
        public int SUBACCID { get; set; }
        public string SUBACCNAME { get; set; }
        public DateTime INVOICE_DATE { get; set; }

        public int InvoiceNo { get; set; }
        public int BAL { get; set; }
        public int AMOUNT { get; set; }
        public int A1 { get; set; }
        public int A2 { get; set; }

        public int A3 { get; set; }
        public int A4 { get; set; }

        public int A5 { get; set; }
        public string BookName { get; set; }

    }

    public class DeleteLogDto
    {
        public int DocNo { get; set; }
        public string Action { get; set; }
        public string DeleteBy { get; set; }
        public DateTime DeleteDate { get; set; }
        public string FormName { get; set; }
    }

    public class FormDto
    {
        public int FormId { get; set; }
        public string FormName { get; set; }
    }
}
