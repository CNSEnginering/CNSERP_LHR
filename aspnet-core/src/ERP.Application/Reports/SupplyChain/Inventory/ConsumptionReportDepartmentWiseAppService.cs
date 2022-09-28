using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace ERP.Reports.SupplyChain.Inventory
{
    public class ConsumptionReportDepartmentWiseAppService : ERPReportAppServiceBase, IConsumptionReportDepartmentWiseAppService
    {
        public List<ConsumptionDepartmentWise> GetData(int? tenantId, string fromLocId, string toLocId, string fromDate
            , string toDate, string fromItem, string toItem, string reportName)
        {
            tenantId = tenantId > 0 ? tenantId : (int)AbpSession.TenantId;
            string str = ConfigurationManager.AppSettings["ConnectionString"];
            string qry = "";
            List<ConsumptionDepartmentWise> ConsumptionSummaryDepartmentList = new List<ConsumptionDepartmentWise>();

            using (SqlConnection cn = new SqlConnection(str))
            {

                if (reportName == "ConsumptionReportItemWise")
                {
                    qry = "sp_ConsumptionReportItemWise";

                }
                else if (reportName == "ConsumptionReportAccWise" || reportName == "ConsumptionReportOrderWise")
                {
                    qry = "ConsumptionReportAccWise";
                }
                else
                {
                    qry = "sp_ConsumptionReport";
                }
                using (SqlCommand cmd = new SqlCommand(qry, cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);
                    cmd.Parameters.AddWithValue("@fromItem", fromItem);
                    cmd.Parameters.AddWithValue("@toItem", toItem);
                    cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    cmd.Parameters.AddWithValue("@toDate", toDate);
                    cmd.Parameters.AddWithValue("@LocId", fromLocId);
                    cn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            ConsumptionDepartmentWise consumptionSummaryDepartment = new ConsumptionDepartmentWise();
                            consumptionSummaryDepartment.DocNo = Convert.ToInt32(rdr["DocNo"]);
                            var date = Convert.ToDateTime(rdr["DocDate"]);
                            consumptionSummaryDepartment.DocDate = date.Day.ToString() + "/" + date.Month.ToString() + "/" + date.Year.ToString();
                            consumptionSummaryDepartment.ItemId = rdr["ItemId"].ToString();
                            consumptionSummaryDepartment.ItemName = rdr["Descp"].ToString();
                            consumptionSummaryDepartment.Rate = Convert.ToDouble(rdr["Cost"]);
                            consumptionSummaryDepartment.Amount = Convert.ToDouble(rdr["Amount"]);
                            consumptionSummaryDepartment.WorkOrder = rdr["WorkOrder"].ToString();
                            consumptionSummaryDepartment.LocId = Convert.ToInt32(rdr["LocId"]);
                            consumptionSummaryDepartment.LocName = rdr["LocName"].ToString();
                            consumptionSummaryDepartment.Issue = Convert.ToDouble(rdr["IssueQty"]);
                            consumptionSummaryDepartment.Return = Convert.ToDouble(rdr["ReturnQty"]);
                            consumptionSummaryDepartment.WorkOrder = rdr["WorkOrder"].ToString();
                            consumptionSummaryDepartment.AccountId = rdr["AccountId"].ToString();
                            consumptionSummaryDepartment.AccountName = rdr["AccountName"].ToString();
                            if (reportName == "ConsumptionDepartmentWise")
                            {
                                consumptionSummaryDepartment.CCId = rdr["CCId"].ToString();
                                consumptionSummaryDepartment.CCName = rdr["CCName"].ToString();
                            }
                            ConsumptionSummaryDepartmentList.Add(consumptionSummaryDepartment);
                        }
                    }
                }
            }
            return ConsumptionSummaryDepartmentList;

        }
    }
}
