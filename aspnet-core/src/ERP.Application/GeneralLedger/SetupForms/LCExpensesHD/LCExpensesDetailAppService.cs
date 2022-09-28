using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.Collections.Extensions;
using System.Collections.Generic;
using ERP.GeneralLedger.SetupForms.LCExpensesHD.Exporting;
using ERP.GeneralLedger.SetupForms.LCExpensesHD.Dtos;

namespace ERP.GeneralLedger.SetupForms.LCExpensesHD
{
    [AbpAuthorize(AppPermissions.Pages_LCExpensesDetail)]
    public class LCExpensesDetailAppService : ERPAppServiceBase, ILCExpensesDetailAppService
    {
        private readonly IRepository<LCExpensesDetail> _lcExpensesDetailRepository;
        private readonly IRepository<LCExpenses.LCExpenses> _lcExpensesRepository;
        private readonly ILCExpensesDetailExcelExporter _lcExpensesDetailExcelExporter;


        public LCExpensesDetailAppService(
            IRepository<LCExpensesDetail> lcExpensesDetailRepository,
            ILCExpensesDetailExcelExporter lcExpensesDetailExcelExporter,
            IRepository<LCExpenses.LCExpenses> lcExpensesRepository)
        {
            _lcExpensesDetailRepository = lcExpensesDetailRepository;
            _lcExpensesDetailExcelExporter = lcExpensesDetailExcelExporter;
            _lcExpensesRepository = lcExpensesRepository;
        }

        public async Task<PagedResultDto<GetLCExpensesDetailForViewDto>> GetAll(GetAllLCExpensesDetailInput input)
        {

            var filteredLCExpensesDetail = _lcExpensesDetailRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ExpDesc.Contains(input.Filter))
                        .WhereIf(input.MinDetIDFilter != null, e => e.DetID >= input.MinDetIDFilter)
                        .WhereIf(input.MaxDetIDFilter != null, e => e.DetID <= input.MaxDetIDFilter)
                        .WhereIf(input.MinLocIDFilter != null, e => e.LocID >= input.MinLocIDFilter)
                        .WhereIf(input.MaxLocIDFilter != null, e => e.LocID <= input.MaxLocIDFilter)
                        .WhereIf(input.MinDocNoFilter != null, e => e.DetID >= input.MinDocNoFilter)
                        .WhereIf(input.MaxDocNoFilter != null, e => e.DetID <= input.MaxDocNoFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ExpDescFilter), e => e.ExpDesc.Trim().ToLower() == input.ExpDescFilter.Trim().ToLower())
                        .WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
                        .WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter);

            var pagedAndFilteredLCExpensesDetail = filteredLCExpensesDetail
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var lcExpensesDetail = from o in pagedAndFilteredLCExpensesDetail
                                   select new GetLCExpensesDetailForViewDto()
                                   {
                                       LCExpensesDetail = new LCExpensesDetailDto
                                       {
                                           DetID = o.DetID,
                                           LocID = o.LocID,
                                           DocNo = o.DocNo,
                                           ExpDesc = o.ExpDesc,
                                           Amount = o.Amount,
                                           Id = o.Id
                                       }
                                   };

            var totalCount = await filteredLCExpensesDetail.CountAsync();

            return new PagedResultDto<GetLCExpensesDetailForViewDto>(
                totalCount,
                await lcExpensesDetail.ToListAsync()
            );
        }


        [AbpAuthorize(AppPermissions.Pages_LCExpensesDetail_Edit)]
        public async Task<GetLCExpensesDetailForEditOutput> GetLCExpensesDetailForEdit(int ID)
        {
            var lcExpensesDetail = await _lcExpensesDetailRepository.GetAllListAsync(x => x.DetID == ID && x.TenantId == AbpSession.TenantId);

            var lcExpensesDetails = from o in lcExpensesDetail
                                    select new CreateOrEditLCExpensesDetailDto
                                    {
                                        DetID = o.DetID,
                                        LocID = o.LocID,
                                        DocNo = o.DocNo,
                                        ExpDesc = o.ExpDesc,
                                        Amount = o.Amount,
                                        Id = o.Id

                                    };


            var output = new GetLCExpensesDetailForEditOutput { LCExpensesDetail = ObjectMapper.Map<ICollection<CreateOrEditLCExpensesDetailDto>>(lcExpensesDetails) };

            return output;
        }

        public async Task CreateOrEdit(ICollection<CreateOrEditLCExpensesDetailDto> input)
        {
            foreach (var item in input)
            {
                if (item.Id == null)
                {
                    await Create(item);
                }
                else
                {
                    await Update(item);
                }
            }
        }

        public async Task<GetLCExpensesDetailForViewDto> GetLCExpensesDetailForView(int id)
        {
            var lcExpensesDetail = await _lcExpensesDetailRepository.GetAsync(id);

            var output = new GetLCExpensesDetailForViewDto { LCExpensesDetail = ObjectMapper.Map<LCExpensesDetailDto>(lcExpensesDetail) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_LCExpensesDetail_Create)]
        protected virtual async Task Create(CreateOrEditLCExpensesDetailDto input)
        {
            LCExpensesDetail LCExpensesDetail = new LCExpensesDetail();
            LCExpensesDetail.DetID = input.DetID;
            LCExpensesDetail.LocID = input.LocID;
            LCExpensesDetail.DocNo = input.DocNo;
            LCExpensesDetail.ExpDesc = input.ExpDesc;
            LCExpensesDetail.Amount = input.Amount;

            //var LCExpensesDetail = ObjectMapper.Map<LCExpensesDetail>(input);

            if (AbpSession.TenantId != null)
            {
                LCExpensesDetail.TenantId = (int)AbpSession.TenantId;
            }


            await _lcExpensesDetailRepository.InsertAsync(LCExpensesDetail);
        }

        [AbpAuthorize(AppPermissions.Pages_LCExpensesDetail_Edit)]
        protected virtual async Task Update(CreateOrEditLCExpensesDetailDto input)
        {
            LCExpensesDetail lcExpensesDetail = new LCExpensesDetail();
            lcExpensesDetail.DetID = input.DetID;
            lcExpensesDetail.LocID = input.LocID;
            lcExpensesDetail.DocNo = input.DocNo;
            lcExpensesDetail.ExpDesc = input.ExpDesc;
            lcExpensesDetail.Amount = input.Amount;
            await _lcExpensesDetailRepository.InsertAsync(lcExpensesDetail);
        }

        [AbpAuthorize(AppPermissions.Pages_LCExpensesDetail_Delete)]
        public async Task Delete(int input)
        {
            await _lcExpensesDetailRepository.DeleteAsync(x => x.DetID == input);
        }

        public async Task<FileDto> GetLCExpensesDetailToExcel(GetAllLCExpensesDetailForExcelInput input)
        {

            var filteredLCExpensesDetail = _lcExpensesDetailRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ExpDesc.Contains(input.Filter))
                        .WhereIf(input.MinDetIDFilter != null, e => e.DetID >= input.MinDetIDFilter)
                        .WhereIf(input.MaxDetIDFilter != null, e => e.DetID <= input.MaxDetIDFilter)
                        .WhereIf(input.MinLocIDFilter != null, e => e.LocID >= input.MinLocIDFilter)
                        .WhereIf(input.MaxLocIDFilter != null, e => e.LocID <= input.MaxLocIDFilter)
                        .WhereIf(input.MinDocNoFilter != null, e => e.DetID >= input.MinDocNoFilter)
                        .WhereIf(input.MaxDocNoFilter != null, e => e.DetID <= input.MaxDocNoFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ExpDescFilter), e => e.ExpDesc.Trim().ToLower() == input.ExpDescFilter.Trim().ToLower())
                        .WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
                        .WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter);

            var query = (from o in filteredLCExpensesDetail
                         select new GetLCExpensesDetailForViewDto()
                         {
                             LCExpensesDetail = new LCExpensesDetailDto
                             {
                                 DetID = o.DetID,
                                 LocID = o.LocID,
                                 DocNo = o.DocNo,
                                 ExpDesc = o.ExpDesc,
                                 Amount = o.Amount,
                                 Id = o.Id
                             }
                         });


            var lcExpensesDetailListDtos = await query.ToListAsync();

            return _lcExpensesDetailExcelExporter.ExportToFile(lcExpensesDetailListDtos);
        }

        public async Task<ListResultDto<CreateOrEditLCExpensesDetailDto>> GetLCExpenses()
        {

            var lcExpenses = _lcExpensesRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId && x.Active==true);

            var lcExpensesDetails = from o in lcExpenses
                                    select new CreateOrEditLCExpensesDetailDto
                                    {
                                        Amount = 0.0,
                                        ExpDesc = o.ExpDesc
                                       };
            return new ListResultDto<CreateOrEditLCExpensesDetailDto>(
                await lcExpensesDetails.ToListAsync()
                );
        }





    }
}