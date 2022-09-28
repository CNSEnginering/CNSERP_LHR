

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.SupplyChain.Inventory.ICOPT5.Exporting;
using ERP.SupplyChain.Inventory.ICOPT5.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ERP.SupplyChain.Inventory.ICOPT5
{
    [AbpAuthorize(AppPermissions.Inventory_ICOPT5)]
    public class ICOPT5AppService : ERPAppServiceBase, IICOPT5AppService
    {
        private readonly IRepository<ICOPT5> _icopT5Repository;
        private readonly IICOPT5ExcelExporter _icopT5ExcelExporter;


        public ICOPT5AppService(IRepository<ICOPT5> icopT5Repository, IICOPT5ExcelExporter icopT5ExcelExporter)
        {
            _icopT5Repository = icopT5Repository;
            _icopT5ExcelExporter = icopT5ExcelExporter;

        }

        public async Task<PagedResultDto<GetICOPT5ForViewDto>> GetAll(GetAllICOPT5Input input)
        {

            var filteredICOPT5 = _icopT5Repository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Descp.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinOptIDFilter != null, e => e.OptID >= input.MinOptIDFilter)
                        .WhereIf(input.MaxOptIDFilter != null, e => e.OptID <= input.MaxOptIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescpFilter), e => e.Descp == input.DescpFilter);

            var pagedAndFilteredICOPT5 = filteredICOPT5
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var icopT5 = from o in pagedAndFilteredICOPT5
                         select new GetICOPT5ForViewDto()
                         {
                             ICOPT5 = new ICOPT5Dto
                             {
                                 OptID = o.OptID,
                                 Descp = o.Descp,
                                 Active = o.Active,
                                 CreatedBy = o.CreatedBy,
                                 CreateDate = o.CreateDate,
                                 AudtUser = o.AudtUser,
                                 AudtDate = o.AudtDate,
                                 Id = o.Id
                             }
                         };

            var totalCount = await filteredICOPT5.CountAsync();

            return await NewMethod(icopT5, totalCount);
        }

        private static async Task<PagedResultDto<GetICOPT5ForViewDto>> NewMethod(IQueryable<GetICOPT5ForViewDto> icopT5, int totalCount)
        {
            return new PagedResultDto<GetICOPT5ForViewDto>(
                            totalCount,
                            await icopT5.ToListAsync()
                        );
        }

        public async Task<GetICOPT5ForViewDto> GetICOPT5ForView(int id)
        {
            var icopT5 = await _icopT5Repository.GetAsync(id);

            var output = new GetICOPT5ForViewDto { ICOPT5 = ObjectMapper.Map<ICOPT5Dto>(icopT5) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Inventory_ICOPT5_Edit)]
        public async Task<GetICOPT5ForEditOutput> GetICOPT5ForEdit(EntityDto input)
        {
            var icopT5 = await _icopT5Repository.FirstOrDefaultAsync(input.Id);

            var output = new GetICOPT5ForEditOutput { ICOPT5 = ObjectMapper.Map<CreateOrEditICOPT5Dto>(icopT5) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditICOPT5Dto input)
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

        [AbpAuthorize(AppPermissions.Inventory_ICOPT5_Create)]
        protected virtual async Task Create(CreateOrEditICOPT5Dto input)
        {
            var icopT5 = ObjectMapper.Map<ICOPT5>(input);


            if (AbpSession.TenantId != null)
            {
                icopT5.TenantId = (int)AbpSession.TenantId;
            }

            icopT5.OptID = GetMaxOptId();
            await _icopT5Repository.InsertAsync(icopT5);
        }

        [AbpAuthorize(AppPermissions.Inventory_ICOPT5_Edit)]
        protected virtual async Task Update(CreateOrEditICOPT5Dto input)
        {
            var icopT5 = await _icopT5Repository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, icopT5);
        }

        [AbpAuthorize(AppPermissions.Inventory_ICOPT5_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _icopT5Repository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetICOPT5ToExcel(GetAllICOPT5ForExcelInput input)
        {

            var filteredICOPT5 = _icopT5Repository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Descp.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinOptIDFilter != null, e => e.OptID >= input.MinOptIDFilter)
                        .WhereIf(input.MaxOptIDFilter != null, e => e.OptID <= input.MaxOptIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescpFilter), e => e.Descp == input.DescpFilter);

            var query = (from o in filteredICOPT5
                         select new GetICOPT5ForViewDto()
                         {
                             ICOPT5 = new ICOPT5Dto
                             {
                                 OptID = o.OptID,
                                 Descp = o.Descp,
                                 Active = o.Active,
                                 AudtUser = o.AudtUser,
                                 AudtDate = o.AudtDate,
                                 CreatedBy = o.CreatedBy,
                                 CreateDate = o.CreateDate,
                                 Id = o.Id
                             }
                         });


            var icopT5ListDtos = await query.ToListAsync();

            return _icopT5ExcelExporter.ExportToFile(icopT5ListDtos);
        }

        public int GetMaxOptId()
        {
            var maxid = ((from tab1 in _icopT5Repository.GetAll().Where(o => o.TenantId == AbpSession.TenantId) select (int?)tab1.OptID).Max() ?? 0) + 1;
            return maxid;
        }


    }
}