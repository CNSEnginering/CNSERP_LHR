using System;
using System.Drawing;
using System.IO;
using DevExpress.XtraPrinting.Drawing;
using DevExpress.XtraReports.UI;
using ERP.Web.DXServices.Common;

namespace ERP.Web.DXServices.Reports.PayRoll
{
    public partial class EmployeeLeaves
    {
        private int _tenantId;
        private bool _imageLogic;
        public EmployeeLeaves()
        {
            InitializeComponent();
            _imageLogic = false;
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

        private void Employees_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DevExpress.XtraReports.Parameters.Parameter param =
                    ((XtraReport)sender).
                     Parameters["TenantId"];
            _tenantId = Convert.ToInt32(param.Value);
        }


    }
}
