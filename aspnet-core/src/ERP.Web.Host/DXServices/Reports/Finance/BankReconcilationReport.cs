using System;
using System.Drawing;
using System.IO;
using DevExpress.XtraPrinting.Drawing;
using DevExpress.XtraReports.UI;
using ERP.Web.DXServices.Common;
using ERP.Web.DXServices.DataHandler;

namespace ERP.Web.DXServices.Reports.Finance
{
    public partial class BankReconcilationReport
    {
        private int _tenantId;
        private string _bankId;
        private string _fromDate;
        private string _toDate;
        public string DecimalPoints = "";
        public BankReconcilationReport()
        {
            InitializeComponent();
        }

        private void BankReconcilationReport_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DevExpress.XtraReports.Parameters.Parameter param2 =
                   ((XtraReport)sender).
                    Parameters["FromDateStr"];
            _fromDate = param2.Value.ToString();

            DevExpress.XtraReports.Parameters.Parameter param3 =
                    ((XtraReport)sender).
                     Parameters["ToDateStr"];
            _toDate = param3.Value.ToString();
            DevExpress.XtraReports.Parameters.Parameter param =
                      ((XtraReport)sender).
                       Parameters["TenantId"];
            _tenantId = Convert.ToInt32(param.Value);

            DevExpress.XtraReports.Parameters.Parameter param1 =
                     ((XtraReport)sender).
                      Parameters["BankId"];
            _bankId = param1.Value.ToString();

            DecimalPoints = ((XtraReport)sender).Parameters["FinancePoint"].Value.ToString();

            this.label14.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label15.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label19.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label4.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label27.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label26.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label5.TextFormatString = "{0:N" + DecimalPoints + "}";


        }
        private void pictureBox1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }

        private void pictureBox2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
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
            var newData = ReportDataHandlerBase.GetBankReconcilationDetailReport(_tenantId, _bankId, Convert.ToDateTime(_fromDate),
                Convert.ToDateTime(_toDate));
            (subreport1.ReportSource as XtraReport).DataSource = newData;
        }
    }
}
