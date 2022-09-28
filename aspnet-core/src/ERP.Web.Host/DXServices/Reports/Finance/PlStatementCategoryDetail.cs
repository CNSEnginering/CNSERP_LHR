using System;
using System.Drawing;
using System.IO;
using DevExpress.XtraPrinting.Drawing;
using DevExpress.XtraReports.UI;
using ERP.Web.DXServices.Common;

namespace ERP.Web.DXServices.Reports.Finance
{
    public partial class PlStatementCategoryDetail
    {
        private bool _imageLogic;
        private int _tenantId;
        public PlStatementCategoryDetail()
        {
            InitializeComponent();
        }

        private void pictureBox1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (_imageLogic == false)
            {
                byte[] bytes = ReportUtils.GetImage(_tenantId);
                MemoryStream mem = new MemoryStream(bytes);
                Bitmap bmp = new Bitmap(mem);
                Image img = bmp;
                XRPictureBox pictureBox = (XRPictureBox)sender;
                pictureBox.ImageSource = new ImageSource(img);
                _imageLogic = true;
            }
        }

        private void PlStatementCategoryDetail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DevExpress.XtraReports.Parameters.Parameter param =
                  ((XtraReport)sender).
                   Parameters["TenantId"];
            _tenantId = Convert.ToInt32(param.Value);
        }
    }
}
