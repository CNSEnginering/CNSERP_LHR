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
    
    public partial class StockTransfer : DevExpress.XtraReports.UI.XtraReport {
        private void InitializeComponent() {
            DevExpress.XtraReports.ReportInitializer reportInitializer = new DevExpress.XtraReports.ReportInitializer(this, "ERP.Web.DXServices.Reports.Inventory.StockTransfer.repx");

            // Controls
            this.TopMargin = reportInitializer.GetControl<DevExpress.XtraReports.UI.TopMarginBand>("TopMargin");
            this.BottomMargin = reportInitializer.GetControl<DevExpress.XtraReports.UI.BottomMarginBand>("BottomMargin");
            this.Detail = reportInitializer.GetControl<DevExpress.XtraReports.UI.DetailBand>("Detail");
            this.GroupHeader1 = reportInitializer.GetControl<DevExpress.XtraReports.UI.GroupHeaderBand>("GroupHeader1");
            this.label49 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRLabel>("label49");
            this.label1 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRLabel>("label1");
            this.pictureBox1 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRPictureBox>("pictureBox1");
            this.label18 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRLabel>("label18");
            this.line5 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRLine>("line5");
            this.line4 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRLine>("line4");
            this.line3 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRLine>("line3");
            this.line2 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRLine>("line2");
            this.line1 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRLine>("line1");
            this.label23 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRLabel>("label23");
            this.label22 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRLabel>("label22");
            this.label21 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRLabel>("label21");
            this.label20 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRLabel>("label20");
            this.label14 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRLabel>("label14");
            this.label12 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRLabel>("label12");
            this.label13 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRLabel>("label13");
            this.label11 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRLabel>("label11");
            this.label19 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRLabel>("label19");
            this.label17 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRLabel>("label17");
            this.label16 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRLabel>("label16");
            this.label15 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRLabel>("label15");
            this.label10 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRLabel>("label10");
            this.label9 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRLabel>("label9");
            this.label8 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRLabel>("label8");
            this.label7 = reportInitializer.GetControl<DevExpress.XtraReports.UI.XRLabel>("label7");

            // Parameters
            this.FromDoc = reportInitializer.GetParameter("FromDoc");
            this.ToDoc = reportInitializer.GetParameter("ToDoc");
            this.CompanyName = reportInitializer.GetParameter("CompanyName");
            this.Address = reportInitializer.GetParameter("Address");
            this.Address2 = reportInitializer.GetParameter("Address2");
            this.Phone = reportInitializer.GetParameter("Phone");
            this.TenantId = reportInitializer.GetParameter("TenantId");
            this.InventoryPoint = reportInitializer.GetParameter("InventoryPoint");

            // Data Sources
            this.jsonDataSource1 = reportInitializer.GetDataSource<DevExpress.DataAccess.Json.JsonDataSource>("jsonDataSource1");
        }
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
        private DevExpress.XtraReports.UI.XRLabel label49;
        private DevExpress.XtraReports.UI.XRLabel label1;
        private DevExpress.XtraReports.UI.XRPictureBox pictureBox1;
        private DevExpress.XtraReports.UI.XRLabel label18;
        private DevExpress.XtraReports.UI.XRLine line5;
        private DevExpress.XtraReports.UI.XRLine line4;
        private DevExpress.XtraReports.UI.XRLine line3;
        private DevExpress.XtraReports.UI.XRLine line2;
        private DevExpress.XtraReports.UI.XRLine line1;
        private DevExpress.XtraReports.UI.XRLabel label23;
        private DevExpress.XtraReports.UI.XRLabel label22;
        private DevExpress.XtraReports.UI.XRLabel label21;
        private DevExpress.XtraReports.UI.XRLabel label20;
        private DevExpress.XtraReports.UI.XRLabel label14;
        private DevExpress.XtraReports.UI.XRLabel label12;
        private DevExpress.XtraReports.UI.XRLabel label13;
        private DevExpress.XtraReports.UI.XRLabel label11;
        private DevExpress.XtraReports.UI.XRLabel label19;
        private DevExpress.XtraReports.UI.XRLabel label17;
        private DevExpress.XtraReports.UI.XRLabel label16;
        private DevExpress.XtraReports.UI.XRLabel label15;
        private DevExpress.XtraReports.UI.XRLabel label10;
        private DevExpress.XtraReports.UI.XRLabel label9;
        private DevExpress.XtraReports.UI.XRLabel label8;
        private DevExpress.XtraReports.UI.XRLabel label7;
        private DevExpress.DataAccess.Json.JsonDataSource jsonDataSource1;
        private DevExpress.XtraReports.Parameters.Parameter FromDoc;
        private DevExpress.XtraReports.Parameters.Parameter ToDoc;
        private DevExpress.XtraReports.Parameters.Parameter CompanyName;
        private DevExpress.XtraReports.Parameters.Parameter Address;
        private DevExpress.XtraReports.Parameters.Parameter Address2;
        private DevExpress.XtraReports.Parameters.Parameter Phone;
        private DevExpress.XtraReports.Parameters.Parameter TenantId;
        private DevExpress.XtraReports.Parameters.Parameter InventoryPoint;
    }
}