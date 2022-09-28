

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.PayRoll.EmployeeLoansType.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ERP.PayRoll.EmployeeLoansType
{
    [AbpAuthorize(AppPermissions.PayRoll_EmployeeLoansType)]
    public class EmployeeLoansTypeAppService : ERPAppServiceBase, IEmployeeLoansTypeAppService
    {
        private readonly IRepository<EmployeeLoansTypes> _employeeLoansTypeRepository;


        public EmployeeLoansTypeAppService(IRepository<EmployeeLoansTypes> employeeLoansTypeRepository)
        {
            _employeeLoansTypeRepository = employeeLoansTypeRepository;

        }

        public async Task<PagedResultDto<GetEmployeeLoansTypeForViewDto>> GetAll(GetAllEmployeeLoansTypeInput input)
        {

            var filteredEmployeeLoansType = _employeeLoansTypeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.LoanTypeName.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LoanTypeNameFilter), e => e.LoanTypeName == input.LoanTypeNameFilter);

            var pagedAndFilteredEmployeeLoansType = filteredEmployeeLoansType
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var employeeLoansType = from o in pagedAndFilteredEmployeeLoansType
                                    select new GetEmployeeLoansTypeForViewDto()
                                    {
                                        EmployeeLoansType = new EmployeeLoansTypeDto
                                        {
                                            LoanTypeName = o.LoanTypeName,
                                            LoanTypeID = o.LoanTypeId,
                                            Id = o.Id
                                        }
                                    };

            var totalCount = await filteredEmployeeLoansType.CountAsync();

            return new PagedResultDto<GetEmployeeLoansTypeForViewDto>(
                totalCount,
                await employeeLoansType.ToListAsync()
            );
        }

        public async Task<GetEmployeeLoansTypeForViewDto> GetEmployeeLoansTypeForView(int id)
        {
            var employeeLoansType = await _employeeLoansTypeRepository.GetAsync(id);

            var output = new GetEmployeeLoansTypeForViewDto { EmployeeLoansType = ObjectMapper.Map<EmployeeLoansTypeDto>(employeeLoansType) };

            return output;
        }

        [AbpAuthorize(AppPermissions.PayRoll_EmployeeLoansType_Edit)]
        public async Task<GetEmployeeLoansTypeForEditOutput> GetEmployeeLoansTypeForEdit(EntityDto input)
        {
            var employeeLoansType = await _employeeLoansTypeRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetEmployeeLoansTypeForEditOutput { EmployeeLoansType = ObjectMapper.Map<CreateOrEditEmployeeLoansTypeDto>(employeeLoansType) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditEmployeeLoansTypeDto input)
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

        [AbpAuthorize(AppPermissions.PayRoll_EmployeeLoansType_Create)]
        protected virtual async Task Create(CreateOrEditEmployeeLoansTypeDto input)
        {
            var employeeLoansType = ObjectMapper.Map<EmployeeLoansTypes>(input);


            if (AbpSession.TenantId != null)
            {
                employeeLoansType.TenantId = (int)AbpSession.TenantId;
            }


            await _employeeLoansTypeRepository.InsertAsync(employeeLoansType);
        }

        [AbpAuthorize(AppPermissions.PayRoll_EmployeeLoansType_Edit)]
        protected virtual async Task Update(CreateOrEditEmployeeLoansTypeDto input)
        {
            var employeeLoansType = await _employeeLoansTypeRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, employeeLoansType);
        }

        [AbpAuthorize(AppPermissions.PayRoll_EmployeeLoansType_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _employeeLoansTypeRepository.DeleteAsync(input.Id);
        }


        public async Task<int> GetMaxID()
        {
            var employee = _employeeLoansTypeRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId);
            int id = employee.Count() > 0 ? await employee.MaxAsync(x => x.LoanTypeId) + 1 : 1;
            return id;
        }
    }
}