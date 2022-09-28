using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using ERP.GeneralLedger.SetupForms;
using ERP.Install.Dto;
using ERP.Reports.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Linq.Extensions;
using ERP.PayRoll.EmployeeLoansType;

namespace ERP.Reports.Filters
{
    public class ReportFilterAppService : ERPAppServiceBase
    {
        private readonly IRepository<ChartofControl, string> _lookup_chartofControlRepository;
        private readonly IRepository<GLCONFIG> _glconfigRepository;

        private readonly IRepository<AccountSubLedger> _accountSubLedgerRepository;
        private readonly IRepository<GLBOOKS> _lookup_glbooksRepository;
        private readonly IRepository<EmployeeLoansTypes> _employeeLoansTypesRepository;
        public ReportFilterAppService(IRepository<ChartofControl, string> lookup_chartofControlRepository,
            IRepository<GLCONFIG> glconfigRepository,
            IRepository<AccountSubLedger> accountSubLedgerRepository,
            IRepository<GLBOOKS> lookup_glbooksRepository,
            IRepository<EmployeeLoansTypes> employeeLoansTypesRepository)
        {
            _lookup_chartofControlRepository = lookup_chartofControlRepository;
            _glconfigRepository = glconfigRepository;
            _accountSubLedgerRepository = accountSubLedgerRepository;
            _lookup_glbooksRepository = lookup_glbooksRepository;
            _employeeLoansTypesRepository = employeeLoansTypesRepository;
        }

        public async Task<ListResultDto<ChartOfControlLookupDto>> GetAllChartofControlList()
        {
            var query = _lookup_chartofControlRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);
            var totalCount = await query.CountAsync();

            var lookupTableDtoList = new List<ChartOfControlLookupDto>();
            foreach (var chartofControl in query)
            {
                lookupTableDtoList.Add(new ChartOfControlLookupDto
                {
                    AccountID = chartofControl.Id,
                    AccountName = chartofControl.AccountName?.ToString()
                });
            }

            return new PagedResultDto<ChartOfControlLookupDto>(
                totalCount,
                lookupTableDtoList
            );
        }


        public async Task<PagedResultDto<AccountSubLedgerListDto>> GetAllAccountSubLedgerAccountList(string accountid)
        {
            int totalCount = 0;
            var lookupTableDtoList = new List<AccountSubLedgerListDto>();
            if (accountid != "" && accountid != null)
            {
                var checkSubLedger = _lookup_chartofControlRepository.GetAll().Where(o => o.Id == accountid).SingleOrDefault();

                if (checkSubLedger != null)
                {
                    if (checkSubLedger.SubLedger)
                    {
                        var subLedgerLst = _accountSubLedgerRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.AccountID == accountid);

                        totalCount = await subLedgerLst.CountAsync();


                        foreach (var accountSubLedger in subLedgerLst)
                        {
                            lookupTableDtoList.Add(new AccountSubLedgerListDto
                            {
                                Id = accountSubLedger.Id,
                                DisplayName = accountSubLedger.SubAccName?.ToString()
                            });
                        }
                    }

                }
            }

            return new PagedResultDto<AccountSubLedgerListDto>(
                   totalCount,
                   lookupTableDtoList
               );
        }

        public async Task<PagedResultDto<AccountSubLedgerListDto>> GetSubledgerForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _accountSubLedgerRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e => e.Id.ToString().Contains(input.Filter)
                ).Where(e => e.AccountID == input.Filter).Where(o => o.TenantId == AbpSession.TenantId);

            var totalCount = await query.CountAsync();

            var glsubledgerList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<AccountSubLedgerListDto>();
            foreach (var glsubledger in glsubledgerList)
            {
                lookupTableDtoList.Add(new AccountSubLedgerListDto
                {
                    Id = glsubledger.Id,
                    DisplayName = glsubledger.SubAccName.ToString()
                });
            }

            return new PagedResultDto<AccountSubLedgerListDto>(
                totalCount,
                lookupTableDtoList
            );
        }
        public async Task<PagedResultDto<LookupDto>> GetChartofControlForLookupTable(GetAllForLookupTableInput input)
        {

            if (input.TargetFilter == "CashBook")
            {
                var q1 = _lookup_chartofControlRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);
                var q2 = _glconfigRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.BookID == "CP" || o.BookID == "CR");

                var q3 = from chartofControl in q1
                         join glconfig in q2 on chartofControl.Id equals glconfig.AccountID
                         select new
                         {
                             glconfig.AccountID,
                             chartofControl.AccountName
                         };

                var q4 = !string.IsNullOrWhiteSpace(input.Filter)
                        ? q3.Where(o => o.AccountName.ToLower().ToString().Contains(input.Filter.ToLower()) || o.AccountID.Contains(input.Filter))
                        : q3;

                var totalCount1 = await q4.CountAsync();

                var chartofControlList1 = await q4
                    .PageBy(input)
                    .ToListAsync();

                var lookupTableDtoList1 = new List<LookupDto>();
                foreach (var chartofControl in chartofControlList1)
                {
                    lookupTableDtoList1.Add(new LookupDto
                    {
                        Id = chartofControl.AccountID,
                        DisplayName = chartofControl.AccountName
                    });
                }

                return new PagedResultDto<LookupDto>(
                    totalCount1,
                    lookupTableDtoList1
                );

            }
            else if (input.TargetFilter == "BankBook")
            {
                var q1 = _lookup_chartofControlRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);
                var q2 = _glconfigRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.BookID == "BP" || o.BookID == "BR");

                var q3 = from chartofControl in q1
                         join glconfig in q2 on chartofControl.Id equals glconfig.AccountID
                         select new
                         {
                             glconfig.AccountID,
                             chartofControl.AccountName
                         };

                var q4 = !string.IsNullOrWhiteSpace(input.Filter)
                        ? q3.Where(o => o.AccountName.ToLower().ToString().Contains(input.Filter.ToLower()) || o.AccountID.Contains(input.Filter))
                        : q3;

                var totalCount1 = await q4.CountAsync();

                var chartofControlList1 = await q4
                    .PageBy(input)
                    .ToListAsync();

                var lookupTableDtoList1 = new List<LookupDto>();
                foreach (var chartofControl in chartofControlList1)
                {
                    lookupTableDtoList1.Add(new LookupDto
                    {
                        Id = chartofControl.AccountID,
                        DisplayName = chartofControl.AccountName
                    });
                }

                return new PagedResultDto<LookupDto>(
                    totalCount1,
                    lookupTableDtoList1
                );

            }

            var query = !string.IsNullOrWhiteSpace(input.Filter) ? _lookup_chartofControlRepository.GetAll().Where(o => o.AccountName.ToLower().ToString().Contains(input.Filter.ToLower())
            || o.Id.ToString().Contains(input.Filter)).Where(o => o.TenantId == AbpSession.TenantId) : _lookup_chartofControlRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);

            var totalCount = await query.CountAsync();

            var chartofControlList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<LookupDto>();
            foreach (var chartofControl in chartofControlList)
            {
                lookupTableDtoList.Add(new LookupDto
                {
                    Id = chartofControl.Id,
                    DisplayName = chartofControl.AccountName.ToString()
                });
            }

            return new PagedResultDto<LookupDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        public List<EmployeeLoansTypes> GetLoanTypes()
        {
            return _employeeLoansTypesRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId).Select(
                x => new EmployeeLoansTypes()
                {
                    LoanTypeId = x.LoanTypeId,
                    LoanTypeName = x.LoanTypeName
                }).ToList();
        }

        //public async Task<PagedResultDto<LookupDto>> GetChartofControlForLookupTable(GetAllForLookupTableInput input)
        //{
        //    //var query = _lookup_chartofControlRepository.GetAll().WhereIf(
        //    //       !string.IsNullOrWhiteSpace(input.Filter),
        //    //      e=> e.Id.ToString().Contains(input.Filter)
        //    //   ).WhereIf(
        //    //       !string.IsNullOrWhiteSpace(input.Filter),
        //    //      e => e.AccountName.ToString().Contains(input.Filter)
        //    //   ).Where(o => o.TenantId == AbpSession.TenantId);


        //    var query = !string.IsNullOrWhiteSpace(input.Filter) ? _lookup_chartofControlRepository.GetAll().Where(o => o.AccountName.ToLower().ToString().Contains(input.Filter.ToLower())
        //    || o.Id.ToString().Contains(input.Filter)).Where(o => o.TenantId == AbpSession.TenantId) : _lookup_chartofControlRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);

        //    var totalCount = await query.CountAsync();

        //    var chartofControlList = await query
        //        .PageBy(input)
        //        .ToListAsync();

        //    var lookupTableDtoList = new List<LookupDto>();
        //    foreach (var chartofControl in chartofControlList)
        //    {
        //        lookupTableDtoList.Add(new LookupDto
        //        {
        //            Id = chartofControl.Id,
        //            DisplayName = chartofControl.AccountName.ToString()
        //        });
        //    }

        //    return new PagedResultDto<LookupDto>(
        //        totalCount,
        //        lookupTableDtoList
        //    );
        //}

        //public async Task<ListResultDto<GLBookListDto>> GLBookListDto()
        //{
        //    var query = _lookup_glbooksRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);
        //    var totalCount = await query.CountAsync();

        //    var lookupTableDtoList = new List<GLBookListDto>();
        //    foreach (var bookobj in query)
        //    {
        //        lookupTableDtoList.Add(new GLBookListDto
        //        {
        //            Id = bookobj.BookID,
        //            DisplayName = bookobj.BookName?.ToString()
        //        });
        //    }

        //    return new PagedResultDto<GLBookListDto>(
        //        totalCount,
        //        lookupTableDtoList
        //    );
        //}
    }
}

