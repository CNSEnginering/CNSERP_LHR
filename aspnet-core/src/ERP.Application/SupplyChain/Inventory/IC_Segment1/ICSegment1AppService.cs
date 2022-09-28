using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using ERP.SupplyChain.Inventory.IC_Segment1.Dto;
using Microsoft.EntityFrameworkCore;
using ERP.Dto;
using Abp.Linq.Extensions;
using System;
using Abp.Authorization;
using ERP.Authorization;
using Abp.Extensions;
using Abp.UI;
using ERP.SupplyChain.Inventory.IC_Segment1.Exporting;

namespace ERP.SupplyChain.Inventory.IC_Segment1
{
    [AbpAuthorize(AppPermissions.Inventory_ICSegment1)]
    public class ICSegmentAppService : ERPAppServiceBase, IICSegment1AppService
    {
        private readonly IRepository<ICSegment1> _icSegment1Repository;
        private readonly IICSegment1ExcelExporter _icSegment1ExcelExporter;
        public ICSegmentAppService(IRepository<ICSegment1> icSegment1Repository, IICSegment1ExcelExporter icSegment1ExcelExporter)
        {
            _icSegment1Repository = icSegment1Repository;
            _icSegment1ExcelExporter = icSegment1ExcelExporter;
        }

        public async Task<PagedResultDto<GetICSegment1ForViewDto>> GetAll(GetAllICSegment1Input input)
        {
            var filteredSegment = _icSegment1Repository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Seg1ID.Contains(input.Filter) || e.Seg1Name.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Seg1IDFilter), e => e.Seg1ID == input.Seg1IDFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Seg1NameFilter), e => e.Seg1Name == input.Seg1NameFilter);

            var pagedAndFilteredSegment = filteredSegment
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var Segments = from o in pagedAndFilteredSegment
                           select new GetICSegment1ForViewDto()
                           {
                               ICSegment1= new ICSegment1Dto
                               {
                                   Id = o.Id,
                                   Seg1ID = o.Seg1ID,
                                   Seg1Name = o.Seg1Name
                               }
                           };
            var totalCount = await filteredSegment.CountAsync();

            return new PagedResultDto<GetICSegment1ForViewDto>(
                totalCount,
                await Segments.ToListAsync()
            );
        }

        [AbpAuthorize(AppPermissions.Inventory_ICSegment1_Edit)]
        public async Task<GetICSegment1ForEditOutput> GetICSegment1ForEdit(EntityDto input)
        {
            var segment1 = await _icSegment1Repository.FirstOrDefaultAsync(input.Id);
            var output = new GetICSegment1ForEditOutput { ICSegment1 = ObjectMapper.Map<CreateOrEditICSegment1Dto>(segment1) };
            return output;
        }

        public async Task<GetICSegment1ForViewDto> GetICSegment1ForView(int id)
        {
            var segment1 = await _icSegment1Repository.FirstOrDefaultAsync(id);

            var output = new GetICSegment1ForViewDto { ICSegment1 = ObjectMapper.Map<ICSegment1Dto>(segment1) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditICSegment1Dto input)
        {
            if (input.flag == false)
            {

                var segment1 = await _icSegment1Repository.FirstOrDefaultAsync(x => x.Seg1ID == input.Seg1ID && x.TenantId == AbpSession.TenantId);

                if (segment1 != null)
                {
                    throw new UserFriendlyException("Ooppps! There is a problem!", "Segment ID " + input.Seg1ID + " already taken....");
                }

                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }
        [AbpAuthorize(AppPermissions.Inventory_ICSegment1_Edit)]
        protected virtual async Task Update(CreateOrEditICSegment1Dto input)
        {
            var segment1 = await _icSegment1Repository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, segment1);
        }
        [AbpAuthorize(AppPermissions.Inventory_ICSegment1_Create)]
        protected virtual async Task Create(CreateOrEditICSegment1Dto input)
        {
            var segment1 = ObjectMapper.Map<ICSegment1>(input);


            if (AbpSession.TenantId != null)
            {
                segment1.TenantId = (int)AbpSession.TenantId;
            }


            await _icSegment1Repository.InsertAsync(segment1);
        }

        [AbpAuthorize(AppPermissions.Inventory_ICSegment1_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _icSegment1Repository.DeleteAsync(input.Id);
        }

        public async Task<PagedResultDto<NameValueDto>> GetICSegment1ForFinder(FindIcSegment1Input input)
        {
            var filteredSegment = _icSegment1Repository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Seg1ID.Contains(input.Filter) || e.Seg1Name.Contains(input.Filter));

            var pagedAndFilteredSegment = filteredSegment
                .OrderBy("id desc")
                .PageBy(input);

            var Segments = from o in pagedAndFilteredSegment
                           select new NameValueDto
                           {
                               Name = o.Seg1Name,
                               Value = o.Seg1ID

                           };
            var totalCount = await filteredSegment.CountAsync();

            return new PagedResultDto<NameValueDto>(
                totalCount,
                await Segments.ToListAsync()
            );
        }
        public async Task<FileDto> GetICSegment1ToExcel(GetAllICSgment1ForExcelInput input)
        {
            var filteredICSegment1s = _icSegment1Repository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Seg1ID.Contains(input.Filter) || e.Seg1Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Seg1IDFilter), e => e.Seg1ID == input.Seg1IDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Seg1NameFilter), e => e.Seg1Name == input.Seg1NameFilter);

            var query = (from o in filteredICSegment1s
                         select new GetICSegment1ForViewDto()
                         {
                             ICSegment1 = new ICSegment1Dto
                             {
                                 Seg1ID = o.Seg1ID,
                                 Seg1Name = o.Seg1Name,
                                 Id = o.Id
                             }
                         });


            var icSegment1ListDtos = await query.ToListAsync();

            return _icSegment1ExcelExporter.ExportToFile(icSegment1ListDtos);
        }

        public string GetSegment1MaxId()
        {
            string maxval = _icSegment1Repository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).OrderByDescending(o => o.Seg1ID).Select(i => i.Seg1ID).FirstOrDefault();
            string maxmumvalue;
            if (Convert.ToInt32(maxval) + 1 > 99)
            {
                maxmumvalue = string.Format("{0:00}", 99);
            }
            else
            {
                maxmumvalue = string.Format("{0:00}", Convert.ToInt32(maxval) + 1);
            }

            return maxmumvalue;

        }
    }
}
