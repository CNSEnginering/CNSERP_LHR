using System;
using System.Linq;
using Abp.Domain.Repositories;
using DevExpress.XtraReports.UI;
using ERP.SupplyChain.Inventory;

namespace ERP.Web.DXServices.Reports.Inventory
{
    public partial class StockReportLevelWise
    {
       
        private int _tenantId;
        public string DecimalPoints = "";

        public StockReportLevelWise()
        {
            InitializeComponent();
        }

        private void ItemStockSegment2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DevExpress.XtraReports.Parameters.Parameter param =
         ((XtraReport)sender).
          Parameters["TenantId"];
            _tenantId = Convert.ToInt32(param.Value);

            DecimalPoints = ((XtraReport)sender).Parameters["InventoryPoint"].Value.ToString();

            this.label42.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label11.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label10.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label7.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label16.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label7.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label18.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label19.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label21.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label22.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label23.TextFormatString = "{0:N" + DecimalPoints + "}";
            this.label24.TextFormatString = "{0:N" + DecimalPoints + "}";

        }
    }
}
