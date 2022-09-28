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
    public partial class BalanceSheet
    {
        public BalanceSheet()
        {
            InitializeComponent();
        }

        private double _propertyPlantAndEquipment;
        private double _intangibleAssets;
        private double _capitalWorkInProgress;
        private double _longTermAdvances;
        private double _longTermDeposits;
        private double _longTermInv;
        private double _storesSparesLooseTools;
        private double _stockInTrade;
        private double _tradeDebitors;
        private double _loanAndAdvances;
        private double _shortTermDepositsAndPrePayments;
        private double _dueFromAssociateUnderTaking;
        private double _otherReceivable;
        private double _cashAndBankBalances;
        private double _totalAssets;
        private double _shareCapital;
        private double _unAppropriateProfitLoss;
        private double _profitForTheYear;
        private double _longTermFinancingSecured;
        private double _directorsCurrentAccount;
        private double _dueToAssociateUndertaking;
        private double _defferedTexation;
        private double _liabilitiesAgainstAssetsSubjectsToFinanceLease;
        private double _tradeAndOtherPayables;
        private double _currentPortionOfLongTermLiabilities;
        private double _markUp;
        private double _shortTermRunningFinance;


        private double _prevPropertyPlantAndEquipment;
        private double _prevIntangibleAssets;
        private double _prevCapitalWorkInProgress;
        private double _prevLongTermAdvances;
        private double _prevLongTermDeposits;
        private double _prevLongTermInv;
        private double _prevStoresSparesLooseTools;
        private double _prevStockInTrade;
        private double _prevTradeDebitors;
        private double _prevLoanAndAdvances;
        private double _prevShortTermDepositsAndPrePayments;
        private double _prevDueFromAssociateUnderTaking;
        private double _prevOtherReceivable;
        private double _prevCashAndBankBalances;
        private double _prevTotalAssets;

        private double _a, _prevA;
        private double _b, _prevB;
        private double _c, _prevC;
        private double _d, _prevD;
        private double _e, _prevE;
        private double _f, _prevF;
        private double _g, _prevG;
        private double _h, _prevH;
        private double _i, _prevI;

        private void label10_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            var data = (dynamic)GetCurrentRow();
            if (data != null)
            {
                if (data.GLPLCtGId == 4 || data.GLPLCtGId == 5 || data.GLPLCtGId == 6 || data.GLPLCtGId == 19 || data.GLPLCtGId == 41 || data.GLPLCtGId == 42)
                {
                    ((XRLabel)sender).Visible = false;
                }
                else
                {
                    ((XRLabel)sender).Visible = true;
                }

            }
        }

        private void label5_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            var data = (dynamic)GetCurrentRow();
            if (data != null)
            {
                if (data.GLPLCtGId == 17 || data.GLPLCtGId == 15 || data.GLPLCtGId == 16 || data.GLPLCtGId == 40 || data.GLPLCtGId == 42)
                {
                    ((XRLabel)sender).Visible = false;
                }
                else
                {
                    ((XRLabel)sender).Visible = true;
                }

            }
        }

        private bool _imageLogic;
        private int _tenantId;
        private DateTime _fromDate;
        private DateTime _toDate;

        public string DecimalPoints = "";

        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            var data = (dynamic)GetCurrentRow();
            if (data != null)
            {
                if (data.GLPLCtGId == 1)
                {
                    _propertyPlantAndEquipment = data.Amount;
                    _prevPropertyPlantAndEquipment = data.PrevAmount;
                    label7.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label6.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                }
                if (data.GLPLCtGId == 4)
                {
                    _longTermAdvances = data.Amount;
                    _prevLongTermAdvances = data.PrevAmount;
                    label7.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label6.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);

                }
                if (data.GLPLCtGId == 5)
                {
                    _longTermDeposits = data.Amount;
                    _prevLongTermDeposits = data.PrevAmount;
                    label7.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label6.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);

                }
                if (data.GLPLCtGId == 6)
                {
                    _longTermInv = data.Amount;
                    _prevLongTermInv = data.PrevAmount;
                    label7.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
                    label6.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);

                }
                else if (data.GLPLCtGId == 2)
                {
                    _intangibleAssets = data.Amount;
                    _prevIntangibleAssets = data.PrevAmount;
                    label7.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label6.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);

                }
                else if (data.GLPLCtGId == 3)
                {
                    _capitalWorkInProgress = data.Amount;
                    _prevCapitalWorkInProgress = data.PrevAmount;
                    label7.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label6.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                }
                //else if (data.GLPLCtGId == 19)
                //{
                //    _longTermAdvances = data.Amount;
                //    _prevLongTermAdvances = data.PrevAmount;
                //    label7.Borders = DevExpress.XtraPrinting.BorderSide.None;
                //    label7.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
                //    label6.Borders = DevExpress.XtraPrinting.BorderSide.None;
                //    label6.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
                //    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                //}
                //else if (data.GLPLCtGId == 20)
                //{
                //    _longTermAdvances = data.Amount;
                //    _prevLongTermAdvances = data.PrevAmount;
                //    label7.Borders = DevExpress.XtraPrinting.BorderSide.None;
                //    label7.Borders = DevExpress.XtraPrinting.BorderSide.None;
                //    label6.Borders = DevExpress.XtraPrinting.BorderSide.None;
                //    label6.Borders = DevExpress.XtraPrinting.BorderSide.None;
                //    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                //}
                //else if (data.GLPLCtGId == 21)
                //{
                //    _longTermInv = data.Amount;
                //    _prevLongTermInv = data.PrevAmount;
                //    label7.Borders = DevExpress.XtraPrinting.BorderSide.None;
                //    label7.Borders = DevExpress.XtraPrinting.BorderSide.None;
                //    label6.Borders = DevExpress.XtraPrinting.BorderSide.None;
                //    label6.Borders = DevExpress.XtraPrinting.BorderSide.None;
                //    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                //}
                else if (data.GLPLCtGId == 7)
                {
                    _storesSparesLooseTools = data.Amount;
                    _prevStoresSparesLooseTools = data.PrevAmount;
                    label7.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label6.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);

                }
                else if (data.GLPLCtGId == 8)
                {
                    _stockInTrade = data.Amount;
                    _prevStockInTrade = data.PrevAmount;
                    label7.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label6.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);

                }
                else if (data.GLPLCtGId == 9)
                {
                    _tradeDebitors = data.Amount;
                    _prevTradeDebitors = data.PrevAmount;
                    label7.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label6.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                }
                else if (data.GLPLCtGId == 10)
                {
                    _loanAndAdvances = data.Amount;
                    _prevLoanAndAdvances = data.PrevAmount;
                    label7.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label6.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                }
                else if (data.GLPLCtGId == 11)
                {
                    _shortTermDepositsAndPrePayments = data.Amount;
                    _prevShortTermDepositsAndPrePayments = data.PrevAmount;
                    label7.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label6.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                }
                else if (data.GLPLCtGId == 12)
                {
                    _dueFromAssociateUnderTaking = data.Amount;
                    _prevDueFromAssociateUnderTaking = data.PrevAmount;
                    label7.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label6.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                }
                else if (data.GLPLCtGId == 13)
                {
                    _otherReceivable = data.Amount;
                    _prevOtherReceivable = data.PrevAmount;
                    label7.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label6.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                }
                else if (data.GLPLCtGId == 14)
                {
                    _cashAndBankBalances = data.Amount;
                    _prevCashAndBankBalances = data.PrevAmount;
                    label7.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
                    label6.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                }
                else if (data.GLPLCtGId == 32)
                {
                    _shareCapital = data.Amount;
                    label7.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label6.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                }
                else if (data.GLPLCtGId == 33)
                {
                    _unAppropriateProfitLoss = data.Amount;
                    label7.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label6.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                }
                else if (data.GLPLCtGId == 34)
                {
                    data.Amount = yearlyProfit();
                    _profitForTheYear = data.Amount;
                    label7.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
                    label6.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                }
                else if (data.GLPLCtGId == 35)
                {
                    _longTermFinancingSecured = data.Amount;
                    label7.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label6.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                }
                else if (data.GLPLCtGId == 36)
                {
                    _directorsCurrentAccount = data.Amount;
                    label7.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label6.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                }
                else if (data.GLPLCtGId == 37)
                {
                    _dueToAssociateUndertaking = data.Amount;
                    label7.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label6.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                }
                else if (data.GLPLCtGId == 38)
                {
                    _defferedTexation = data.Amount;
                    label7.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label6.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                }
                else if (data.GLPLCtGId == 39)
                {
                    _liabilitiesAgainstAssetsSubjectsToFinanceLease = data.Amount;
                    label7.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
                    label6.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                }
                else if (data.GLPLCtGId == 25)
                {
                    _tradeAndOtherPayables = data.Amount;
                    label7.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label6.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                }
                else if (data.GLPLCtGId == 26)
                {
                    _currentPortionOfLongTermLiabilities = data.Amount;
                    label7.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label6.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                }
                else if (data.GLPLCtGId == 27)
                {
                    _markUp = data.Amount;
                    label7.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label6.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                }
                else if (data.GLPLCtGId == 28)
                {
                    _shortTermRunningFinance = data.Amount;
                    label7.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
                    label6.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Regular);
                }
                else if (data.GLPLCtGId == 15)
                {
                    label7.Borders = DevExpress.XtraPrinting.BorderSide.Top;
                    label6.Borders = DevExpress.XtraPrinting.BorderSide.Top;
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
                }
                else if (data.GLPLCtGId == 16)
                {
                    label7.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label6.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
                }
                else if (data.GLPLCtGId == 17)
                {
                    label7.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label6.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
                }
                else if (data.GLPLCtGId == 30)
                {
                    label7.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
                    label6.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
                }
                else if (data.GLPLCtGId == 31)
                {
                    label7.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label6.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
                }
                else if (data.GLPLCtGId == 19)
                {
                    label7.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
                    label6.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
                }
                else if (data.GLPLCtGId == 40)
                {
                    label7.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label6.Borders = DevExpress.XtraPrinting.BorderSide.None;
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
                }
                else if (data.GLPLCtGId == 42)
                {
                    label7.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
                    label6.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
                    label5.Font = new Font(Font.FontFamily, Font.Size, FontStyle.Bold);
                }


            }
        }

        private void label7_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            var data = (dynamic)GetCurrentRow();
            if (data != null)
            {
                if (data.GLPLCtGId == 15)
                {
                    _a = _propertyPlantAndEquipment + _intangibleAssets + _capitalWorkInProgress;
                    ((XRLabel)sender).Text = _a.ToString("n2");

                }
                else if (data.GLPLCtGId == 16)
                {
                    _b = _longTermDeposits + _longTermAdvances + _longTermInv;
                    ((XRLabel)sender).Text = _b.ToString("n2");

                }
                else if (data.GLPLCtGId == 17)
                {
                    _c = _a + _b;
                    ((XRLabel)sender).Text = _c.ToString("n2");

                }
                else if (data.GLPLCtGId == 19)
                {
                    _d = _storesSparesLooseTools + _stockInTrade + _tradeDebitors + _loanAndAdvances + _shortTermDepositsAndPrePayments + _dueFromAssociateUnderTaking + _otherReceivable + _cashAndBankBalances;
                    ((XRLabel)sender).Text = _d.ToString("n2");

                }
                else if (data.GLPLCtGId == 30)
                {
                    _e = _c + _d;
                    ((XRLabel)sender).Text = _e.ToString("n2");

                }
                else if (data.GLPLCtGId == 31)
                {
                    _g = _shareCapital + _unAppropriateProfitLoss + _profitForTheYear;
                    ((XRLabel)sender).Text = _g.ToString("n2");

                    if (Convert.ToDecimal(((XRLabel)sender).Text) > 0)
                    {
                        ((XRLabel)sender).Text = "( " + string.Format("{0:n2}", Math.Abs(Convert.ToDecimal(((XRLabel)sender).Text))) + " )";

                    }

                }
                else if (data.GLPLCtGId == 40)
                {
                    _h = _longTermFinancingSecured + _directorsCurrentAccount + _dueToAssociateUndertaking + _defferedTexation + _liabilitiesAgainstAssetsSubjectsToFinanceLease;
                    ((XRLabel)sender).Text = _h.ToString("n2");

                    if (Convert.ToDecimal(((XRLabel)sender).Text) > 0)
                    {
                        ((XRLabel)sender).Text = "( " + string.Format("{0:n2}", Math.Abs(Convert.ToDecimal(((XRLabel)sender).Text))) + " )";

                    }

                }
                else if (data.GLPLCtGId == 42)
                {
                    _i = _tradeAndOtherPayables + _currentPortionOfLongTermLiabilities + _markUp + _shortTermRunningFinance;
                    ((XRLabel)sender).Text = _i.ToString("n2");

                    if (Convert.ToDecimal(((XRLabel)sender).Text) > 0)
                    {
                        ((XRLabel)sender).Text = "( " + string.Format("{0:n2}", Math.Abs(Convert.ToDecimal(((XRLabel)sender).Text))) + " )";

                    }

                }
                else if (data.GLPLCtGId == 41)
                {
                    ((XRLabel)sender).Text = (_g + _h + _i).ToString("n2");

                    if (Convert.ToDecimal(((XRLabel)sender).Text) > 0)
                    {
                        ((XRLabel)sender).Text = "( " + string.Format("{0:n2}", Math.Abs(Convert.ToDecimal(((XRLabel)sender).Text))) + " )";

                    }

                }
                else if (data.GLPLCtGId == 25 || data.GLPLCtGId == 26 || data.GLPLCtGId == 27 || data.GLPLCtGId == 28 || data.GLPLCtGId == 32
                      || data.GLPLCtGId == 33 || data.GLPLCtGId == 34 || data.GLPLCtGId == 35 || data.GLPLCtGId == 36 || data.GLPLCtGId == 37
                      || data.GLPLCtGId == 38 || data.GLPLCtGId == 39

                    )
                {
                    if (Convert.ToDecimal(((XRLabel)sender).Text) > 0)
                    {
                        ((XRLabel)sender).Text = "( " + string.Format("{0:n2}", Math.Abs(Convert.ToDecimal(((XRLabel)sender).Text))) + " )";

                    }
                }


                if (!((XRLabel)sender).Text.StartsWith("(") && Math.Sign(Convert.ToDecimal(((XRLabel)sender).Text)) == -1)
                {
                    ((XRLabel)sender).Text = string.Format("{0:n2}", Math.Abs(Convert.ToDecimal(((XRLabel)sender).Text)));

                }

                label11.Text = (_e - (_g + _h + _i)).ToString("n2");
            }
        }

        private void label6_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            var data = (dynamic)GetCurrentRow();
            if (data != null)
            {
                if (data.GLPLCtGId == 15)
                {
                    _prevA = _propertyPlantAndEquipment + _intangibleAssets + _capitalWorkInProgress;
                    ((XRLabel)sender).Text = _prevA.ToString("n2");

                }
                else if (data.GLPLCtGId == 16)
                {
                    _prevB = _longTermDeposits + _longTermAdvances + _longTermInv;
                    ((XRLabel)sender).Text = _prevB.ToString("n2");

                }
                else if (data.GLPLCtGId == 17)
                {
                    _prevC = _a + _b;
                    ((XRLabel)sender).Text = _prevC.ToString("n2");


                }
                else if (data.GLPLCtGId == 19)
                {
                    _prevD = _storesSparesLooseTools + _stockInTrade + _tradeDebitors + _loanAndAdvances + _shortTermDepositsAndPrePayments + _dueFromAssociateUnderTaking + _otherReceivable + _cashAndBankBalances;
                    ((XRLabel)sender).Text = _prevD.ToString("n2");

                }
                else if (data.GLPLCtGId == 30)
                {
                    _prevE = _c + _d;
                    ((XRLabel)sender).Text = _prevE.ToString("n2");

                }
                else if (data.GLPLCtGId == 31)
                {
                    _prevG = _shareCapital + _unAppropriateProfitLoss + _profitForTheYear;
                    ((XRLabel)sender).Text = _prevG.ToString("n2");

                }
                else if (data.GLPLCtGId == 40)
                {
                    _prevH = _longTermFinancingSecured + _directorsCurrentAccount + _dueToAssociateUndertaking + _defferedTexation + _liabilitiesAgainstAssetsSubjectsToFinanceLease;
                    ((XRLabel)sender).Text = _prevH.ToString("n2");

                }
                else if (data.GLPLCtGId == 42)
                {
                    _prevI = _tradeAndOtherPayables + _currentPortionOfLongTermLiabilities + _markUp + _shortTermRunningFinance;
                    ((XRLabel)sender).Text = _prevI.ToString("n2");

                }
                else if (data.GLPLCtGId == 41)
                {
                    ((XRLabel)sender).Text = (_prevG + _prevH + _prevI).ToString("n2");

                }
            }
        }
        private void pictureBox1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }
        private void BalanceSheet_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DevExpress.XtraReports.Parameters.Parameter param = ((XtraReport)sender).Parameters["Tenant"];
            DevExpress.XtraReports.Parameters.Parameter fromD = ((XtraReport)sender).Parameters["FromDate"];
            DevExpress.XtraReports.Parameters.Parameter toD = ((XtraReport)sender).Parameters["ToDate"];

            _tenantId = Convert.ToInt32(param.Value);
            _fromDate = Convert.ToDateTime(fromD.Value);
            _toDate = Convert.ToDateTime(toD.Value);

            DecimalPoints = ((XtraReport)sender).Parameters["FinancePoint"].Value.ToString();
            this.label6.TextFormatString = "{0:f" + DecimalPoints + "}";
            this.label7.TextFormatString = "{0:f" + DecimalPoints + "}";
        }

        private void pictureBox1_BeforePrint_1(object sender, System.Drawing.Printing.PrintEventArgs e)
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

        private void GroupHeader2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            var data = (dynamic)GetCurrentRow();
            if (data != null)
            {
                if (data.GLPLCtGId == 17 || data.GLPLCtGId == 30 || data.GLPLCtGId == 19 || data.GLPLCtGId == 40 || data.GLPLCtGId == 42 || data.GLPLCtGId == 15
                    || data.GLPLCtGId == 16)
                {
                    GroupHeader2.Visible = false;
                }
                else
                {
                    GroupHeader2.Visible = true;
                }
            }
        }

        private double yearlyProfit()
        {
            string str = ConfigurationManager.AppSettings["ConnectionString"];
            double expense = 0;
            double income = 0;
            using (SqlConnection cn = new SqlConnection(str))
            {
                SqlCommand cmd = new SqlCommand("dbo.GetPlAmounts", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@fromDate", _fromDate.Date.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@toDate", _toDate.Date.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@TenantID", _tenantId);
                cn.Open();
                using (SqlDataReader rowData = cmd.ExecuteReader())
                {

                    while (rowData.Read())
                    {
                        if (Convert.ToString(rowData["TypeID"]).ToLower() == "expenses")
                            expense = Convert.ToDouble(rowData["Amount"]);
                        else
                            income = Convert.ToDouble(rowData["Amount"]);
                    }
                }

                //  // cn.Close();
            }
            return income - expense;
        }

    }
}

