using System;
using System.Drawing;
using System.IO;
using DevExpress.XtraPrinting.Drawing;
using DevExpress.XtraReports.UI;
using ERP.Web.DXServices.Common;

namespace ERP.Web.DXServices.Reports.PayRoll
{
    public partial class AllowanceBank
    {
        bool _imageLogic;
        int _tenantId;
        public AllowanceBank()
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
                        e.Text = ReportUtils.NumberToWords(Convert.ToInt32(splitNum[0].ToString().Replace(",","")));
                    }

                    e.Text = e.Text + " only";
                    e.Text = e.Text.ToUpper();
                }
            }
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

        private void Allowance_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DevExpress.XtraReports.Parameters.Parameter param =
                   ((XtraReport)sender).
                    Parameters["TenantId"];
            _tenantId = Convert.ToInt32(param.Value);
        }
    }
}
