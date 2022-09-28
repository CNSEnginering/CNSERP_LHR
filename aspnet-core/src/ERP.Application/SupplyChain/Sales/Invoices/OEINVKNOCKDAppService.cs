using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.SupplyChain.Sales.Invoices.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using ERP.Storage;

namespace ERP.SupplyChain.Sales.Invoices
{
    [AbpAuthorize(AppPermissions.Pages_OEINVKNOCKD)]
    public class OEINVKNOCKDAppService : ERPAppServiceBase, IOEINVKNOCKDAppService
    {
        private readonly IRepository<OEINVKNOCKD> _oeinvknockdRepository;

        public OEINVKNOCKDAppService(IRepository<OEINVKNOCKD> oeinvknockdRepository)
        {
            _oeinvknockdRepository = oeinvknockdRepository;

        }

        public async Task<PagedResultDto<GetOEINVKNOCKDForViewDto>> GetAll(GetAllOEINVKNOCKDInput input)
        {

            var filteredOEINVKNOCKD = _oeinvknockdRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.InvDate.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter) || e.AudtUser.Contains(input.Filter))
                        .WhereIf(input.MinDetIDFilter != null, e => e.DetID >= input.MinDetIDFilter)
                        .WhereIf(input.MaxDetIDFilter != null, e => e.DetID <= input.MaxDetIDFilter)
                        .WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
                        .WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
                        .WhereIf(input.MinInvNoFilter != null, e => e.InvNo >= input.MinInvNoFilter)
                        .WhereIf(input.MaxInvNoFilter != null, e => e.InvNo <= input.MaxInvNoFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.InvDateFilter), e => e.InvDate == input.InvDateFilter)
                        .WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
                        .WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
                        .WhereIf(input.MinAlreadyPaidFilter != null, e => e.AlreadyPaid >= input.MinAlreadyPaidFilter)
                        .WhereIf(input.MaxAlreadyPaidFilter != null, e => e.AlreadyPaid <= input.MaxAlreadyPaidFilter)
                        .WhereIf(input.MinPendingFilter != null, e => e.Pending >= input.MinPendingFilter)
                        .WhereIf(input.MaxPendingFilter != null, e => e.Pending <= input.MaxPendingFilter)
                        .WhereIf(input.MinAdjustFilter != null, e => e.Adjust >= input.MinAdjustFilter)
                        .WhereIf(input.MaxAdjustFilter != null, e => e.Adjust <= input.MaxAdjustFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreatedDateFilter != null, e => e.CreatedDate >= input.MinCreatedDateFilter)
                        .WhereIf(input.MaxCreatedDateFilter != null, e => e.CreatedDate <= input.MaxCreatedDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter);

            var pagedAndFilteredOEINVKNOCKD = filteredOEINVKNOCKD
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var oeinvknockd = from o in pagedAndFilteredOEINVKNOCKD
                              select new
                              {

                                  o.DetID,
                                  o.DocNo,
                                  o.InvNo,
                                  o.InvDate,
                                  o.Amount,
                                  o.AlreadyPaid,
                                  o.Pending,
                                  o.Adjust,
                                  o.CreatedBy,
                                  o.CreatedDate,
                                  o.AudtUser,
                                  o.AudtDate,
                                  Id = o.Id
                              };

            var totalCount = await filteredOEINVKNOCKD.CountAsync();

            var dbList = await oeinvknockd.ToListAsync();
            var results = new List<GetOEINVKNOCKDForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetOEINVKNOCKDForViewDto()
                {
                    OEINVKNOCKD = new OEINVKNOCKDDto
                    {

                        DetID = o.DetID,
                        DocNo = o.DocNo,
                        InvNo = o.InvNo,
                        InvDate = o.InvDate,
                        Amount = o.Amount,
                        AlreadyPaid = o.AlreadyPaid,
                        Pending = o.Pending,
                        Adjust = o.Adjust,
                        CreatedBy = o.CreatedBy,
                        CreatedDate = o.CreatedDate,
                        AudtUser = o.AudtUser,
                        AudtDate = o.AudtDate,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetOEINVKNOCKDForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetOEINVKNOCKDForViewDto> GetOEINVKNOCKDForView(int id)
        {
            var oeinvknockd = await _oeinvknockdRepository.GetAsync(id);

            var output = new GetOEINVKNOCKDForViewDto { OEINVKNOCKD = ObjectMapper.Map<OEINVKNOCKDDto>(oeinvknockd) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_OEINVKNOCKD_Edit)]
        public async Task<GetOEINVKNOCKDForEditOutput> GetOEINVKNOCKDForEdit(EntityDto input)
        {
            var oeinvknockd = await _oeinvknockdRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetOEINVKNOCKDForEditOutput { OEINVKNOCKD = ObjectMapper.Map<CreateOrEditOEINVKNOCKDDto>(oeinvknockd) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditOEINVKNOCKDDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_OEINVKNOCKD_Create)]
        protected virtual async Task Create(CreateOrEditOEINVKNOCKDDto input)
        {
            var oeinvknockd = ObjectMapper.Map<OEINVKNOCKD>(input);

            if (AbpSession.TenantId != null)
            {
                oeinvknockd.TenantId = (int)AbpSession.TenantId;
            }

            await _oeinvknockdRepository.InsertAsync(oeinvknockd);

        }

        [AbpAuthorize(AppPermissions.Pages_OEINVKNOCKD_Edit)]
        protected virtual async Task Update(CreateOrEditOEINVKNOCKDDto input)
        {
            var oeinvknockd = await _oeinvknockdRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, oeinvknockd);

        }

        [AbpAuthorize(AppPermissions.Pages_OEINVKNOCKD_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _oeinvknockdRepository.DeleteAsync(input.Id);
        }

    }
}