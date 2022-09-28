using Abp.Domain.Repositories;
using ERP.Reports.SupplyChain.Inventory.Dtos;
using ERP.SupplyChain.Inventory;
using ERP.SupplyChain.Inventory.IC_Item;
using ERP.SupplyChain.Inventory.IC_Segment1;
using ERP.SupplyChain.Inventory.IC_Segment2;
using ERP.SupplyChain.Inventory.IC_Segment3;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ERP.Reports.SupplyChain
{
    public class ItemListingAppService : ERPReportAppServiceBase, IItemListingAppService
    {
        private readonly IRepository<ICItem> _itemRepository;
        private readonly IRepository<ICSegment1> _icSegment1Repository;
        private readonly IRepository<ICSegment2> _icSegment2Repository;
        private readonly IRepository<ICSegment3> _icSegment3Repository;
        public ItemListingAppService(IRepository<ICItem> itemRepository,
            IRepository<ICSegment1> icSegment1Repository,
            IRepository<ICSegment2> icSegment2Repository,
            IRepository<ICSegment3> icSegment3Repository)
        {
            _itemRepository = itemRepository;
            _icSegment1Repository = icSegment1Repository;
            _icSegment2Repository = icSegment2Repository;
            _icSegment3Repository = icSegment3Repository;
        }
        public List<ItemListingDto> GetData(string itemType)
        {
            var TenantId = AbpSession.TenantId;
            if (TenantId == null)
            {
                TenantId = AbpSession.TenantId;

            }
            List<int> Itemtyps = new List<int>();
            if (itemType != "")
            {
                Itemtyps = itemType.Split(',').Select(int.Parse).ToList();
            }
            var data = from a in _itemRepository.GetAll().Where(o => o.TenantId == TenantId && Itemtyps.Contains((int)o.ItemType))
                       join 
                       b in _icSegment1Repository.GetAll().Where(o => o.TenantId == TenantId)
                       on new { A = a.Seg1Id , B = a.TenantId } equals new {A = b.Seg1ID , B = b.TenantId}
                        join c in _icSegment2Repository.GetAll().Where(o => o.TenantId == TenantId)
                       on new { A = a.Seg2Id, B = a.TenantId } equals new { A = c.Seg2Id, B = c.TenantId }
                       join
                       d in _icSegment3Repository.GetAll().Where(o => o.TenantId == TenantId)
                       on new { A = a.Seg3Id, B = a.TenantId } equals new { A = d.Seg3Id, B = d.TenantId }
                       select new ItemListingDto()
                       {
                           ItemId = a.ItemId,
                           Descp = a.Descp,
                           Seg1 = a.Seg1Id,
                           Seg2 = a.Seg2Id,
                           Seg3 = a.Seg3Id,
                           Unit = a.StockUnit,
                           Seg1Desc = b.Seg1Name,
                           Seg2Desc = c.Seg2Name,
                           ItemType=a.ItemType,
                           Seg3Desc = d.Seg3Name,
                           CreationDate = a.CreationDate.ToString()
                       };


            return data.ToList();
        }
        public List<AgeingInvoiceListingDto> GetAgeingInvoice(int? TenantId, string fromDate, string fromAcc, int FromsubAcc, int TosubAcc)
        {
            if (TenantId == null)
            {
                TenantId = AbpSession.TenantId;

            }
            TenantId = TenantId == 0 ? AbpSession.TenantId : TenantId;
            SqlCommand cmd;
            string str = ConfigurationManager.AppSettings["ConnectionString"];
            List<AgeingInvoiceListingDto> AgeingDtoList = new List<AgeingInvoiceListingDto>();
            using (SqlConnection cn = new SqlConnection(str))
            {


                cmd = new SqlCommand("sp_glaging_inv", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TENANTID", AbpSession.TenantId);
                cmd.Parameters.AddWithValue("@ASOnDocDate", fromDate);
                cmd.Parameters.AddWithValue("@DebtorCtrlAc", fromAcc);
                cmd.Parameters.AddWithValue("@FROM_SubAcc", FromsubAcc);
                cmd.Parameters.AddWithValue("@TO_SubAcc", TosubAcc);

                cn.Open();
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        AgeingInvoiceListingDto AgeingInvoice = new AgeingInvoiceListingDto()
                        {
                            ACCOUNTID = rdr["ACCOUNTID"] is DBNull ? "" : rdr["ACCOUNTID"].ToString(),
                            ACCOUNTNAME = rdr["AccountName"] is DBNull ? "" : rdr["AccountName"].ToString(),
                            SUBACCID = rdr["subAccid"] is DBNull ? 0 : Convert.ToInt32(rdr["subAccid"]),
                            SUBACCNAME = rdr["SubAccName"] is DBNull ? "" : rdr["SubAccName"].ToString(),
                            INVOICE_DATE = rdr["InvDate"] is DBNull ? DateTime.Now : DateTime.ParseExact(rdr["InvDate"].ToString(), "dd/MM/yyyy", null),
                            InvoiceNo = rdr["InvNo"] is DBNull ? 0 : Convert.ToInt32(rdr["InvNo"]),
                            BAL = rdr["DUE"] is DBNull ? 0 : Convert.ToInt32(rdr["DUE"]),
                            AMOUNT = rdr["AMOUNT"] is DBNull ? 0 : Convert.ToInt32(rdr["AMOUNT"]),
                            A1 = rdr["0-30"] is DBNull ? 0 : Convert.ToInt32(rdr["0-30"]),
                            A2 = rdr["31-60"] is DBNull ? 0 : Convert.ToInt32(rdr["31-60"]),
                            A3 = rdr["61-90"] is DBNull ? 0 : Convert.ToInt32(rdr["61-90"]),
                            A4 = rdr["91-120"] is DBNull ? 0 : Convert.ToInt32(rdr["91-120"]),
                            A5 = rdr["121_ABOVE"] is DBNull ? 0 : Convert.ToInt32(rdr["121_ABOVE"])
                            //BookName = rdr["BookName"] is DBNull ? "" : rdr["BookName"].ToString()
                        };

                        AgeingDtoList.Add(AgeingInvoice);

                    }

                }
            }
            return AgeingDtoList;

        }

        public List<DeleteLogDto> GetDeleteLog(int? TenantId, string FormName)
        {
            if (TenantId == null)
            {
                TenantId = AbpSession.TenantId;

            }
            TenantId = TenantId == 0 ? AbpSession.TenantId : TenantId;
            SqlCommand cmd;
            string str = ConfigurationManager.AppSettings["ConnectionString"];
            List<DeleteLogDto> DeleteLogList = new List<DeleteLogDto>();
            using (SqlConnection cn = new SqlConnection(str))
            {


                cmd = new SqlCommand("sp_LogRep", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TenantId", AbpSession.TenantId);
                cmd.Parameters.AddWithValue("@FormName", FormName);
                //cmd.Parameters.AddWithValue("@DocNo", DocNo);


                cn.Open();
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        DeleteLogDto DeleteLog = new DeleteLogDto()
                        {

                            DocNo = rdr["DocNo"] is DBNull ? 0 : Convert.ToInt32(rdr["DocNo"]),
                            Action = rdr["Action"] is DBNull ? "" : rdr["Action"].ToString(),
                            DeleteDate = rdr["ApprovedDateTime"] is DBNull ? DateTime.Now : Convert.ToDateTime(rdr["ApprovedDateTime"]),
                            DeleteBy = rdr["ApprovedBy"] is DBNull ? "" : rdr["ApprovedBy"].ToString()
                        };

                        DeleteLogList.Add(DeleteLog);

                    }

                }
            }
            return DeleteLogList;

        }
    }
}
