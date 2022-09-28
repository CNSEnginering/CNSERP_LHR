using System;
using DevExpress.XtraReports.UI;
using ERP.Web.DXServices.Common;
using ERP.Web.DXServices.DataHandler;

namespace ERP.Web.DXServices.Reports.Finance
{
    public partial class ReceivingSlip
    {
        private int _insType;
        private int _tenantId;
        private int _docNo;
        private double? _amount;
        private DateTime _docDate;
        private string _bookId;
        public ReceivingSlip()
        {
            InitializeComponent();
        }

        private void label39_SummaryCalculated(object sender, TextFormatEventArgs e)
        {

        }

        private void TopMargin_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }

        private void label9_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }

        private void label9_BeforePrint_1(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if ((sender as XRLabel).Value.ToString().ToLower().StartsWith('b'))
            {
                label17.Visible = true;
                label38.Visible = true;
                label13.Visible = true;
                label15.Visible = true;
            }
            else
            {
                label17.Visible = false;
                label38.Visible = false;
                label13.Visible = false;
                label15.Visible = false;
            }
        }

        private void label39_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }

        private void label39_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {

        }

        private void label39_SummaryCalculated_1(object sender, TextFormatEventArgs e)
        {
            if (e.Value != null)
            {
                if (e.Value.ToString() != "")
                {
                    var splitNum = e.Value.ToString().Split(".");
                    e.Text = " ";
                    if (splitNum.Length > 1)
                    {
                        e.Text += ReportUtils.NumberToWords(Convert.ToInt32(splitNum[0]));
                        var decimalPart = Math.Round(Convert.ToDecimal(splitNum[1]), 2);
                        e.Text = "Rupees " + e.Text;
                        e.Text += " " + ReportUtils.NumberToWords(Convert.ToInt32(decimalPart)) + " paisas only";
                    }
                    else
                    {
                        e.Text += ReportUtils.NumberToWords(Convert.ToInt32(splitNum[0]));
                        e.Text = "Rupees " + e.Text + " and zero paisas only";
                        e.Text = e.Text.ToUpper();
                    }
                }
            }
        }

        private void label15_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            var value = _insType;
            // var ds = (dynamic)GetCurrentRow();
            if (value != null)
            {
                if (!string.IsNullOrEmpty(value.ToString()))
                {
                    if (Convert.ToInt32(value) == 1)
                    {
                        label15.Text = "Cheque-Cash";
                    }
                    else if (Convert.ToInt32(value) == 2)
                    {
                        label15.Text = "Cheque-Cross";
                    }
                    else if (Convert.ToInt32(value) == 3)
                    {
                        label15.Text = "PO";
                    }
                    else if (Convert.ToInt32(value) == 4)
                    {
                        label15.Text = "Online";
                    }
                    else if (Convert.ToInt32(value) == 5)
                    {
                        label15.Text = "Other";
                    }
                }
            }
        }

        private void ReceivingSlip_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DevExpress.XtraReports.Parameters.Parameter param =
              ((XtraReport)sender).
               Parameters["insType"];
            if (param.Value.ToString() != "")
                _insType = Convert.ToInt32(param.Value);

            DevExpress.XtraReports.Parameters.Parameter param1 =
             ((XtraReport)sender).
              Parameters["TenantId"];
            if (param1.Value.ToString() != "")
                _tenantId = Convert.ToInt32(param1.Value);

            DevExpress.XtraReports.Parameters.Parameter param2 =
          ((XtraReport)sender).
           Parameters["DocNo"];
            if (param2.Value.ToString() != "")
                _docNo = Convert.ToInt32(param2.Value);

            DevExpress.XtraReports.Parameters.Parameter param3 =
        ((XtraReport)sender).
         Parameters["BookId"];
            if (param3.Value.ToString() != "")
                _bookId = param3.Value.ToString();

            DevExpress.XtraReports.Parameters.Parameter param4 =
        ((XtraReport)sender).
         Parameters["DocDate"];
            if (param4.Value.ToString() != "")
                _docDate = Convert.ToDateTime(param4.Value);

            _amount = ReportDataHandlerBase.GetDebitSum(_tenantId, _docNo, _bookId, _docDate.Month);

        }

        private void label18_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (_amount != null)
            {
                if (_amount.ToString() != "")
                {
                    var splitNum = _amount.ToString().Split(".");
                    ((XRLabel)sender).Text = " ";
                    if (splitNum.Length > 1)
                    {
                        ((XRLabel)sender).Text += ReportUtils.NumberToWords(Convert.ToInt32(splitNum[0]));
                        var decimalPart = Math.Round(Convert.ToDecimal(splitNum[1]), 2);
                        ((XRLabel)sender).Text = "Rupees " + ((XRLabel)sender).Text;
                        ((XRLabel)sender).Text += " " + ReportUtils.NumberToWords(Convert.ToInt32(decimalPart)) + " paisas only";
                    }
                    else
                    {
                        ((XRLabel)sender).Text += ReportUtils.NumberToWords(Convert.ToInt32(splitNum[0]));
                        ((XRLabel)sender).Text = "Rupees " + ((XRLabel)sender).Text + " and zero paisas only";
                        ((XRLabel)sender).Text = ((XRLabel)sender).Text.ToUpper();
                    }
                }
            }
            // ((XRLabel)sender).Text = _amount.ToString();
        }

        private void label19_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            ((XRLabel)sender).Text = _amount.Value.ToString("N2");
        }
    }
}
