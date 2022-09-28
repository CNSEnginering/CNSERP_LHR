using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using ERP.Dto;
using ERP.SupplyChain.Inventory.IC_Segment2.Dto;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using ERP.SupplyChain.Inventory.IC_Segment1.Dto;
using Abp.UI;
using ERP.SupplyChain.Inventory.IC_Segment1;
using ERP.SupplyChain.Inventory.IC_Segment2.Exporting;

namespace ERP.SupplyChain.Inventory.IC_Segment2
{
    public class ICSegment2AppService : ERPAppServiceBase, IICSegment2AppService
    {
        private readonly IICSegment2ExcelExporter _icSegment2ExcelExporter;
        private readonly IRepository<ICSegment2> _ICSegment2Repository;
        private readonly IRepository<ICSegment1> _ICSegment1Repository;
        public ICSegment2AppService(IRepository<ICSegment2> ICSegment2Repository, IRepository<ICSegment1> ICSegment1Repository, IICSegment2ExcelExporter icSegment2ExcelExporter)
        {
            _ICSegment2Repository = ICSegment2Repository;
            _ICSegment1Repository = ICSegment1Repository;
            _icSegment2ExcelExporter = icSegment2ExcelExporter;
        }

        public async Task<PagedResultDto<GetICSegment2ForViewDto>> GetAll(GetAllICSegment2Input input)
        {
            var filteredSegment = _ICSegment2Repository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Seg2Id.Contains(input.Filter) || e.Seg2Name.Contains(input.Filter));

            var pagedAndFilteredSegment = filteredSegment
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var Segments = from o in pagedAndFilteredSegment
                           select new GetICSegment2ForViewDto()
                           {
                               ICSegment2 = new ICSegment2Dto
                               {
                                   Id = o.Id,
                                   Seg1Id = o.Seg1Id,
                                   Seg2Id = o.Seg2Id,
                                   Seg2Name = o.Seg2Name
                               }
                           };
            var totalCount = await filteredSegment.CountAsync();

            return new PagedResultDto<GetICSegment2ForViewDto>(
                totalCount,
                await Segments.ToListAsync()
            );
        }

        public async Task<GetICSegment2ForEditOutput> GetICSegment2ForEdit(EntityDto input)
        {
            var segment2= await _ICSegment2Repository.FirstOrDefaultAsync(input.Id);
            var output = new GetICSegment2ForEditOutput { ICSegment2 = ObjectMapper.Map<CreateOrEditICSegment2Dto>(segment2) };
            if (output != null)
            {
               
                var icSegment1 = await _ICSegment1Repository.FirstOrDefaultAsync(x => x.Seg1ID == output.ICSegment2.Seg1Id && x.TenantId == AbpSession.TenantId);
                output.Seg1Name = icSegment1.Seg1Name;
            }
            return output;
        }

        public async Task<GetICSegment2ForViewDto> GetICSegment2ForView(int id)
        {
            var segment2 = await _ICSegment2Repository.FirstOrDefaultAsync(id);

            var output = new GetICSegment2ForViewDto { ICSegment2 = ObjectMapper.Map<ICSegment2Dto>(segment2) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditICSegment2Dto input)
        {
            if (input.flag == false)
            {
                var ICsSegment2 = await _ICSegment2Repository.FirstOrDefaultAsync(x => x.Seg1Id == input.Seg1Id.Trim() + '-' + input.Seg2Id.Trim() && x.TenantId == AbpSession.TenantId);

                if (ICsSegment2 != null)
                {
                    throw new UserFriendlyException("Ooppps! There is a problem!", "Segment ID " + input.Seg1Id.Trim() + '-' + input.Seg2Id + " already taken....");
                }
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        protected virtual async Task Update(CreateOrEditICSegment2Dto input)
        {
           // input.Seg2Id = input.Seg1Id + "-" + input.Seg2Id;
            var segment2 = await _ICSegment2Repository.FirstOrDefaultAsync(input.Id);
            ObjectMapper.Map(input, segment2);
        }

        protected virtual async Task Create(CreateOrEditICSegment2Dto input)
        {
            var segment2 = ObjectMapper.Map<ICSegment2>(input);

            segment2.Seg2Id = input.Seg1Id + '-' + input.Seg2Id;
            if (AbpSession.TenantId != null)
            {
                segment2.TenantId = (int)AbpSession.TenantId;
            }

            await _ICSegment2Repository.InsertAsync(segment2);
        }

        public async Task Delete(EntityDto input)
        {
            await _ICSegment2Repository.DeleteAsync(input.Id);
        }


        public async Task<FileDto> GetICSegment2ToExcel(GetAllICSegment2ForExcelInput input)
        {
            var filteredICSegment2 = _ICSegment2Repository.GetAll().Where(x => x.TenantId == AbpSession.TenantId)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Seg2Name.Contains(input.Filter) || e.Seg2Id.Contains(input.Filter) || e.Seg1Id.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Seg2NameFilter), e => e.Seg2Name == input.Seg2NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Seg2IdFilter), e => e.Seg2Id == input.Seg2IdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Seg1IdFilter), e => e.Seg1Id == input.Seg1IdFilter);

            var query = (from o in filteredICSegment2
                         join seg1 in _ICSegment1Repository.GetAll() on new { o.Seg1Id, o.TenantId } equals new { Seg1Id = seg1.Seg1ID, seg1.TenantId}
                         select new GetICSegment2ForViewDto()
                         {
                             ICSegment2 = new ICSegment2Dto
                             {
                                 Seg2Name = o.Seg2Name,
                                 Seg2Id = o.Seg2Id,
                                 Seg1Id = o.Seg1Id,
                                 Seg1Name = seg1.Seg1Name,
                                 Id = o.Id
                             }
                         });


            var icSegment2ListDtos = await query.ToListAsync();

            return _icSegment2ExcelExporter.ExportToFile(icSegment2ListDtos);
        }

        public async Task<PagedResultDto<NameValueDto>> GetICSegment2ForFinder(GetAllIcSegment2InputForFinder input)
        {
            var filteredSegment = _ICSegment2Repository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Seg1Id == input.seg1ID)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Seg2Id.Contains(input.Filter) || e.Seg2Name.Contains(input.Filter));

            var pagedAndFilteredSegment = filteredSegment
                .OrderBy("id desc")
                .PageBy(input);

            var Segments = from o in pagedAndFilteredSegment
                           select new NameValueDto
                           {
                               Name = o.Seg2Name,
                               Value = o.Seg2Id

                           };
            var totalCount = await filteredSegment.CountAsync();

            return new PagedResultDto<NameValueDto>(
                totalCount,
                await Segments.ToListAsync()
            );
        }

        public string GetSegment2MaxId(string id)
        {

            var filteredSegment2 = _ICSegment2Repository.GetAll().Where(x => x.TenantId == AbpSession.TenantId);
            //string x = "" +  id + "%";
            string[] xstring;
            string xformat = "";
            string nString = "";
            string finalSting = "";
            var getMaxID = filteredSegment2.Where(c => EF.Functions.Like(c.Seg2Id, $"{id}%") && c.TenantId == AbpSession.TenantId).OrderByDescending(o => o.Seg2Id).Select(x => x.Seg2Id).FirstOrDefault();

            if (getMaxID == null)
            {

                xformat = string.Format("{0:000}", 1);
                finalSting = xformat; //id + "-" + xformat;
            }
            else
            {
                xstring = getMaxID.Split('-');
                nString = xstring[1];
                if (Convert.ToInt32(nString) + 1 > 999)
                {
                    xformat = string.Format("{0:000}", 999);
                    finalSting = xformat; //id + "-" + xformat;
                }
                else
                {
                    xformat = string.Format("{0:000}", Convert.ToInt32(nString) + 1);
                    finalSting = xformat; //id + "-" + xformat;
                }
            }

            return finalSting;
        }


    }
}
