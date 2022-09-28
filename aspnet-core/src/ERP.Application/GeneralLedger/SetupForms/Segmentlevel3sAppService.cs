using ERP.GeneralLedger.SetupForms;



using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.GeneralLedger.SetupForms.Exporting;
using ERP.GeneralLedger.SetupForms.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;

namespace ERP.GeneralLedger.SetupForms
{
	[AbpAuthorize(AppPermissions.SetupForms_Segmentlevel3s)]
    public class Segmentlevel3sAppService : ERPAppServiceBase, ISegmentlevel3sAppService
    {
		 private readonly IRepository<Segmentlevel3> _segmentlevel3Repository;
		 private readonly ISegmentlevel3sExcelExporter _segmentlevel3sExcelExporter;
		 private readonly IRepository<ControlDetail> _lookup_controlDetailRepository;
		 private readonly IRepository<SubControlDetail> _lookup_subControlDetailRepository;
		 

		  public Segmentlevel3sAppService(IRepository<Segmentlevel3> segmentlevel3Repository, ISegmentlevel3sExcelExporter segmentlevel3sExcelExporter , IRepository<ControlDetail> lookup_controlDetailRepository, IRepository<SubControlDetail> lookup_subControlDetailRepository) 
		  {
			_segmentlevel3Repository = segmentlevel3Repository;
			_segmentlevel3sExcelExporter = segmentlevel3sExcelExporter;
			_lookup_controlDetailRepository = lookup_controlDetailRepository;
		_lookup_subControlDetailRepository = lookup_subControlDetailRepository;
		
		  }

		 public async Task<PagedResultDto<GetSegmentlevel3ForViewDto>> GetAll(GetAllSegmentlevel3sInput input)
         {
            var query = from o in _segmentlevel3Repository.GetAll().Where(x => x.TenantId == AbpSession.TenantId)
                        join o1 in _lookup_controlDetailRepository.GetAll() on new { Seg1ID = o.Seg3ID.Substring(0, o.Seg3ID.Length - 7), o.TenantId } equals new { o1.Seg1ID, o1.TenantId }
                        join o2 in _lookup_subControlDetailRepository.GetAll() on new { Seg2ID = o.Seg3ID.Substring(0, o.Seg3ID.Length - 3), o.TenantId } equals new
                        { o2.Seg2ID, o2.TenantId }

                        select new
                        {
                            o.Id,
                            o.Seg3ID,
                            o.SegmentName,
                            o.OldCode,
                            o.TenantId,
                            Seg1Name = o1.SegmentName,
                            Seg2Name = o2.SegmentName
                        };


            var filteredSegmentlevel3s = query
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Seg3ID.Contains(input.Filter) || e.SegmentName.Contains(input.Filter) || e.Seg1Name.Contains(input.Filter) || e.Seg2Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Seg3IDFilter), e => e.Seg3ID == input.Seg3IDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SegmentNameFilter), e => e.SegmentName.ToLower() == input.SegmentNameFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Seg1Filter), e => e.Seg1Name.ToLower() == input.Seg1Filter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Seg2Filter), e => e.Seg2Name.ToLower() == input.Seg2Filter.ToLower().Trim());

            var pagedAndFilteredSegmentlevel3s = filteredSegmentlevel3s
                .OrderBy(input.Sorting ?? "Seg3ID desc")
                .PageBy(input);

            var test = filteredSegmentlevel3s.Select(x => x.Seg3ID.Substring(0, x.Seg3ID.Length - 3));
            //change id
            var segmentlevel3s = from o in pagedAndFilteredSegmentlevel3s
                         join o1 in _lookup_controlDetailRepository.GetAll() on new { Seg1ID = o.Seg3ID.Substring(0, o.Seg3ID.Length - 7),o.TenantId } equals new { o1.Seg1ID,o1.TenantId } 

                                 join o2 in _lookup_subControlDetailRepository.GetAll() on new { Seg2ID = o.Seg3ID.Substring(0, o.Seg3ID.Length - 3), o.TenantId } equals new
                                 { o2.Seg2ID,o2.TenantId }

                                 select new GetSegmentlevel3ForViewDto() {
							Segmentlevel3 = new Segmentlevel3Dto
							{
                               Seg3ID = o.Seg3ID,
                                SegmentName = o.SegmentName,
                                OldCode = o.OldCode,
                                Id = o.Id
							},
                         	ControlDetailDesc = o1.SegmentName,
                         	SubControlDetailDesc = o2.SegmentName
						};

            var totalCount = await filteredSegmentlevel3s.CountAsync();

            return new PagedResultDto<GetSegmentlevel3ForViewDto>(
                totalCount,
                await segmentlevel3s.ToListAsync()
            );
         }
		 
		 public async Task<GetSegmentlevel3ForViewDto> GetSegmentlevel3ForView(int id)
         {
            var segmentlevel3 = await _segmentlevel3Repository.GetAsync(id);

            var output = new GetSegmentlevel3ForViewDto { Segmentlevel3 = ObjectMapper.Map<Segmentlevel3Dto>(segmentlevel3) };

            string[] contID = output.Segmentlevel3.Seg3ID.Split('-');

            if (contID[0] != null)
            {
                var _lookupControlDetail = await _lookup_controlDetailRepository.FirstOrDefaultAsync(x=>x.Seg1ID == (string)contID[0]);
                output.ControlDetailId = _lookupControlDetail.Seg1ID.ToString();
                output.ControlDetailDesc = _lookupControlDetail.SegmentName.ToString();
            }

		    if (contID[1] != null)
            {
               
                var _lookupSubControlDetail = await _lookup_subControlDetailRepository.FirstOrDefaultAsync(x => x.Seg2ID == contID[0] + '-' + contID[1] );
                output.SubControlDetailId = _lookupSubControlDetail.Seg2ID.ToString();
                output.SubControlDetailDesc = _lookupSubControlDetail.SegmentName.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.SetupForms_Segmentlevel3s_Edit)]
		 public async Task<GetSegmentlevel3ForEditOutput> GetSegmentlevel3ForEdit(EntityDto input)
         {
            var segmentlevel3 = await _segmentlevel3Repository.FirstOrDefaultAsync(input.Id);
           

            var output = new GetSegmentlevel3ForEditOutput {Segmentlevel3 = ObjectMapper.Map<CreateOrEditSegmentlevel3Dto>(segmentlevel3)};
            string[] contID = output.Segmentlevel3.Seg3ID.Split('-');
            if (contID[0]!= null)
            {
                var _lookupControlDetail = await _lookup_controlDetailRepository.FirstOrDefaultAsync(x => x.Seg1ID == (string)contID[0] && x.TenantId == AbpSession.TenantId);
                output.ControlDetailId = _lookupControlDetail.Seg1ID.ToString();
                output.ControlDetailDesc = _lookupControlDetail.SegmentName;
            }

		    if (contID[1] != null)
            {
                var _lookupSubControlDetail = await _lookup_subControlDetailRepository.FirstOrDefaultAsync(x => x.Seg2ID == (string)contID[0] + '-' + contID[1] && x.TenantId == AbpSession.TenantId);
                output.SubControlDetailId = _lookupSubControlDetail.Seg2ID.ToString();
                output.SubControlDetailDesc = _lookupSubControlDetail.SegmentName;
            }

            output.Segmentlevel3.Seg3ID = contID[2];


            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditSegmentlevel3Dto input)
         {
            
            if (input.Flag != true)
            {
                var segmentlevel3 = await _segmentlevel3Repository.FirstOrDefaultAsync(x => x.Seg3ID == input.SubControlDetailId + '-'+ input.Seg3ID && x.TenantId == AbpSession.TenantId);

                if (segmentlevel3 != null)
                {
                    throw new UserFriendlyException("Ooppps! There is a problem!", "Segment ID " + input.Seg3ID + " already taken....");
                }
                await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.SetupForms_Segmentlevel3s_Create)]
		 protected virtual async Task Create(CreateOrEditSegmentlevel3Dto input)
         {
            var segmentlevel3 = ObjectMapper.Map<Segmentlevel3>(input);

			
			if (AbpSession.TenantId != null)
			{
				segmentlevel3.TenantId =  (int)AbpSession.TenantId;
			}
            segmentlevel3.Seg3ID = input.SubControlDetailId + '-' + input.Seg3ID;


                await _segmentlevel3Repository.InsertAsync(segmentlevel3);

         }

		 [AbpAuthorize(AppPermissions.SetupForms_Segmentlevel3s_Edit)]
		 protected virtual async Task Update(CreateOrEditSegmentlevel3Dto input)
         {
            input.Seg3ID = input.SubControlDetailId + '-' + input.Seg3ID;
            var segmentlevel3 = await _segmentlevel3Repository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, segmentlevel3);
         }

		 [AbpAuthorize(AppPermissions.SetupForms_Segmentlevel3s_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _segmentlevel3Repository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetSegmentlevel3sToExcel(GetAllSegmentlevel3sForExcelInput input)
         {

            var filteredSegmentlevel3s = _segmentlevel3Repository.GetAll().Where(x => x.TenantId == AbpSession.TenantId)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Seg3ID.Contains(input.Filter) || e.SegmentName.Contains(input.Filter) || e.OldCode.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SegmentNameFilter), e => e.SegmentName.ToLower() == input.SegmentNameFilter.ToLower().Trim());
            // change id here
            var query = (from o in filteredSegmentlevel3s
                         join o1 in _lookup_controlDetailRepository.GetAll() on new { Seg1ID = o.Seg3ID.Substring(0, o.Seg3ID.Length - 7), o.TenantId } equals new { o1.Seg1ID, o1.TenantId } into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_subControlDetailRepository.GetAll() on new { Seg2ID = o.Seg3ID.Substring(0, o.Seg3ID.Length - 3), o.TenantId } equals new
                         { o2.Seg2ID, o2.TenantId } into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetSegmentlevel3ForViewDto() { 
							Segmentlevel3 = new Segmentlevel3Dto
							{
                                Seg3ID = o.Seg3ID,
                                SegmentName = o.SegmentName,
                                OldCode = o.OldCode,
                                Id = o.Id
							},
                         	ControlDetailId = s1 == null ? "" : s1.Seg1ID.ToString(),
                            ControlDetailDesc = s1 == null ? "" : s1.SegmentName,
                             SubControlDetailId = s2 == null ? "" : s2.Seg2ID.ToString(),
                             SubControlDetailDesc = s2 == null ? "" : s2.SegmentName
                         });


            var segmentlevel3ListDtos = await query.ToListAsync();
            return _segmentlevel3sExcelExporter.ExportToFile(segmentlevel3ListDtos);
         }



		[AbpAuthorize(AppPermissions.SetupForms_Segmentlevel3s)]
         public async Task<PagedResultDto<Segmentlevel3ControlDetailLookupTableDto>> GetAllControlDetailForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_controlDetailRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter), e => false ||
                   e.Seg1ID.ToString().Contains(input.Filter) || e.SegmentName.ToLower().ToString().Contains(input.Filter.ToLower())
                );

            var totalCount = await query.CountAsync();

            var controlDetailList = await query
                .OrderBy(input.Sorting ?? "Seg1ID desc")
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<Segmentlevel3ControlDetailLookupTableDto>();
			foreach(var controlDetail in controlDetailList){
                lookupTableDtoList.Add(new Segmentlevel3ControlDetailLookupTableDto
                {
                    Id = controlDetail.Seg1ID,
                    DisplayName = controlDetail.SegmentName
				});
			}

            return new PagedResultDto<Segmentlevel3ControlDetailLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.SetupForms_Segmentlevel3s)]
         public async Task<PagedResultDto<Segmentlevel3SubControlDetailLookupTableDto>> GetAllSubControlDetailForLookupTable(GetAllForLookupTableInput input, string Seg1ID)
         {


            var query = _lookup_subControlDetailRepository.GetAll().Where(c => EF.Functions.Like(c.Seg2ID, $"{Seg1ID}%") && c.TenantId == AbpSession.TenantId).WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e => false || e.Seg2ID.ToString().Contains(input.Filter) || e.SegmentName.ToLower().ToString().Contains(input.Filter.ToLower()));

            //var query = _lookup_subControlDetailRepository.GetAll().WhereIf(
            //        !string.IsNullOrWhiteSpace(input.Filter),
            //       e=> e.Id.ToString().Contains(input.Filter)
            //    );

            var totalCount = await query.CountAsync();

            var subControlDetailList = await query
                .OrderBy(input.Sorting ?? "Seg2ID desc")
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<Segmentlevel3SubControlDetailLookupTableDto>();
			foreach(var subControlDetail in subControlDetailList){
				lookupTableDtoList.Add(new Segmentlevel3SubControlDetailLookupTableDto
				{
					Id = subControlDetail.Seg2ID,
					DisplayName = subControlDetail.SegmentName
				});
			}

            return new PagedResultDto<Segmentlevel3SubControlDetailLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }


        public string GetMaxSeg3ID(string id)
        {
            var filteredsegmentlevel3 = _segmentlevel3Repository.GetAll().Where(x => x.TenantId == AbpSession.TenantId);
            //string x = "" +  id + "%";
            string[] xstring;
            string xformat = "";
            string nString = "";
            string finalSting = "";
            var getMaxID = filteredsegmentlevel3.Where(c => EF.Functions.Like(c.Seg3ID, $"{id}%") && c.TenantId == AbpSession.TenantId).OrderByDescending(o => o.Id).Select(x => x.Seg3ID).FirstOrDefault();

            if (getMaxID == null)
            {

                xformat = string.Format("{0:00}", 1);
                finalSting =  xformat;
            }
            else
            {
                xstring = getMaxID.Split('-');
                nString = xstring[2];

                if (Convert.ToInt32(nString) + 1 > 99)
                {
                    xformat = string.Format("{0:00}", 99);
                    finalSting =  xformat;
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