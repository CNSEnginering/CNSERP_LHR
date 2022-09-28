using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using DevExpress.XtraPrinting.Drawing;
using DevExpress.XtraReports.UI;
using ERP.Web.DXServices.Common;

namespace ERP.Web.DXServices.Reports.Finance
{
    public partial class PLStatement
    {
        private int _tenantId;
        public PLStatement()
        {
            InitializeComponent();
        }

        private void vendorLogo_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            byte[] bytes = ReportUtils.GetImage(_tenantId);
            MemoryStream mem = new MemoryStream(bytes);
            Bitmap bmp = new Bitmap(mem);
            Image img = bmp;
            XRPictureBox pictureBox = (XRPictureBox)sender;
            pictureBox.ImageSource = new ImageSource(img);
        }

        private void PLStatement_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DevExpress.XtraReports.Parameters.Parameter param =
                               ((XtraReport)sender).
                                Parameters["TenantId"];
            _tenantId = Convert.ToInt32(param.Value);
        }

        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

            //XtraReportBase report = (sender as XRControl).Report;
            //string first = report.GetCurrentColumnValue<string>("HeaderDescription");

            if (this.GetCurrentColumnValue("HeaderDescription") == null)
            {
                e.Cancel = true;
            }
            else {
                e.Cancel = false;
            }



        }
      
        private void GroupFooter1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

            if (this.GetCurrentColumnValue("AccountType") == null)
            {
                e.Cancel = true;
            }
        }
    }
}
