using System;
using System.Drawing;
using System.IO;
using DevExpress.XtraPrinting.Drawing;
using DevExpress.XtraReports.UI;
using ERP.Web.DXServices.Common;

namespace ERP.Web.DXServices.Reports.Purchase
{
    public partial class PurchaseOrderStatus
    {
        private int _tenantId;
        public string DecimalPoints = "";
        public PurchaseOrderStatus()
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

            this.label7.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label20.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label19.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label17.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label6.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label3.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label2.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label1.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label14.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label13.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label12.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label9.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label8.TextFormatString = "{0:N" + DecimalPoints + "}";

        }
        
    }
}
