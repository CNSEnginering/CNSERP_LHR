using System;
using System.Drawing;
using System.IO;
using DevExpress.XtraPrinting.Drawing;
using DevExpress.XtraReports.UI;
using ERP.Web.DXServices.Common;
using ERP.Web.DXServices.DataHandler;

namespace ERP.Web.DXServices.Reports.Finance
{
    public partial class Subledger
    {
        private int _tenantId;
        private double _balance;
        private long _accSubCode;
        private string _accCode;
        public string DecimalPoints = "";
        public Subledger()
        {
            InitializeComponent();
        }

        private void SubledgerReport_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DevExpress.XtraReports.Parameters.Parameter param =
                     ((XtraReport)sender).
                      Parameters["TenantId"];
            _tenantId = Convert.ToInt32(param.Value);

            //DevExpress.XtraReports.Parameters.Parameter param1 =
            //             ((XtraReport)sender).
            //              Parameters["summary"];
            //Detail.Visible = !(bool)param1.Value;
            DecimalPoints = ((XtraReport)sender).Parameters["FinancePoint"].Value.ToString();

            this.lineTotal.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.tableCell22.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.tableCell26.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.tableCell27.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.tableCell29.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.tableCell20.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.tableCell23.TextFormatString = "{0:N" + DecimalPoints + "}";
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

        private void subreport1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRSubreport subreport = sender as XRSubreport;
            var data = (dynamic)GetCurrentRow();
            if (data != null)
            {
                var newData = ReportDataHandlerBase.GetPDCSubReport(Convert.ToInt32(data.SubledgerCode), Convert.ToInt32(_tenantId), data.AccountCode);
                if (newData.Count > 0)
                {
                    newData[0].Balance = newData[0].Type == "AR" ? ((_balance) - newData[0].Balance) : ((_balance) + newData[0].Balance);
                    for (var i = 1; i < newData.Count; i++)
                    {
                        newData[i].Balance = newData[0].Type == "AR" ? ((newData[i - 1].Balance) - Math.Abs(newData[i].ChequeAmount)) : ((newData[i - 1].Balance) + Math.Abs(newData[i].ChequeAmount));
                    }
                    (subreport1.ReportSource as XtraReport).DataSource = newData;
                    subreport1.Visible = true;
                }
                else
                    subreport1.Visible = false;
            }else
            {
                subreport1.Visible = false;
            }
        }

        private void tableCell23_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            
        }

        private void tableCell23_BeforePrint_1(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            var data = (dynamic)GetCurrentRow();
            if(data != null)
            {
                if (data.AccountCode == "01-001-01-0001")
                {

                }
                if (_accCode == data.AccountCode && _accSubCode == data.SubledgerCode)
                {
                    _balance += data.Debit - Math.Abs(data.Credit);
                }
                else
                {
                    _accCode = data.AccountCode;
                    _accSubCode = data.SubledgerCode;
                    _balance = 0;
                    _balance += data.Opening + (data.Debit - Math.Abs(data.Credit));
                }
            }
        }
    }
}
