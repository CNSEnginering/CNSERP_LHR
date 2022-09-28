using System;
using System.Drawing;
using System.IO;
using DevExpress.XtraPrinting.Drawing;
using DevExpress.XtraReports.UI;
using ERP.Web.DXServices.Common;

namespace ERP.Web.DXServices.Reports.Finance
{
    public partial class CashFlowStatement
    {
        private bool _imageLogic;
        private int _tenantId;
        private double _curProfit;
        private double _preProfit;
        private double _curAsset;
        private double _preAsset;
        private double _curliab;
        private double _preliab;
        private double _presum;
        private double _cursum;
        private double _curexp;
        private double _preexp;
        private double _curcap;
        private double _precap;
        public CashFlowStatement()
        {
            InitializeComponent();
        }

        private void PLStatement_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DevExpress.XtraReports.Parameters.Parameter param =
           ((XtraReport)sender).
            Parameters["TenantId"];
            _tenantId = Convert.ToInt32(param.Value);
        }

        private void vendorLogo_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
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

        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            var data = (dynamic)GetCurrentRow();
            if (data != null)
            {
                if (data.GLPLCtGId == 0)
                {
                     
                    label4.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label5.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label3.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
                    label3.Visible = true;
                    label5.Visible = true;
                    label4.Visible = true;
                }
                else if (data.GLPLCtGId == 1)
                {
                   
                    label4.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label5.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label3.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                    label3.Visible = true;
                    label5.Visible = true;
                    label4.Visible = true;
                }
                else if (data.GLPLCtGId == 2)
                {
                  
                    label3.Visible = true;
                    label5.Visible = true;
                    label4.Visible = true;
                }
                else if (data.GLPLCtGId == 7)
                {

                    label3.Visible = true;
                    label5.Visible = true;
                    label4.Visible = true;
                    label3.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
                    label4.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
                    label4.Borders = DevExpress.XtraPrinting.BorderSide.Top;
                    //label4.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
                    label5.Borders = DevExpress.XtraPrinting.BorderSide.Top;
                    //label5.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
                }
                else if (data.GLPLCtGId == 8)
                {
                    label3.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
                    label3.Visible = true;
                    label5.Visible = false;
                    label4.Visible = false;
                }
                else if (data.GLPLCtGId == 9)
                {

                    label4.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label5.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label3.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                    label4.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                    label3.Visible = true;
                    label5.Visible = true;
                    label4.Visible = true;
                }
                else if (data.GLPLCtGId == 13)
                {

                    label3.Visible = true;
                    label5.Visible = true;
                    label4.Visible = true;
                    label3.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
                    label4.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
                    label4.Borders = DevExpress.XtraPrinting.BorderSide.Top;
                    //label4.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
                    label5.Borders = DevExpress.XtraPrinting.BorderSide.Top;
                    //label5.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
                }
                else if (data.GLPLCtGId == 14)
                {
                    label3.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
                    label3.Visible = true;
                    label5.Visible = false;
                    label4.Visible = false;
                }
                else if (data.GLPLCtGId == 15)
                {

                    label4.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label5.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label3.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                    label4.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                    label3.Visible = true;
                    label5.Visible = true;
                    label4.Visible = true;
                }
                else if (data.GLPLCtGId == 16)
                {
                    label3.Visible = true;
                    label5.Visible = true;
                    label4.Visible = true;
                    label3.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
                    label4.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
                    label4.Borders = DevExpress.XtraPrinting.BorderSide.Top;
                    //label4.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
                    label5.Borders = DevExpress.XtraPrinting.BorderSide.Top;
                }
                else if (data.GLPLCtGId == 18)
                {
                    label3.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
                    label3.Visible = true;
                    label5.Visible = false;
                    label4.Visible = false;
                }
                else if (data.GLPLCtGId == 19)
                {

                    label4.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label5.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label3.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                    label4.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                    label3.Visible = true;
                    label5.Visible = true;
                    label4.Visible = true;
                }
                else if (data.GLPLCtGId == 20)
                {
                    label3.Visible = true;
                    label5.Visible = true;
                    label4.Visible = true;
                    label3.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
                    label4.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
                    label4.Borders = DevExpress.XtraPrinting.BorderSide.Top;
                    //label4.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
                    label5.Borders = DevExpress.XtraPrinting.BorderSide.Top;
                }
                else if (data.GLPLCtGId == 21)
                {
                    label3.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
                    label3.Visible = true;
                    label5.Visible = false;
                    label4.Visible = false;
                }
                else if (data.GLPLCtGId == 22)
                {

                    label4.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label5.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label3.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                    label4.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                    label3.Visible = true;
                    label5.Visible = true;
                    label4.Visible = true;
                }
                else if (data.GLPLCtGId == 26)
                {
                    label3.Visible = true;
                    label5.Visible = true;
                    label4.Visible = true;
                    label3.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
                    label4.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
                    label4.Borders = DevExpress.XtraPrinting.BorderSide.Top;
                    //label4.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
                    label5.Borders = DevExpress.XtraPrinting.BorderSide.Top;
                }
                else if (data.GLPLCtGId == 27 || data.GLPLCtGId==28)
                {
                    label3.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
                    label3.Visible = true;
                    label5.Visible = false;
                    label4.Visible = false;
                }
                else if (data.GLPLCtGId == 29)
                {

                    label4.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label5.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label3.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                    label4.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                    label3.Visible = true;
                    label5.Visible = true;
                    label4.Visible = true;
                }
                else if (data.GLPLCtGId == 33)
                {
                    label3.Visible = true;
                    label5.Visible = true;
                    label4.Visible = true;
                    label3.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
                    label4.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
                    label4.Borders = DevExpress.XtraPrinting.BorderSide.Top;
                    //label4.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
                    label5.Borders = DevExpress.XtraPrinting.BorderSide.Top;
                }else if (data.GLPLCtGId==35)
                {
                    label4.Borders = DevExpress.XtraPrinting.BorderSide.Top;
                    label5.Borders = DevExpress.XtraPrinting.BorderSide.Top;
                }
               
               
            }
        }

        private void label4_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            var data = (dynamic)GetCurrentRow();
            if (data != null)
            {
                if (data.GLPLCtGId == 1)
                {
                    label4.Visible = false;
                    label5.Visible = false;
                } else if (data.GLPLCtGId == 0)
                {
                    _curProfit = data.Amount;
                    label4.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
                }
                else if (data.GLPLCtGId == 2)
                {
                    _curProfit = _curProfit + data.Amount;
                    label4.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                    label3.Visible = true;
                    label5.Visible = true;
                    label4.Visible = true;
                }
                else if (data.GLPLCtGId == 3 || data.GLPLCtGId == 4 || data.GLPLCtGId == 5 || data.GLPLCtGId == 6)
                {
                    _curProfit = _curProfit + data.Amount;
                }
                else if (data.GLPLCtGId == 7)
                {
                    ((XRLabel)sender).Text = _curProfit.ToString("N0");
                    label4.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
                    //label4.Borders = DevExpress.XtraPrinting.BorderSide.Top;
                    //label4.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
                    label3.Visible = true;
                    label5.Visible = true;
                    label4.Visible = true;
                }
                //else if (data.GLPLCtGId == 9 || data.GLPLCtGId == 10 || data.GLPLCtGId == 11 || data.GLPLCtGId == 12)
                //{
                //    _curAsset = _curAsset + (-data.Amount);
                //}
                else if (data.GLPLCtGId == 12)
                {
                    _curAsset = _curAsset + (-data.Amount);
                    ((XRLabel)sender).Text = (-data.Amount).ToString("N0");
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                    label3.Visible = true;
                    label5.Visible = true;
                    label4.Visible = true;
                }
                else if (data.GLPLCtGId == 11)
                {
                    _curAsset = _curAsset + (-data.Amount);
                    ((XRLabel)sender).Text = (-data.Amount).ToString("N0");
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                    label3.Visible = true;
                    label5.Visible = true;
                    label4.Visible = true;
                }
                else if (data.GLPLCtGId == 10)
                {
                    _curAsset = _curAsset + (-data.Amount);
                    ((XRLabel)sender).Text = (-data.Amount).ToString("N0");
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                    label3.Visible = true;
                    label5.Visible = true;
                    label4.Visible = true;
                }
                else if (data.GLPLCtGId == 9)
                {
                    _curAsset = _curAsset + (-data.Amount);
                    ((XRLabel)sender).Text = (-data.Amount).ToString("N0");
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                    label3.Visible = true;
                    label5.Visible = true;
                    label4.Visible = true;
                }
                else if (data.GLPLCtGId == 13)
                {
                    ((XRLabel)sender).Text = _curAsset.ToString("N0");

                    label3.Visible = true;
                    label5.Visible = true;
                    label4.Visible = true;
                }
                else if (data.GLPLCtGId == 15)
                {
                    _curliab = (-data.Amount);
                    ((XRLabel)sender).Text = _curliab.ToString("N0");
                }
                else if (data.GLPLCtGId == 16)
                {
                    ((XRLabel)sender).Text = _curliab.ToString("N0");

                }
                else if (data.GLPLCtGId == 17)
                {
                    _cursum = _curProfit + _curAsset + _curliab;
                    ((XRLabel)sender).Text = _cursum.ToString("N0");

                }
                else if (data.GLPLCtGId == 19)
                {
                    _cursum = _cursum + data.Amount;

                }
                else if (data.GLPLCtGId == 20)
                {
                    ((XRLabel)sender).Text = _cursum.ToString("N0");

                }
                else if (data.GLPLCtGId==25)
                {
                    _curexp = _curexp + (-data.Amount);
                    ((XRLabel)sender).Text = (-data.Amount).ToString("N0");
                }else if (data.GLPLCtGId==24)
                {
                    _curexp = _curexp + (-data.Amount);
                    ((XRLabel)sender).Text = (-data.Amount).ToString("N0");
                }else if (data.GLPLCtGId==22)
                {
                    _curexp = _curexp + (-data.Amount);
                    ((XRLabel)sender).Text = (-data.Amount).ToString("N0");
                } else if (data.GLPLCtGId==23)
                {
                    _curexp = _curexp + (-data.Amount);
                    ((XRLabel)sender).Text = (-data.Amount).ToString("N0");
                }
                else if (data.GLPLCtGId == 26)
                {
                    ((XRLabel)sender).Text = _curexp.ToString("N0");

                }
                //else if (data.GLPLCtGId == 29 || data.GLPLCtGId == 30 || data.GLPLCtGId == 31 || data.GLPLCtGId == 32)
                //{
                //    _curcap = _curcap + data.Amount;
                //}
                else if (data.GLPLCtGId == 29)
                {
                    _curcap = _curcap + (-data.Amount);
                    ((XRLabel)sender).Text = (-data.Amount).ToString("N0");
                }
                else if (data.GLPLCtGId == 30)
                {
                    _curcap = _curcap + (-data.Amount);
                    ((XRLabel)sender).Text = (-data.Amount).ToString("N0");
                }
                else if (data.GLPLCtGId == 31)
                {
                    _curcap = _curcap + (-data.Amount);
                    ((XRLabel)sender).Text = (-data.Amount).ToString("N0");
                }
                else if (data.GLPLCtGId == 32)
                {
                    _curcap = _curcap + (-data.Amount);
                    ((XRLabel)sender).Text = (-data.Amount).ToString("N0");
                }
                else if (data.GLPLCtGId == 33)
                {
                    ((XRLabel)sender).Text = _curcap.ToString("N0");

                }
                else if (data.GLPLCtGId == 34)
                {
                    _cursum = _cursum + _curexp + _curcap;
                    ((XRLabel)sender).Text = _cursum.ToString("N0");

                }
                else if (data.GLPLCtGId == 35)
                {
                    
                    //((XRLabel)sender).Text = _cursum.ToString("N0");
                    _cursum = _cursum + data.Amount;
                }
                else if (data.GLPLCtGId == 36)
                {
                    ((XRLabel)sender).Text = _cursum.ToString("N0");

                }
                else if (data.GLPLCtGId == 37)
                {
                    _cursum = _cursum - data.Amount;

                }
                else if (data.GLPLCtGId == 38)
                {
                    ((XRLabel)sender).Text = _cursum.ToString("N0");

                }
            }
        }

        private void label5_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            var data = (dynamic)GetCurrentRow();
            if (data != null)
            {
                if (data.GLPLCtGId == 1)
                {
                    //_costOfSales = data.Amount;
                    //_prevCostOfSales = data.PrevAmount;
                    
                    label4.Visible = false;
                    label5.Visible = false;
                }
                else if (data.GLPLCtGId == 0)
                {
                    _preProfit = data.PrevAmount;
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
                }
                else if (data.GLPLCtGId == 2)
                {
                    _preProfit = _preProfit + data.PrevAmount;
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                    label3.Visible = true;
                    label5.Visible = true;
                    label4.Visible = true;
                }
                else if (data.GLPLCtGId == 3 || data.GLPLCtGId == 2 || data.GLPLCtGId == 4 || data.GLPLCtGId == 5 || data.GLPLCtGId == 6)
                {
                    _preProfit = _preProfit + data.PrevAmount;
                }
                else if (data.GLPLCtGId == 7)
                {
                    ((XRLabel)sender).Text = _preProfit.ToString("N0");
                    
                    label3.Visible = true;
                    label5.Visible = true;
                    label4.Visible = true;
                }
                //else if (data.GLPLCtGId == 12)
                //{
                //    _preAsset = _preAsset + (data.PrevAmount);
                //}
                else if (data.GLPLCtGId == 12)
                {
                    _preAsset = _preAsset + (-data.PrevAmount);
                    ((XRLabel)sender).Text = (-data.PrevAmount).ToString("N0");
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                    label3.Visible = true;
                    label5.Visible = true;
                    label4.Visible = true;
                } else if (data.GLPLCtGId == 11)
                {
                    _preAsset = _preAsset + (-data.PrevAmount);
                    ((XRLabel)sender).Text = (-data.PrevAmount).ToString("N0");
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                    label3.Visible = true;
                    label5.Visible = true;
                    label4.Visible = true;
                } else if (data.GLPLCtGId == 10)
                {
                    _preAsset = _preAsset + (-data.PrevAmount);
                    ((XRLabel)sender).Text = (-data.PrevAmount).ToString("N0");
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                    label3.Visible = true;
                    label5.Visible = true;
                    label4.Visible = true;
                } else if (data.GLPLCtGId == 9)
                {
                    _preAsset = _preAsset + (-data.PrevAmount);
                    ((XRLabel)sender).Text = (-data.PrevAmount).ToString("N0");
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                    label3.Visible = true;
                    label5.Visible = true;
                    label4.Visible = true;
                }
                else if (data.GLPLCtGId == 13)
                {
                    ((XRLabel)sender).Text = _preAsset.ToString("N0");

                    label3.Visible = true;
                    label5.Visible = true;
                    label4.Visible = true;
                }
                else if (data.GLPLCtGId == 15)
                {
                    _preliab = (-data.PrevAmount);
                    ((XRLabel)sender).Text = _preliab.ToString("N0");
                }
                else if (data.GLPLCtGId == 16)
                {
                    ((XRLabel)sender).Text = _preliab.ToString("N0");

                }
                else if (data.GLPLCtGId == 17)
                {
                    _presum = _preProfit + _preAsset + _preliab;
                    ((XRLabel)sender).Text = _presum.ToString("N0");

                }
                else if (data.GLPLCtGId == 19)
                {
                    _presum = _presum + data.PrevAmount;

                }
                else if (data.GLPLCtGId == 20)
                {
                    ((XRLabel)sender).Text = _presum.ToString("N0");

                }
                //else if (data.GLPLCtGId == 22 || data.GLPLCtGId == 23 || data.GLPLCtGId == 24 || data.GLPLCtGId == 25)
                //{
                //    _preexp = _preexp + data.PrevAmount;
                //}
                else if (data.GLPLCtGId == 25)
                {
                    _preexp = _preexp + (-data.PrevAmount);
                    ((XRLabel)sender).Text = (-data.PrevAmount).ToString("N0");
                }
                else if (data.GLPLCtGId == 24)
                {
                    _preexp = _preexp + (-data.PrevAmount);
                    ((XRLabel)sender).Text = (-data.PrevAmount).ToString("N0");
                }
                else if (data.GLPLCtGId == 22)
                {
                    _preexp = _preexp + (-data.PrevAmount);
                    ((XRLabel)sender).Text = (-data.PrevAmount).ToString("N0");
                }
                else if (data.GLPLCtGId == 23)
                {
                    _preexp = _preexp + (-data.PrevAmount);
                    ((XRLabel)sender).Text = (-data.PrevAmount).ToString("N0");
                }
                else if (data.GLPLCtGId == 26)
                {
                    ((XRLabel)sender).Text = _preexp.ToString("N0");

                }
                //else if (data.GLPLCtGId == 29 || data.GLPLCtGId == 30 || data.GLPLCtGId == 31 || data.GLPLCtGId == 32)
                //{
                //    _precap = _precap + data.PrevAmount;
                //}
                else if (data.GLPLCtGId == 29)
                {
                    _precap = _precap + (-data.PrevAmount);
                    ((XRLabel)sender).Text = (-data.PrevAmount).ToString("N0");
                }
                else if (data.GLPLCtGId == 30)
                {
                    _precap = _precap + (-data.PrevAmount);
                    ((XRLabel)sender).Text = (-data.PrevAmount).ToString("N0");
                }
                else if (data.GLPLCtGId == 31)
                {
                    _precap = _precap + (-data.PrevAmount);
                    ((XRLabel)sender).Text = (-data.PrevAmount).ToString("N0");
                }
                else if (data.GLPLCtGId == 32)
                {
                    _precap = _precap + (-data.PrevAmount);
                    ((XRLabel)sender).Text = (-data.PrevAmount).ToString("N0");
                }
                else if (data.GLPLCtGId == 33)
                {
                    ((XRLabel)sender).Text = _precap.ToString("N0");

                }
                else if (data.GLPLCtGId == 34)
                {
                    _presum = _presum + _preexp + _precap;
                    ((XRLabel)sender).Text = _presum.ToString("N0");

                }
                else if (data.GLPLCtGId == 35)
                {
                    _presum = _presum + data.PrevAmount;
                    //((XRLabel)sender).Text = _presum.ToString("N0");

                }
                else if (data.GLPLCtGId == 36)
                {
                    ((XRLabel)sender).Text = _presum.ToString("N0");

                }
                else if (data.GLPLCtGId == 37)
                {
                    _presum = _presum - data.PrevAmount;

                }
                else if (data.GLPLCtGId == 38)
                {
                    ((XRLabel)sender).Text = _presum.ToString("N0");

                }
            }
        }
    }
}
