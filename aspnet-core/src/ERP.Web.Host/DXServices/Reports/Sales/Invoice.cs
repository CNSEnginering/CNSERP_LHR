using System;
using System.Drawing;
using System.IO;
using DevExpress.XtraPrinting.Drawing;
using DevExpress.XtraReports.UI;
using ERP.Web.DXServices.Common;

namespace ERP.Web.DXServices.Reports.Sales
{
    public partial class Invoice
    {
        string total;
        private int _tenantId;
        public Invoice()
        {
            InitializeComponent();
        }

        private void InvoiceTotal_AfterPrint(object sender, EventArgs e)
        {
           
        }

        private void InvoiceTotal_SummaryCalculated(object sender, TextFormatEventArgs e)
        {
            total = ReportUtils.NumberToWords(Convert.ToInt32(e.Value));
        }

        private void TotalInWords_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            ((XRLabel)sender).Text = total;
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

        private void InwardGatePass_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DevExpress.XtraReports.Parameters.Parameter param =
              ((XtraReport)sender).
               Parameters["TenantId"];
            _tenantId = Convert.ToInt32(param.Value);

        }
    }
}
