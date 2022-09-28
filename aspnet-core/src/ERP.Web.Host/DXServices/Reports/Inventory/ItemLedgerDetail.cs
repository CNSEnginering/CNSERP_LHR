using System;
using System.Drawing;
using System.IO;
using DevExpress.XtraPrinting.Drawing;
using DevExpress.XtraReports.UI;
using ERP.Web.DXServices.Common;

namespace ERP.Web.DXServices.Reports.Inventory
{
    public partial class ItemLedgerDetail
    {
        // double BalanceAmount = 0;
        private int _tenantId;
        string DecimalPoints = "";
        public ItemLedgerDetail()
        {
            InitializeComponent();
        }

        private void TopMargin_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }

        private void BalanceAmt_TextChanged(object sender, EventArgs e)
        {
            //if(!string.IsNullOrEmpty(BalanceAmt.Text))
            //{
            //    BalanceAmount = BalanceAmount + Convert.ToDouble(BalanceAmt.Text);
            //    BalanceAmt.Text = "";
            //    BalanceAmt.Text = (BalanceAmount).ToString();
            //}
        }

        private void BalanceQty_TextChanged(object sender, EventArgs e)
        {

        }

        private void BalanceAmt_SummaryReset(object sender, EventArgs e)
        {
            //BalanceAmount = 0;
        }

        private void BalanceAmt_SummaryRowChanged(object sender, EventArgs e)
        {
           // BalanceAmount += Convert.ToDouble(GetCurrentColumnValue("BalanceAmt"));
        }

        private void BalanceAmt_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
           // e.Result = BalanceAmount;
           // e.Handled = true;
        }

        private void ItemLedgerDetail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DevExpress.XtraReports.Parameters.Parameter param =
                      ((XtraReport)sender).
                       Parameters["TenantId"];
            _tenantId = Convert.ToInt32(param.Value);
            DecimalPoints = ((XtraReport)sender).Parameters["InventoryPoint"].Value.ToString();

            this.Receipt.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.ReceiptRate.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.ReceiptAmt.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.Issue.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.IssueRate.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.IssueAmount.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.BalanceQty.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label29.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.BalanceAmt.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label20.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label21.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label25.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label22.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label23.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label24.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label26.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label30.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label27.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label19.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label12.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label18.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label16.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label17.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label15.TextFormatString = "{0:N" + DecimalPoints + "}";
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
