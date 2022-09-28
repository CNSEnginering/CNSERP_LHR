using System;
using System.Drawing;
using System.IO;
using DevExpress.XtraPrinting.Drawing;
using DevExpress.XtraReports.UI;
using ERP.Web.DXServices.Common;

namespace ERP.Web.DXServices.Reports.Finance
{
    public partial class RptMonthlyConsolidated
    {
        public RptMonthlyConsolidated()
        {
            InitializeComponent();
        }
        private int _tenantId;
        public string DecimalPoints = "";
        private void pictureBox1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            byte[] bytes = ReportUtils.GetImage(_tenantId);
            MemoryStream mem = new MemoryStream(bytes);
            Bitmap bmp = new Bitmap(mem);
            Image img = bmp;
            XRPictureBox pictureBox = (XRPictureBox)sender;
            pictureBox.ImageSource = new ImageSource(img);
        }

        private void RptMonthlyConsolidated_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DevExpress.XtraReports.Parameters.Parameter param =
                    ((XtraReport)sender).
                     Parameters["TenantId"];
            _tenantId = Convert.ToInt32(param.Value);
            DecimalPoints = ((XtraReport)sender).Parameters["FinancePoint"].Value.ToString();

            this.label17.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label18.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label19.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label20.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label21.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label22.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label23.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label24.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label25.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label26.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label27.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label28.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label43.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label31.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label32.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label33.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label34.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label35.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label36.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label37.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label38.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label39.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label40.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label41.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label42.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label44.TextFormatString = "{0:N" + DecimalPoints + "}";

        }
    }
}
