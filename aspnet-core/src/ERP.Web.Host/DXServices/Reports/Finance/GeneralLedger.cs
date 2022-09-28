using System.Data;
using DevExpress.XtraReports.UI;
using ERP.Web.DXServices.Common;
using System;
using System.IO;
using System.Drawing;
using DevExpress.XtraPrinting.Drawing;
using DevExpress.Compatibility.System.Windows.Forms;

namespace ERP.Web.DXServices.Reports.Finance
{
    public partial class GeneralLedger
    {

        private int _tenantId;
        private bool imageLogic;
        public string DecimalPoints = "";
        public GeneralLedger()
        {
            InitializeComponent();
            imageLogic = false;
        }

        private void GeneralLedgerReport_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DevExpress.XtraReports.Parameters.Parameter param =
                     ((XtraReport)sender).
                      Parameters["TenantId"];
            _tenantId = Convert.ToInt32(param.Value);

            DecimalPoints = ((XtraReport)sender).Parameters["FinancePoint"].Value.ToString();

            this.lineTotal.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.tableCell22.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.tableCell23.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.tableCell26.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.tableCell27.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.tableCell29.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.tableCell30.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.tableCell20.TextFormatString = "{0:N" + DecimalPoints + "}";
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
        
    }
}
