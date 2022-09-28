using System;
using System.Drawing;
using System.IO;
using DevExpress.XtraPrinting.Drawing;
using DevExpress.XtraReports.UI;
using ERP.Web.DXServices.Common;

namespace ERP.Web.DXServices.Reports.Inventory
{
    public partial class ConsumptionReport
    {
        private int _tenantId;
        public string DecimalPoints = "";
        public ConsumptionReport()
        {
            InitializeComponent();
        }

        private void pictureBox1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            byte[] bytes = ReportUtils.GetImage(_tenantId);
            MemoryStream mem = new MemoryStream(bytes);
            Bitmap bmp = new Bitmap(mem);
            Image img = bmp;
            XRPictureBox pictureBox = (XRPictureBox)sender;
            pictureBox.ImageSource = new ImageSource(img);
        }

        private void AdjustmentReport_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DevExpress.XtraReports.Parameters.Parameter param =
             ((XtraReport)sender).
              Parameters["TenantId"];
            _tenantId = Convert.ToInt32(param.Value);
            DecimalPoints = ((XtraReport)sender).Parameters["InventoryPoint"].Value.ToString();

            this.lineTotal.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.tableCell13.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.unitPrice.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.quantity.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.tableCell12.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.tableCell15.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.totalCaption.TextFormatString = "{0:N" + DecimalPoints + "}";

        }
    }
}
