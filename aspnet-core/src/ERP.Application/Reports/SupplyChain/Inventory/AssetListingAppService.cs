using Abp.Domain.Repositories;
using Abp.Collections.Extensions;
using ERP.GeneralLedger.SetupForms;
using ERP.Reports.SupplyChain.Inventory.Dtos;
using ERP.SupplyChain.Inventory;
using ERP.SupplyChain.Inventory.AssetRegistration;
using ERP.SupplyChain.Inventory.IC_Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Runtime.Session;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace ERP.Reports.SupplyChain.Inventory
{
   public class AssetListingAppService : ERPReportAppServiceBase
    {
        private readonly IRepository<AssetRegistration> _assetRegRepository;
        private readonly IRepository<ICLocation> _icLocationRepository;
        private readonly IRepository<ICItem> _itemRepository;
        private readonly IRepository<GLSecChartofControl, string> _glSecChartofControlRepository;
        public AssetListingAppService(IRepository<AssetRegistration> assetRegRepository,
            IRepository<ICLocation> icLocationRepository,
            IRepository<ICItem> itemRepository, IRepository<GLSecChartofControl, string> glSecChartofControlRepository)
        {
            _assetRegRepository = assetRegRepository;
            _icLocationRepository = icLocationRepository;
            _itemRepository = itemRepository;
            _glSecChartofControlRepository = glSecChartofControlRepository;
        }
        public List<Assetdetail> GetData(int TenantId, string fromLocId, string toLocId, string fromDate, string toDate)
        {
            List<int> LocIds = new List<int>();
            if (fromLocId != "")
            {
                LocIds = fromLocId.Split(',').Select(int.Parse).ToList();
            }

            if (TenantId ==0)
            {
                TenantId = (int)AbpSession.TenantId;

            }
            //var FlocId = int.Parse(fromLocId);
            //var TlocId = int.Parse(toLocId);
            var assetsReg = _assetRegRepository.GetAll().Where(o => o.TenantId == TenantId && LocIds.Contains((int)o.LocID) && o.RegDate>= Convert.ToDateTime(fromDate) && o.RegDate<=Convert.ToDateTime(toDate));
            var locations = _icLocationRepository.GetAll().Where(o => o.TenantId == TenantId);
            var items = _itemRepository.GetAll().Where(o => o.TenantId == TenantId);
            var accounts = _glSecChartofControlRepository.GetAll().Where(o => o.TenantId == TenantId);
            var data = from a in assetsReg
                       join b in locations on a.LocID equals b.LocID
                       join c in items on a.ItemID equals c.ItemId
                       join d in accounts on a.AccAsset equals d.Id
                       join e in accounts on a.AccDepr equals e.Id
                       join f in accounts on a.AccExp equals f.Id
                       select new Assetdetail()
                       {
                           AssetID = a.FmtAssetID,
                           AssetName = a.AssetName,
                           RegDate = a.RegDate == null ? a.RegDate.ToString() : a.RegDate.Value.ToString("dd/MM/yyyy"),
                           AssetType = a.AssetType == 1 ? "Computer Equipment" : a.AssetType == 2 ? "Office Equipment" : a.AssetType == 3 ? "Plant/Machine" : "",
                           LocID = a.LocID.ToString(),
                           Location = b.LocName,
                           Warranty = a.Warranty == true ? "Yes" : "No",
                           ExpiryDate = a.ExpiryDate == null ? a.ExpiryDate.ToString() : a.ExpiryDate.Value.ToString("dd/MM/yyyy"),
                           Insurance = a.Insurance == true ? "Yes" : "No",
                           Finance = a.Finance == true ? "Yes" : "No",
                           PurchaseDate = a.PurchaseDate == null ? a.PurchaseDate.ToString() : a.PurchaseDate.Value.ToString("dd/MM/yyyy"),
                           PurchasePrice = a.PurchasePrice.ToString(),
                           SerialNo = a.SerialNumber,
                           ItemID = a.ItemID,
                           Item = c.Descp,
                           AssetAccID = a.AccAsset,
                           AssetAccName = d.AccountName,
                           AccDeprID = a.AccDepr,
                           AccDeprName = e.AccountName,
                           DepExpAccID = a.AccExp,
                           DepExpAccName = f.AccountName,
                           DepRate = a.DepRate.ToString(),
                           Narration = a.Narration,
                           DepStartDate = a.DepStartDate == null ? a.DepStartDate.ToString() : a.DepStartDate.Value.ToString("dd/MM/yyyy"),
                           DepMethod = a.DepMethod == 1 ? "Straight Line Methods" : a.DepMethod == 2 ? "Declining Methods" : "",
                           EffectiveLife = a.AssetLife.ToString(),
                           BookValue = a.BookValue.ToString() + " Years",
                           LastDepAmount = a.LastDepAmount.ToString(),
                           LastDepDate = a.LastDepDate == null ? a.LastDepDate.ToString() : a.LastDepDate.Value.ToString("dd/MM/yyyy"),
                           AccumolatedDep = a.AccumulatedDepAmt.ToString(),
                       };
            return data.ToList();
        }

        public List<itemStatus> GetItemData(string fromDate, string toDate,string fromLocId, string toLocId, string fromitem, string toitem,string fromseg,string toseg)
        {
            
               var TenantId = (int)AbpSession.TenantId;

            string str = ConfigurationManager.AppSettings["ConnectionString"];
            List<itemStatus> ItemList = new List<itemStatus>();
            using (SqlConnection cn = new SqlConnection(str))
            {


                SqlCommand cmd = new SqlCommand("sp_ItemStatus", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                var GrandAmt1 = 0.0;
                cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);
                cmd.Parameters.AddWithValue("@fromDate", fromDate);
                cmd.Parameters.AddWithValue("@toDate", toDate);
                cmd.Parameters.AddWithValue("@fromLoc", fromLocId);
                cmd.Parameters.AddWithValue("@toLoc", toLocId);
                cmd.Parameters.AddWithValue("@fromItem", fromitem);
                cmd.Parameters.AddWithValue("@toItem", toitem);
                cmd.Parameters.AddWithValue("@fromSeg", fromseg);
                cmd.Parameters.AddWithValue("@toseg", toseg);
                cn.Open();
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        itemStatus item = new itemStatus()
                        {
                            dates = rdr["DDate"] is DBNull ? "" : rdr["DDate"].ToString(),
                            Seg3id = rdr["Seg3Id"] is DBNull ? "" : rdr["Seg3Id"].ToString(),
                            Seg3Name = rdr["Seg3Name"] is DBNull ? "" : rdr["Seg3Name"].ToString(),
                            LocId = rdr["LocID"] is DBNull ? "" : rdr["LocID"].ToString(),
                            LocName = rdr["LocName"] is DBNull ? "" : rdr["LocName"].ToString(),
                            ItemID = rdr["ItemId"] is DBNull ? "" :rdr["ItemId"].ToString(),
                            ItemName = rdr["Descp"] is DBNull ? "" :rdr["Descp"].ToString(),
                            Status=rdr["optName"] is DBNull ? "":rdr["optName"].ToString(),
                            Unit = rdr["Unit"] is DBNull ? "" : rdr["Unit"].ToString(),
                        };

                        ItemList.Add(item);

                    }

                }
            }

            return ItemList.ToList();
        }
    }
    public class itemStatus
    {
        public string  Seg3Name { get; set; }
        public string  Seg3id { get; set; }
        public string  LocId { get; set; }
        public string  LocName { get; set; }
        public string  ItemID { get; set; }
        public string  ItemName { get; set; }
        public string  Status { get; set; }
        public string  dates { get; set; }
        public string Unit { get; set; }
    }
}
