//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ERP.Web.DXServices.Reports.PayRoll {
    
    public partial class EmployeeType : DevExpress.XtraReports.UI.XtraReport {
        private void InitializeComponent() {
            DevExpress.XtraReports.ReportInitializer reportInitializer = new DevExpress.XtraReports.ReportInitializer(this, "ERP.Web.DXServices.Reports.PayRoll.EmployeeType.repx");

            // Controls
            this.TopMargin = reportInitializer.GetControl<DevExpress.XtraReports.UI.TopMarginBand>("TopMargin");
            this.BottomMargin = reportInitializer.GetControl<DevExpress.XtraReports.UI.BottomMarginBand>("BottomMargin");
            this.ReportHeader = reportInitializer.GetControl<DevExpress.XtraReports.UI.ReportHeaderBand>("ReportHeader");
            this.GroupHeader1 = reportInitializer.GetControl<DevExpress.XtraReports.UI.GroupHeaderBand>("GroupHeader1");
            this.Detail = reportInitializer.GetControl<DevExpress.XtraReports.UI.DetailBand>("Detail");
            this.label1 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRLabel>("label1");
            this.pictureBox1 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRPictureBox>("pictureBox1");
            this.label49 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRLabel>("label49");
            this.label13 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRLabel>("label13");
            this.label11 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRLabel>("label11");
            this.pageInfo1 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRPageInfo>("pageInfo1");
            this.pageInfo2 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRPageInfo>("pageInfo2");
            this.table1 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTable>("table1");
            this.tableRow1 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableRow>("tableRow1");
            this.tableCell1 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell1");
            this.tableCell2 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell2");
            this.table2 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTable>("table2");
            this.tableRow2 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableRow>("tableRow2");
            this.tableCell3 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell3");
            this.tableCell4 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRTableCell>("tableCell4");

            // Parameters
            this.Address = reportInitializer.GetParameter("Address");
            this.CompanyName = reportInitializer.GetParameter("CompanyName");
            this.Phone = reportInitializer.GetParameter("Phone");
            this.TenantId = reportInitializer.GetParameter("TenantId");
            this.Address2 = reportInitializer.GetParameter("Address2");

            // Data Sources
            this.jsonDataSource1 = reportInitializer.GetDataSource<DevExpress.DataAccess.Json.JsonDataSource>("jsonDataSource1");

            // Styles
            this.Title = reportInitializer.GetStyle("Title");
            this.DetailCaption1 = reportInitializer.GetStyle("DetailCaption1");
            this.DetailData1 = reportInitializer.GetStyle("DetailData1");
            this.DetailData3_Odd = reportInitializer.GetStyle("DetailData3_Odd");
            this.PageInfo = reportInitializer.GetStyle("PageInfo");
        }
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.XRLabel label1;
        private DevExpress.XtraReports.UI.XRPictureBox pictureBox1;
        private DevExpress.XtraReports.UI.XRLabel label49;
        private DevExpress.XtraReports.UI.XRLabel label13;
        private DevExpress.XtraReports.UI.XRLabel label11;
        private DevExpress.XtraReports.UI.XRPageInfo pageInfo1;
        private DevExpress.XtraReports.UI.XRPageInfo pageInfo2;
        private DevExpress.XtraReports.UI.XRTable table1;
        private DevExpress.XtraReports.UI.XRTableRow tableRow1;
        private DevExpress.XtraReports.UI.XRTableCell tableCell1;
        private DevExpress.XtraReports.UI.XRTableCell tableCell2;
        private DevExpress.XtraReports.UI.XRTable table2;
        private DevExpress.XtraReports.UI.XRTableRow tableRow2;
        private DevExpress.XtraReports.UI.XRTableCell tableCell3;
        private DevExpress.XtraReports.UI.XRTableCell tableCell4;
        private DevExpress.DataAccess.Json.JsonDataSource jsonDataSource1;
        private DevExpress.XtraReports.UI.XRControlStyle Title;
        private DevExpress.XtraReports.UI.XRControlStyle DetailCaption1;
        private DevExpress.XtraReports.UI.XRControlStyle DetailData1;
        private DevExpress.XtraReports.UI.XRControlStyle DetailData3_Odd;
        private DevExpress.XtraReports.UI.XRControlStyle PageInfo;
        private DevExpress.XtraReports.Parameters.Parameter Address;
        private DevExpress.XtraReports.Parameters.Parameter CompanyName;
        private DevExpress.XtraReports.Parameters.Parameter Phone;
        private DevExpress.XtraReports.Parameters.Parameter TenantId;
        private DevExpress.XtraReports.Parameters.Parameter Address2;
    }
}
