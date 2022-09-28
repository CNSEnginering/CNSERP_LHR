

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.PayRoll.Employees.Exporting;
using ERP.PayRoll.Employees.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ERP.PayRoll.Location;

namespace ERP.PayRoll.Employees
{
    [AbpAuthorize(AppPermissions.PayRoll_Employees)]
    public class EmployeesAppService : ERPAppServiceBase, IEmployeesAppService
    {
        private readonly IRepository<Employees> _employeesRepository;
        private readonly IRepository<Education.Education> _educationRepository;
        private readonly IRepository<Religion.Religions> _religionRepository;
        private readonly IRepository<Locations> _locationRepository;
        private readonly IRepository<Department.Department> _departmentRepository;
        private readonly IRepository<Designation.Designations> _designationRepository;
        private readonly IRepository<SubDesignations.SubDesignations> _subDesignationsRepository;
        private readonly IRepository<Shifts.Shift> _shiftRepository;
        private readonly IRepository<EmployeeType.EmployeeType> _employeeTypeRepository;
        private readonly IRepository<Section.Section> _sectionRepository;

        private readonly IEmployeesExcelExporter _employeesExcelExporter;


        public EmployeesAppService(
            IRepository<Employees> employeesRepository,
            IEmployeesExcelExporter employeesExcelExporter,
            IRepository<Education.Education> educationRepository,
            IRepository<Religion.Religions> religionRepository,
            IRepository<Locations> locationRepository,
            IRepository<Department.Department> departmentRepository,
            IRepository<Designation.Designations> designationRepository,
            IRepository<SubDesignations.SubDesignations> subDesignationsRepository,
            IRepository<Shifts.Shift> shiftRepository,
            IRepository<EmployeeType.EmployeeType> employeeTypeRepository,
            IRepository<Section.Section> sectionRepository
      )
        {
            _employeesRepository = employeesRepository;
            _employeesExcelExporter = employeesExcelExporter;
            _educationRepository = educationRepository;
            _religionRepository = religionRepository;
            _locationRepository = locationRepository;
            _departmentRepository = departmentRepository;
            _designationRepository = designationRepository;
            _subDesignationsRepository = subDesignationsRepository;
            _shiftRepository = shiftRepository;
            _employeeTypeRepository = employeeTypeRepository;
            _sectionRepository = sectionRepository;



        }

        public async Task<PagedResultDto<GetEmployeesForViewDto>> GetAll(GetAllEmployeesInput input)
        {

            var filteredEmployees = _employeesRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.EmployeeName.Contains(input.Filter) || e.FatherName.Contains(input.Filter) || e.home_address.Contains(input.Filter) || e.PhoneNo.Contains(input.Filter) || e.NTN.Contains(input.Filter) || e.City.Contains(input.Filter) || e.Cnic.Contains(input.Filter) || e.Gender.Contains(input.Filter) || e.payment_mode.Contains(input.Filter) || e.bank_name.Contains(input.Filter) || e.account_no.Contains(input.Filter) || e.academic_qualification.Contains(input.Filter) || e.professional_qualification.Contains(input.Filter) || e.BloodGroup.Contains(input.Filter) || e.Reference.Contains(input.Filter) || e.Visa_Details.Contains(input.Filter) || e.Driving_Licence.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinEmployeeIDFilter != null, e => e.EmployeeID >= input.MinEmployeeIDFilter)
                        .WhereIf(input.MaxEmployeeIDFilter != null, e => e.EmployeeID <= input.MaxEmployeeIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter), e => e.EmployeeName == input.EmployeeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FatherNameFilter), e => e.FatherName == input.FatherNameFilter)
                        .WhereIf(input.Mindate_of_birthFilter != null, e => e.date_of_birth >= input.Mindate_of_birthFilter)
                        .WhereIf(input.Maxdate_of_birthFilter != null, e => e.date_of_birth <= input.Maxdate_of_birthFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.home_addressFilter), e => e.home_address == input.home_addressFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PhoneNoFilter), e => e.PhoneNo == input.PhoneNoFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NTN_Filter), e => e.NTN == input.NTN_Filter)
                        .WhereIf(input.Minapointment_dateFilter != null, e => e.apointment_date >= input.Minapointment_dateFilter)
                        .WhereIf(input.Maxapointment_dateFilter != null, e => e.apointment_date <= input.Maxapointment_dateFilter)
                        .WhereIf(input.Mindate_of_joiningFilter != null, e => e.date_of_joining >= input.Mindate_of_joiningFilter)
                        .WhereIf(input.Maxdate_of_joiningFilter != null, e => e.date_of_joining <= input.Maxdate_of_joiningFilter)
                        .WhereIf(input.Mindate_of_leavingFilter != null, e => e.date_of_leaving >= input.Mindate_of_leavingFilter)
                        .WhereIf(input.Maxdate_of_leavingFilter != null, e => e.date_of_leaving <= input.Maxdate_of_leavingFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityFilter), e => e.City == input.CityFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CnicFilter), e => e.Cnic == input.CnicFilter)
                        .WhereIf(input.MinEdIDFilter != null, e => e.EdID >= input.MinEdIDFilter)
                        .WhereIf(input.MaxEdIDFilter != null, e => e.EdID <= input.MaxEdIDFilter)
                        .WhereIf(input.MinDeptIDFilter != null, e => e.DeptID >= input.MinDeptIDFilter)
                        .WhereIf(input.MaxDeptIDFilter != null, e => e.DeptID <= input.MaxDeptIDFilter)
                        .WhereIf(input.MinDesignationIDFilter != null, e => e.DesignationID >= input.MinDesignationIDFilter)
                        .WhereIf(input.MaxDesignationIDFilter != null, e => e.DesignationID <= input.MaxDesignationIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.GenderFilter), e => e.Gender == input.GenderFilter)
                        //.WhereIf(input.StatusFilter > -1, e => (input.StatusFilter == 1 && e.Status) || (input.StatusFilter == 0 && !e.Status))
                        //.WhereIf(input.StatusFilter > -1, e => Convert.ToInt32(e.Status) == input.StatusFilter)

                        .WhereIf(input.MinShiftIDFilter != null, e => e.ShiftID >= input.MinShiftIDFilter)
                        .WhereIf(input.MaxShiftIDFilter != null, e => e.ShiftID <= input.MaxShiftIDFilter)
                        .WhereIf(input.MinTypeIDFilter != null, e => e.TypeID >= input.MinTypeIDFilter)
                        .WhereIf(input.MaxTypeIDFilter != null, e => e.TypeID <= input.MaxTypeIDFilter)
                        .WhereIf(input.MinSecIDFilter != null, e => e.SecID >= input.MinSecIDFilter)
                        .WhereIf(input.MaxSecIDFilter != null, e => e.SecID <= input.MaxSecIDFilter)
                        .WhereIf(input.MinReligionIDFilter != null, e => e.ReligionID >= input.MinReligionIDFilter)
                        .WhereIf(input.MaxReligionIDFilter != null, e => e.ReligionID <= input.MaxReligionIDFilter)
                        //.WhereIf(input.social_securityFilter > -1, e => (input.social_securityFilter == 1 && e.social_security) || (input.social_securityFilter == 0 && !e.social_security))
                        //.WhereIf(input.eobiFilter > -1, e => (input.eobiFilter == 1 && e.eobi) || (input.eobiFilter == 0 && !e.eobi))
                        //.WhereIf(input.wppfFilter > -1, e => (input.wppfFilter == 1 && e.wppf) || (input.wppfFilter == 0 && !e.wppf))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.payment_modeFilter), e => e.payment_mode == input.payment_modeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.bank_nameFilter), e => e.bank_name == input.bank_nameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.account_noFilter), e => e.account_no == input.account_noFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.academic_qualificationFilter), e => e.academic_qualification == input.academic_qualificationFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.professional_qualificationFilter), e => e.professional_qualification == input.professional_qualificationFilter)
                        .WhereIf(input.Minfirst_rest_daysFilter != null, e => e.first_rest_days >= input.Minfirst_rest_daysFilter)
                        .WhereIf(input.Maxfirst_rest_daysFilter != null, e => e.first_rest_days <= input.Maxfirst_rest_daysFilter)
                        .WhereIf(input.Minsecond_rest_daysFilter != null, e => e.second_rest_days >= input.Minsecond_rest_daysFilter)
                        .WhereIf(input.Maxsecond_rest_daysFilter != null, e => e.second_rest_days <= input.Maxsecond_rest_daysFilter)
                        .WhereIf(input.Minfirst_rest_days_w2Filter != null, e => e.first_rest_days_w2 >= input.Minfirst_rest_days_w2Filter)
                        .WhereIf(input.Maxfirst_rest_days_w2Filter != null, e => e.first_rest_days_w2 <= input.Maxfirst_rest_days_w2Filter)
                        .WhereIf(input.Minsecond_rest_days_w2Filter != null, e => e.second_rest_days_w2 >= input.Minsecond_rest_days_w2Filter)
                        .WhereIf(input.Maxsecond_rest_days_w2Filter != null, e => e.second_rest_days_w2 <= input.Maxsecond_rest_days_w2Filter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BloodGroupFilter), e => e.BloodGroup == input.BloodGroupFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ReferenceFilter), e => e.Reference == input.ReferenceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Visa_DetailsFilter), e => e.Reference == input.Visa_DetailsFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Driving_LicenceFilter), e => e.Reference == input.Driving_LicenceFilter)
                        .WhereIf(input.MinDuty_HoursFilter != null, e => e.Duty_Hours >= input.MinDuty_HoursFilter)
                        .WhereIf(input.MaxDuty_HoursFilter != null, e => e.Duty_Hours <= input.MaxDuty_HoursFilter)
                        //.WhereIf(input.ConfirmedFilter > -1, e => (input.ConfirmedFilter == 1 && e.Confirmed) || (input.ConfirmedFilter == 0 && !e.Confirmed))
                        //.WhereIf(input.ActiveFilter > -1, e => Convert.ToInt32(e.Active) == input.ActiveFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var pagedAndFilteredEmployees = filteredEmployees
                .OrderBy(input.Sorting ?? "EmployeeID desc")
                .PageBy(input);

            var employees = from o in pagedAndFilteredEmployees
                            select new GetEmployeesForViewDto()
                            {
                                Employees = new EmployeesDto
                                {
                                    EmployeeID = o.EmployeeID,
                                    EmployeeName = o.EmployeeName,
                                    FatherName = o.FatherName,
                                    date_of_birth = o.date_of_birth,
                                    home_address = o.home_address,
                                    PhoneNo = o.PhoneNo,
                                    NTN = o.NTN,
                                    apointment_date = o.apointment_date,
                                    date_of_joining = o.date_of_joining,
                                    date_of_leaving = o.date_of_leaving,
                                    City = o.City,
                                    Cnic = o.Cnic,
                                    EdID = o.EdID,
                                    Department=_departmentRepository.GetAll().Where(c=>c.TenantId==AbpSession.TenantId && c.DeptID==o.DeptID).FirstOrDefault().DeptName,
                                    Designation=_designationRepository.GetAll().Where(v=>v.TenantId==AbpSession.TenantId && v.DesignationID==o.DesignationID).FirstOrDefault().Designation,
                                    DeptID = o.DeptID,
                                    DesignationID = o.DesignationID,
                                    Gender = o.Gender,
                                    Status = o.Status,
                                    ShiftID = o.ShiftID,
                                    TypeID = o.TypeID,
                                    SecID = o.SecID,
                                    ReligionID = o.ReligionID,
                                    social_security = o.social_security,
                                    eobi = o.eobi,
                                    EBankID=o.EBankID,
                                    wppf = o.wppf,
                                    payment_mode = o.payment_mode,
                                    bank_name = o.bank_name,
                                    account_no = o.account_no,
                                    academic_qualification = o.academic_qualification,
                                    professional_qualification = o.professional_qualification,
                                    first_rest_days = o.first_rest_days,
                                    second_rest_days = o.second_rest_days,
                                    first_rest_days_w2 = o.first_rest_days_w2,
                                    second_rest_days_w2 = o.second_rest_days_w2,
                                    BloodGroup = o.BloodGroup,
                                    Reference = o.Reference,
                                    Visa_Details = o.Visa_Details,
                                    Driving_Licence = o.Driving_Licence,
                                    Duty_Hours = o.Duty_Hours,
                                    Active = o.Active,
                                    Confirmed = o.Confirmed,
                                    AudtUser = o.AudtUser,
                                    AudtDate = o.AudtDate,
                                    CreatedBy = o.CreatedBy,
                                    CreateDate = o.CreateDate,
                                    WaveOff = o.WaveOff,
                                    type_of_notice_period = o.type_of_notice_period,
                                    Reinstate = o.Reinstate,
                                    ReinstateDate = o.ReinstateDate,
                                    ReinstateReason = o.ReinstateReason,
                                    Id = o.Id,
                                    
                                }
                            };

            var totalCount = await filteredEmployees.CountAsync();

            return new PagedResultDto<GetEmployeesForViewDto>(
                totalCount,
                await employees.ToListAsync()
            );
        }

        [AbpAuthorize(AppPermissions.PayRoll_Employees_Edit)]
        public async Task<GetEmployeesForEditOutput> GetEmployeesForEdit(EntityDto input)
        {
            var employees = await _employeesRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetEmployeesForEditOutput { Employees = ObjectMapper.Map<CreateOrEditEmployeesDto>(employees) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditEmployeesDto input)
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

        [AbpAuthorize(AppPermissions.PayRoll_Employees_Create)]
        protected virtual async Task Create(CreateOrEditEmployeesDto input)
        {
            var employees = ObjectMapper.Map<Employees>(input);


            if (AbpSession.TenantId != null)
            {
                employees.TenantId = (int)AbpSession.TenantId;
            }


            await _employeesRepository.InsertAsync(employees);
        }

        [AbpAuthorize(AppPermissions.PayRoll_Employees_Edit)]
        protected virtual async Task Update(CreateOrEditEmployeesDto input)
        {
            input.AllowanceAmt = input.AllowanceAmt ?? 0;
            input.AllowanceQty = input.AllowanceQty ?? 0;
            var employees = await _employeesRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, employees);
        }

        [AbpAuthorize(AppPermissions.PayRoll_Employees_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _employeesRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetEmployeesToExcel(GetAllEmployeesForExcelInput input)
        {

            var filteredEmployees = _employeesRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.EmployeeName.Contains(input.Filter) || e.FatherName.Contains(input.Filter) || e.home_address.Contains(input.Filter) || e.PhoneNo.Contains(input.Filter) || e.NTN.Contains(input.Filter) || e.City.Contains(input.Filter) || e.Cnic.Contains(input.Filter) || e.Gender.Contains(input.Filter) || e.payment_mode.Contains(input.Filter) || e.bank_name.Contains(input.Filter) || e.account_no.Contains(input.Filter) || e.academic_qualification.Contains(input.Filter) || e.professional_qualification.Contains(input.Filter) || e.BloodGroup.Contains(input.Filter) || e.Reference.Contains(input.Filter) || e.Visa_Details.Contains(input.Filter) || e.Driving_Licence.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinEmployeeIDFilter != null, e => e.EmployeeID >= input.MinEmployeeIDFilter)
                        .WhereIf(input.MaxEmployeeIDFilter != null, e => e.EmployeeID <= input.MaxEmployeeIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter), e => e.EmployeeName == input.EmployeeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FatherNameFilter), e => e.FatherName == input.FatherNameFilter)
                        .WhereIf(input.Mindate_of_birthFilter != null, e => e.date_of_birth >= input.Mindate_of_birthFilter)
                        .WhereIf(input.Maxdate_of_birthFilter != null, e => e.date_of_birth <= input.Maxdate_of_birthFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.home_addressFilter), e => e.home_address == input.home_addressFilter)
                        .WhereIf(input.Minapointment_dateFilter != null, e => e.apointment_date >= input.Minapointment_dateFilter)
                        .WhereIf(input.Maxapointment_dateFilter != null, e => e.apointment_date <= input.Maxapointment_dateFilter)
                        .WhereIf(input.Mindate_of_joiningFilter != null, e => e.date_of_joining >= input.Mindate_of_joiningFilter)
                        .WhereIf(input.Maxdate_of_joiningFilter != null, e => e.date_of_joining <= input.Maxdate_of_joiningFilter)
                        .WhereIf(input.Mindate_of_leavingFilter != null, e => e.date_of_leaving >= input.Mindate_of_leavingFilter)
                        .WhereIf(input.Maxdate_of_leavingFilter != null, e => e.date_of_leaving <= input.Maxdate_of_leavingFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityFilter), e => e.City == input.CityFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CnicFilter), e => e.Cnic == input.CnicFilter)
                        .WhereIf(input.MinEdIDFilter != null, e => e.EdID >= input.MinEdIDFilter)
                        .WhereIf(input.MaxEdIDFilter != null, e => e.EdID <= input.MaxEdIDFilter)
                        .WhereIf(input.MinDeptIDFilter != null, e => e.DeptID >= input.MinDeptIDFilter)
                        .WhereIf(input.MaxDeptIDFilter != null, e => e.DeptID <= input.MaxDeptIDFilter)
                        .WhereIf(input.MinDesignationIDFilter != null, e => e.DesignationID >= input.MinDesignationIDFilter)
                        .WhereIf(input.MaxDesignationIDFilter != null, e => e.DesignationID <= input.MaxDesignationIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.GenderFilter), e => e.Gender == input.GenderFilter)
                        //.WhereIf(input.StatusFilter > -1,  e => (input.StatusFilter == 1 && e.Status) || (input.StatusFilter == 0 && !e.Status) )
                        .WhereIf(input.MinShiftIDFilter != null, e => e.ShiftID >= input.MinShiftIDFilter)
                        .WhereIf(input.MaxShiftIDFilter != null, e => e.ShiftID <= input.MaxShiftIDFilter)
                        .WhereIf(input.MinTypeIDFilter != null, e => e.TypeID >= input.MinTypeIDFilter)
                        .WhereIf(input.MaxTypeIDFilter != null, e => e.TypeID <= input.MaxTypeIDFilter)
                        .WhereIf(input.MinSecIDFilter != null, e => e.SecID >= input.MinSecIDFilter)
                        .WhereIf(input.MaxSecIDFilter != null, e => e.SecID <= input.MaxSecIDFilter)
                        .WhereIf(input.MinReligionIDFilter != null, e => e.ReligionID >= input.MinReligionIDFilter)
                        .WhereIf(input.MaxReligionIDFilter != null, e => e.ReligionID <= input.MaxReligionIDFilter)
                        //.WhereIf(input.social_securityFilter > -1,  e => (input.social_securityFilter == 1 && e.social_security) || (input.social_securityFilter == 0 && !e.social_security) )
                        //.WhereIf(input.eobiFilter > -1,  e => (input.eobiFilter == 1 && e.eobi) || (input.eobiFilter == 0 && !e.eobi) )
                        //.WhereIf(input.wppfFilter > -1,  e => (input.wppfFilter == 1 && e.wppf) || (input.wppfFilter == 0 && !e.wppf) )
                        .WhereIf(!string.IsNullOrWhiteSpace(input.payment_modeFilter), e => e.payment_mode == input.payment_modeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.bank_nameFilter), e => e.bank_name == input.bank_nameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.account_noFilter), e => e.account_no == input.account_noFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.academic_qualificationFilter), e => e.academic_qualification == input.academic_qualificationFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.professional_qualificationFilter), e => e.professional_qualification == input.professional_qualificationFilter)
                        .WhereIf(input.Minfirst_rest_daysFilter != null, e => e.first_rest_days >= input.Minfirst_rest_daysFilter)
                        .WhereIf(input.Maxfirst_rest_daysFilter != null, e => e.first_rest_days <= input.Maxfirst_rest_daysFilter)
                        .WhereIf(input.Minsecond_rest_daysFilter != null, e => e.second_rest_days >= input.Minsecond_rest_daysFilter)
                        .WhereIf(input.Maxsecond_rest_daysFilter != null, e => e.second_rest_days <= input.Maxsecond_rest_daysFilter)
                        .WhereIf(input.Minfirst_rest_days_w2Filter != null, e => e.first_rest_days_w2 >= input.Minfirst_rest_days_w2Filter)
                        .WhereIf(input.Maxfirst_rest_days_w2Filter != null, e => e.first_rest_days_w2 <= input.Maxfirst_rest_days_w2Filter)
                        .WhereIf(input.Minsecond_rest_days_w2Filter != null, e => e.second_rest_days_w2 >= input.Minsecond_rest_days_w2Filter)
                        .WhereIf(input.Maxsecond_rest_days_w2Filter != null, e => e.second_rest_days_w2 <= input.Maxsecond_rest_days_w2Filter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BloodGroupFilter), e => e.BloodGroup == input.BloodGroupFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ReferenceFilter), e => e.Reference == input.ReferenceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Visa_DetailsFilter), e => e.Reference == input.Visa_DetailsFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Driving_LicenceFilter), e => e.Reference == input.Driving_LicenceFilter)
                        .WhereIf(input.MinDuty_HoursFilter != null, e => e.Duty_Hours >= input.MinDuty_HoursFilter)
                        .WhereIf(input.MaxDuty_HoursFilter != null, e => e.Duty_Hours <= input.MaxDuty_HoursFilter)
                        //.WhereIf(input.ActiveFilter > -1, e => Convert.ToInt32(e.Active) == input.ActiveFilter)
                        //.WhereIf(input.ConfirmedFilter > -1,  e => (input.ConfirmedFilter == 1 && e.Confirmed) || (input.ConfirmedFilter == 0 && !e.Confirmed) )
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var query = (from o in filteredEmployees
                         select new GetEmployeesForViewDto()
                         {
                             Employees = new EmployeesDto
                             {
                                 EmployeeID = o.EmployeeID,
                                 EmployeeName = o.EmployeeName,
                                 FatherName = o.FatherName,
                                 date_of_birth = o.date_of_birth,
                                 home_address = o.home_address,
                                 PhoneNo = o.home_address,
                                 NTN = o.NTN,
                                 apointment_date = o.apointment_date,
                                 date_of_joining = o.date_of_joining,
                                 date_of_leaving = o.date_of_leaving,
                                 City = o.City,
                                 Cnic = o.Cnic,
                                 EdID = o.EdID,
                                 DeptID = o.DeptID,
                                 DesignationID = o.DesignationID,
                                 Gender = o.Gender,
                                 Status = o.Status,
                                 ShiftID = o.ShiftID,
                                 TypeID = o.TypeID,
                                 SecID = o.SecID,
                                 ReligionID = o.ReligionID,
                                 social_security = o.social_security,
                                 eobi = o.eobi,
                                 wppf = o.wppf,
                                 payment_mode = o.payment_mode,
                                 bank_name = o.bank_name,
                                 account_no = o.account_no,
                                 academic_qualification = o.academic_qualification,
                                 professional_qualification = o.professional_qualification,
                                 first_rest_days = o.first_rest_days,
                                 second_rest_days = o.second_rest_days,
                                 first_rest_days_w2 = o.first_rest_days_w2,
                                 second_rest_days_w2 = o.second_rest_days_w2,
                                 BloodGroup = o.BloodGroup,
                                 Reference = o.Reference,
                                 Visa_Details = o.Visa_Details,
                                 Driving_Licence = o.Driving_Licence,
                                 Duty_Hours = o.Duty_Hours,
                                 Active = o.Active,
                                 Confirmed = o.Confirmed,
                                 AudtUser = o.AudtUser,
                                 AudtDate = o.AudtDate,
                                 CreatedBy = o.CreatedBy,
                                 CreateDate = o.CreateDate,
                                 Id = o.Id
                             }
                         });


            var employeesListDtos = await query.ToListAsync();

            return _employeesExcelExporter.ExportToFile(employeesListDtos);
        }

        public int GetMaxID()
        {
            var maxid = ((from tab1 in _employeesRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId) select (int?)tab1.EmployeeID).Max() ?? 0) + 1;
            return maxid;
        }

        public string GetPickerName(int ID, string type)
        {
            string pickerName;
            switch (type)
            {
                case "Education":
                    pickerName = _educationRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.EdID == ID).Count() > 0 ?
                                       _educationRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.EdID == ID).SingleOrDefault().Eduction : "";
                    break;
                case "Religion":
                    pickerName = _religionRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.ReligionID == ID).Count() > 0 ?
                                       _religionRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.ReligionID == ID).SingleOrDefault().Religion : "";
                    break;
                case "Location":
                    pickerName = _locationRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LocID == ID).Count() > 0 ?
                                       _locationRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LocID == ID).SingleOrDefault().Location : "";
                    break;
                case "Department":
                    pickerName = _departmentRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.DeptID == ID).Count() > 0 ?
                                       _departmentRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.DeptID == ID).SingleOrDefault().DeptName : "";
                    break;
                case "Designation":
                    pickerName = _designationRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.DesignationID == ID).Count() > 0 ?
                                       _designationRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.DesignationID == ID).SingleOrDefault().Designation : "";
                    break;
                case "SubDesignation":
                    pickerName = _subDesignationsRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.SubDesignationID == ID).Count() > 0 ?
                                       _subDesignationsRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.SubDesignationID == ID).SingleOrDefault().SubDesignation : "";
                    break;
                case "Shift":
                    pickerName = _shiftRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.ShiftID == ID).Count() > 0 ?
                                       _shiftRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.ShiftID == ID).SingleOrDefault().ShiftName : "";
                    break;
                case "EmploymentType":
                    pickerName = _employeeTypeRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.TypeID == ID).Count() > 0 ?
                                       _employeeTypeRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.TypeID == ID).SingleOrDefault().EmpType : "";
                    break;
                case "Section":
                    pickerName = _sectionRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.SecID == ID).Count() > 0 ?
                                       _sectionRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.SecID == ID).SingleOrDefault().SecName : "";
                    break;
                default:
                    pickerName = "Invalid Picker Name";
                    break;

            }
            return pickerName;

        }

        public async Task<bool> GetEmployeeIDValid(int empId)
        {
            var employees = await _employeesRepository.FirstOrDefaultAsync(x=>x.EmployeeID == empId && x.TenantId==AbpSession.TenantId);

            return employees != null ? false : true;
        }


        public async Task<List<EmployeesDto>> GetActiveEmployees()
        {
            var employees = await _employeesRepository.GetAllListAsync(x => x.TenantId == AbpSession.TenantId && x.Active == true);

            List<EmployeesDto> activeEmp = new List<EmployeesDto>();

            ObjectMapper.Map(employees, activeEmp);

            return activeEmp;
        }

    }


}