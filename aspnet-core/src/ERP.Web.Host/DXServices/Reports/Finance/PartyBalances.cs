using System;
using System.Drawing;
using System.IO;
using DevExpress.XtraPrinting.Drawing;
using DevExpress.XtraReports.UI;
using ERP.Web.DXServices.Common;

namespace ERP.Web.DXServices.Reports.Finance
{
    public partial class PartyBalances
    {
        private int _tenantId;
        public string DecimalPoints = "";
        public PartyBalances()
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

        private void TrialBalance_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DevExpress.XtraReports.Parameters.Parameter param =
                 ((XtraReport)sender).
                  Parameters["TenantId"];
            _tenantId = Convert.ToInt32(param.Value);

            DecimalPoints = ((XtraReport)sender).Parameters["FinancePoint"].Value.ToString();

            this.label53.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label54.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label55.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label56.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label57.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label58.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label59.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label10.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label11.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label60.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label61.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label62.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label63.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label64.TextFormatString = "{0:N" + DecimalPoints + "}";
        }
    }
}
