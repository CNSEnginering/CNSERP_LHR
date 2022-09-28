using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using ERP.Dto;
using ERP.SupplyChain.Inventory.IC_Segment1;
using ERP.SupplyChain.Inventory.IC_Segment1.Dto;
using ERP.SupplyChain.Inventory.IC_Segment2;
using ERP.SupplyChain.Inventory.IC_Segment3.Dto;
using ERP.SupplyChain.Inventory.IC_Segment3.Exporting;
using Microsoft.EntityFrameworkCore;

namespace ERP.SupplyChain.Inventory.IC_Segment3
{
    public class ICSegment3AppService : ERPAppServiceBase, IICSegment3AppService
    {

        private readonly IRepository<ICSegment3> _ICSegment3Repository;
        private readonly IRepository<ICSegment1> _ICSegment1Repository;
        private readonly IRepository<ICSegment2> _ICSegment2Repository;
        private readonly IICSegment3ExcelExporter _icSegment3ExcelExporter;
        public ICSegment3AppService(IRepository<ICSegment3> ICSegment3Repository, IRepository<ICSegment1> ICSegment1Repository, IRepository<ICSegment2> ICSegment2Repository,  IICSegment3ExcelExporter icSegment3ExcelExporter) {
            _ICSegment3Repository = ICSegment3Repository;
            _ICSegment2Repository = ICSegment2Repository;
            _ICSegment1Repository = ICSegment1Repository;
            _icSegment3ExcelExporter = icSegment3ExcelExporter;
        }

        public async Task<PagedResultDto<GetICSegment3ForViewDto>> GetAll(GetAllICSegment3Input input)
        {

            var filteredSegment = _ICSegment3Repository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Seg3Id.Contains(input.Filter) || e.Seg3Name.Contains(input.Filter)); ;

            var pagedAndFilteredSegment = filteredSegment
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var Segments = from o in pagedAndFilteredSegment
                           select new GetICSegment3ForViewDto()
                           {
                               ICSegment3 = new ICSegment3Dto
                               {

                                   Id = o.Id,
                                   Seg1Id = o.Seg1Id,
                                   Seg2Id = o.Seg2Id,
                                   Seg3Id = o.Seg3Id,
                                   Seg3Name = o.Seg3Name
                               }
                           };
            var totalCount = await filteredSegment.CountAsync();

            return new PagedResultDto<GetICSegment3ForViewDto>(
                totalCount,
                await Segments.ToListAsync()
            );
        }

        public async Task<GetICSegment3ForEditOutput> GetICSegment3ForEdit(EntityDto input)
        {
            var segment3 = await _ICSegment3Repository.FirstOrDefaultAsync(input.Id);
            var output = new GetICSegment3ForEditOutput { ICSegment3 = ObjectMapper.Map<CreateOrEditICSegment3Dto>(segment3) };

            var segment2 = await _ICSegment2Repository.FirstOrDefaultAsync(x=>x.Seg2Id == output.ICSegment3.Seg2Id && x.TenantId == AbpSession.TenantId);
            output.Seg2Name = segment2.Seg2Name;

            var segment1 = await _ICSegment1Repository.FirstOrDefaultAsync(x => x.Seg1ID == output.ICSegment3.Seg1Id &&  x.TenantId == AbpSession.TenantId);
            output.Seg1Name = segment1.Seg1Name;
            return output;
        }

        public async Task<GetICSegment3ForViewDto> GetICSegment3ForView(int id)
        {
            var segment3 = await _ICSegment3Repository.FirstOrDefaultAsync(id);

            var output = new GetICSegment3ForViewDto { ICSegment3 = ObjectMapper.Map<ICSegment3Dto>(segment3) };

            if (output != null)
            {

                var icSegment1 = await _ICSegment1Repository.FirstOrDefaultAsync(x => x.Seg1ID == output.ICSegment3.Seg1Id && x.TenantId == AbpSession.TenantId);
                output.Seg1Name = icSegment1.Seg1Name;
                var icSegment2 = await _ICSegment2Repository.FirstOrDefaultAsync(x => x.Seg2Id == output.ICSegment3.Seg2Id && x.TenantId == AbpSession.TenantId);
                output.Seg2Name = icSegment2.Seg2Name;

            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditICSegment3Dto input)
        {
            if (input.flag == false)
            {
                var segment3 = await _ICSegment3Repository.FirstOrDefaultAsync(x => x.Seg3Id == input.Seg2Id + '-' + input.Seg3Id && x.TenantId == AbpSession.TenantId);

                if (segment3 != null)
                {
                    throw new UserFriendlyException("Ooppps! There is a problem!", "Segment ID " + input.Seg3Id + " already taken....");
                }
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        protected virtual async Task Update(CreateOrEditICSegment3Dto input)
        {
            var segment3 = await _ICSegment3Repository.FirstOrDefaultAsync(input.Id);
            ObjectMapper.Map(input, segment3);
        }

        protected virtual async Task Create(CreateOrEditICSegment3Dto input)
        {
            var segment3 = ObjectMapper.Map<ICSegment3>(input);


            if (AbpSession.TenantId != null)
            {
                segment3.TenantId = (int)AbpSession.TenantId;
            }
            segment3.Seg3Id = segment3.Seg2Id + '-' + segment3.Seg3Id;

            await _ICSegment3Repository.InsertAsync(segment3);
        }

        public async Task Delete(EntityDto input)
        {
            await _ICSegment3Repository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetICSegment3ToExcel(GetAllICSegment3ForExcelInput input)
        {
            var filteredICSegment3 = _ICSegment3Repository.GetAll().Where(x  =>x.TenantId == AbpSession.TenantId)
                         .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Seg3Name.Contains(input.Filter) || e.Seg3Id.Contains(input.Filter) || e.Seg1Id.Contains(input.Filter) || e.Seg2Id.Contains(input.Filter))
                         .WhereIf(!string.IsNullOrWhiteSpace(input.Seg3NameFilter), e => e.Seg3Name == input.Seg3NameFilter)
                         .WhereIf(!string.IsNullOrWhiteSpace(input.Seg3IdFilter), e => e.Seg3Id == input.Seg3IdFilter)
                         .WhereIf(!string.IsNullOrWhiteSpace(input.Seg1IdFilter), e => e.Seg1Id == input.Seg1IdFilter)
                         .WhereIf(!string.IsNullOrWhiteSpace(input.Seg2IdFilter), e => e.Seg2Id == input.Seg2IdFilter);

            var query = (from o in filteredICSegment3
                         join seg1 in _ICSegment1Repository.GetAll() on new { o.Seg1Id, o.TenantId} equals new { Seg1Id =  seg1.Seg1ID , seg1.TenantId}
                         join seg2 in _ICSegment2Repository.GetAll() on new {o.Seg2Id, o.TenantId} equals new { seg2.Seg2Id, seg2.TenantId}
                         select new GetICSegment3ForViewDto()
                         {
                             ICSegment3 = new ICSegment3Dto
                             {
                                 Seg3Name = o.Seg3Name,
                                 Seg3Id = o.Seg3Id,
                                 Seg1Id = o.Seg1Id,
                                 Seg2Id = o.Seg2Id,
                                 Id = o.Id
                             },
                             Seg1Name = seg1.Seg1Name,
                             Seg2Name = seg2.Seg2Name
                            
                         });


            var icSegment3ListDtos = await query.ToListAsync();
            return _icSegment3ExcelExporter.ExportToFile(icSegment3ListDtos);
        }

        public async Task<PagedResultDto<NameValueDto>> GetICSegment3ForFinder(FindIcSegment3Input input)
        {
            var filteredSegment = _ICSegment3Repository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Seg2Id == input.Seg2ID)
                 .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Seg3Id.Contains(input.Filter) || e.Seg3Name.Contains(input.Filter));

            var pagedAndFilteredSegment = filteredSegment
                .OrderBy("id desc")
                .PageBy(input);

            var Segments = from o in pagedAndFilteredSegment
                           select new NameValueDto
                           {
                               Name = o.Seg3Name,
                               Value = o.Seg3Id

                           };
            var totalCount = await filteredSegment.CountAsync();

            return new PagedResultDto<NameValueDto>(
                totalCount,
                await Segments.ToListAsync()
            );
        }

        public string GetSegment3MaxId(string id)
        {
            var filteredsegmentlevel3 = _ICSegment3Repository.GetAll().Where(x => x.TenantId == AbpSession.TenantId);
            //string x = "" +  id + "%";
            string[] xstring;
            string xformat = "";
            string nString = "";
            string finalSting = "";
            var getMaxID = filteredsegmentlevel3.Where(c => EF.Functions.Like(c.Seg3Id, $"{id}%") && c.TenantId == AbpSession.TenantId).OrderByDescending(o => o.Id).Select(x => x.Seg3Id).FirstOrDefault();

            if (getMaxID == null)
            {

                xformat = string.Format("{0:00}", 1);
                finalSting = xformat;
            }
            else
            {
                xstring = getMaxID.Split('-');
                nString = xstring[2];

                if (Convert.ToInt32(nString) + 1 > 99)
                {
                    xformat = string.Format("{0:00}", 99);
                    finalSting = xformat;
                }
                else
                {
                    xformat = string.Format("{0:00}", Convert.ToInt32(nString) + 1);
                    finalSting = xformat;
                }
            }
            return finalSting;
        }
    }
}
