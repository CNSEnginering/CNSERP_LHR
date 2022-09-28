using System;
using DevExpress.XtraReports.UI;
using ERP.Web.DXServices.DataHandler;
using Newtonsoft.Json;

namespace ERP.Web.DXServices.Reports.Finance
{
    public partial class PDCSubReport
    {
        public PDCSubReport()
        {
            InitializeComponent();
        }

        private void PDCSubReport_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            var data = (dynamic)GetCurrentRow();
            if(data != null)
            {
                if (data.Type == "AP")
                {
                    label14.Visible = false;
                    label2.Visible = false;
                    label16.Visible = false;

                    label5.Visible = true;
                    label8.Visible = true;
                    label9.Visible = true;
                }
                else
                {
                    label5.Visible = false;
                    label8.Visible = false;
                    label9.Visible = false;

                    label14.Visible = true;
                    label16.Visible = true;
                    label2.Visible = true;
                }
            }

            //DevExpress.XtraReports.Parameters.Parameter paramTenantId =
            //    ((XtraReport)sender).
            //     Parameters["TenantId"];
            //var tenantId = Convert.ToInt32(paramTenantId.Value);

            //DevExpress.XtraReports.Parameters.Parameter paramAccId =
            //   ((XtraReport)sender).
            //    Parameters["AccountId"];
            //var accId = paramAccId.Value.ToString();

            //DevExpress.XtraReports.Parameters.Parameter paramSubledgerCode =
            //  ((XtraReport)sender).
            //   Parameters["SubLedgerCode"];
            //var subledgerCode = Convert.ToInt32(paramSubledgerCode.Value);

            //XtraReport report = new PDCSubReport();
            //var data = (dynamic)GetCurrentRow();
            //var newData = ReportDataHandlerBase.GetPDCSubReport(subledgerCode, tenantId, accId);

            //// Assign the data source to the report.
            //report.DataSource = newData;
        }
    }
}
