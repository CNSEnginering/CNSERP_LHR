

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.PayRoll.EmployeeType.Exporting;
using ERP.PayRoll.EmployeeType.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ERP.PayRoll.EmployeeType
{
	[AbpAuthorize(AppPermissions.PayRoll_EmployeeType)]
    public class EmployeeTypeAppService : ERPAppServiceBase, IEmployeeTypeAppService
    {
		 private readonly IRepository<EmployeeType> _employeeTypeRepository;
		 private readonly IEmployeeTypeExcelExporter _employeeTypeExcelExporter;
		 

		  public EmployeeTypeAppService(IRepository<EmployeeType> employeeTypeRepository, IEmployeeTypeExcelExporter employeeTypeExcelExporter ) 
		  {
			_employeeTypeRepository = employeeTypeRepository;
			_employeeTypeExcelExporter = employeeTypeExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetEmployeeTypeForViewDto>> GetAll(GetAllEmployeeTypeInput input)
         {
			
			var filteredEmployeeType = _employeeTypeRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.EmpType.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
						.WhereIf(input.MinTypeIDFilter != null, e => e.TypeID >= input.MinTypeIDFilter)
						.WhereIf(input.MaxTypeIDFilter != null, e => e.TypeID <= input.MaxTypeIDFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.EmpTypeFilter),  e => e.EmpType == input.EmpTypeFilter)
						.WhereIf(input.ActiveFilter > -1,  e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter),  e => e.AudtUser == input.AudtUserFilter)
						.WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
						.WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter),  e => e.CreatedBy == input.CreatedByFilter)
						.WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
						.WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

			var pagedAndFilteredEmployeeType = filteredEmployeeType
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

			var employeeType = from o in pagedAndFilteredEmployeeType
                         select new GetEmployeeTypeForViewDto() {
							EmployeeType = new EmployeeTypeDto
							{
                                TypeID = o.TypeID,
                                EmpType = o.EmpType,
                                Active = o.Active,
                                AudtUser = o.AudtUser,
                                AudtDate = o.AudtDate,
                                CreatedBy = o.CreatedBy,
                                CreateDate = o.CreateDate,
                                Id = o.Id
							}
						};

            var totalCount = await filteredEmployeeType.CountAsync();

            return new PagedResultDto<GetEmployeeTypeForViewDto>(
                totalCount,
                await employeeType.ToListAsync()
            );
         }
		 
		 [AbpAuthorize(AppPermissions.PayRoll_EmployeeType_Edit)]
		 public async Task<GetEmployeeTypeForEditOutput> GetEmployeeTypeForEdit(EntityDto input)
         {
            var employeeType = await _employeeTypeRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetEmployeeTypeForEditOutput {EmployeeType = ObjectMapper.Map<CreateOrEditEmployeeTypeDto>(employeeType)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditEmployeeTypeDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.PayRoll_EmployeeType_Create)]
		 protected virtual async Task Create(CreateOrEditEmployeeTypeDto input)
         {
            var employeeType = ObjectMapper.Map<EmployeeType>(input);

			
			if (AbpSession.TenantId != null)
			{
				employeeType.TenantId = (int) AbpSession.TenantId;
			}

            employeeType.TypeID = GetMaxID();
            await _employeeTypeRepository.InsertAsync(employeeType);
         }

		 [AbpAuthorize(AppPermissions.PayRoll_EmployeeType_Edit)]
		 protected virtual async Task Update(CreateOrEditEmployeeTypeDto input)
         {
            var employeeType = await _employeeTypeRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, employeeType);
         }

		 [AbpAuthorize(AppPermissions.PayRoll_EmployeeType_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _employeeTypeRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetEmployeeTypeToExcel(GetAllEmployeeTypeForExcelInput input)
         {
			
			var filteredEmployeeType = _employeeTypeRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.EmpType.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
						.WhereIf(input.MinTypeIDFilter != null, e => e.TypeID >= input.MinTypeIDFilter)
						.WhereIf(input.MaxTypeIDFilter != null, e => e.TypeID <= input.MaxTypeIDFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.EmpTypeFilter),  e => e.EmpType == input.EmpTypeFilter)
						.WhereIf(input.ActiveFilter > -1,  e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active) )
						.WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter),  e => e.AudtUser == input.AudtUserFilter)
						.WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
						.WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter),  e => e.CreatedBy == input.CreatedByFilter)
						.WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
						.WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

			var query = (from o in filteredEmployeeType
                         select new GetEmployeeTypeForViewDto() { 
							EmployeeType = new EmployeeTypeDto
							{
                                TypeID = o.TypeID,
                                EmpType = o.EmpType,
                                Active = o.Active,
                                AudtUser = o.AudtUser,
                                AudtDate = o.AudtDate,
                                CreatedBy = o.CreatedBy,
                                CreateDate = o.CreateDate,
                                Id = o.Id
							}
						 });


            var employeeTypeListDtos = await query.ToListAsync();

            return _employeeTypeExcelExporter.ExportToFile(employeeTypeListDtos);
         }
        public int GetMaxID()
        {
            var maxid = ((from tab1 in _employeeTypeRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId) select (int?)tab1.TypeID).Max() ?? 0) + 1;
            return maxid;
        }

    }
}