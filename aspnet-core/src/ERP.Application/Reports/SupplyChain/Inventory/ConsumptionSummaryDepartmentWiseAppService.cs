using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;

namespace ERP.Reports.SupplyChain.Inventory
{
    public class ConsumptionSummaryDepartmentWiseAppService : ERPReportAppServiceBase, IConsumptionSummaryDepartmentWiseAppService
    {
        public List<ConsumptionSummaryDepartmentWise> GetData(int? tenantId, int fromLocId, int toLocId)
        {
            tenantId = tenantId > 0 ? tenantId : (int)AbpSession.TenantId;
            string str = ConfigurationManager.AppSettings["ConnectionString"];
            List<ConsumptionSummaryDepartmentWise> ConsumptionSummaryDepartmentList = new List<ConsumptionSummaryDepartmentWise>();
            List<ItemStockSegment2> itemStockSegment2List = new List<ItemStockSegment2>();
            using (SqlConnection cn = new SqlConnection(str))
            {

              
                //string qry = "select LocID,LocName,isnull(sum(a.qty),0) as IssueQty, isnull(sum(b.qty),0) as RetQty from ( select ICCNSH.DocNo," +
                //    " ICCNSH.TenantId, ICCNSD.Qty, ICLOC.LocID, ICLOC.LocName from ICCNSH inner join ICCNSD on ICCNSH.DocNo = ICCNSD.DocNo" +
                //                                " and ICCNSH.TenantId = ICCNSD.TenantId inner join ICLOC on ICCNSH.LocID = ICLOC.LocID and ICCNSH.TenantId = " +
                //                                " icloc.TenantId " +
                //                                " ) as a left join (select ConDocNo, ICADJH.TenantId, ICADJD.Qty from ICADJH inner join ICADJD on " +
                //                                " ICADJH.DocNo = ICADJD.DocNo and ICADJH.TenantId = ICADJD.TenantId ) as b on a.DocNo =" +
                //                                " b.ConDocNo and a.TenantId = b.TenantId where a.TenantId = " + tenantId + " and a.LocID between " + fromLocId + " and " + toLocId + " group by a.LocID, a.LocName";

                string qry = "select LocID,LocName,isnull(sum(a.qty),0) as IssueQty, isnull(sum(b.qty),0) as RetQty,a.CCID,a.CCName from ( select ICCNSH.DocNo, ICCNSH.TenantId, " +
                             "ICCNSD.Qty, ICLOC.LocID, ICLOC.LocName,ICCCNT.CCID,ICCCNT.CCName from ICCNSH inner join ICCNSD on ICCNSH.DocNo = ICCNSD.DocNo and ICCNSH.TenantId = ICCNSD.TenantId " +
                             "inner join ICLOC on ICCNSH.LocID = ICLOC.LocID and ICCNSH.TenantId = icloc.TenantId " +
                             " inner join ICCCNT on ICCNSH.CCID = ICCCNT.CCID and ICCNSH.TenantId = ICCCNT.TenantId " +
                             " ) as a left join(select ConDocNo, ICADJH.TenantId, ICADJD.Qty from ICADJH inner join ICADJD on ICADJH.DocNo = ICADJD.DocNo and ICADJH.TenantId = ICADJD.TenantId) as b on a.DocNo = b.ConDocNo " +
                             " and a.TenantId = b.TenantId where a.TenantId = " + tenantId + " and a.LocID between " + fromLocId + " and " + toLocId + " group by a.LocID, a.LocName,a.CCID,a.CCName ";
                using (SqlCommand cmd = new SqlCommand(qry, cn))
                {
                    cn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            ConsumptionSummaryDepartmentWise consumptionSummaryDepartment = new ConsumptionSummaryDepartmentWise();
                            consumptionSummaryDepartment.LocId = Convert.ToInt32(rdr["LocId"]);
                            consumptionSummaryDepartment.LocName = rdr["LocName"].ToString();
                            consumptionSummaryDepartment.IssueQty = rdr["IssueQty"].ToString();
                            consumptionSummaryDepartment.RetQty = rdr["RetQty"].ToString();
                            consumptionSummaryDepartment.CCId = rdr["CCId"].ToString().Trim();
                            consumptionSummaryDepartment.CCName = rdr["CCName"].ToString();
                            ConsumptionSummaryDepartmentList.Add(consumptionSummaryDepartment);
                        }
                    }
                }
            }
            return ConsumptionSummaryDepartmentList;
        }
    }
}
