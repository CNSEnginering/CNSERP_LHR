

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.Runtime.Session;
using System.Data.SqlClient;
using ERP.GeneralLedger.SetupForms.LCExpensesHD.Dtos;
using ERP.GeneralLedger.SetupForms.LCExpensesHD.Exporting;

namespace ERP.GeneralLedger.SetupForms.LCExpensesHD
{
    [AbpAuthorize(AppPermissions.Pages_LCExpensesHeader)]
    public class LCExpensesHeaderAppService : ERPAppServiceBase, ILCExpensesHeaderAppService
    {
        private readonly IRepository<LCExpensesHeader> _lcExpensesHeaderRepository;
        private readonly ILCExpensesHeaderExcelExporter _lcExpensesHeadersExcelExporter;
        private readonly ILCExpensesDetailAppService _lcExpensesDetailRepository;
        private readonly IRepository<ChartofControl, string> _chartofControlRepository;
        private readonly IRepository<GLLocation> _glLocationRepository;




        public LCExpensesHeaderAppService(
            IRepository<LCExpensesHeader> lcExpensesHeaderRepository,
            ILCExpensesHeaderExcelExporter lcExpensesHeadersExcelExporter,
            ILCExpensesDetailAppService lcExpensesDetailRepository,
            IRepository<ChartofControl, string> chartofControlRepository,
            IRepository<GLLocation> glLocationRepository)
        {
            _lcExpensesHeaderRepository = lcExpensesHeaderRepository;
            _lcExpensesHeadersExcelExporter = lcExpensesHeadersExcelExporter;
            _lcExpensesDetailRepository = lcExpensesDetailRepository;
            _chartofControlRepository = chartofControlRepository;
            _glLocationRepository = glLocationRepository;

        }

        public async Task<PagedResultDto<GetLCExpensesHeaderForViewDto>> GetAll(GetAllLCExpensesHeaderInput input)
        {

            var filteredLCExpensesHeaders = _lcExpensesHeaderRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.TypeID.Contains(input.Filter) || e.AccountID.Contains(input.Filter) || e.PayableAccID.Contains(input.Filter) || e.LCNumber.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TypeIDFilter), e => e.TypeID.Trim().ToLower() == input.TypeIDFilter.Trim().ToLower())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AccountIDFilter), e => e.AccountID.Trim().ToLower() == input.AccountIDFilter.Trim().ToLower())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PayableAccIDFilter), e => e.PayableAccID.Trim().ToLower() == input.PayableAccIDFilter.Trim().ToLower())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LCNumberFilter), e => e.LCNumber.Trim().ToLower() == input.LCNumberFilter.Trim().ToLower())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser.Trim().ToLower() == input.AudtUserFilter.Trim().ToLower())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy.Trim().ToLower() == input.CreatedByFilter.Trim().ToLower())
                        .WhereIf(input.MinLocIDFilter != null, e => e.LocID >= input.MinLocIDFilter)
                        .WhereIf(input.MaxLocIDFilter != null, e => e.LocID <= input.MaxLocIDFilter)
                        .WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
                        .WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
                        .WhereIf(input.MinDocDateFilter != null, e => e.DocDate >= input.MinDocDateFilter)
                        .WhereIf(input.MaxDocDateFilter != null, e => e.DocDate <= input.MaxDocDateFilter)
                        .WhereIf(input.MinSubAccIDFilter != null, e => e.SubAccID >= input.MinSubAccIDFilter)
                        .WhereIf(input.MaxSubAccIDFilter != null, e => e.SubAccID <= input.MaxSubAccIDFilter)
                        .WhereIf(input.ActiveFilter > -1, e => Convert.ToInt32(e.Active) == input.ActiveFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.AudtDate <= input.MaxCreateDateFilter);


            var pagedAndFilteredLCExpensesHeaders = filteredLCExpensesHeaders
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var lcExpensesHeaders = from o in pagedAndFilteredLCExpensesHeaders
                                    select new GetLCExpensesHeaderForViewDto()
                                    {
                                        LCExpensesHeader = new LCExpensesHeaderDto
                                        {
                                            LocID = o.LocID,
                                            DocNo = o.DocNo,
                                            DocDate = o.DocDate,
                                            TypeID = o.TypeID,
                                            AccountID = o.AccountID,
                                            SubAccID = o.SubAccID,
                                            PayableAccID = o.PayableAccID,
                                            LCNumber = o.LCNumber,
                                            Active = o.Active,
                                            AudtUser = o.AudtUser,
                                            AudtDate = o.AudtDate,
                                            CreatedBy = o.CreatedBy,
                                            CreateDate = o.CreateDate,
                                            Id = o.Id
                                        }
                                    };

            var totalCount = await filteredLCExpensesHeaders.CountAsync();

            return new PagedResultDto<GetLCExpensesHeaderForViewDto>(
                totalCount,
                await lcExpensesHeaders.ToListAsync()
            );
        }

        [AbpAuthorize(AppPermissions.Pages_LCExpensesHeader_Edit)]
        public async Task<GetLCExpensesHeaderForEditOutput> GetLCExpensesHeaderForEdit(EntityDto input)
        {
            var lcExpensesHeader = await _lcExpensesHeaderRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetLCExpensesHeaderForEditOutput { LCExpensesHeader = ObjectMapper.Map<CreateOrEditLCExpensesHeaderDto>(lcExpensesHeader) };

            var lcExpensesDetail = await _lcExpensesDetailRepository.GetLCExpensesDetailForEdit((int)output.LCExpensesHeader.Id);
            output.LCExpensesHeader.LCExpensesDetail = lcExpensesDetail.LCExpensesDetail;
            return output;
        }

        public async Task CreateOrEdit(CreateOrEditLCExpensesHeaderDto input)
        {

            if (!input.flag)
            {
                await Create(input);

            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_LCExpensesHeader_Create)]
        protected virtual async Task Create(CreateOrEditLCExpensesHeaderDto input)
        {
            var lcExpensesHeader = ObjectMapper.Map<LCExpensesHeader>(input);


            if (AbpSession.TenantId != null)
            {
                lcExpensesHeader.TenantId = (int)AbpSession.TenantId;
            }
            lcExpensesHeader.DocNo = GetMaxID();

            var Hid = await _lcExpensesHeaderRepository.InsertAndGetIdAsync(lcExpensesHeader);
            foreach (var item in input.LCExpensesDetail)
            {
                item.DetID = Hid;
            }

            await _lcExpensesDetailRepository.CreateOrEdit(input.LCExpensesDetail);
        }

        [AbpAuthorize(AppPermissions.Pages_LCExpensesHeader_Edit)]
        protected virtual async Task Update(CreateOrEditLCExpensesHeaderDto input)
        {
            var lcExpensesHeader = await _lcExpensesHeaderRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, lcExpensesHeader);

            foreach (var item in input.LCExpensesDetail)
            {
                item.DetID = (int)input.Id;
            }

            await _lcExpensesDetailRepository.Delete((int)input.Id);
            await _lcExpensesDetailRepository.CreateOrEdit(input.LCExpensesDetail);
        }

        [AbpAuthorize(AppPermissions.Pages_LCExpensesHeader_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _lcExpensesHeaderRepository.DeleteAsync(input.Id);
            await _lcExpensesDetailRepository.Delete(input.Id);
        }

        public async Task<FileDto> GetLCExpensesHeaderToExcel(GetAllLCExpensesHeaderForExcelInput input)
        {

            var filteredLCExpensesHeaders = _lcExpensesHeaderRepository.GetAll().WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.TypeID.Contains(input.Filter) || e.AccountID.Contains(input.Filter) || e.PayableAccID.Contains(input.Filter) || e.LCNumber.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TypeIDFilter), e => e.TypeID.Trim().ToLower() == input.TypeIDFilter.Trim().ToLower())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AccountIDFilter), e => e.AccountID.Trim().ToLower() == input.AccountIDFilter.Trim().ToLower())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PayableAccIDFilter), e => e.PayableAccID.Trim().ToLower() == input.PayableAccIDFilter.Trim().ToLower())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LCNumberFilter), e => e.LCNumber.Trim().ToLower() == input.LCNumberFilter.Trim().ToLower())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser.Trim().ToLower() == input.AudtUserFilter.Trim().ToLower())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy.Trim().ToLower() == input.CreatedByFilter.Trim().ToLower())
                        .WhereIf(input.MinLocIDFilter != null, e => e.LocID >= input.MinLocIDFilter)
                        .WhereIf(input.MaxLocIDFilter != null, e => e.LocID <= input.MaxLocIDFilter)
                        .WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
                        .WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
                        .WhereIf(input.MinDocDateFilter != null, e => e.DocDate >= input.MinDocDateFilter)
                        .WhereIf(input.MaxDocDateFilter != null, e => e.DocDate <= input.MaxDocDateFilter)
                        .WhereIf(input.MinSubAccIDFilter != null, e => e.SubAccID >= input.MinSubAccIDFilter)
                        .WhereIf(input.MaxSubAccIDFilter != null, e => e.SubAccID <= input.MaxSubAccIDFilter)
                        .WhereIf(input.ActiveFilter > -1, e => Convert.ToInt32(e.Active) == input.ActiveFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.AudtDate <= input.MaxCreateDateFilter);

            var query = (from o in filteredLCExpensesHeaders
                         select new GetLCExpensesHeaderForViewDto()
                         {
                             LCExpensesHeader = new LCExpensesHeaderDto
                             {
                                 LocID = o.LocID,
                                 DocNo = o.DocNo,
                                 DocDate = o.DocDate,
                                 TypeID = o.TypeID,
                                 AccountID = o.AccountID,
                                 SubAccID = o.SubAccID,
                                 PayableAccID = o.PayableAccID,
                                 LCNumber = o.LCNumber,
                                 Active = o.Active,
                                 AudtUser = o.AudtUser,
                                 AudtDate = o.AudtDate,
                                 CreatedBy = o.CreatedBy,
                                 CreateDate = o.CreateDate,
                                 Id = o.Id
                             }
                         });


            var lcExpensesHeaderListDtos = await query.ToListAsync();

            return _lcExpensesHeadersExcelExporter.ExportToFile(lcExpensesHeaderListDtos);
        }

        public int GetMaxID()
        {
            var maxid = ((from tab1 in _lcExpensesHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId) select (int?)tab1.DocNo).Max() ?? 0) + 1;
            return maxid;
        }
        public string GetLocationName(int locID)
        {

            string locName = _glLocationRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LocId == locID).Count() > 0 ?
                                       _glLocationRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LocId == locID).SingleOrDefault().LocDesc : "";
            return locName;




        }
        public string GetPayableAccName(string accID)
        {
           
            string accName = _chartofControlRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == accID).Count() > 0 ?
                                       _chartofControlRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == accID).SingleOrDefault().AccountName : "";
            return accName;




        }
    }

    
}