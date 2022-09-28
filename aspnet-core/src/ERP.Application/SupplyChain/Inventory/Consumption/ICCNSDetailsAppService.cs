

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.SupplyChain.Inventory.Consumption.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ERP.SupplyChain.Inventory.IC_Item;
using ERP.SupplyChain.Inventory.WorkOrder;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace ERP.SupplyChain.Inventory.Consumption
{
    [AbpAuthorize(AppPermissions.Inventory_ICCNSDetails)]
    public class ICCNSDetailsAppService : ERPAppServiceBase, IICCNSDetailsAppService
    {
        private readonly IRepository<ICCNSHeader> _iccnsHeaderRepository;
        private readonly IRepository<ICCNSDetail> _iccnsDetailRepository;
        private readonly IRepository<SubCostCenter> _subCostCenterRepository;
        private readonly IRepository<ICItem> _ICItemRepository;
        private readonly IRepository<ICWODetail> _icwoDetailRepository;
        private ConsumptionAppService _consumptionAppService;
        private readonly IRepository<ICLEDG> _icledgRepository;

        public ICCNSDetailsAppService(IRepository<ICCNSDetail> iccnsDetailRepository, IRepository<SubCostCenter> subCostCenterRepository,
            IRepository<ICItem> ICItemRepository, IRepository<ICWODetail> icwoDetailRepository,
            IRepository<ICCNSHeader> iccnsHeaderRepository,
            ConsumptionAppService consumptionAppService,
            IRepository<ICLEDG> icledgRepository
            )
        {
            _iccnsDetailRepository = iccnsDetailRepository;
            _subCostCenterRepository = subCostCenterRepository;
            _ICItemRepository = ICItemRepository;
            _icwoDetailRepository = icwoDetailRepository;
            _consumptionAppService = consumptionAppService;
            _iccnsHeaderRepository = iccnsHeaderRepository;
            _icledgRepository = icledgRepository;
        }

        public async Task<PagedResultDto<ICCNSDetailDto>> GetICCNSDData(int detId, string ccId)
        {

            var filteredICADJDetails = _iccnsDetailRepository.GetAll().Where(e => e.DocNo == detId && e.TenantId == AbpSession.TenantId);
            var subCost = _subCostCenterRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId);
            var locId = _iccnsHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.DocNo == detId).Count() > 0
                ? _iccnsHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.DocNo == detId).FirstOrDefault().LocID : 0;

            //var qtyInHand =
            //    string.IsNullOrEmpty(_iccnsHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == detId).First().OrdNo) ?
            //    _consumptionAppService.GetQtyInHand(filteredICADJDetails.FirstOrDefault().ItemID, locId, filteredICADJDetails.FirstOrDefault().DocNo)
            //    : GetWorkOrdQty(Convert.ToInt32(_iccnsHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == detId).First().OrdNo));

            var DocDate = _iccnsHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.DocNo == detId).Count() > 0
                ? _iccnsHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.DocNo == detId).FirstOrDefault().DocDate : DateTime.Now;

            var a = _icledgRepository.GetAll().Where(w => w.TenantId == AbpSession.TenantId && w.ItemID == "15-005-13-0007" && w.LocID == 1 && w.DocDate <= DocDate).Select(y => y.Qty).Sum();


            var icadjDetails = from o in filteredICADJDetails
                                   //join sc in subCost on o.SubCCID equals sc.SUBCCID into sb
                                   // from scc in sb.DefaultIfEmpty()
                               select new ICCNSDetailDto
                               {
                                   DetID = o.DetID,
                                   DocNo = o.DocNo,
                                   ItemID = o.ItemID,
                                   ItemDesc = _ICItemRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.ItemId == o.ItemID).Count() > 0 ? _ICItemRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.ItemId == o.ItemID).SingleOrDefault().Descp : "",
                                   Unit = o.Unit,
                                   Conver = o.Conver,
                                   QtyInHand = _icledgRepository.GetAll().Where(w => w.TenantId == AbpSession.TenantId && w.ItemID == o.ItemID && w.LocID == locId && w.DocDate <= DocDate).Select(x => x.Qty).Sum() == null ? 0 + o.Qty : _icledgRepository.GetAll().Where(w => w.TenantId == AbpSession.TenantId && w.ItemID == o.ItemID && w.LocID == locId && w.DocDate <= DocDate).Select(x => x.Qty).Sum() + o.Qty, //qtyInHand, //+ o.Qty,
                                   Qty = o.Qty,
                                   Cost = o.Cost,
                                   Amount = o.Amount,
                                   Remarks = o.Remarks != null ? o.Remarks : "",
                                   EngNo = o.EngNo != null ? o.EngNo : "",
                                   SubCCID = o.SubCCID,
                                   SubCCName = _subCostCenterRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.SUBCCID == o.SubCCID && e.CCID == ccId).Count() > 0 ? _subCostCenterRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.SUBCCID == o.SubCCID && e.CCID == ccId).SingleOrDefault().SubCCName : "",
                                   Id = o.Id
                               };

            var totalCount = await icadjDetails.CountAsync();

            return new PagedResultDto<ICCNSDetailDto>(
                totalCount,
                await icadjDetails.ToListAsync()
            );
        }


        public double? GetWorkOrdQty(int orderNo)
        {

            var filteredICADJDetails = _icwoDetailRepository.GetAll().Where(e => e.DocNo == orderNo && e.TenantId == AbpSession.TenantId);

            //var consumptionQty = (from a in _iccnsHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
            //                      join
            //                      b in _iccnsDetailRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
            //                      on new { A = a.Id, B = a.TenantId } equals new { A = b.DetID, B = b.TenantId }
            //                      where (a.OrdNo == orderNo.ToString())
            //                      select (b.Qty)
            //                      ).Sum();

            var icwoDetails = from o in filteredICADJDetails
                              select new ICCNSDetailDto
                              {
                                  QtyInHand = (o.Qty) - (
                                  (from a in _iccnsHeaderRepository.GetAll().Where(p => p.TenantId == AbpSession.TenantId)
                                   join
                                   b in _iccnsDetailRepository.GetAll().Where(p => p.TenantId == AbpSession.TenantId)
                                   on new { A = a.Id, B = a.TenantId } equals new { A = b.DetID, B = b.TenantId }
                                   where (a.OrdNo == orderNo.ToString()
                                   &&
                                   b.ItemID == o.ItemID
                                   )
                                   select (b.Qty)
                                  ).Sum()
                                  )
                              };

            return icwoDetails.Count() > 0 ? icwoDetails.FirstOrDefault().QtyInHand : 0;
        }

        public async Task<PagedResultDto<ICCNSDetailDto>> GetICWODData(int orderNo, int locId)
        {
            var headerData = _iccnsHeaderRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId && x.DocNo == orderNo).FirstOrDefault();
            var filteredICADJDetails = _icwoDetailRepository.GetAll().Where(e => e.DocNo == orderNo && e.TenantId == AbpSession.TenantId);

            //var consumptionQty = (from a in _iccnsHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
            //                      join
            //                      b in _iccnsDetailRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
            //                      on new { A = a.Id, B = a.TenantId } equals new { A = b.DetID, B = b.TenantId }
            //                      where (a.OrdNo == orderNo.ToString())
            //                      select (b.Qty)
            //                      ).Sum();

            var icwoDetails = from o in filteredICADJDetails
                              select new ICCNSDetailDto
                              {
                                  //DetID = o.DetID,
                                  DocNo = o.DocNo.Value,
                                  ItemID = o.ItemID,
                                  ItemDesc = _ICItemRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.ItemId == o.ItemID).Count() > 0 ? _ICItemRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.ItemId == o.ItemID).SingleOrDefault().Descp : "",
                                  Unit = o.Unit,
                                  Conver = o.Conver,
                                  SubCCID = o.SubCCID,
                                  SubCCName = _subCostCenterRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.SUBCCID == o.SubCCID && e.CCID == headerData.CCID).Count() > 0 ? _subCostCenterRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.SUBCCID == o.SubCCID && e.CCID == headerData.CCID).SingleOrDefault().SubCCName : "",
                                  //QtyInHand = (o.Qty) - (
                                  //(from a in _iccnsHeaderRepository.GetAll().Where(p => p.TenantId == AbpSession.TenantId)
                                  // join
                                  // b in _iccnsDetailRepository.GetAll().Where(p => p.TenantId == AbpSession.TenantId)
                                  // on new { A = a.Id, B = a.TenantId } equals new { A = b.DetID, B = b.TenantId }
                                  // where (a.OrdNo == orderNo.ToString()
                                  // &&
                                  // b.ItemID == o.ItemID)
                                  // select (b.Qty)
                                  //).Sum() 
                                  //),
                                  Qty = o.Qty,
                                  Cost = 0,
                                  Amount = 0,
                                  Remarks = o.Remarks,
                                  //Id = o.Id
                              };

            var totalCount = await icwoDetails.CountAsync();
            var icwoDList = await icwoDetails.ToListAsync();

            foreach (var item in icwoDList)
            {
                item.QtyInHand = GetQtyInHandAdj(item.ItemID, locId);

            }

            return new PagedResultDto<ICCNSDetailDto>(
                totalCount,
                icwoDList
            );
        }

        [AbpAuthorize(AppPermissions.Inventory_ICCNSDetails_Edit)]
        public async Task<ICCNSDetailDto> GetICCNSDetailForEdit(EntityDto input)
        {
            var iccnsDetail = await _iccnsDetailRepository.FirstOrDefaultAsync(input.Id);

            var output = ObjectMapper.Map<ICCNSDetailDto>(iccnsDetail);

            return output;
        }

        public double GetQtyInHandAdj(string itemId, int locId)
        {
            string str = ConfigurationManager.AppSettings["ConnectionString"];
            double qty = 0;
            using (SqlConnection cn = new SqlConnection(str))
            {
                using (SqlCommand cmd = new SqlCommand("dbo.sp_GetItemStock", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@itemId", itemId);
                    cmd.Parameters.AddWithValue("@locid", locId);
                    cmd.Parameters.AddWithValue("@TenantID", AbpSession.TenantId);
                    cmd.Parameters.AddWithValue("@date", DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"));
                    cn.Open();
                    using (SqlDataReader rowData = cmd.ExecuteReader())
                    {

                        while (rowData.Read())
                        {
                            qty = Convert.ToDouble(rowData["qty"]);
                        }
                    }
                }
                // cn.Close();
            }
            return qty;
        }

        public double GetQtyInHand(string itemId, int locId, DateTime? docDate)
        {
            string str = ConfigurationManager.AppSettings["ConnectionString"];
            double qty = 0;
            using (SqlConnection cn = new SqlConnection(str))
            {
                using (SqlCommand cmd = new SqlCommand("dbo.sp_GetItemStock", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@itemId", itemId);
                    cmd.Parameters.AddWithValue("@locid", locId);
                    cmd.Parameters.AddWithValue("@TenantID", AbpSession.TenantId);
                    //cmd.Parameters.AddWithValue("@date", DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@date", docDate.Value.ToString("yyyy-MM-dd"));
                    cn.Open();
                    using (SqlDataReader rowData = cmd.ExecuteReader())
                    {

                        while (rowData.Read())
                        {
                            qty = Convert.ToDouble(rowData["qty"]);
                        }
                    }
                }
                // cn.Close();
            }
            return qty;
        }

    }
}