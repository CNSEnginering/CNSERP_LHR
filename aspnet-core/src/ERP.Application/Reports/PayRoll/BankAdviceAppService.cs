using Abp.Domain.Repositories;
using ERP.PayRoll;
using ERP.PayRoll.Employees;
using ERP.PayRoll.SalarySheet;
using ERP.Reports.PayRoll.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
namespace ERP.Reports.PayRoll
{
    public class BankAdviceAppService : ERPReportAppServiceBase, IBankAdviceAppService
    {
        private readonly IRepository<SalarySheet> _salarySheetRepository;
        private readonly IRepository<Employees> _employeesRepository;
        private readonly IRepository<EmployerBank> _employerBankRepository;

        public BankAdviceAppService(IRepository<SalarySheet> salarySheetRepository, IRepository<Employees> employeesRepository, IRepository<EmployerBank> employerBankRepository)
        {
            _salarySheetRepository = salarySheetRepository;
            _employeesRepository = employeesRepository;
            _employerBankRepository = employerBankRepository;
        }

        public List<BankAdviceDto> GetData(int? TenantId, short SalaryMonth, short SalaryYear, int typeID)
        {
            if (TenantId == null)
            {
                TenantId = AbpSession.TenantId;
            }

            var salarySheetData = _salarySheetRepository.GetAll().Where(e => e.SalaryMonth == SalaryMonth && e.SalaryYear == SalaryYear && e.TenantId == TenantId && e.ModOfPay == 1);
            var employeesData = _employeesRepository.GetAll().Where(o => o.TenantId == TenantId && o.TypeID == typeID && o.payment_mode == "1");
            var employerBank = _employerBankRepository.GetAll().Where(x => x.TenantId == TenantId && x.Active == true);

            var data = from a in salarySheetData
                       join b in employeesData on a.EmployeeID equals b.EmployeeID
                       join c in employerBank on b.EBankID equals c.EBankID
                       orderby a.EmployeeID
                       where a.work_days != 0
                       select new BankAdviceDto()
                       {
                           ClientAccNo = c.EBranchID+"-"+c.EBankAccNumber,
                           Date = DateTime.Now.ToString("dd MMMM yyyy"),
                           SalaryAcc = b.account_no.Substring(b.account_no.IndexOf("-")+1, b.account_no.Length),
                           AccTitle = b.EmployeeName,
                           Amount = a.total_earnings.Value.ToString("N2"),
                           OurBranch = c.EBranchID,
                           TheirBranch = b.account_no.Substring(0,b.account_no.LastIndexOf("-"))

                       };

            return data.ToList();
        }

    }
}
