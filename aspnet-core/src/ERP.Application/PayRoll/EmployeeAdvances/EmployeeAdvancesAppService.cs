

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.PayRoll.EmployeeAdvances.Exporting;
using ERP.PayRoll.EmployeeAdvances.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ERP.SupplyChain.Inventory;
using ERP.Authorization.Users;

namespace ERP.PayRoll.EmployeeAdvances
{
    [AbpAuthorize(AppPermissions.PayRoll_EmployeeAdvances)]
    public class EmployeeAdvancesAppService : ERPAppServiceBase, IEmployeeAdvancesAppService
    {
        private readonly IRepository<EmployeeAdvances> _EmployeeAdvancesRepository;
        private readonly IRepository<EmployeeSalary.EmployeeSalary> _EmployeeSalary;
        private readonly IEmployeeAdvancesExcelExporter _EmployeeAdvancesExcelExporter;
        private readonly CommonAppService _commonappRepository;
        private readonly IRepository<User, long> _userRepository;


        public EmployeeAdvancesAppService(IRepository<EmployeeAdvances> EmployeeAdvancesRepository,
            IEmployeeAdvancesExcelExporter EmployeeAdvancesExcelExporter, IRepository<User, long> userRepository, CommonAppService commonappRepository, IRepository<EmployeeSalary.EmployeeSalary> EmployeeSalary)
        {
            _EmployeeAdvancesRepository = EmployeeAdvancesRepository;
            _EmployeeAdvancesExcelExporter = EmployeeAdvancesExcelExporter;
            _EmployeeSalary = EmployeeSalary;
            _userRepository = userRepository;
            _commonappRepository = commonappRepository;

        }

        public async Task<PagedResultDto<GetEmployeeAdvancesForViewDto>> GetAll(GetAllEmployeeAdvancesInput input)
        {

            var filteredEmployeeAdvances = _EmployeeAdvancesRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter));
                      

            var pagedAndFilteredEmployeeAdvances = filteredEmployeeAdvances
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var EmployeeAdvances = from o in pagedAndFilteredEmployeeAdvances
                                     select new GetEmployeeAdvancesForViewDto()
                                     {
                                         EmployeeAdvances = new EmployeeAdvancesDto
                                         {
                                             AdvanceID = o.AdvanceID,
                                             EmployeeID = o.EmployeeID,
                                             EmployeeName = o.EmployeeName,
                                             SalaryYear = o.SalaryYear,
                                             SalaryMonth = o.SalaryMonth,
                                             AdvanceDate = o.AdvanceDate,
                                             Amount = o.Amount,
                                             Active = o.Active,
                                             Remarks = o.Remarks,
                                             AudtUser = o.AudtUser,
                                             AudtDate = o.AudtDate,
                                             CreatedBy = o.CreatedBy,
                                             CreateDate = o.CreateDate,
                                             Id = o.Id
                                         }
                                     };

            var totalCount = await filteredEmployeeAdvances.CountAsync();

            return new PagedResultDto<GetEmployeeAdvancesForViewDto>(
                totalCount,
                await EmployeeAdvances.ToListAsync()
            );
        }

        [AbpAuthorize(AppPermissions.PayRoll_EmployeeAdvances_Edit)]
        public async Task<GetEmployeeAdvancesForEditOutput> GetEmployeeAdvancesForEdit(EntityDto input)
        {
            var EmployeeAdvances = await _EmployeeAdvancesRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetEmployeeAdvancesForEditOutput { EmployeeAdvances = ObjectMapper.Map<CreateOrEditEmployeeAdvancesDto>(EmployeeAdvances) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditEmployeeAdvancesDto input)
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

        [AbpAuthorize(AppPermissions.PayRoll_EmployeeAdvances_Create)]
        protected virtual async Task Create(CreateOrEditEmployeeAdvancesDto input)
        {
            var EmployeeAdvances = ObjectMapper.Map<EmployeeAdvances>(input);


            if (AbpSession.TenantId != null)
            {
                EmployeeAdvances.TenantId = (int)AbpSession.TenantId;
            }

            EmployeeAdvances.AdvanceID = GetMaxID();

            await _EmployeeAdvancesRepository.InsertAsync(EmployeeAdvances);
        }

        [AbpAuthorize(AppPermissions.PayRoll_EmployeeAdvances_Edit)]
        protected virtual async Task Update(CreateOrEditEmployeeAdvancesDto input)
        {
            var EmployeeAdvances = await _EmployeeAdvancesRepository.FirstOrDefaultAsync((int)input.Id);
            if (input.TenantID==0)
            {
                input.TenantID = Convert.ToInt32(AbpSession.TenantId);
            }
            ObjectMapper.Map(input, EmployeeAdvances);
        }

        [AbpAuthorize(AppPermissions.PayRoll_EmployeeAdvances_Delete)]
        public async Task Delete(EntityDto input)
        {
            DeleteLog(input.Id);
            await _EmployeeAdvancesRepository.DeleteAsync(input.Id);
        }
        public void DeleteLog(int detid)
        {
            var data = _EmployeeAdvancesRepository.FirstOrDefault(c => c.Id == detid);
            LogModel model = new LogModel()
            {
                Action = "Delete",
                Detid = detid,
                DocNo = data.AdvanceID,
                FormName = "Advance",
                Status = true,
                ApprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName,
                TenantId = AbpSession.TenantId
            };
            _commonappRepository.ApproveLog(model);
        }
        public async Task<FileDto> GetEmployeeAdvancesToExcel(GetAllEmployeeAdvancesForExcelInput input)
        {

            var filteredEmployeeAdvances = _EmployeeAdvancesRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter));

            var query = (from o in filteredEmployeeAdvances
                         select new GetEmployeeAdvancesForViewDto()
                         {
                             EmployeeAdvances = new EmployeeAdvancesDto
                             {
                                 AdvanceID = o.AdvanceID,
                                 EmployeeID = o.EmployeeID,
                                 EmployeeName = o.EmployeeName,
                                 SalaryYear = o.SalaryYear,
                                 SalaryMonth = o.SalaryMonth,
                                 AdvanceDate = o.AdvanceDate,
                                 Amount = o.Amount,
                                 Active = o.Active,
                                 AudtUser = o.AudtUser,
                                 AudtDate = o.AudtDate,
                                 CreatedBy = o.CreatedBy,
                                 CreateDate = o.CreateDate,
                                 Id = o.Id
                             }
                         });


            var EmployeeAdvancesListDtos = await query.ToListAsync();

            return _EmployeeAdvancesExcelExporter.ExportToFile(EmployeeAdvancesListDtos);
        }
        public int GetMaxID()
        {
            var maxid = ((from tab1 in _EmployeeAdvancesRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId) select (int?)tab1.AdvanceID).Max() ?? 0) + 1;
            return maxid;
        }

        public bool GetIsValidAmount(int advanceAmount, int EmpID)
        {
            var salary = _EmployeeSalary.FirstOrDefault(x => x.EmployeeID == EmpID && x.TenantId == AbpSession.TenantId).Gross_Salary;

            return advanceAmount <= salary;
        }
    }
}