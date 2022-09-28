using Abp.Domain.Repositories;
using ERP.SupplyChain.Inventory;
using ERP.SupplyChain.Inventory.IC_Item;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ERP.Reports.SupplyChain.Inventory
{
    public class StockAssemblyAppService : ERPReportAppServiceBase , IStockAssemblyAppService
    {
        private IRepository<Assembly> _assemblyHeadRepository;
        private IRepository<AssemblyDetails> _assemblyDetailsRepository;
        private IRepository<ICItem> _itemRepository;
        public StockAssemblyAppService(IRepository<Assembly> assemblyHeadRepository,
            IRepository<AssemblyDetails> assemblyDetailsRepository,
            IRepository<ICItem> itemRepository
            )
        {
            _assemblyHeadRepository = assemblyHeadRepository;
            _assemblyDetailsRepository = assemblyDetailsRepository;
            _itemRepository = itemRepository;
        }
        public List<AssemblyStock> GetData(int tenantId, string fromLocId, string toLocId,
            string fromItem, string toItem, string fromDate, string toDate)
        {
            List<int> LocIds = new List<int>();
            if (fromLocId != "")
            {
                LocIds = fromLocId.Split(',').Select(int.Parse).ToList();
            }
            tenantId = tenantId > 0 ? tenantId : (int)AbpSession.TenantId;
            var data = from a in _assemblyHeadRepository.GetAll().Where(o => o.TenantId == tenantId)
                       join
                       b in _assemblyDetailsRepository.GetAll().Where(o => o.TenantId == tenantId)
                       on new { A = a.TenantId, B = a.Id } equals new { A = b.TenantId, B = b.DetID }
                       join c in _itemRepository.GetAll().Where(o => o.TenantId == tenantId)
                       on new { A = b.TenantId, B = b.ItemID } equals new { A = c.TenantId, B = c.ItemId }
                       where (b.ItemID.CompareTo(fromItem) >= 0 && b.ItemID.CompareTo(toItem) <= 0 && a.TenantId == tenantId &&
                            LocIds.Contains((int)a.LocID)
                                && a.DocDate >= Convert.ToDateTime(fromDate) && a.DocDate <= Convert.ToDateTime(toDate))
                       orderby b.TransType descending
                       select new AssemblyStock()
                       {
                           ItemId = b.ItemID,
                           ItemName = c.Descp,
                           Convr = b.Conver,
                           Narration = a.Narration,
                           Qty = b.Qty,
                           TransType = b.TransType == 7 ? "Product" : "Raw material",
                           Unit = b.Unit,
                           DocNo = a.DocNo
                       };
            return data.ToList();
        }

        public List<AssemblyStockCost> GetDataForCost(int tenantId, string OrderNo)
        {
            tenantId = tenantId > 0 ? tenantId : (int)AbpSession.TenantId;
            tenantId = tenantId > 0 ? tenantId : (int)AbpSession.TenantId;
            string str = ConfigurationManager.AppSettings["ConnectionString"];
            List<AssemblyStockCost> itemStockAssemblyCostList = new List<AssemblyStockCost>();
            using (SqlConnection cn = new SqlConnection(str))
            {
                using (SqlCommand cmd = new SqlCommand("ORDERCOST", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ORDERNO", OrderNo);
                    cmd.Parameters.AddWithValue("@TenantID", tenantId);
                   
                    cn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            AssemblyStockCost itemStockCost = new AssemblyStockCost();
                            itemStockCost.DocDate = rdr["DocDate"].ToString();
                            itemStockCost.LocID = rdr["LocID"].ToString();
                            itemStockCost.DocNo =Convert.ToInt32(rdr["DocNo"].ToString());
                            itemStockCost.Ordno = rdr["OrdNo"].ToString();
                            itemStockCost.Overhead = rdr["OverHead"].ToString();
                            itemStockCost.ItemId = rdr["ItemID"].ToString();
                            itemStockCost.ItemName = rdr["Descp"].ToString();
                            itemStockCost.Unit = rdr["Unit"].ToString();
                            itemStockCost.Qty =Convert.ToDecimal(rdr["Qty"].ToString());
                            itemStockCost.Amount = Convert.ToDecimal(rdr["Amount"].ToString());
                            itemStockCost.TransType = rdr["TransType"].ToString();
                            itemStockAssemblyCostList.Add(itemStockCost);
                        }
                    }
                }
                // cn.Close();
            }
            
            return itemStockAssemblyCostList;
        }


    }


}
