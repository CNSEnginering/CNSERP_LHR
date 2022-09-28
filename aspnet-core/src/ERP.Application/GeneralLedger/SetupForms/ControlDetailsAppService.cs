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
	[AbpAuthorize(AppPermissions.SetupForms_ControlDetails)]
    public class ControlDetailsAppService : ERPAppServiceBase, IControlDetailsAppService
    {
		 private readonly IRepository<ControlDetail> _controlDetailRepository;
		 private readonly IControlDetailsExcelExporter _controlDetailsExcelExporter;
		 private readonly IRepository<GroupCategory,int> _lookup_groupCategoryRepository;
		 

		  public ControlDetailsAppService(IRepository<ControlDetail> controlDetailRepository, IControlDetailsExcelExporter controlDetailsExcelExporter , IRepository<GroupCategory, int> lookup_groupCategoryRepository) 
		  {
			_controlDetailRepository = controlDetailRepository;
			_controlDetailsExcelExporter = controlDetailsExcelExporter;
			_lookup_groupCategoryRepository = lookup_groupCategoryRepository;
		
		  }

		 public async Task<PagedResultDto<GetControlDetailForViewDto>> GetAll(GetAllControlDetailsInput input)
         {

            var filteredControlDetails = _controlDetailRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId)
               .Join(
                _lookup_groupCategoryRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId),
                x => x.Family,
                gc => gc.GRPCTCODE,
                (x, gc) => new { x, gc })
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false ||  e.x.Seg1ID.Contains(input.Filter) || e.x.SegmentName.ToLower().Contains(input.Filter.ToLower()) || e.gc.GRPCTDESC.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Seg1IDFilter), e => e.x.Seg1ID == input.Seg1IDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SegmentNameFilter), e => e.x.SegmentName.ToLower() == input.SegmentNameFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FamilyFilter), e => e.gc.GRPCTDESC.ToLower() == input.FamilyFilter.ToLower())
                        .Select(e=> new {e.x.Id, e.x.Seg1ID, e.x.SegmentName, e.x.Family, e.x.TenantId,   GroupCategory = new { e.gc.Id, e.gc.GRPCTCODE, e.gc.GRPCTDESC, e.gc.TenantId } })
                        ;

			var pagedAndFilteredControlDetails = filteredControlDetails
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);


            var _groupCategory = _lookup_groupCategoryRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId);


            var controlDetails = from o in pagedAndFilteredControlDetails
                                 join o1 in _groupCategory on
                                 o.Family equals o1.GRPCTCODE into j1
                                 from s1 in j1.DefaultIfEmpty()
                               
                                 select new GetControlDetailForViewDto()
                                 {
                                     ControlDetail = new ControlDetailDto
                                     {
                                         
                                         Seg1ID = o.Seg1ID,
                                         SegmentName = o.SegmentName,
                                         Family = o.Family,
                                         Id = o.Id
                                     },
                                     FamilyDesc = s1 == null ? "" : s1.GRPCTDESC
                                 };

            var totalCount = await filteredControlDetails.CountAsync();

            return new PagedResultDto<GetControlDetailForViewDto>(
                totalCount,
                await controlDetails.ToListAsync()
            );
         }
		 
		 public async Task<GetControlDetailForViewDto> GetControlDetailForView(int id)
         {
            var controlDetail = await _controlDetailRepository.GetAsync(id);

            var output = new GetControlDetailForViewDto { ControlDetail = ObjectMapper.Map<ControlDetailDto>(controlDetail) };

		    if (output.ControlDetail.Family != null)
            {
                var _lookupGroupCategory = await _lookup_groupCategoryRepository.FirstOrDefaultAsync(x=>x.Id ==  (int)output.ControlDetail.Family && x.TenantId == AbpSession.TenantId);
                output.FamilyDesc = _lookupGroupCategory.GRPCTDESC;
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.SetupForms_ControlDetails_Edit)]
		 public async Task<GetControlDetailForEditOutput> GetControlDetailForEdit(EntityDto input)
         {
            var controlDetail = await _controlDetailRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetControlDetailForEditOutput {ControlDetail = ObjectMapper.Map<CreateOrEditControlDetailDto>(controlDetail)};

		    //if (output.ControlDetail.Family != null)
      //      {
      //          var _lookupGroupCategory = await _lookup_groupCategoryRepository.FirstOrDefaultAsync((int)output.ControlDetail.Family);
      //          output.Family = _lookupGroupCategory.Id;
      //      }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditControlDetailDto input)
         {
            if (input.Flag!= true)
            {
                var controlDetail = await _controlDetailRepository.FirstOrDefaultAsync(x=>x.Seg1ID == input.Seg1ID && x.TenantId == AbpSession.TenantId);

                if (controlDetail != null)
                {
                    throw new UserFriendlyException("Ooppps! There is a problem!", "Segment ID "+ input.Seg1ID + " already taken....");
                }

                await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.SetupForms_ControlDetails_Create)]
		 protected virtual async Task Create(CreateOrEditControlDetailDto input)
         {
            var controlDetail = ObjectMapper.Map<ControlDetail>(input);

			
			if (AbpSession.TenantId != null)
			{
				controlDetail.TenantId =  (int)AbpSession.TenantId;
			}
            //controlDetail.Seg1ID = ConDetailMaxid();

            await _controlDetailRepository.InsertAsync(controlDetail);
         }

		 [AbpAuthorize(AppPermissions.SetupForms_ControlDetails_Edit)]
		 protected virtual async Task Update(CreateOrEditControlDetailDto input)
         {
            var controlDetail = await _controlDetailRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, controlDetail);
         }

		 [AbpAuthorize(AppPermissions.SetupForms_ControlDetails_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _controlDetailRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetControlDetailsToExcel(GetAllControlDetailsForExcelInput input)
         {
			
			var filteredControlDetails = _controlDetailRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId)
               .Join(
                _lookup_groupCategoryRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId),
                x => x.Family,
                gc => gc.GRPCTCODE,
                (x, gc) => new { x, gc })
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.x.Seg1ID.Contains(input.Filter) || e.x.SegmentName.ToLower().Contains(input.Filter.ToLower()) || e.gc.GRPCTDESC.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Seg1IDFilter), e => e.x.Seg1ID == input.Seg1IDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SegmentNameFilter), e => e.x.SegmentName.ToLower() == input.SegmentNameFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FamilyFilter), e => e.gc.GRPCTDESC.ToLower() == input.FamilyFilter.ToLower())
                        .Select(e => new { e.x.Id, e.x.Seg1ID, e.x.SegmentName, e.x.Family, e.x.TenantId, GroupCategory = new { e.gc.Id, e.gc.GRPCTCODE, e.gc.GRPCTDESC, e.gc.TenantId } })
                        ;
            var query = (from o in filteredControlDetails
                         join o1 in _lookup_groupCategoryRepository.GetAll() on new { o.Family,o.TenantId } equals new { Family = o1.GRPCTCODE,o1.TenantId } into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetControlDetailForViewDto() { 
							ControlDetail = new ControlDetailDto
							{
                                
                                SegmentName = o.SegmentName,
                                Family = o.Family,
                                Seg1ID = o.Seg1ID,
							},
                         	FamilyDesc = s1 == null ? "" : s1.GRPCTDESC
						 });


            var controlDetailListDtos = await query.ToListAsync();

            return _controlDetailsExcelExporter.ExportToFile(controlDetailListDtos);
         }


        public string ConDetailMaxid()
        { 
            string maxval = _controlDetailRepository.GetAll().Where(x=> x.TenantId == AbpSession.TenantId).OrderByDescending(o => o.Seg1ID).Select(i => i.Seg1ID).FirstOrDefault();
            string maxmumvalue;
            if (Convert.ToInt32(maxval) + 1 > 99)
            {
                maxmumvalue = string.Format("{0:00}", 99);
            }
            else {
                maxmumvalue = string.Format("{0:00}", Convert.ToInt32(maxval) + 1);
            }

            return maxmumvalue;

        }

       
    }
}