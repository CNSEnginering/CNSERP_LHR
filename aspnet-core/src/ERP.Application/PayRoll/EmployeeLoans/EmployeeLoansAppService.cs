

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.PayRoll.EmployeeLoans.Dtos;
using ERP.Dto;
using ERP.PayRoll.Employees.Dtos;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ERP.Authorization.Users;
using ERP.SupplyChain.Inventory;
using Microsoft.AspNetCore.Mvc;
using ERP.GeneralLedger.Transaction.VoucherEntry;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRD.Dtos;
using ERP.GeneralLedger.Transaction.VoucherEntry.Dtos;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH.Dtos;

namespace ERP.PayRoll.EmployeeLoans
{
    [AbpAuthorize(AppPermissions.PayRoll_EmployeeLoans)]
    public class EmployeeLoansAppService : ERPAppServiceBase, IEmployeeLoansAppService
    {
        private readonly IRepository<EmployeeLoans> _employeeLoansRepository;
        private readonly IRepository<ERP.PayRoll.Employees.Employees> _employeeRepository;
        private readonly IRepository<ERP.PayRoll.EmployeeLoansType.EmployeeLoansTypes> _employeeLoansTypesRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly CommonAppService _commonappRepository;
        private VoucherEntryAppService _voucherEntryAppService;
        private readonly IRepository<hrmSetup.HrmSetup> _hrmsetupRepository;

        public EmployeeLoansAppService(IRepository<EmployeeLoans> employeeLoansRepository, IRepository<User, long> userRepository,
            IRepository<ERP.PayRoll.Employees.Employees> employeeRepository, CommonAppService commonappRepository,
            IRepository<ERP.PayRoll.EmployeeLoansType.EmployeeLoansTypes> employeeLoansTypesRepository, VoucherEntryAppService voucherEntryAppService, IRepository<hrmSetup.HrmSetup> hrmsetupRepository)
        {

            _employeeLoansRepository = employeeLoansRepository;
            _userRepository = userRepository;
            _employeeRepository = employeeRepository;
            _employeeLoansTypesRepository = employeeLoansTypesRepository;
            _commonappRepository = commonappRepository;
            _voucherEntryAppService = voucherEntryAppService;
            _hrmsetupRepository = hrmsetupRepository;
        }

        public async Task<PagedResultDto<GetEmployeeLoansForViewDto>> GetAll(GetAllEmployeeLoansInput input)
        {

            var filteredEmployeeLoans = _employeeLoansRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Remarks.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinEmployeeIDFilter != null, e => e.EmployeeID >= input.MinEmployeeIDFilter)
                        .WhereIf(input.MaxEmployeeIDFilter != null, e => e.EmployeeID <= input.MaxEmployeeIDFilter)
                        .WhereIf(input.MinLoanIDFilter != null, e => e.LoanID >= input.MinLoanIDFilter)
                        .WhereIf(input.MaxLoanIDFilter != null, e => e.LoanID <= input.MaxLoanIDFilter)
                        .WhereIf(input.MinLoanDateFilter != null, e => e.LoanDate >= input.MinLoanDateFilter)
                        .WhereIf(input.MaxLoanDateFilter != null, e => e.LoanDate <= input.MaxLoanDateFilter)
                        .WhereIf(input.MinLoanTypeIDFilter != null, e => e.LoanTypeID >= input.MinLoanTypeIDFilter)
                        .WhereIf(input.MaxLoanTypeIDFilter != null, e => e.LoanTypeID <= input.MaxLoanTypeIDFilter)
                        .WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
                        .WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
                        .WhereIf(input.MinNOIFilter != null, e => e.NOI >= input.MinNOIFilter)
                        .WhereIf(input.MaxNOIFilter != null, e => e.NOI <= input.MaxNOIFilter)
                        .WhereIf(input.MinInsAmtFilter != null, e => e.InsAmt >= input.MinInsAmtFilter)
                        .WhereIf(input.MaxInsAmtFilter != null, e => e.InsAmt <= input.MaxInsAmtFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RemarksFilter), e => e.Remarks == input.RemarksFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter)
                        .Where(o => o.TenantId == AbpSession.TenantId);

            var pagedAndFilteredEmployeeLoans = filteredEmployeeLoans
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var employeeLoans = from o in pagedAndFilteredEmployeeLoans
                                select new GetEmployeeLoansForViewDto()
                                {
                                    EmployeeLoans = new EmployeeLoansDto
                                    {
                                        EmployeeID = o.EmployeeID,
                                        LoanID = o.LoanID,
                                        LoanDate = o.LoanDate,
                                        LoanTypeID = o.LoanTypeID,
                                        Amount = o.Amount,
                                        NOI = o.NOI,
                                        InsAmt = o.InsAmt,
                                        Remarks = o.Remarks,
                                        AudtUser = o.AudtUser,
                                        AudtDate = o.AudtDate,
                                        CreatedBy = o.CreatedBy,
                                        CreateDate = o.CreateDate,
                                        Cancelled = o.Cancelled,
                                        //CancelledData=o.CancelledData,
                                        Id = o.Id
                                    }
                                };

            var totalCount = await filteredEmployeeLoans.CountAsync();

            return new PagedResultDto<GetEmployeeLoansForViewDto>(
                totalCount,
                await employeeLoans.ToListAsync()
            );
        }

        public async Task<GetEmployeeLoansForViewDto> GetEmployeeLoansForView(int id)
        {
            var employeeLoans = await _employeeLoansRepository.GetAsync(id);

            var output = new GetEmployeeLoansForViewDto { EmployeeLoans = ObjectMapper.Map<EmployeeLoansDto>(employeeLoans) };

            return output;
        }

        [AbpAuthorize(AppPermissions.PayRoll_EmployeeLoans_Edit)]
        public async Task<GetEmployeeLoansForEditOutput> GetEmployeeLoansForEdit(EntityDto input)
        {
            var employeeLoans = await _employeeLoansRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetEmployeeLoansForEditOutput { EmployeeLoans = ObjectMapper.Map<CreateOrEditEmployeeLoansDto>(employeeLoans) };
            output.EmployeeLoans.EmployeeName = _employeeRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.EmployeeID == output.EmployeeLoans.EmployeeID)
                .FirstOrDefault().EmployeeName;
            output.EmployeeLoans.EmployeeLoanTypeName = _employeeLoansTypesRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LoanTypeId == output.EmployeeLoans.LoanTypeID)
              .FirstOrDefault().LoanTypeName;
            return output;
        }

        public async Task CreateOrEdit(CreateOrEditEmployeeLoansDto input)
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

        [AbpAuthorize(AppPermissions.PayRoll_EmployeeLoans_Create)]
        protected virtual async Task Create(CreateOrEditEmployeeLoansDto input)
        {
            var employeeLoans = ObjectMapper.Map<EmployeeLoans>(input);
            employeeLoans.AudtUser = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            employeeLoans.CreatedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            employeeLoans.AudtDate = DateTime.Now;
            employeeLoans.CreateDate = DateTime.Now;
            employeeLoans.LoanID = ((from tab1 in _employeeLoansRepository.GetAll().Where(o => o.EmployeeID == employeeLoans.EmployeeID && o.TenantId == AbpSession.TenantId) select (int?)tab1.LoanID).Max() ?? 0) + 1;
            if (AbpSession.TenantId != null)
            {
                employeeLoans.TenantId = (int)AbpSession.TenantId;
            }


            await _employeeLoansRepository.InsertAsync(employeeLoans);
        }

        [AbpAuthorize(AppPermissions.PayRoll_EmployeeLoans_Edit)]
        protected virtual async Task Update(CreateOrEditEmployeeLoansDto input)
        {
            var employeeLoans = await _employeeLoansRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, employeeLoans);
        }

        [AbpAuthorize(AppPermissions.PayRoll_EmployeeLoans_Delete)]
        public async Task Delete(EntityDto input)
        {
            DeleteLog(input.Id);
            await _employeeLoansRepository.DeleteAsync(input.Id);
        }
        public void DeleteLog(int detid)
        {
            var data = _employeeLoansRepository.FirstOrDefault(c => c.Id == detid);
            LogModel model = new LogModel()
            {
                Action = "Delete",
                Detid = detid,
                DocNo = data.LoanID,
                FormName = "Loan",
                Status = true,
                ApprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName,
                TenantId = AbpSession.TenantId
            };
            _commonappRepository.ApproveLog(model);
        }
        [HttpGet]
        public void statusloan(int Id, int stat)
        {
            try
            {

                var DocNo = 0;

                if (stat == 0)
                {
                    (from a in _employeeLoansRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == Id)
                     select a).ToList().ForEach(x =>
                     {
                         x.Approved = false;
                         x.ApprovedDate = DateTime.Now;
                         x.ApprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                         DocNo = x.LoanID;
                     });

                }
                else
                {
                    (from a in _employeeLoansRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == Id)
                     select a).ToList().ForEach(x =>
                     {
                         x.Approved = true;
                         x.ApprovedDate = DateTime.Now;
                         x.ApprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                         DocNo = x.LoanID;
                     });
                }
                LogModel Log = new LogModel()
                {
                    Status = stat == 1 ? true : false,
                    ApprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName,
                    Detid = Id,
                    DocNo = DocNo,
                    FormName = "Loan",
                    Action = stat == 1 ? "Approval" : "UnApproval",
                    TenantId = AbpSession.TenantId
                };
                _commonappRepository.ApproveLog(Log);

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        [HttpGet]
        public string ProcessAdvance(int Id)
        {
            var alertMsg = "";
            var currency = _voucherEntryAppService.GetBaseCurrency();
            var user = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            var empAdvance = _employeeLoansRepository.GetAll().Where(c => c.Id == Id && c.TenantId == AbpSession.TenantId).FirstOrDefault();
            if (empAdvance != null)
            {
                var hrmsetup = _hrmsetupRepository.GetAll().Where(c => c.TenantId == AbpSession.TenantId).FirstOrDefault();
                if (hrmsetup != null)
                {
                    List<CreateOrEditGLTRDetailDto> gltrdetailsList = new List<CreateOrEditGLTRDetailDto>();
                    gltrdetailsList.Add(new CreateOrEditGLTRDetailDto
                    {
                        Amount = -empAdvance.Amount,
                        AccountID = hrmsetup.AdvToPayable,
                        Narration = empAdvance.Remarks,
                        SubAccID = 0,
                        LocId = 2,
                        IsAuto = false
                    });

                    gltrdetailsList.Add(new CreateOrEditGLTRDetailDto
                    {
                        Amount = empAdvance.Amount,
                        AccountID = hrmsetup.AdvToStSal,
                        Narration = empAdvance.Remarks,
                        SubAccID = empAdvance.EmployeeID,
                        LocId = 2,
                        IsAuto = false
                    });
                    VoucherEntryDto autoEntry = new VoucherEntryDto()
                    {
                        GLTRHeader = new CreateOrEditGLTRHeaderDto
                        {
                            BookID = "JV",
                            NARRATION = empAdvance.Remarks,
                            DocDate = Convert.ToDateTime(empAdvance.LoanDate),
                            DocMonth = Convert.ToDateTime(empAdvance.LoanDate).Month,
                            Approved = true,
                            AprovedBy = user,
                            AprovedDate = DateTime.Now,
                            Posted = false,
                            LocId = 2,
                            CreatedBy = user,
                            CreatedOn = DateTime.Now,
                            AuditUser = user,
                            AuditTime = DateTime.Now,
                            CURID = currency.Id,
                            CURRATE = currency.CurrRate,
                            ConfigID = 0
                        },
                        GLTRDetail = gltrdetailsList
                    };

                    if (autoEntry.GLTRDetail.Count() > 0 && autoEntry.GLTRHeader != null)
                    {
                        var voucher = _voucherEntryAppService.ProcessVoucherEntry(autoEntry);
                        empAdvance.Posted = true;
                        empAdvance.PostedBy = user;
                        empAdvance.PostedDate = DateTime.Now;

                        var transh = _employeeLoansRepository.FirstOrDefault((int)empAdvance.Id);

                        ObjectMapper.Map(empAdvance, transh);

                        alertMsg = "Save";
                    }
                    else
                    {
                        alertMsg = "NoRecord";
                    }
                }
                else
                {
                    alertMsg = "NoAccount";
                }
            }
            return alertMsg;
        }
    }
}