using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using ERP.GeneralLedger.SetupForms;
using Abp.Linq.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ERP.Finders.Dtos;
using Abp.Collections.Extensions;
using System.Linq;
using ERP.AccountPayables;
using ERP.PayRoll.Employees;
using ERP.PayRoll.Shifts;
using ERP.PayRoll.Education;
using ERP.PayRoll.Religion;
using ERP.PayRoll.Department;
using ERP.PayRoll.Designation;
using ERP.PayRoll.EmployeeType;
using ERP.PayRoll.Section;
using ERP.PayRoll.SubDesignations;
using System.Globalization;
using ERP.PayRoll.EmployeeLoans;
using ERP.PayRoll.EmployeeLoansType;
using ERP.PayRoll.Location;

namespace ERP.Finders.PayRoll
{
    public class PayRollFindersAppService : ERPAppServiceBase
    {
        private readonly IRepository<Employees> _employeeRepository;
        private readonly IRepository<EmployeeLoansTypes> _employeeLoansTypesRepository;
        private readonly IRepository<Shift> _shiftRepository;
        private readonly IRepository<Education> _educationRepository;
        private readonly IRepository<Religions> _religionRepository;
        private readonly IRepository<Department> _departmentRepository;
        private readonly IRepository<Designations> _designationRepository;
        private readonly IRepository<EmployeeType> _employmentTypeRepository;
        private readonly IRepository<Section> _sectionRepository;
        private readonly IRepository<SubDesignations> _subDesignationsRepository;
        private readonly IRepository<Locations> _locationRepository;
        private readonly IRepository<ChartofControl, string> _chartofControlRepository;

        public PayRollFindersAppService(
            IRepository<Employees> employeeRepository,
            IRepository<Shift> shiftRepository,
            IRepository<ChartofControl, string> chartofControlRepository,
            IRepository<Education> educationRepository,
            IRepository<Religions> religionRepository,
            IRepository<Department> departmentRepository,
            IRepository<Designations> designationRepository,
            IRepository<EmployeeType> employmentTypeRepository,
            IRepository<Section> sectionRepository,
            IRepository<SubDesignations> subDesignationsRepository,
            IRepository<EmployeeLoansTypes> employeeLoansTypesRepository,
            IRepository<Locations> locationRepository
            )
        {
            _employeeRepository = employeeRepository;
            _shiftRepository = shiftRepository;
            _educationRepository = educationRepository;
            _religionRepository = religionRepository;
            _chartofControlRepository = chartofControlRepository;
            _departmentRepository = departmentRepository;
            _designationRepository = designationRepository;
            _employmentTypeRepository = employmentTypeRepository;
            _sectionRepository = sectionRepository;
            _subDesignationsRepository = subDesignationsRepository;
            _employeeLoansTypesRepository = employeeLoansTypesRepository;
            _locationRepository = locationRepository;
        }

        public async Task<PagedResultDto<PayRollFindersDto>> GetPayRollLookupTable(GetAllForLookupTableInput input)
        {
            var resultDtos = new LookupDto<PayRollFindersDto>();
            switch (input.Target)
            {
                case "Employee":
                    resultDtos = await GetAllEmployeeForLookupTable(input);
                    break;
                case "EmployeeLoansType":
                    resultDtos = await GetAllEmployeeLoansTypeForLookupTable(input);
                    break;
                case "Shift":
                    resultDtos = await GetAllShiftForLookupTable(input);
                    break;
                case "Education":
                    resultDtos = await GetAllEducationForLookupTable(input);
                    break;
                case "Religion":
                    resultDtos = await GetAllReligionForLookupTable(input);
                    break;
                case "Department":
                    resultDtos = await GetAllDepartmentForLookupTable(input);
                    break;
                case "Designation":
                    resultDtos = await GetAllDesignationForLookupTable(input);
                    break;
                case "SubDesignation":
                    resultDtos = await GetAllSubDesignationForLookupTable(input);
                    break;
                case "EmploymentType":
                    resultDtos = await GetAllEmploymentTypeForLookupTable(input);
                    break;
                case "Section":
                    resultDtos = await GetAllSectionForLookupTable(input);
                    break;
                case "Location":
                    resultDtos = await GetAllLocationForLookupTable(input);
                    break;
                case "Allowancetype":
                    resultDtos = await GetAllAllowanceTypeForLookupTable(input);
                    break;
                case "ChartOfAccount":
                    resultDtos = await GetAllChartOfAccountForLookupTable(input);
                    break;
            }
            return new PagedResultDto<PayRollFindersDto>(
                    resultDtos.Count,
                    resultDtos.Items
                );
        }
        private async Task<LookupDto<PayRollFindersDto>> GetAllChartOfAccountForLookupTable(GetAllForLookupTableInput input)
        {

            var TenantID = (Int32)AbpSession.TenantId;
            var query = _chartofControlRepository.GetAll().WhereIf(
                  !string.IsNullOrWhiteSpace(input.Filter),
                 e => false || e.Id.ToString().Trim().ToUpper().Contains(input.Filter.ToUpper()) || e.AccountName.Trim().ToUpper().Contains(input.Filter.ToUpper())
               ).Where(o => o.TenantId == AbpSession.TenantId);

            var lookupTableDto = from o in query
                                 select new PayRollFindersDto
                                 {
                                     Id = o.Id.ToString(),
                                     DisplayName = o.AccountName
                                 };


            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<PayRollFindersDto>(
               lookupTableDtoList.Count,
                lookupTableDtoList
            );

            return getData;
        }

        private async Task<LookupDto<PayRollFindersDto>> GetAllAllowanceTypeForLookupTable(GetAllForLookupTableInput input)
        {
          
            PayRollFindersDto dto = new PayRollFindersDto();
            List<PayRollFindersDto> lookupTableDto = new List<PayRollFindersDto>();
            for (int i=1;i<3;i++)
            {
                dto = new PayRollFindersDto();
                if (i==1)
                {
                    dto.Id = "1";
                    dto.DisplayName = "Car";
                }
                if (i == 2)
                {
                    dto.Id = "2";
                    dto.DisplayName = "Motor Cycle";
                }

                lookupTableDto.Add(dto);
            }

          
            var getData = new LookupDto<PayRollFindersDto>(
               2,
                lookupTableDto
            );

            return getData;
        }
        private async Task<LookupDto<PayRollFindersDto>> GetAllEmployeeLoansTypeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _employeeLoansTypesRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);

            var lookupTableDto = from o in query
                                 select new PayRollFindersDto
                                 {
                                     Id = o.LoanTypeId.ToString(),
                                     DisplayName = o.LoanTypeName
                                 };

            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<PayRollFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }
        private async Task<LookupDto<PayRollFindersDto>> GetAllEmployeeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _employeeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => false || e.EmployeeID.ToString().Trim().ToUpper().Contains(input.Filter.ToUpper()) || e.EmployeeName.Trim().ToUpper().Contains(input.Filter.ToUpper())
                ).Where(o => o.TenantId == AbpSession.TenantId);

            var lookupTableDto = from o in query
                                 select new PayRollFindersDto
                                 {
                                     Id = o.EmployeeID.ToString(),
                                     DisplayName = o.EmployeeName,
                                     JoiningDate = o.date_of_joining.Value.Year + "/" + o.date_of_joining.Value.Month + "/" + o.date_of_joining.Value.Day
                                 };

            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<PayRollFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }
        private async Task<LookupDto<PayRollFindersDto>> GetAllShiftForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _shiftRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => false || e.ShiftID.ToString().Trim().ToUpper().Contains(input.Filter.ToUpper()) || e.ShiftName.Trim().ToUpper().Contains(input.Filter.ToUpper())
                ).Where(o => o.TenantId == AbpSession.TenantId);

            var lookupTableDto = from o in query
                                 select new PayRollFindersDto
                                 {
                                     Id = o.ShiftID.ToString(),
                                     DisplayName = o.ShiftName,
                                     DutyHours = o.TotalHour.ToString()
                                 };

            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<PayRollFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }

        private async Task<LookupDto<PayRollFindersDto>> GetAllEducationForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _educationRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => false || e.EdID.ToString().Trim().ToUpper().Contains(input.Filter.ToUpper()) || e.Eduction.Trim().ToUpper().Contains(input.Filter.ToUpper())
                ).Where(o => o.TenantId == AbpSession.TenantId);

            var lookupTableDto = from o in query
                                 select new PayRollFindersDto
                                 {
                                     Id = o.EdID.ToString(),
                                     DisplayName = o.Eduction
                                 };

            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<PayRollFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }

        private async Task<LookupDto<PayRollFindersDto>> GetAllReligionForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _religionRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => false || e.ReligionID.ToString().Trim().ToUpper().Contains(input.Filter.ToUpper()) || e.Religion.Trim().ToUpper().Contains(input.Filter.ToUpper())
                ).Where(o => o.TenantId == AbpSession.TenantId);

            var lookupTableDto = from o in query
                                 select new PayRollFindersDto
                                 {
                                     Id = o.ReligionID.ToString(),
                                     DisplayName = o.Religion
                                 };

            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<PayRollFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }

        private async Task<LookupDto<PayRollFindersDto>> GetAllDepartmentForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _departmentRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => false || e.DeptID.ToString().Trim().ToUpper().Contains(input.Filter.ToUpper()) || e.DeptName.Trim().ToUpper().Contains(input.Filter.ToUpper())
                ).Where(o => o.TenantId == AbpSession.TenantId);

            var lookupTableDto = from o in query
                                 select new PayRollFindersDto
                                 {
                                     Id = o.DeptID.ToString(),
                                     DisplayName = o.DeptName
                                 };

            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<PayRollFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }

        private async Task<LookupDto<PayRollFindersDto>> GetAllSubDesignationForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _subDesignationsRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => false || e.SubDesignationID.ToString().Trim().ToUpper().Contains(input.Filter.ToUpper()) || e.SubDesignation.Trim().ToUpper().Contains(input.Filter.ToUpper())
                ).Where(o => o.TenantId == AbpSession.TenantId);

            var lookupTableDto = from o in query
                                 select new PayRollFindersDto
                                 {
                                     Id = o.SubDesignationID.ToString(),
                                     DisplayName = o.SubDesignation
                                 };

            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<PayRollFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }

        private async Task<LookupDto<PayRollFindersDto>> GetAllDesignationForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _designationRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => false || e.DesignationID.ToString().Trim().ToUpper().Contains(input.Filter.ToUpper()) || e.Designation.Trim().ToUpper().Contains(input.Filter.ToUpper())
                ).Where(o => o.TenantId == AbpSession.TenantId);

            var lookupTableDto = from o in query
                                 select new PayRollFindersDto
                                 {
                                     Id = o.DesignationID.ToString(),
                                     DisplayName = o.Designation
                                 };

            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<PayRollFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }

        private async Task<LookupDto<PayRollFindersDto>> GetAllEmploymentTypeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _employmentTypeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => false || e.TypeID.ToString().Trim().ToUpper().Contains(input.Filter.ToUpper()) || e.EmpType.Trim().ToUpper().Contains(input.Filter.ToUpper())
                ).Where(o => o.TenantId == AbpSession.TenantId);

            var lookupTableDto = from o in query
                                 select new PayRollFindersDto
                                 {
                                     Id = o.TypeID.ToString(),
                                     DisplayName = o.EmpType
                                 };

            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<PayRollFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }

        private async Task<LookupDto<PayRollFindersDto>> GetAllSectionForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _sectionRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => false || e.SecID.ToString().Trim().ToUpper().Contains(input.Filter.ToUpper()) || e.SecName.Trim().ToUpper().Contains(input.Filter.ToUpper())
                ).Where(o => o.TenantId == AbpSession.TenantId);

            var lookupTableDto = from o in query
                                 select new PayRollFindersDto
                                 {
                                     Id = o.SecID.ToString(),
                                     DisplayName = o.SecName
                                 };

            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<PayRollFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }


        private async Task<LookupDto<PayRollFindersDto>> GetAllLocationForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _locationRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => false || e.LocID.ToString().Trim().ToUpper().Contains(input.Filter.ToUpper()) || e.Location.Trim().ToUpper().Contains(input.Filter.ToUpper())
                ).Where(o => o.TenantId == AbpSession.TenantId);

            var lookupTableDto = from o in query
                                 select new PayRollFindersDto
                                 {
                                     Id = o.LocID.ToString(),
                                     DisplayName = o.Location
                                 };

            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<PayRollFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }

    }
}
