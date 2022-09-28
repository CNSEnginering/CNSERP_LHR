using System;
using System.Drawing;
using System.IO;
using DevExpress.XtraPrinting.Drawing;
using DevExpress.XtraReports.UI;
using ERP.Web.DXServices.Common;

namespace ERP.Web.DXServices.Reports.Inventory
{
    public partial class ConsumptionSummaryDepartmentWise
    {
        private int _tenantId;
        public string DecimalPoints = "";
        public ConsumptionSummaryDepartmentWise()
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

        private void ConsumptionReportDepartmentWise_BeforePrint_1(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DevExpress.XtraReports.Parameters.Parameter param =
          ((XtraReport)sender).
           Parameters["TenantId"];
            _tenantId = Convert.ToInt32(param.Value);
            DecimalPoints = ((XtraReport)sender).Parameters["InventoryPoint"].Value.ToString();

            this.label12.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label11.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label10.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label9.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label8.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label38.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label37.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label36.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label35.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label33.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label32.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label31.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label30.TextFormatString = "{0:N" + DecimalPoints + "}";

        }
    }
}
