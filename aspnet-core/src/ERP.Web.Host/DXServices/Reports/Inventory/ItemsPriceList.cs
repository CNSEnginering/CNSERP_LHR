using System;
using DevExpress.XtraReports.UI;

namespace ERP.Web.DXServices.Reports.Inventory
{
    public partial class ItemsPriceList
    {
        public string DecimalPoints = "";
        public ItemsPriceList()
        {
            InitializeComponent();
        }

        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            ((XRLabel)sender).Text = GetCurrentColumnValue("salesPrice").ToString();
            DecimalPoints = ((XtraReport)sender).Parameters["InventoryPoint"].Value.ToString();

            this.tableCell16.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.tableCell15.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.tableCell14.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.tableCell13.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.tableCell12.TextFormatString = "{0:N" + DecimalPoints + "}";

        }
    }
}
