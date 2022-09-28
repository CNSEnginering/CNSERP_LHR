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

namespace ERP.GeneralLedger.SetupForms
{
	[AbpAuthorize(AppPermissions.SetupForms_GroupCodes)]
    public class GroupCodesAppService : ERPAppServiceBase, IGroupCodesAppService
    {
		 private readonly IRepository<GroupCode> _groupCodeRepository;
		 private readonly IGroupCodesExcelExporter _groupCodesExcelExporter;
		 private readonly IRepository<GroupCategory> _lookup_groupCategoryRepository;
		 
        // TESTS

		  public GroupCodesAppService(IRepository<GroupCode> groupCodeRepository, IGroupCodesExcelExporter groupCodesExcelExporter , IRepository<GroupCategory> lookup_groupCategoryRepository) 
		  {
			_groupCodeRepository = groupCodeRepository;
			_groupCodesExcelExporter = groupCodesExcelExporter;
			_lookup_groupCategoryRepository = lookup_groupCategoryRepository;
		
		  }

		 public async Task<PagedResultDto<GetGroupCodeForViewDto>> GetAll(GetAllGroupCodesInput input)
         {

            var filteredGroupCodes = _groupCodeRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId)
               .Join(
                _lookup_groupCategoryRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId),
                x => x.GRPCTCODE,
                gc => gc.GRPCTCODE,
                (x, gc) => new { x, gc })

                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.x.GRPCODE.ToString().Contains(input.Filter) || e.x.GRPDESC.ToLower().Contains(input.Filter.ToLower()) ||
                        e.gc.GRPCTDESC.ToLower().Contains(input.Filter.ToLower()))
                        .WhereIf(input.MinGRPCODEFilter != null, e => e.x.GRPCODE >= input.MinGRPCODEFilter)
                        .WhereIf(input.MaxGRPCODEFilter != null, e => e.x.GRPCODE <= input.MaxGRPCODEFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.GRPDESCFilter), e => e.x.GRPDESC.ToLower() == input.GRPDESCFilter.ToLower())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.GRPCTDESCFilter), e => e.gc.GRPCTDESC.ToLower() == input.GRPCTDESCFilter.ToLower())
                        
                        .Select(e=> new { e.x.Id,e.x.GRPCODE,e.x.GRPDESC, e.x.GRPCTCODE, e.x.TenantId } );

          

            var pagedAndFilteredGroupCodes = filteredGroupCodes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var groupCodes = from o in pagedAndFilteredGroupCodes
                             join o1 in _lookup_groupCategoryRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId)
                         on o.GRPCTCODE equals o1.GRPCTCODE into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetGroupCodeForViewDto() {
							GroupCode = new GroupCodeDto
							{
                             GRPCODE = o.GRPCODE, 
                                GRPDESC = o.GRPDESC,
                                GRPCTCODE = o.GRPCTCODE,
                                Id = o.Id
							},
                             GRPCTCODEDESC = s1 == null ? "" : s1.GRPCTDESC
                            
                         };

            var totalCount = await filteredGroupCodes.CountAsync();

            return new PagedResultDto<GetGroupCodeForViewDto>(
                totalCount,
                await groupCodes.ToListAsync()
            );
         }
		 
		 public async Task<GetGroupCodeForViewDto> GetGroupCodeForView(int id)
         {
            var groupCode = await _groupCodeRepository.GetAsync(id);

            var output = new GetGroupCodeForViewDto { GroupCode = ObjectMapper.Map<GroupCodeDto>(groupCode) };

		    if (output.GroupCode.GRPCTCODE != null)
            {
                var _lookupGroupCategory = await _lookup_groupCategoryRepository.FirstOrDefaultAsync((output.GroupCode.GRPCTCODE));
                output.GRPCTCODEDESC = _lookupGroupCategory.GRPCTDESC;
            }
			
            return output;
         }

        public GetMaxIDGroupCodes  Maxid() {

            var GroupCategroylist = _groupCodeRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId);
            var maxid  = ((from tab1 in GroupCategroylist select (int?)tab1.GRPCODE).Max() ?? 0) + 1;
            var IdList = GroupCategroylist.Select(x=> x.Id).ToArray();
            GetMaxIDGroupCodes dd = new GetMaxIDGroupCodes();
            dd.GroupID = IdList;
            dd.MaxID = maxid;
            return dd;
        }

		 [AbpAuthorize(AppPermissions.SetupForms_GroupCodes_Edit)]
		 public async Task<GetGroupCodeForEditOutput> GetGroupCodeForEdit(EntityDto input)
         {
            var groupCode = await _groupCodeRepository.FirstOrDefaultAsync(x=>x.Id == input.Id && x.TenantId == AbpSession.TenantId);
           
		    var output = new GetGroupCodeForEditOutput {GroupCode = ObjectMapper.Map<CreateOrEditGroupCodeDto>(groupCode)};

		    if (output.GroupCode.GRPCTCODE != null)
            {
                var _lookupGroupCategory = await _lookup_groupCategoryRepository.FirstOrDefaultAsync(x=>x.GRPCTCODE == output.GroupCode.GRPCTCODE);
                output.GRPCTCODE = _lookupGroupCategory.Id;
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditGroupCodeDto input)
         {

            
            //var groupCode =  _groupCodeRepository.FirstOrDefault((int)input.Id);

            if (input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.SetupForms_GroupCodes_Create)]
		 protected virtual async Task Create(CreateOrEditGroupCodeDto input)
         {
            var groupCode = ObjectMapper.Map<GroupCode>(input);

			
			if (AbpSession.TenantId != null)
			{
				groupCode.TenantId = (int) AbpSession.TenantId;
			}
		

            await _groupCodeRepository.InsertAsync(groupCode);
         }

		 [AbpAuthorize(AppPermissions.SetupForms_GroupCodes_Edit)]
		 protected virtual async Task Update(CreateOrEditGroupCodeDto input)
         {
            var groupCode = await _groupCodeRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, groupCode);
         }

		 [AbpAuthorize(AppPermissions.SetupForms_GroupCodes_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _groupCodeRepository.DeleteAsync(x => x.Id == input.Id && x.TenantId == AbpSession.TenantId);
         } 

		public async Task<FileDto> GetGroupCodesToExcel(GetAllGroupCodesForExcelInput input)
         {

            var filteredGroupCodes = _groupCodeRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId)
               .Join(
                _lookup_groupCategoryRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId),
                x => x.GRPCTCODE,
                gc => gc.GRPCTCODE,
                (x, gc) => new { x, gc })

                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.x.GRPCODE.ToString().Contains(input.Filter) || e.x.GRPDESC.ToLower().Contains(input.Filter.ToLower()) ||
                        e.gc.GRPCTDESC.ToLower().Contains(input.Filter.ToLower()))
                        .WhereIf(input.MinGRPCODEFilter != null, e => e.x.GRPCODE >= input.MinGRPCODEFilter)
                        .WhereIf(input.MaxGRPCODEFilter != null, e => e.x.GRPCODE <= input.MaxGRPCODEFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.GRPDESCFilter), e => e.x.GRPDESC.ToLower() == input.GRPDESCFilter.ToLower())
                        .WhereIf(input.GRPCTCODEFilter != null, e => e.gc.GRPCTCODE == input.GRPCTCODEFilter)

                        .Select(e => new { e.x.Id, e.x.GRPCODE, e.x.GRPDESC, e.x.GRPCTCODE, e.x.TenantId });


            var query = (from o in filteredGroupCodes
                         join o1 in _lookup_groupCategoryRepository.GetAll() on o.GRPCTCODE equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetGroupCodeForViewDto() {
                             GroupCode = new GroupCodeDto
                             {

                                 GRPDESC = o.GRPDESC,

                                 Id = o.Id
                             },
                             GRPCTCODEDESC = s1 == null ? "" : s1.GRPCTDESC
						 });


            var groupCodeListDtos = await query.ToListAsync();

            return _groupCodesExcelExporter.ExportToFile(groupCodeListDtos);
         }

		[AbpAuthorize(AppPermissions.SetupForms_GroupCodes)]
        public async Task<ListResultDto<GroupCategoryForComboboxDto>> GetGroupCategoryForCombobox()
         {
            var query = _lookup_groupCategoryRepository.GetAll().Where(e=>e.TenantId == AbpSession.TenantId);

            var totalCount = await query.CountAsync();

            var groupCategoryList = await query
                .ToListAsync();

			var lookupTableDtoList = new List<GroupCategoryForComboboxDto>();
			foreach(var groupCategory in groupCategoryList){
				lookupTableDtoList.Add(new GroupCategoryForComboboxDto
                {
                    
					Id = groupCategory.GRPCTCODE,
					DisplayName = groupCategory.GRPCTDESC.ToString()
				});
			}

            return new ListResultDto<GroupCategoryForComboboxDto>(
                lookupTableDtoList
            );
         }

        
    }
}