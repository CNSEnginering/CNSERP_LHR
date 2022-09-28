

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRD.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ERP.GeneralLedger.SetupForms;

namespace ERP.GeneralLedger.Transaction.VoucherEntry.GLTRD
{
	[AbpAuthorize(AppPermissions.Pages_GLTRDetails)]
    public class GLTRDetailsAppService : ERPAppServiceBase, IGLTRDetailsAppService
    {
		 private readonly IRepository<GLTRDetail> _gltrDetailRepository;
        private readonly IRepository<ChartofControl, string> _chartofControlRepository;
        private readonly IRepository<AccountSubLedger> _accountSubLedgerRepository;


        public GLTRDetailsAppService(IRepository<GLTRDetail> gltrDetailRepository, IRepository<ChartofControl, string> chartofControlRepository, IRepository<AccountSubLedger> accountSubLedgerRepository) 
		  {
			_gltrDetailRepository = gltrDetailRepository;
            _chartofControlRepository = chartofControlRepository;
            _accountSubLedgerRepository = accountSubLedgerRepository;
		  }

		 public async Task<PagedResultDto<GetGLTRDetailForViewDto>> FilterGLTRDData(int input)
         {

            var filteredGLTRDetails = _gltrDetailRepository.GetAll().Where(e => e.DetID == input);
            var chartAcc = _chartofControlRepository.GetAll().Where(x=>x.TenantId==AbpSession.TenantId);
            var subAcc = _accountSubLedgerRepository.GetAll().Where(x=>x.TenantId==AbpSession.TenantId);

			var gltrDetails = from ca in chartAcc
                              join d in filteredGLTRDetails on ca.Id equals d.AccountID
                              join sl in subAcc on new { X = d.AccountID, Y = (int)d.SubAccID } equals new { X = sl.AccountID, Y = sl.Id } into sb
                              from p in sb.DefaultIfEmpty()
                              select new GetGLTRDetailForViewDto() {
							GLTRDetail = new GLTRDetailDto
							{
                                DetID = d.DetID,
                                AccountID = d.AccountID,
                                AccountDesc= ca.AccountName,
                                SubAccID = d.SubAccID,
                                SubAccDesc= p.SubAccName != null ? p.SubAccName : "",
                                Narration = d.Narration,
                                Amount = d.Amount,
                                ChequeNo = d.ChequeNo,
                                IsAuto=d.IsAuto,
                                LocId=d.LocId,
                                Id = d.Id
							}
						};

            var totalCount = await gltrDetails.CountAsync();

            return new PagedResultDto<GetGLTRDetailForViewDto>(
                totalCount,
                await gltrDetails.ToListAsync()
            );
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_GLTRDetails_Edit)]
		 public async Task<GetGLTRDetailForEditOutput> GetGLTRDetailForEdit(EntityDto input)
         {
            var gltrDetail = await _gltrDetailRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetGLTRDetailForEditOutput {GLTRDetail = ObjectMapper.Map<CreateOrEditGLTRDetailDto>(gltrDetail)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditGLTRDetailDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_GLTRDetails_Create)]
		 protected virtual async Task Create(CreateOrEditGLTRDetailDto input)
         {
            var gltrDetail = ObjectMapper.Map<GLTRDetail>(input);

			
			if (AbpSession.TenantId != null)
			{
				gltrDetail.TenantId = (int) AbpSession.TenantId;
			}
		

            await _gltrDetailRepository.InsertAsync(gltrDetail);
         }

		 [AbpAuthorize(AppPermissions.Pages_GLTRDetails_Edit)]
		 protected virtual async Task Update(CreateOrEditGLTRDetailDto input)
         {
            var gltrDetail = await _gltrDetailRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, gltrDetail);
         }

		 [AbpAuthorize(AppPermissions.Pages_GLTRDetails_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _gltrDetailRepository.DeleteAsync(input.Id);
         }
    }
}