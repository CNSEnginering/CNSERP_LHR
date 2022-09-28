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
    public partial class PLStatementNew
    {
        private double _sales;
        private double _costOfSales;
        private double _adminExpenses;
        private double _sellingExpenses;
        private double _grossProfit;
        private double _financialCost;
        private double _otherIncome;
        private double _taxationExp;
        private double _operatingProfit;
        private double _profitBeforeTax;
        private double _profitForTheYear;
        private double _ProfitBF;
        private double _ProfitCF;

        private double _prevSales;
        private double _prevCostOfSales;
        private double _prevAdminExpenses;
        private double _prevSellingExpenses;
        private double _prevGrossProfit;
        private double _prevFinancialCost;
        private double _prevOtherIncome;
        private double _prevTaxationExp;
        private double _prevOperatingProfit;
        private double _prevProfitBeforeTax;
        private double _prevProfitForTheYear;
        private double _prevProfitBF;
        private double _prevProfitCF;
        private bool _imageLogic;
        private int _tenantId;
        public string DecimalPoints = "";
        public PLStatementNew()
        {
            InitializeComponent();
        }

        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            var data = (dynamic)GetCurrentRow();
            if (data != null)
            {
                if (data.GLPLCtGId == 2)
                {
                    _sales = data.Amount;
                    _prevSales = data.PrevAmount;
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
                    label5.Visible = true;
                    label6.Visible = true;
                    label7.Visible = true;
                }
                else if (data.GLPLCtGId == 8)
                {
                    _costOfSales = data.Amount;
                    _prevCostOfSales = data.PrevAmount;
                    label7.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label6.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                    label5.Visible = true;
                    label6.Visible = true;
                    label7.Visible = true;
                }
                else if (data.GLPLCtGId == 27)
                {
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
                    label7.Borders = DevExpress.XtraPrinting.BorderSide.Top;
                    label6.Borders = DevExpress.XtraPrinting.BorderSide.Top;
                    label5.Visible = true;
                    label6.Visible = true;
                    label7.Visible = true;
                }
                else if (data.GLPLCtGId == 28)
                {
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
                    label7.Borders = DevExpress.XtraPrinting.BorderSide.Top;
                    label6.Borders = DevExpress.XtraPrinting.BorderSide.Top;
                    label5.Visible = true;
                    label6.Visible = true;
                    label7.Visible = true;
                }
                else if (data.GLPLCtGId == 34)
                {
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
                    label7.Borders = DevExpress.XtraPrinting.BorderSide.Top;
                    label6.Borders = DevExpress.XtraPrinting.BorderSide.Top;
                    label5.Visible = true;
                    label6.Visible = true;
                    label7.Visible = true;
                }
                else if (data.GLPLCtGId == 36)
                {
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
                    label7.Borders = DevExpress.XtraPrinting.BorderSide.Top;
                    label6.Borders = DevExpress.XtraPrinting.BorderSide.Top;
                    label7.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
                    label6.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
                    label5.Visible = true;
                    label6.Visible = true;
                    label7.Visible = true;
                }
                else if (data.GLPLCtGId == 29)
                {
                    _adminExpenses = data.Amount;
                    _prevAdminExpenses = data.PrevAmount;
                    label7.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label6.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                    label5.Visible = true;
                    label6.Visible = true;
                    label7.Visible = true;
                }
                else if (data.GLPLCtGId == 30)
                {
                    _sellingExpenses = data.Amount;
                    _prevSellingExpenses = data.PrevAmount;
                    label7.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label6.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                    label5.Visible = true;
                    label6.Visible = true;
                    label7.Visible = true;
                }
                else if (data.GLPLCtGId == 31)
                {
                    label7.Borders = DevExpress.XtraPrinting.BorderSide.Top;
                    label6.Borders = DevExpress.XtraPrinting.BorderSide.Top;
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
                    label5.Visible = true;
                    label6.Visible = true;
                    label7.Visible = true;
                }
                else if (data.GLPLCtGId == 32)
                {
                    _financialCost = data.Amount;
                    _prevFinancialCost = data.PrevAmount;
                    label7.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label6.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                    label5.Visible = true;
                    label6.Visible = true;
                    label7.Visible = true;
                }
                else if (data.GLPLCtGId == 33)
                {
                    _otherIncome = data.Amount;
                    _prevOtherIncome = data.PrevAmount;
                    label7.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label6.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                    label5.Visible = true;
                    label6.Visible = true;
                    label7.Visible = true;
                }
                else if (data.GLPLCtGId == 35)
                {
                    _taxationExp = data.Amount;
                    _prevTaxationExp = data.PrevAmount;
                    label7.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label7.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
                    label6.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label6.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                    label5.Visible = true;
                    label6.Visible = true;
                    label7.Visible = true;
                }
                else if (data.GLPLCtGId == 37)
                {
                    _ProfitBF = data.Amount;
                    _prevProfitBF = data.PrevAmount;
                    label7.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
                    label6.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                    label5.Visible = false;
                    label6.Visible = false;
                    label7.Visible = false;
                }
                else if (data.GLPLCtGId == 38)
                {
                    label7.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
                    label6.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
                    label5.Visible = false;
                    label6.Visible = false;
                    label7.Visible = false;
                }
                else
                {
                    label5.Visible = true;
                    label6.Visible = true;
                    label7.Visible = true;
                }
            }
        }

        private void label7_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            var data = (dynamic)GetCurrentRow();
            if (data != null)
            {
                if (data.GLPLCtGId == 27)
                {
                    _grossProfit = _sales - _costOfSales;
                    ((XRLabel)sender).Text = _grossProfit.ToString();
                }
                else if (data.GLPLCtGId == 31)
                {
                    _operatingProfit = _grossProfit - _adminExpenses - _sellingExpenses;
                    ((XRLabel)sender).Text = String.Format("{0:n2}", _operatingProfit);
                }
                else if (data.GLPLCtGId == 34)
                {
                    _profitBeforeTax = _operatingProfit - _financialCost + _otherIncome;
                    ((XRLabel)sender).Text = String.Format("{0:n2}", _profitBeforeTax);
                    SetPLAmount(_profitBeforeTax, _prevProfitBeforeTax);
                }
                else if (data.GLPLCtGId == 36)
                {
                    _profitForTheYear = _profitBeforeTax - _taxationExp;
                    ((XRLabel)sender).Text = String.Format("{0:n2}", _profitForTheYear) ;
                }
                else if (data.GLPLCtGId == 38)
                {
                    _profitForTheYear = _profitForTheYear - _ProfitBF;
                    ((XRLabel)sender).Text = String.Format("{0:n2}", _profitForTheYear) ;

                }


                //if (Math.Sign(Convert.ToDecimal(((XRLabel)sender).Text)) == -1)
                //{
                //    ((XRLabel)sender).Text = "( " + String.Format("{0:n0}", Math.Abs(Convert.ToDecimal(((XRLabel)sender).Text))) + " )";

                //}
            }
        }
        public void SetPLAmount(double? CurAmt, double? PrevAmt)
        {
            try
            {
                SqlCommand cmd, cmd1;
                string str = ConfigurationManager.AppSettings["ConnectionString"];
                using (SqlConnection cn = new SqlConnection(str))
                {


                    cmd1 = new SqlCommand("delete from tblSetPL where TenantId='" + _tenantId + "' ", cn);
                    cmd = new SqlCommand("insert into tblSetPL(TenantId,CurrentPL,PrevPL) values('" + _tenantId + "','" + CurAmt + "','" + PrevAmt + "')", cn);
                    cmd.CommandType = CommandType.Text;
                    cn.Open();
                    cmd1.ExecuteNonQuery();
                    cmd.ExecuteNonQuery();
                    cn.Close();
                }
            }
            catch (Exception ex)
            {


            }
        }
        private void label6_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            var data = (dynamic)GetCurrentRow();
            if (data != null)
            {
                if (data.GLPLCtGId == 27)
                {
                    _prevGrossProfit = _prevSales - _prevCostOfSales;
                    ((XRLabel)sender).Text = _prevGrossProfit.ToString();
                }
                else if (data.GLPLCtGId == 31)
                {
                    _prevOperatingProfit = _prevGrossProfit - (_prevAdminExpenses - _prevSellingExpenses);
                    ((XRLabel)sender).Text = _prevOperatingProfit.ToString();
                }
                else if (data.GLPLCtGId == 34)
                {
                    _prevProfitBeforeTax = _prevOperatingProfit - _prevFinancialCost - _prevOtherIncome;
                    ((XRLabel)sender).Text = _prevProfitBeforeTax.ToString();
                }
                else if (data.GLPLCtGId == 36)
                {
                    _prevProfitForTheYear = _prevProfitBeforeTax - _taxationExp;
                    ((XRLabel)sender).Text = _prevProfitForTheYear.ToString();
                }
                else if (data.GLPLCtGId == 38)
                {
                    _prevProfitForTheYear = _prevProfitForTheYear - _prevProfitBF;
                    ((XRLabel)sender).Text = _prevProfitForTheYear.ToString();

                }


                //if (Math.Sign(Convert.ToDecimal(((XRLabel)sender).Text)) == -1)
                //{
                //    ((XRLabel)sender).Text = "( " + String.Format("{0:n0}", Math.Abs(Convert.ToDecimal(((XRLabel)sender).Text))) + " )";

                //}
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

        private void PLStatementNew_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DevExpress.XtraReports.Parameters.Parameter param =
             ((XtraReport)sender).
              Parameters["Tenant"];
            _tenantId = Convert.ToInt32(param.Value);

            DecimalPoints = ((XtraReport)sender).Parameters["FinancePoint"].Value.ToString();

            this.label7.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label6.TextFormatString = "{0:N" + DecimalPoints + "}";
        }



    }
}
