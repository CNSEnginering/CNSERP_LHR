

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.PayRoll.Department.Exporting;
using ERP.PayRoll.Department.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ERP.GeneralLedger.SetupForms;
using ERP.GeneralLedger.SetupForms.LedgerType;


namespace ERP.PayRoll.Department
{
	[AbpAuthorize(AppPermissions.PayRoll_Departments)]
    public class DepartmentsAppService : ERPAppServiceBase, IDepartmentsAppService
    {
		 private readonly IRepository<Department> _departmentRepository;
		 private readonly IDepartmentsExcelExporter _departmentsExcelExporter;
        private readonly IRepository<GLSecChartofControl, string> _glSecChartofControlRepository;
        private readonly IRepository<LedgerType> _ledgerTypeRepository;
        private readonly IRepository<Cader.Cader> _caderRepository;


        public DepartmentsAppService(IRepository<Department> departmentRepository, IRepository<LedgerType> ledgerTypeRepository, IRepository<GLSecChartofControl, string> glSecChartofControlRepository, IDepartmentsExcelExporter departmentsExcelExporter, IRepository<Cader.Cader> caderRepository)
        {
			_departmentRepository = departmentRepository;
			_departmentsExcelExporter = departmentsExcelExporter;
            _glSecChartofControlRepository = glSecChartofControlRepository;
            _ledgerTypeRepository = ledgerTypeRepository;
            _caderRepository = caderRepository;

        }

		 public async Task<PagedResultDto<GetDepartmentForViewDto>> GetAll(GetAllDepartmentsInput input)
         {

            var filteredDepartments = _departmentRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.DeptName.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinDeptIDFilter != null, e => e.DeptID >= input.MinDeptIDFilter)
                        .WhereIf(input.MaxDeptIDFilter != null, e => e.DeptID <= input.MaxDeptIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DeptNameFilter), e => e.DeptName == input.DeptNameFilter)
                        .WhereIf(input.MinActiveFilter != null, e => e.Active >= input.MinActiveFilter)
                        .WhereIf(input.MaxActiveFilter != null, e => e.Active <= input.MaxActiveFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var pagedAndFilteredDepartments = filteredDepartments
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

			var departments = from o in pagedAndFilteredDepartments
                         select new GetDepartmentForViewDto() {
							Department = new DepartmentDto
							{
                                DeptID = o.DeptID,
                                DeptName = o.DeptName,
                                Active = Convert.ToBoolean(o.Active),
                                AudtUser = o.AudtUser,
                                AudtDate = o.AudtDate,
                                CreatedBy = o.CreatedBy,
                                CreateDate = o.CreateDate,
                                Id = o.Id
							}
						};

            var totalCount = await filteredDepartments.CountAsync();

            return new PagedResultDto<GetDepartmentForViewDto>(
                totalCount,
                await departments.ToListAsync()
            );
         }
		 
		 [AbpAuthorize(AppPermissions.PayRoll_Departments_Edit)]
		 public async Task<GetDepartmentForEditOutput> GetDepartmentForEdit(EntityDto input)
         {
            var department = await _departmentRepository.FirstOrDefaultAsync(input.Id);
            var cader = _caderRepository.GetAll().Where(e => e.Id == department.Cader_Id && e.TenantId == AbpSession.TenantId).FirstOrDefault();
            var output = new GetDepartmentForEditOutput {Department = ObjectMapper.Map<CreateOrEditDepartmentDto>(department)};

            var query = from o in _glSecChartofControlRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == output.Department.ExpenseAcc)
                        join o1 in _ledgerTypeRepository.GetAll()
                        on new { SLType = (int)o.SLType, o.TenantId } equals new { SLType = o1.LedgerID, o1.TenantId }
                        into f
                        from o1 in f.DefaultIfEmpty()
                        select new { o.Id, o.AccountName, o.SubLedger, o.SLType, LedgerDesc = o1 != null ? o1.LedgerDesc : "" };
            if (query.ToList().Count() > 0)
            {
                output.Department.ExpenseAccName = query.FirstOrDefault().AccountName;
            }
            if (cader != null)
            {
                output.Department.CaderName = cader.CADER_NAME;
            }
            return output;
        }

        public async Task CreateOrEdit(CreateOrEditDepartmentDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.PayRoll_Departments_Create)]
		 protected virtual async Task Create(CreateOrEditDepartmentDto input)
         {
            var department = ObjectMapper.Map<Department>(input);

			
			if (AbpSession.TenantId != null)
			{
				department.TenantId = (int) AbpSession.TenantId;
			}

            department.DeptID = GetMaxID();
            await _departmentRepository.InsertAsync(department);
         }

		 [AbpAuthorize(AppPermissions.PayRoll_Departments_Edit)]
		 protected virtual async Task Update(CreateOrEditDepartmentDto input)
         {
            var department = await _departmentRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, department);
         }

		 [AbpAuthorize(AppPermissions.PayRoll_Departments_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _departmentRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetDepartmentsToExcel(GetAllDepartmentsForExcelInput input)
         {
			
			var filteredDepartments = _departmentRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.DeptName.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
						.WhereIf(input.MinDeptIDFilter != null, e => e.DeptID >= input.MinDeptIDFilter)
						.WhereIf(input.MaxDeptIDFilter != null, e => e.DeptID <= input.MaxDeptIDFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DeptNameFilter),  e => e.DeptName == input.DeptNameFilter)
						.WhereIf(input.MinActiveFilter != null, e => e.Active >= input.MinActiveFilter)
						.WhereIf(input.MaxActiveFilter != null, e => e.Active <= input.MaxActiveFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter),  e => e.AudtUser == input.AudtUserFilter)
						.WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
						.WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter),  e => e.CreatedBy == input.CreatedByFilter)
						.WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
						.WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

			var query = (from o in filteredDepartments
                         select new GetDepartmentForViewDto() { 
							Department = new DepartmentDto
							{
                                DeptID = o.DeptID,
                                DeptName = o.DeptName,
                                Active = Convert.ToBoolean(o.Active),
                                AudtUser = o.AudtUser,
                                AudtDate = o.AudtDate,
                                CreatedBy = o.CreatedBy,
                                CreateDate = o.CreateDate,
                                Id = o.Id
							}
						 });


            var departmentListDtos = await query.ToListAsync();

            return _departmentsExcelExporter.ExportToFile(departmentListDtos);
         }

        public int GetMaxID()
        {
            var maxid = ((from tab1 in _departmentRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId) select (int?)tab1.DeptID).Max() ?? 0) + 1;
            return maxid;
        }

    }
}