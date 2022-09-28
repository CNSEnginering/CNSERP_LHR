using Abp.Domain.Repositories;
using ERP.SupplyChain.Inventory;
using ERP.SupplyChain.Inventory.IC_Item;
using ERP.SupplyChain.Inventory.IC_Segment1;
using ERP.SupplyChain.Inventory.IC_Segment2;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ERP.Reports.SupplyChain.Inventory
{
    public class StockReportSegment2AppService : ERPReportAppServiceBase, IStockReportSegment2AppService
    {
        private IRepository<ICLEDG> _icledgRepository;
        private IRepository<ICSegment1> _icSegment1Repository;
        private IRepository<ICSegment2> _icSegment2Repository;
        private IRepository<ICItem> _icitemRepository;
        private IRepository<ICLocation> _locRepository;
        public StockReportSegment2AppService(IRepository<ICLEDG> icledgRepository,
            IRepository<ICSegment2> icSegment2Repository,
            IRepository<ICItem> icitemRepository,
            IRepository<ICSegment1> icSegment1Repository,
            IRepository<ICLocation> locRepository)
        {
            _icledgRepository = icledgRepository;
            _icSegment2Repository = icSegment2Repository;
            _icitemRepository = icitemRepository;
            _icSegment1Repository = icSegment1Repository;
            _locRepository = locRepository;
        }

        public List<ItemStockSegment2> GetData(int tenantId, string fromLocId, string toLocId, string fromItem, string toItem, string fromDate, string toDate)
        {
            tenantId = tenantId > 0 ? tenantId : (int)AbpSession.TenantId;
            string str = ConfigurationManager.AppSettings["ConnectionString"];
            List<ItemStockSegment2> itemStockSegment2List = new List<ItemStockSegment2>();
            using (SqlConnection cn = new SqlConnection(str))
            {

                using (SqlCommand cmd = new SqlCommand("sp_consolidatedseg2", cn))
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
                            //itemStockSegment2.ItemId = rdr["ItemID"].ToString();
                            itemStockSegment2.OpeningQty = Convert.ToDouble(rdr["opqty"]);
                            itemStockSegment2.ReceiptQty = Convert.ToDouble(rdr["rcqty"]);
                            itemStockSegment2.OpeningAmount = Convert.ToDouble(rdr["opamt"]);
                            itemStockSegment2.ReceiptAmount = Convert.ToDouble(rdr["rcamt"]);
                            itemStockSegment2.IssueQty = Convert.ToDouble(rdr["isqty"]);
                            itemStockSegment2.IssueAmount = Convert.ToDouble(rdr["isamt"]);
                            //  itemStockSegment2.ItemDescp = rdr["descp"].ToString();
                            itemStockSegment2.Unit = rdr["stockUnit"].ToString();
                            itemStockSegment2.Seg1Id = rdr["Seg1Id"].ToString();
                            itemStockSegment2.Seg2Id = rdr["Seg2id"].ToString();
                            // itemStockSegment2.Seg3Id = rdr["Seg3id"].ToString();
                            itemStockSegment2.Seg2IdDescp = rdr["seg2name"].ToString();
                            itemStockSegment2.LocId = Convert.ToInt32(rdr["LocID"]);
                            itemStockSegment2.LocDesc = rdr["locname"].ToString();
                            itemStockSegment2List.Add(itemStockSegment2);
                        }
                    }
                }
                // cn.Close();
            }
            //var opening = from a in _icledgRepository.GetAll().Where(o => o.TenantId == tenantId)
            //              join
            //              b in _icitemRepository.GetAll().Where(o => o.TenantId == tenantId)
            //              on new { A = a.ItemID, B = a.TenantId } equals new { A = b.ItemId, B = b.TenantId }
            //              join
            //              c in _icSegment2Repository.GetAll().Where(o => o.TenantId == tenantId)
            //              on new { A = b.Seg2Id, B = b.TenantId } equals new { A = c.Seg2Id, B = c.TenantId }
            //              where (a.ItemID.CompareTo(fromItem) >= 0 && a.ItemID.CompareTo(toItem) <= 0 && Convert.ToDateTime(a.DocDate) < Convert.ToDateTime(fromDate)
            //                       && a.LocID >= Convert.ToInt32(fromLocId) && a.LocID <=
            //                      Convert.ToInt32(toLocId) && a.TenantId == tenantId)
            //              select new
            //              {
            //                  a.ItemID,
            //                  a.Descp,
            //                  b.Seg2Id,
            //                  c.Seg2Name,
            //                  a.Qty,
            //                  a.Rate,
            //                  a.Amount,
            //                  a.TenantId
            //              };

            //var receiptAndIssue = from a in _icledgRepository.GetAll().Where(o => o.TenantId == tenantId)
            //                      join
            //                      b in _icitemRepository.GetAll().Where(o => o.TenantId == tenantId)
            //                      on new { A = a.ItemID, B = a.TenantId } equals new { A = b.ItemId, B = b.TenantId }
            //                      join
            //                      c in _icSegment2Repository.GetAll().Where(o => o.TenantId == tenantId)
            //                      on new { A = b.Seg2Id, B = b.TenantId } equals new { A = c.Seg2Id, B = c.TenantId }
            //                      where (a.ItemID.CompareTo(fromItem) >= 0 && a.ItemID.CompareTo(toItem) <= 0 && Convert.ToDateTime(a.DocDate) >= Convert.ToDateTime(fromDate)
            //                              && Convert.ToDateTime(a.DocDate) <= Convert.ToDateTime(toDate) && a.LocID >= Convert.ToInt32(fromLocId) && a.LocID <=
            //                              Convert.ToInt32(toLocId) && a.TenantId == tenantId)
            //                      select new
            //                      {
            //                          b.Seg2Id,
            //                          b.Descp,
            //                          //Opening = opening.Where(o => o.ItemID == b.ItemId && o.Seg2Id == b.Seg2Id).Count() > 0 ?
            //                          //opening.Where(o => o.ItemID == b.ItemId && o.Seg2Id == b.Seg2Id).FirstOrDefault().Qty : 0,
            //                          Receipt = (a.Qty > 0 ? a.Qty : 0),
            //                          ReceiptRate = (a.Qty > 0 ? a.Rate : 0),
            //                          ReceiptAmount = (a.Qty > 0 ? a.Amount : 0),
            //                          Issue = (a.Qty < 0 ? a.Qty : 0),
            //                          IssueRate = (a.Qty < 0 ? a.Rate : 0),
            //                          IssueAmount = (a.Qty < 0 ? a.Amount : 0),
            //                          b.ItemId,
            //                          b.TenantId,
            //                          Seg2IdDescp = c.Seg2Name
            //                      };

            //var stock = from a in _icSegment2Repository.GetAll().Where(o => o.TenantId == tenantId)
            //            join
            //            b in _icitemRepository.GetAll().Where(o => o.TenantId == tenantId)
            //            on new { A = a.Seg2Id, B = a.TenantId } equals new { A = b.Seg2Id, B = b.TenantId }
            //            join
            //            a1 in _icSegment1Repository.GetAll().Where(o => o.TenantId == tenantId)
            //            on new { A = b.Seg1Id, B = a.TenantId } equals new { A = a1.Seg1ID, B = a1.TenantId }
            //            join
            //            c in receiptAndIssue
            //            on new { A = b.ItemId, B = b.TenantId } equals new { A = c.ItemId, B = c.TenantId } into gj
            //            from subset in gj.DefaultIfEmpty()
            //            join
            //            d in _icledgRepository.GetAll().Where(o => o.TenantId == tenantId)
            //            on new { A = b.ItemId, B = b.TenantId } equals new { A = d.ItemID, B = d.TenantId }
            //            join
            //            e in _locRepository.GetAll().Where(o => o.TenantId == tenantId)
            //            on new { A = Convert.ToInt32(d.LocID), B = d.TenantId } equals new { A = e.LocID, B = e.TenantId }
            //            select new ItemStockSegment2()
            //            {
            //                Seg1Id = b.Seg1Id,
            //                Seg2Id = b.Seg2Id,
            //                Seg1IdDescp = a1.Seg1Name,
            //                ItemDescp = b.Descp,
            //                OpeningQty = opening.Where(o => o.ItemID == b.ItemId && o.Seg2Id == b.Seg2Id).Count() > 0 ?
            //                opening.Where(o => o.ItemID == b.ItemId && o.Seg2Id == b.Seg2Id).FirstOrDefault().Qty : 0,
            //                OpeningRate  = opening.Where(o => o.ItemID == b.ItemId && o.Seg2Id == b.Seg2Id).Count() > 0 ?
            //                opening.Where(o => o.ItemID == b.ItemId && o.Seg2Id == b.Seg2Id).FirstOrDefault().Rate : 0,
            //                OpeningAmount = opening.Where(o => o.ItemID == b.ItemId && o.Seg2Id == b.Seg2Id).Count() > 0 ?
            //                opening.Where(o => o.ItemID == b.ItemId && o.Seg2Id == b.Seg2Id).FirstOrDefault().Amount : 0,
            //                ReceiptQty = !subset.Receipt.HasValue ? 0 : subset.Receipt,
            //                ReceiptRate = !subset.ReceiptRate.HasValue ? 0 : subset.ReceiptRate,
            //                ReceiptAmount = !subset.ReceiptAmount.HasValue ? 0 : subset.ReceiptAmount,
            //                IssueQty = !subset.Issue.HasValue ? 0 : subset.Issue,
            //                IssueRate = !subset.IssueRate.HasValue ? 0 : subset.IssueRate,
            //                IssueAmount = !subset.IssueAmount.HasValue ? 0 : subset.IssueAmount,
            //                ItemId = b.ItemId,
            //                Seg2IdDescp = a.Seg2Name,
            //                Unit = b.StockUnit,
            //                LocId = Convert.ToInt32(d.LocID),
            //                LocDesc = e.LocName
            //            };
            return itemStockSegment2List;
        }

        public List<ItemStockSegment2> GetDataSeg3(int tenantId, string fromLocId, string toLocId, string fromItem, string toItem, string fromDate, string toDate)
        {
            tenantId = tenantId > 0 ? tenantId : (int)AbpSession.TenantId;
            string str = ConfigurationManager.AppSettings["ConnectionString"];
            List<ItemStockSegment2> itemStockSegment2List = new List<ItemStockSegment2>();
            using (SqlConnection cn = new SqlConnection(str))
            {

                using (SqlCommand cmd = new SqlCommand("sp_consolidatedseg3", cn))
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
                            //itemStockSegment2.ItemId = rdr["ItemID"].ToString();
                            itemStockSegment2.OpeningQty = Convert.ToDouble(rdr["opqty"]);
                            itemStockSegment2.ReceiptQty = Convert.ToDouble(rdr["rcqty"]);
                            itemStockSegment2.OpeningAmount = Convert.ToDouble(rdr["opamt"]);
                            itemStockSegment2.ReceiptAmount = Convert.ToDouble(rdr["rcamt"]);
                            itemStockSegment2.IssueQty = Convert.ToDouble(rdr["isqty"]);
                            itemStockSegment2.IssueAmount = Convert.ToDouble(rdr["isamt"]);
                            //  itemStockSegment2.ItemDescp = rdr["descp"].ToString();
                            itemStockSegment2.Unit = rdr["stockUnit"].ToString();
                            itemStockSegment2.Seg2Id = rdr["Seg3id"].ToString();
                            //itemStockSegment2.Seg3Id = rdr["Seg3id"].ToString();
                            itemStockSegment2.Seg2IdDescp = rdr["seg3name"].ToString();
                            itemStockSegment2.LocId = Convert.ToInt32(rdr["LocID"]);
                            itemStockSegment2.LocDesc = rdr["locname"].ToString();
                            itemStockSegment2List.Add(itemStockSegment2);
                        }
                    }
                }
                // cn.Close();
            }
            //var opening = from a in _icledgRepository.GetAll().Where(o => o.TenantId == tenantId)
            //              join
            //              b in _icitemRepository.GetAll().Where(o => o.TenantId == tenantId)
            //              on new { A = a.ItemID, B = a.TenantId } equals new { A = b.ItemId, B = b.TenantId }
            //              join
            //              c in _icSegment2Repository.GetAll().Where(o => o.TenantId == tenantId)
            //              on new { A = b.Seg2Id, B = b.TenantId } equals new { A = c.Seg2Id, B = c.TenantId }
            //              where (a.ItemID.CompareTo(fromItem) >= 0 && a.ItemID.CompareTo(toItem) <= 0 && Convert.ToDateTime(a.DocDate) < Convert.ToDateTime(fromDate)
            //                       && a.LocID >= Convert.ToInt32(fromLocId) && a.LocID <=
            //                      Convert.ToInt32(toLocId) && a.TenantId == tenantId)
            //              select new
            //              {
            //                  a.ItemID,
            //                  a.Descp,
            //                  b.Seg2Id,
            //                  c.Seg2Name,
            //                  a.Qty,
            //                  a.Rate,
            //                  a.Amount,
            //                  a.TenantId
            //              };

            //var receiptAndIssue = from a in _icledgRepository.GetAll().Where(o => o.TenantId == tenantId)
            //                      join
            //                      b in _icitemRepository.GetAll().Where(o => o.TenantId == tenantId)
            //                      on new { A = a.ItemID, B = a.TenantId } equals new { A = b.ItemId, B = b.TenantId }
            //                      join
            //                      c in _icSegment2Repository.GetAll().Where(o => o.TenantId == tenantId)
            //                      on new { A = b.Seg2Id, B = b.TenantId } equals new { A = c.Seg2Id, B = c.TenantId }
            //                      where (a.ItemID.CompareTo(fromItem) >= 0 && a.ItemID.CompareTo(toItem) <= 0 && Convert.ToDateTime(a.DocDate) >= Convert.ToDateTime(fromDate)
            //                              && Convert.ToDateTime(a.DocDate) <= Convert.ToDateTime(toDate) && a.LocID >= Convert.ToInt32(fromLocId) && a.LocID <=
            //                              Convert.ToInt32(toLocId) && a.TenantId == tenantId)
            //                      select new
            //                      {
            //                          b.Seg2Id,
            //                          b.Descp,
            //                          //Opening = opening.Where(o => o.ItemID == b.ItemId && o.Seg2Id == b.Seg2Id).Count() > 0 ?
            //                          //opening.Where(o => o.ItemID == b.ItemId && o.Seg2Id == b.Seg2Id).FirstOrDefault().Qty : 0,
            //                          Receipt = (a.Qty > 0 ? a.Qty : 0),
            //                          ReceiptRate = (a.Qty > 0 ? a.Rate : 0),
            //                          ReceiptAmount = (a.Qty > 0 ? a.Amount : 0),
            //                          Issue = (a.Qty < 0 ? a.Qty : 0),
            //                          IssueRate = (a.Qty < 0 ? a.Rate : 0),
            //                          IssueAmount = (a.Qty < 0 ? a.Amount : 0),
            //                          b.ItemId,
            //                          b.TenantId,
            //                          Seg2IdDescp = c.Seg2Name
            //                      };

            //var stock = from a in _icSegment2Repository.GetAll().Where(o => o.TenantId == tenantId)
            //            join
            //            b in _icitemRepository.GetAll().Where(o => o.TenantId == tenantId)
            //            on new { A = a.Seg2Id, B = a.TenantId } equals new { A = b.Seg2Id, B = b.TenantId }
            //            join
            //            a1 in _icSegment1Repository.GetAll().Where(o => o.TenantId == tenantId)
            //            on new { A = b.Seg1Id, B = a.TenantId } equals new { A = a1.Seg1ID, B = a1.TenantId }
            //            join
            //            c in receiptAndIssue
            //            on new { A = b.ItemId, B = b.TenantId } equals new { A = c.ItemId, B = c.TenantId } into gj
            //            from subset in gj.DefaultIfEmpty()
            //            join
            //            d in _icledgRepository.GetAll().Where(o => o.TenantId == tenantId)
            //            on new { A = b.ItemId, B = b.TenantId } equals new { A = d.ItemID, B = d.TenantId }
            //            join
            //            e in _locRepository.GetAll().Where(o => o.TenantId == tenantId)
            //            on new { A = Convert.ToInt32(d.LocID), B = d.TenantId } equals new { A = e.LocID, B = e.TenantId }
            //            select new ItemStockSegment2()
            //            {
            //                Seg1Id = b.Seg1Id,
            //                Seg2Id = b.Seg2Id,
            //                Seg1IdDescp = a1.Seg1Name,
            //                ItemDescp = b.Descp,
            //                OpeningQty = opening.Where(o => o.ItemID == b.ItemId && o.Seg2Id == b.Seg2Id).Count() > 0 ?
            //                opening.Where(o => o.ItemID == b.ItemId && o.Seg2Id == b.Seg2Id).FirstOrDefault().Qty : 0,
            //                OpeningRate  = opening.Where(o => o.ItemID == b.ItemId && o.Seg2Id == b.Seg2Id).Count() > 0 ?
            //                opening.Where(o => o.ItemID == b.ItemId && o.Seg2Id == b.Seg2Id).FirstOrDefault().Rate : 0,
            //                OpeningAmount = opening.Where(o => o.ItemID == b.ItemId && o.Seg2Id == b.Seg2Id).Count() > 0 ?
            //                opening.Where(o => o.ItemID == b.ItemId && o.Seg2Id == b.Seg2Id).FirstOrDefault().Amount : 0,
            //                ReceiptQty = !subset.Receipt.HasValue ? 0 : subset.Receipt,
            //                ReceiptRate = !subset.ReceiptRate.HasValue ? 0 : subset.ReceiptRate,
            //                ReceiptAmount = !subset.ReceiptAmount.HasValue ? 0 : subset.ReceiptAmount,
            //                IssueQty = !subset.Issue.HasValue ? 0 : subset.Issue,
            //                IssueRate = !subset.IssueRate.HasValue ? 0 : subset.IssueRate,
            //                IssueAmount = !subset.IssueAmount.HasValue ? 0 : subset.IssueAmount,
            //                ItemId = b.ItemId,
            //                Seg2IdDescp = a.Seg2Name,
            //                Unit = b.StockUnit,
            //                LocId = Convert.ToInt32(d.LocID),
            //                LocDesc = e.LocName
            //            };
            return itemStockSegment2List;
        }

    }

}
