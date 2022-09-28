using System;
using System.Drawing;
using System.IO;
using DevExpress.XtraPrinting.Drawing;
using DevExpress.XtraReports.UI;
using ERP.Web.DXServices.Common;

namespace ERP.Web.DXServices.Reports
{
    public partial class rptSubLedgerTrail
    {

        private int _tenantId;
        public string DecimalPoints = "";
        public rptSubLedgerTrail()
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

        private void rptSubLedgerTrail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DevExpress.XtraReports.Parameters.Parameter param =
                                 ((XtraReport)sender).
                                  Parameters["TenantId"];
            _tenantId = Convert.ToInt32(param.Value);
            DecimalPoints = ((XtraReport)sender).Parameters["FinancePoint"].Value.ToString();

            this.tableCell10.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.tableCell27.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.tableCell28.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.tableCell11.TextFormatString = "{0:N" + DecimalPoints + "}";
        }
    }
}
