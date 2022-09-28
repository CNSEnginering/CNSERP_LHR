

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.SupplyChain.Inventory.WorkOrder.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ERP.SupplyChain.Inventory.IC_Item;
using ERP.SupplyChain.Sales.SaleQutation;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace ERP.SupplyChain.Inventory.WorkOrder
{
	[AbpAuthorize(AppPermissions.Inventory_ICWODetails)]
    public class ICWODetailsAppService : ERPAppServiceBase, IICWODetailsAppService
    {
		 private readonly IRepository<ICWODetail> _icwoDetailRepository;
        private readonly IRepository<ICItem> _ICItemRepository;
        private readonly IRepository<OEQD> _oeqDRepository;
        private readonly IRepository<SubCostCenter> _subCostCenterRepository;
        public ICWODetailsAppService(IRepository<ICWODetail> icwoDetailRepository, IRepository<OEQD> oeqDRepository, IRepository<ICItem> ICItemRepository,
             IRepository<SubCostCenter> subCostCenterRepository) 
		{
		_icwoDetailRepository = icwoDetailRepository;
        _ICItemRepository = ICItemRepository;
            _oeqDRepository = oeqDRepository;
            _subCostCenterRepository = subCostCenterRepository;
        }
        public async Task<PagedResultDto<ICWODetailDto>> GetOEQHForWorkOrder(string id)
        {
            var DocNo = Convert.ToInt32(id);
            var filteredICADJDetails = _oeqDRepository.GetAll().Where(e => e.DocNo == DocNo && e.TenantId == AbpSession.TenantId);
            
            var icwoDetails = from o in filteredICADJDetails
                              select new ICWODetailDto
                              {
                                  DetID = 0,
                                  DocNo = 0,
                                  ItemID = o.ItemID,
                                  ItemDesc = _ICItemRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.ItemId == o.ItemID).Count() > 0 ? _ICItemRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.ItemId == o.ItemID).SingleOrDefault().Descp : "",
                                  Unit = o.Unit,
                                  Conver = Convert.ToDouble(o.Conver),
                                  Qty = Convert.ToDouble(o.Qty),
                                  Cost = Convert.ToDouble(o.Rate),
                                  Amount = Convert.ToDouble(o.Amount),
                                  Remarks = o.Remarks,
                                  Id = 0
                              };

            var totalCount = await icwoDetails.CountAsync();

            return new PagedResultDto<ICWODetailDto>(
                totalCount,
                await icwoDetails.ToListAsync()
            );
        }

        public string GetOEQHForQty(string id,string DocNo)
        {
            string str = ConfigurationManager.AppSettings["ConnectionString"];
            var qty = "";
            using (SqlConnection cn = new SqlConnection(str))
            {
                SqlCommand cmd = new SqlCommand("spGetPendingQutation", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tenantID", AbpSession.TenantId);
                cmd.Parameters.AddWithValue("@ItemID", id);
                cmd.Parameters.AddWithValue("@DocNo", DocNo);
                SqlDataReader rdr = null;

              
                cn.Open();
                rdr= cmd.ExecuteReader();
                while (rdr.Read())
                {
                    // get the results of each column
                   qty= rdr["Qty"].ToString();
                    //   string field2 = (string)rdr["YourField2"];
                }
                cn.Close();
            }
            return qty;
        }

        public async Task<PagedResultDto<ICWODetailDto>> GetICWODData(int detId, string ccId)
        {

            var filteredICADJDetails = _icwoDetailRepository.GetAll().Where(e => e.DetID == detId && e.TenantId == AbpSession.TenantId);

            var icwoDetails = from o in filteredICADJDetails
                               select new ICWODetailDto
                               {
                                   DetID = o.DetID,
                                   DocNo = o.DocNo,
                                   ItemID = o.ItemID,
                                   ItemDesc = _ICItemRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.ItemId == o.ItemID).Count() > 0 ? _ICItemRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.ItemId == o.ItemID).SingleOrDefault().Descp : "",
                                   Unit = o.Unit,
                                   Conver = o.Conver,
                                   Qty = o.Qty,
                                   Cost = o.Cost,
                                   Amount = o.Amount,
                                   Remarks = o.Remarks,
                                   SubCCID = o.SubCCID,
                                   SubCCName = _subCostCenterRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.SUBCCID == o.SubCCID && e.CCID == ccId).Count() > 0 ? _subCostCenterRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.SUBCCID == o.SubCCID && e.CCID == ccId).SingleOrDefault().SubCCName : "",
                                   Id = o.Id
                               };

            var totalCount = await icwoDetails.CountAsync();

            return new PagedResultDto<ICWODetailDto>(
                totalCount,
                await icwoDetails.ToListAsync()
            );
        }

        [AbpAuthorize(AppPermissions.Inventory_ICWODetails_Edit)]
		 public async Task<ICWODetailDto> GetICWODetailForEdit(EntityDto input)
         {
            var icwoDetail = await _icwoDetailRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = ObjectMapper.Map<ICWODetailDto>(icwoDetail);
			
            return output;
         }

    }
}