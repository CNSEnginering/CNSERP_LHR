//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ERP.Web.DXServices.Reports.Inventory {
    
    public partial class ConsumptionActivityReportCostWise : DevExpress.XtraReports.UI.XtraReport {
        private void InitializeComponent() {
            DevExpress.XtraReports.ReportInitializer reportInitializer = new DevExpress.XtraReports.ReportInitializer(this, "ERP.Web.DXServices.Reports.Inventory.ConsumptionActivityReportCostWise.repx");

            // Controls
            this.Detail = reportInitializer.GetControl<DevExpress.XtraReports.UI.DetailBand>("Detail");
            this.TopMargin = reportInitializer.GetControl<DevExpress.XtraReports.UI.TopMarginBand>("TopMargin");
            this.BottomMargin = reportInitializer.GetControl<DevExpress.XtraReports.UI.BottomMarginBand>("BottomMargin");
            this.GroupHeader2 = reportInitializer.GetControl<DevExpress.XtraReports.UI.GroupHeaderBand>("GroupHeader2");
            this.GroupHeader1 = reportInitializer.GetControl<DevExpress.XtraReports.UI.GroupHeaderBand>("GroupHeader1");
            this.GroupFooter1 = reportInitializer.GetControl<DevExpress.XtraReports.UI.GroupFooterBand>("GroupFooter1");
            this.GroupHeader3 = reportInitializer.GetControl<DevExpress.XtraReports.UI.GroupHeaderBand>("GroupHeader3");
            this.detailTable = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTable>("detailTable");
            this.detailTableRow = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableRow>("detailTableRow");
            this.productName = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("productName");
            this.tableCell11 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell11");
            this.tableCell12 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell12");
            this.quantity = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("quantity");
            this.tableCell14 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell14");
            this.unitPrice = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("unitPrice");
            this.tableCell13 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell13");
            this.lineTotal = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("lineTotal");
            this.label4 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRLabel>("label4");
            this.customerTable = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTable>("customerTable");
            this.invoiceLabel = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRLabel>("invoiceLabel");
            this.label3 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRLabel>("label3");
            this.label1 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRLabel>("label1");
            this.pictureBox1 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRPictureBox>("pictureBox1");
            this.customerNameRow = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableRow>("customerNameRow");
            this.customerName = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("customerName");
            this.line1 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRLine>("line1");
            this.label2 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRLabel>("label2");
            this.pageInfo2 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRPageInfo>("pageInfo2");
            this.pageInfo1 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRPageInfo>("pageInfo1");
            this.headerTable = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTable>("headerTable");
            this.headerTableRow = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableRow>("headerTableRow");
            this.productNameCaption = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("productNameCaption");
            this.tableCell7 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell7");
            this.tableCell8 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell8");
            this.tableCell9 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell9");
            this.quantityCaption = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("quantityCaption");
            this.unitPriceCaption = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("unitPriceCaption");
            this.lineTotalCaptionCell = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("lineTotalCaptionCell");
            this.tableCell10 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell10");
            this.table1 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTable>("table1");
            this.tableRow2 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableRow>("tableRow2");
            this.tableCell1 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell1");
            this.tableCell2 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell2");
            this.tableCell4 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell4");
            this.tableCell3 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell3");
            this.label6 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRLabel>("label6");

            // Parameters
            this.TenantId = reportInitializer.GetParameter("TenantId");
            this.CompanyName = reportInitializer.GetParameter("CompanyName");
            this.Address = reportInitializer.GetParameter("Address");
            this.Address2 = reportInitializer.GetParameter("Address2");
            this.Phone = reportInitializer.GetParameter("Phone");
            this.InventoryPoint = reportInitializer.GetParameter("InventoryPoint");

            // Data Sources
            this.ConsumptionDataSource = reportInitializer.GetDataSource<DevExpress.DataAccess.Json.JsonDataSource>("ConsumptionDataSource");

            // Styles
            this.baseControlStyle = reportInitializer.GetStyle("baseControlStyle");
        }
        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader2;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
        private DevExpress.XtraReports.UI.GroupFooterBand GroupFooter1;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader3;
        private DevExpress.XtraReports.UI.XRTable detailTable;
        private DevExpress.XtraReports.UI.XRTableRow detailTableRow;
        private DevExpress.XtraReports.UI.XRTableCell productName;
        private DevExpress.XtraReports.UI.XRTableCell tableCell11;
        private DevExpress.XtraReports.UI.XRTableCell tableCell12;
        private DevExpress.XtraReports.UI.XRTableCell quantity;
        private DevExpress.XtraReports.UI.XRTableCell tableCell14;
        private DevExpress.XtraReports.UI.XRTableCell unitPrice;
        private DevExpress.XtraReports.UI.XRTableCell tableCell13;
        private DevExpress.XtraReports.UI.XRTableCell lineTotal;
        private DevExpress.XtraReports.UI.XRLabel label4;
        private DevExpress.XtraReports.UI.XRTable customerTable;
        private DevExpress.XtraReports.UI.XRLabel invoiceLabel;
        private DevExpress.XtraReports.UI.XRLabel label3;
        private DevExpress.XtraReports.UI.XRLabel label1;
        private DevExpress.XtraReports.UI.XRPictureBox pictureBox1;
        private DevExpress.XtraReports.UI.XRTableRow customerNameRow;
        private DevExpress.XtraReports.UI.XRTableCell customerName;
        private DevExpress.XtraReports.UI.XRLine line1;
        private DevExpress.XtraReports.UI.XRLabel label2;
        private DevExpress.XtraReports.UI.XRPageInfo pageInfo2;
        private DevExpress.XtraReports.UI.XRPageInfo pageInfo1;
        private DevExpress.XtraReports.UI.XRTable headerTable;
        private DevExpress.XtraReports.UI.XRTableRow headerTableRow;
        private DevExpress.XtraReports.UI.XRTableCell productNameCaption;
        private DevExpress.XtraReports.UI.XRTableCell tableCell7;
        private DevExpress.XtraReports.UI.XRTableCell tableCell8;
        private DevExpress.XtraReports.UI.XRTableCell tableCell9;
        private DevExpress.XtraReports.UI.XRTableCell quantityCaption;
        private DevExpress.XtraReports.UI.XRTableCell unitPriceCaption;
        private DevExpress.XtraReports.UI.XRTableCell lineTotalCaptionCell;
        private DevExpress.XtraReports.UI.XRTableCell tableCell10;
        private DevExpress.XtraReports.UI.XRTable table1;
        private DevExpress.XtraReports.UI.XRTableRow tableRow2;
        private DevExpress.XtraReports.UI.XRTableCell tableCell1;
        private DevExpress.XtraReports.UI.XRTableCell tableCell2;
        private DevExpress.XtraReports.UI.XRTableCell tableCell4;
        private DevExpress.XtraReports.UI.XRTableCell tableCell3;
        private DevExpress.XtraReports.UI.XRLabel label6;
        private DevExpress.DataAccess.Json.JsonDataSource ConsumptionDataSource;
        private DevExpress.XtraReports.UI.XRControlStyle baseControlStyle;
        private DevExpress.XtraReports.Parameters.Parameter TenantId;
        private DevExpress.XtraReports.Parameters.Parameter CompanyName;
        private DevExpress.XtraReports.Parameters.Parameter Address;
        private DevExpress.XtraReports.Parameters.Parameter Address2;
        private DevExpress.XtraReports.Parameters.Parameter Phone;
        private DevExpress.XtraReports.Parameters.Parameter InventoryPoint;
    }
}