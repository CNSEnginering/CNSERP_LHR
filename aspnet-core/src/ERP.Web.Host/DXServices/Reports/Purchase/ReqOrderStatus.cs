using System;
using System.Drawing;
using System.IO;
using DevExpress.XtraPrinting.Drawing;
using DevExpress.XtraReports.UI;
using ERP.Web.DXServices.Common;

namespace ERP.Web.DXServices.Reports.Purchase
{
    public partial class ReqOrderStatus
    {
        private int _tenantId;
        public string DecimalPoints = "";
        public ReqOrderStatus()
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

        private void ReqOrderStatus_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DevExpress.XtraReports.Parameters.Parameter param =
             ((XtraReport)sender).
              Parameters["TenantId"];
            _tenantId = Convert.ToInt32(param.Value);
            DecimalPoints = ((XtraReport)sender).Parameters["InventoryPoint"].Value.ToString();

            this.label16.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label13.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label9.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label6.TextFormatString = "{0:N" + DecimalPoints + "}";

        }
    }
}
