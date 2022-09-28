using System;
using System.Drawing;
using System.IO;
using DevExpress.XtraPrinting.Drawing;
using DevExpress.XtraReports.UI;
using ERP.Web.DXServices.Common;

namespace ERP.Web.DXServices.Reports.Purchase
{
    public partial class ReceiptWithoutRates
    {
        private int _tenantId;
        public string DecimalPoints = "";
        public ReceiptWithoutRates()
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

        private void InwardGatePass_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DevExpress.XtraReports.Parameters.Parameter param =
           ((XtraReport)sender).
            Parameters["TenantId"];
            _tenantId = Convert.ToInt32(param.Value);
            DecimalPoints = ((XtraReport)sender).Parameters["InventoryPoint"].Value.ToString();

            
            this.label8.TextFormatString = "{0:N" + DecimalPoints + "}";
            
            this.label24.TextFormatString = "{0:N" + DecimalPoints + "}";
            

        }
        private void label57_SummaryCalculated(object sender, TextFormatEventArgs e)
        {
            if (e.Value != null)
            {
                if (e.Value.ToString() != "")
                {
                    var splitNum = e.Value.ToString().Split(".");
                    if (splitNum.Length > 1)
                    {
                        e.Text = ReportUtils.NumberToWords(Convert.ToInt32(splitNum[0]));
                        var decimalPart = Math.Round(Convert.ToDecimal(splitNum[1]), 2);
                        e.Text = "Rupees " + e.Text;
                        e.Text += " " + ReportUtils.NumberToWords(Convert.ToInt32(decimalPart)) + " paisas only";
                    }
                    else
                    {
                        e.Text = ReportUtils.NumberToWords(Convert.ToInt32(splitNum[0]));
                        e.Text = "Rupees " + e.Text + " and zero paisas only";
                    }
                    e.Text = e.Text.ToUpper();
                }
            }
        }

        private void label51_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }

        private void label57_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }

        private void label38_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }

        private void label37_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }

        private void label36_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }

        private void label32_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }

        private void label10_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }

        private void label12_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }

        private void label18_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }

        private void label34_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }
    }
}
