using Abp.Domain.Repositories;
using ERP.SupplyChain.Inventory;
using ERP.SupplyChain.Inventory.IC_Item;
using ERP.SupplyChain.Inventory.IC_Segment2;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ERP.Reports.SupplyChain.Inventory
{
    public class StockReportAppService : ERPReportAppServiceBase, IStockReportAppService
    {
        private IRepository<ICLEDG> _icledgRepository;
        private IRepository<ICSegment2> _icSegment2Repository;
        private IRepository<ICItem> _icitemRepository;
        public StockReportAppService(IRepository<ICLEDG> icledgRepository,
            IRepository<ICSegment2> icSegment2Repository,
            IRepository<ICItem> icitemRepository)
        {
            _icledgRepository = icledgRepository;
            _icSegment2Repository = icSegment2Repository;
            _icitemRepository = icitemRepository;
        }

        public List<ItemStockSegment2> GetData(int tenantId, string fromLocId, string toLocId, string fromItem, string toItem, string fromDate, string toDate)
        {
            tenantId = tenantId > 0 ? tenantId : (int)AbpSession.TenantId;
            string str = ConfigurationManager.AppSettings["ConnectionString"];
            List<ItemStockSegment2> itemStockSegment2List = new List<ItemStockSegment2>();
            using (SqlConnection cn = new SqlConnection(str))
            {
                using (SqlCommand cmd = new SqlCommand("sp_consolidatedstock_Loc", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fromdate", fromDate);
                    cmd.Parameters.AddWithValue("@todate", toDate);
                    cmd.Parameters.AddWithValue("@TenantID", tenantId);
                    cmd.Parameters.AddWithValue("@fromitem", fromItem);
                    cmd.Parameters.AddWithValue("@toitem", toItem);
                    cmd.Parameters.AddWithValue("@fromloc", fromLocId);
                    cmd.Parameters.AddWithValue("@toloc", toLocId);
                    cn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            ItemStockSegment2 itemStockSegment2 = new ItemStockSegment2();
                            itemStockSegment2.ItemId = rdr["ItemID"].ToString();
                            itemStockSegment2.OpeningQty = Convert.ToDouble(rdr["opqty"]);
                            itemStockSegment2.ReceiptQty = Convert.ToDouble( rdr["rcqty"]);
                            itemStockSegment2.OpeningAmount = Convert.ToDouble(rdr["opamt"]);
                            itemStockSegment2.ReceiptAmount = Convert.ToDouble(rdr["rcamt"]);
                            itemStockSegment2.IssueQty = Convert.ToDouble(rdr["isqty"]);
                            itemStockSegment2.IssueAmount = Convert.ToDouble(rdr["isamt"]);
                            itemStockSegment2.ItemDescp = rdr["descp"].ToString();
                            itemStockSegment2.Unit = rdr["stockUnit"].ToString();
                            itemStockSegment2.Seg1Id = rdr["Seg1Id"].ToString();
                            itemStockSegment2.Seg1IdDescp = rdr["Seg1Name"].ToString();
                            itemStockSegment2.Seg2Id = rdr["Seg2id"].ToString();
                            itemStockSegment2.Seg3Id = rdr["Seg3id"].ToString();
                            itemStockSegment2.Seg2IdDescp = rdr["Seg2Name"].ToString();
                            itemStockSegment2.Seg3IdDescp = rdr["Seg3Name"].ToString();
                            itemStockSegment2.LocId = Convert.ToInt32(rdr["LocID"]);
                            itemStockSegment2.LocDesc = rdr["LocName"].ToString();
                            itemStockSegment2List.Add(itemStockSegment2);
                        }
                    }
                }
                // cn.Close();
            }
            //    var opening = from a in _icledgRepository.GetAll().Where(o => o.TenantId == tenantId)
            //                  join
            //                  b in _icitemRepository.GetAll().Where(o => o.TenantId == tenantId)
            //                  on new { A = a.ItemID, B = a.TenantId } equals new { A = b.ItemId, B = b.TenantId }
            //                  join
            //                  c in _icSegment2Repository.GetAll().Where(o => o.TenantId == tenantId)
            //                  on new { A = b.Seg2Id, B = b.TenantId } equals new { A = c.Seg2Id, B = c.TenantId }
            //                  where (a.ItemID.CompareTo(fromItem) >= 0 && a.ItemID.CompareTo(toItem) <= 0 && Convert.ToDateTime(a.DocDate) < Convert.ToDateTime(fromDate)
            //                           && a.LocID >= Convert.ToInt32(fromLocId) && a.LocID <=
            //                          Convert.ToInt32(toLocId) && a.TenantId == tenantId)
            //                  select new
            //                  {
            //                      a.ItemID,
            //                      a.Descp,
            //                      b.Seg2Id,
            //                      c.Seg2Name,
            //                      a.Qty,
            //                      a.TenantId
            //                  };

            //    var receiptAndIssue = from a in _icledgRepository.GetAll().Where(o => o.TenantId == tenantId)
            //                join
            //                b in _icitemRepository.GetAll().Where(o => o.TenantId == tenantId)
            //                on new { A = a.ItemID, B = a.TenantId } equals new { A = b.ItemId, B = b.TenantId }
            //                join
            //                c in _icSegment2Repository.GetAll().Where(o => o.TenantId == tenantId)
            //                on new { A = b.Seg2Id, B = b.TenantId } equals new { A = c.Seg2Id, B = c.TenantId }
            //                where (a.ItemID.CompareTo(fromItem) >= 0 && a.ItemID.CompareTo(toItem) <= 0 && Convert.ToDateTime(a.DocDate) >= Convert.ToDateTime(fromDate)
            //                        && Convert.ToDateTime(a.DocDate) <= Convert.ToDateTime(toDate) && a.LocID >= Convert.ToInt32(fromLocId) && a.LocID <=
            //                        Convert.ToInt32(toLocId) && a.TenantId == tenantId)
            //                select new 
            //                {
            //                    b.Seg2Id,
            //                    b.Descp,
            //                    //Opening = opening.Where(o => o.ItemID == b.ItemId && o.Seg2Id == b.Seg2Id).Count() > 0 ?
            //                    //opening.Where(o => o.ItemID == b.ItemId && o.Seg2Id == b.Seg2Id).FirstOrDefault().Qty : 0,
            //                    Receipt = (a.Qty > 0 ? a.Qty : 0),
            //                    Issue = (a.Qty < 0 ? a.Qty : 0),
            //                    b.ItemId,
            //                    b.TenantId,
            //                    Seg2IdDescp = c.Seg2Name
            //                };

            //    var stock = from a in _icSegment2Repository.GetAll().Where(o => o.TenantId == tenantId)
            //                join
            //                b in _icitemRepository.GetAll().Where(o => o.TenantId == tenantId)
            //                on new { A = a.Seg2Id, B = a.TenantId } equals new { A = b.Seg2Id, B = b.TenantId } 
            //                join
            //                c in receiptAndIssue
            //                on new { A = b.ItemId, B = b.TenantId } equals new { A = c.ItemId, B = c.TenantId } into gj
            //                from subset in gj.DefaultIfEmpty()
            //                select new ItemStock()
            //                {
            //                    Seg2Id = b.Seg2Id,
            //                    ItemDescp = b.Descp,
            //                    Opening = opening.Where(o => o.ItemID == b.ItemId && o.Seg2Id == b.Seg2Id).Count() > 0 ?
            //                    opening.Where(o => o.ItemID == b.ItemId && o.Seg2Id == b.Seg2Id).FirstOrDefault().Qty : 0,
            //                    Receipt = !subset.Receipt.HasValue ? 0 : subset.Receipt,
            //                    Issue = !subset.Issue.HasValue ? 0 : subset.Issue,
            //                    ItemId = b.ItemId,
            //                    Seg2IdDescp = a.Seg2Name

            //                };
            //   return stock.ToList();
            //}
            return itemStockSegment2List;
        }
        //public class ItemStock
        //{
        //    public string ItemId { get; set; }
        //    public string ItemDescp { get; set; }
        //    public string Seg2Id { get; set; }
        //    public string Seg2IdDescp { get; set; }
        //    public string Unit { get; set; }
        //    public double? Opening { get; set; }
        //    public double? Receipt { get; set; }
        //    public double? Issue { get; set; }
        //}
    }
}
