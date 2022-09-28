

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.SupplyChain.Purchase.ReceiptEntry.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ERP.GeneralLedger.SetupForms;

namespace ERP.SupplyChain.Purchase.ReceiptEntry
{
	[AbpAuthorize(AppPermissions.Purchase_ICRECAExps)]
    public class ICRECAExpsAppService : ERPAppServiceBase, IICRECAExpsAppService
    {
		 private readonly IRepository<ICRECAExp> _icrecaExpRepository;
         private readonly IRepository<ChartofControl, string> _chartofControlRepository;


        public ICRECAExpsAppService(IRepository<ICRECAExp> icrecaExpRepository, IRepository<ChartofControl, string> chartofControlRepository) 
		  {
			_icrecaExpRepository = icrecaExpRepository;
            _chartofControlRepository = chartofControlRepository;

          }

		 public async Task<PagedResultDto<ICRECAExpDto>> GetICRECAExpData(int detId)
         {

            var filteredICRECAExps = _icrecaExpRepository.GetAll().Where(e => e.DetID == detId && e.TenantId == AbpSession.TenantId);


            var icrecaExps = from o in filteredICRECAExps
                             select new ICRECAExpDto
							{
                                DetID = o.DetID,
                                LocID = o.LocID,
                                DocNo = o.DocNo,
                                ExpType = o.ExpType,
                                AccountID = o.AccountID,
                                AccountName= _chartofControlRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.Id == o.AccountID).Count() > 0 ? _chartofControlRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.Id == o.AccountID).SingleOrDefault().AccountName : "",
                                Amount = o.Amount,
                                Id = o.Id
						    };

            var totalCount = await filteredICRECAExps.CountAsync();

            return new PagedResultDto<ICRECAExpDto>(
                totalCount,
                await icrecaExps.ToListAsync()
            );
         }
		 
		 [AbpAuthorize(AppPermissions.Purchase_ICRECAExps_Edit)]
		 public async Task<ICRECAExpDto> GetICRECAExpForEdit(EntityDto input)
         {
            var icrecaExp = await _icrecaExpRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = ObjectMapper.Map<ICRECAExpDto>(icrecaExp);
			
            return output;
         }
    }
}