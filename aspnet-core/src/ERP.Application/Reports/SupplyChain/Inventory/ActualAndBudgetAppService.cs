using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;

namespace ERP.Reports.SupplyChain.Inventory
{
    public class ActualAndBudgetAppService : ERPReportAppServiceBase, IActualAndBudgetAppService
    {
        public List<ActualAndBudget> GetData(int? tenantId, string fromDoc, string toDoc)
        {
            tenantId = tenantId > 0 ? tenantId : (int)AbpSession.TenantId;
            string str = ConfigurationManager.AppSettings["ConnectionString"];
            List<ActualAndBudget> actualAndBudgetList = new List<ActualAndBudget>();
            using (SqlConnection cn = new SqlConnection(str))
            {
                
                
                //string qry = "select a.docno,a.DocDate,a.Unit,a.itemid,descp,isnull(Aqty,0) as  Aqty,isnull(c.Bqty,0) as Bqty,isnull(c.Brate,0) as  Brate,isnull(Arate,0) as Arate,WorkOrder,OrdNo,LocID,LocName," +
                //             " isnull(conDocNo, 0) as ConDocNo,isnull(b.ReturnQty, 0) as ReturnQty" +
                //             " from (select ICCNSd.Unit, ICCNSH.DocNo, ICCNSH.DocDate, ICCNSH.OrdNo, ICCNSH.TenantId, icitem.ItemId, ICITEM.Descp, ICCNSD.Qty as Aqty, ICCNSD.cost as Arate," +
                //             " ICCNSH.OrdNo as WorkOrder, icloc.LocID, LocName from ICCNSH inner join ICCNSD on ICCNSH.DocNo = ICCNSD.DocNo" +
                //             " and ICCNSH.TenantId = ICCNSD.TenantId  inner join ICITEM on ICCNSD.ItemID = ICITEM.ItemId and ICCNSD.TenantId = ICITEM.TenantId inner join ICLOC on ICCNSH.LocID = " +
                //             " ICLOC.LocID and ICCNSH.TenantId = ICLOC.TenantId left join ICCCNT on ICCNSH.CCID = ICCCNT.CCID and ICCNSH.TenantId = ICCCNT.TenantId " +
                //             " left join GLAMF on ICCCNT.AccountID = GLAMF.AccountID and ICCCNT.TenantId = GLAMF.TenantId " +
                //             " ) as a left join(select ConDocNo, ICADJH.TenantId, Sum(ICADJD.Qty) as ReturnQty from ICADJH inner join ICADJD" +
                //             " on ICADJH.DocNo = ICADJD.DocNo   and ICADJH.TenantId = ICADJD.TenantId where ICADJH.ConDocNo is not null group by ICADJH.TenantId,ConDocNo) " +
                //             " as b on a.DocNo = b.ConDocNo and a.TenantId = b.TenantId " +
                //             " left join (select ICWOH.DocNo, ICWOH.DocDate, ICWOD.ItemID, ICWOD.TenantId, ICWOD.Qty as Bqty,ICWOD.Cost as Brate from ICWOH inner join ICWOD on " +
                //             " ICWOH.id = ICWOD.DetID and ICWOH.TenantId = ICWOD.TenantId) as c   on a.ItemId = c.ItemID and a.TenantId = c.TenantId and a.OrdNo = c.DocNo " +
                //             " where cast (a.DocDate as Date) between '" + fromDate + "' and '" + toDate + "' and a.TenantId = " + tenantId + "";
                string qry = "select a.WorkOrder,a.Unit,a.itemid,descp,isnull(Aqty,0) as  Aqty,isnull(c.Bqty,0) as Bqty,isnull(c.Brate,0)  as Brate, isnull(Arate, 0) as Arate,isnull(a.ReturnQty, 0) " +
                             " as ReturnQty from (select ICCNSd.Unit, ICCNSH.OrdNo, ICCNSH.TenantId, icitem.ItemId, " +
                             " ICITEM.Descp, sum(ICCNSD.Qty) as Aqty, isnull(sum(subQry.ReturnQty), 0) as ReturnQty, ICCNSD.cost as Arate, ICCNSH.OrdNo as WorkOrder from ICCNSH " +
                             " inner join ICCNSD on ICCNSH.DocNo = ICCNSD.DocNo and ICCNSH.TenantId = ICCNSD.TenantId  inner join ICITEM on ICCNSD.ItemID = " +
                             "ICITEM.ItemId and ICCNSD.TenantId = ICITEM.TenantId inner join ICLOC on ICCNSH.LocID = ICLOC.LocID and ICCNSH.TenantId = " +
                             "ICLOC.TenantId left join (  select ItemID, ICADJH.TenantId, ICADJH.ConDocNo, " +
                             "Sum(ICADJD.Qty) as ReturnQty from ICADJH inner join ICADJD on ICADJH.DocNo = ICADJD.DocNo   and ICADJH.TenantId = " +
                             "ICADJD.TenantId where ICADJH.ConDocNo is not null group by ICADJH.TenantId, ItemID, ConDocNo " +
                             " ) as subQry on ICCNSH.DocNo = subQry.ConDocNo and ICCNSH.TenantId = subQry.TenantId and ICCNSD.ItemID = subQry.ItemID " +
                             "group by ICCNSd.Unit, ICCNSH.OrdNo, ICCNSH.TenantId, icitem.ItemId, ICITEM.Descp, ICCNSD.cost, ICCNSH.OrdNo ) as a  left join(select ICWOH.DocNo, ICWOH.DocDate, ICWOD.ItemID, ICWOD.TenantId, ICWOD.Qty " +
                             "as Bqty,ICWOD.Cost as Brate from ICWOH inner join ICWOD on ICWOH.id = ICWOD.DetID and ICWOH.TenantId = ICWOD.TenantId) " +
                             "as c  on a.ItemId = c.ItemID and a.TenantId = c.TenantId and a.OrdNo = c.DocNo  where a.TenantId = " + tenantId + "" +
                             "and a.OrdNo is not null and a.OrdNo != '' and a.OrdNo between " + fromDoc + " and " + toDoc + "";

                using (SqlCommand cmd = new SqlCommand(qry, cn))
                {
                    cn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            ActualAndBudget actualAndBudget = new ActualAndBudget();
                            //actualAndBudget.DocNo = rdr["docno"].ToString();
                            actualAndBudget.Unit = rdr["Unit"].ToString();
                            // var date = Convert.ToDateTime(rdr["DocDate"]);
                            // actualAndBudget.DocDate = date.Day.ToString() + "-" + date.Month.ToString() + "-" + date.Year.ToString();
                            // actualAndBudget.Unit = rdr["Unit"].ToString();
                            actualAndBudget.ItemDesc = rdr["descp"].ToString();
                            actualAndBudget.ItemId = rdr["itemid"].ToString();
                            actualAndBudget.AQty = Convert.ToDouble(rdr["AQty"]);
                            actualAndBudget.ARate = Convert.ToDouble(rdr["ARate"]);
                            actualAndBudget.BQty = Convert.ToDouble(rdr["BQty"]);
                            actualAndBudget.BRate = Convert.ToDouble(rdr["ARate"]);
                            actualAndBudget.WorkOrder = rdr["WorkOrder"].ToString();
                            actualAndBudget.Return = Convert.ToDouble(rdr["ReturnQty"]);
                            actualAndBudgetList.Add(actualAndBudget);
                        }
                    }
                }

            }
            return actualAndBudgetList;
        }
    }
}
