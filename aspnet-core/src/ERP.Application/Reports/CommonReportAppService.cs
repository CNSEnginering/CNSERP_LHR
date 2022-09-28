using Abp.Domain.Repositories;
using ERP.CommonServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Reports
{
    public class CommonReportAppService : ERPReportAppServiceBase
    {
        private readonly IRepository<CompanyProfile, string> _companySetupRepository;
        public CommonReportAppService(IRepository<CompanyProfile, string> companySetupRepository)
        {
            _companySetupRepository = companySetupRepository;
        }

        public List<CommonReportData> GetCommonReportData()
        {
            var commonReportDataList = new List<CommonReportData>();
            var companies = _companySetupRepository.GetAll();
            foreach (var company in companies)
            {
                commonReportDataList.Add(new CommonReportData()
                {
                    CompanyName = company.CompanyName,
                    TenantId = company.TenantId,
                    Sign1 = company.Sign1,
                    Sign2 = company.Sign2,
                    Sign3 = company.Sign3,
                    Sign4 = company.Sign4,
                    Sign5 = company.Sign5,

                });
            }
            return commonReportDataList;
        }

       
    }


    public class CommonReportData
    {
        public string CompanyName { get; set; }
        public int TenantId { get; set; }
        public string Sign1 { get; set; }

        public string Sign2 { get; set; }

        public string Sign3 { get; set; }

        public string Sign4 { get; set; }
        public string Sign5 { get; set; }
    }
}
