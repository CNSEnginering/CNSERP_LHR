using System;
using DevExpress.XtraReports.UI;
using System.Drawing;
using System.IO;
using DevExpress.XtraPrinting.Drawing;
using ERP.Web.DXServices.Common;

namespace ERP.Web.DXServices.Reports.GeneralLedger
{
    public partial class GLLevel1
    {
        private int _tenantId;

        public GLLevel1()
        {
            InitializeComponent();
        }
        private void GLLevel1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DevExpress.XtraReports.Parameters.Parameter param =
                      ((XtraReport)sender).
                       Parameters["TenantId"];
            _tenantId = Convert.ToInt32(param.Value);
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
    }
}
