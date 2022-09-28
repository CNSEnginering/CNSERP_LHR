using System;
using DevExpress.XtraReports.UI;
using ERP.Web.DXServices.Common;

namespace ERP.Web.DXServices.Reports.PayRoll
{
    public partial class TaxCertificate
    {
        public TaxCertificate()
        {
            InitializeComponent();
        }

        private void label4_SummaryCalculated(object sender, TextFormatEventArgs e)
        {
            if (e.Value != null)
            {
                if (e.Value.ToString() != "")
                {

                    var splitNum = e.Value.ToString().Split(".");
                    if (splitNum.Length >= 1)
                    {
                        e.Text = ReportUtils.NumberToWords(Convert.ToInt32(splitNum[0].ToString().Replace(",", "")));
                    }

                    e.Text = e.Text + " only";
                    e.Text = e.Text.ToUpper();
                }
            }
        }

        private void label7_SummaryCalculated(object sender, TextFormatEventArgs e)
        {

        }
    }
}
