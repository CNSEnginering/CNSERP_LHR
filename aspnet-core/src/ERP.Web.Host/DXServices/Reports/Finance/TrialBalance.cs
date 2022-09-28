using System;
using System.Drawing;
using System.IO;
using DevExpress.XtraPrinting.Drawing;
using DevExpress.XtraReports.UI;
using ERP.Web.DXServices.Common;

namespace ERP.Web.DXServices.Reports.Finance
{
    public partial class TrialBalance
    {
        private int _tenantId;
        public TrialBalance()
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

        private void TrialBalance_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DevExpress.XtraReports.Parameters.Parameter param =
                    ((XtraReport)sender).
                     Parameters["TenantId"];
            _tenantId = Convert.ToInt32(param.Value);
            DevExpress.XtraReports.Parameters.Parameter includeLevel3 =
                   ((XtraReport)sender).
                    Parameters["includeLevel3"];
            GroupHeader3.Visible = Convert.ToBoolean(includeLevel3.Value);
        }
    }
}
