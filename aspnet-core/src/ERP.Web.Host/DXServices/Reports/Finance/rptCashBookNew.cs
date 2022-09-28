using System;
using System.Data;
using System.Drawing;
using System.IO;
using DevExpress.Compatibility.System.Windows.Forms;
using DevExpress.XtraPrinting.Drawing;
using DevExpress.XtraReports.UI;
using ERP.Web.DXServices.Common;

namespace ERP.Web.DXServices.Reports.Finance
{
    public partial class rptCashBookNew
    {

        private int _tenantId;
        public string DecimalPoints = "";
        public rptCashBookNew()
        {
            InitializeComponent();
        }

        private void CashBookReport_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DevExpress.XtraReports.Parameters.Parameter param =
                     ((XtraReport)sender).
                      Parameters["TenantId"];
            _tenantId = Convert.ToInt32(param.Value);

            DecimalPoints = ((XtraReport)sender).Parameters["FinancePoint"].Value.ToString();

            this.lineTotal.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.tableCell22.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.tableCell26.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.tableCell27.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.tableCell29.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.tableCell30.TextFormatString = "{0:N" + DecimalPoints + "}";
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

        private void tableCell24_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            ((XRLabel)sender).Tag = GetCurrentRow();
        }

        private void tableCell24_PreviewClick(object sender, PreviewMouseEventArgs e)
        {
            var id = (int)((DataRowView)e.Brick.Value).Row["DocNo"];
        }

        private void tableCell24_PreviewMouseMove(object sender, PreviewMouseEventArgs e)
        {
            //Cursor.Current = Cursors.Hand;
        }

        private void tableCell24_HtmlItemCreated(object sender, HtmlEventArgs e)
        {
            e.ContentCell.Style.Add("cursor", "hand");
        }

        private void invoiceLabel_HtmlItemCreated(object sender, HtmlEventArgs e)
        {
            e.ContentCell.Style.Add("cursor", "hand");
        }
    }
}
