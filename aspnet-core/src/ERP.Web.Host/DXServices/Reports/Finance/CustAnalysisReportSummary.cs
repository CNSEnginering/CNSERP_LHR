using System;
using System.Drawing;
using System.IO;
using DevExpress.XtraPrinting.Drawing;
using DevExpress.XtraReports.UI;
using ERP.Web.DXServices.Common;

namespace ERP.Web.DXServices.Reports.Finance
{
    public partial class CustAnalysisReportSummary
    {
        private int _tenantId;
        private bool imageLogic;
        public string DecimalPoints = "";
        public CustAnalysisReportSummary()
        {
            InitializeComponent();
        }

        private void pictureBox1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (imageLogic == false)
            {
                byte[] bytes = ReportUtils.GetImage(_tenantId);
                MemoryStream mem = new MemoryStream(bytes);
                Bitmap bmp = new Bitmap(mem);
                Image img = bmp;
                XRPictureBox pictureBox = (XRPictureBox)sender;
                pictureBox.ImageSource = new ImageSource(img);
                imageLogic = true;
            }
        }

        private void CustAnalysisReport_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

            DevExpress.XtraReports.Parameters.Parameter param =
                ((XtraReport)sender).
                 Parameters["TenantId"];
            _tenantId = Convert.ToInt32(param.Value);

            DecimalPoints = ((XtraReport)sender).Parameters["FinancePoint"].Value.ToString();

            this.label13.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label3.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label10.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label11.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label22.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label23.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label26.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label27.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label15.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label121.TextFormatString = "{0:N" + DecimalPoints + "}";
        }

        private void label13_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }
    }
}
