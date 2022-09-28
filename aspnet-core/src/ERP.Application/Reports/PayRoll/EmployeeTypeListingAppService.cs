using Abp.Domain.Repositories;
using ERP.Reports.PayRoll.Dtos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERP.PayRoll.EmployeeType;
using Abp.Authorization;
using ERP.Authorization;

namespace ERP.Reports.PayRoll
{
    [AbpAuthorize(AppPermissions.PayRollReports_SetupReports)]
    public class EmployeeTypeAppService : ERPReportAppServiceBase, IEmployeeTypeAppService
    {
        private readonly IRepository<EmployeeType> _employeeTypeRepository;
        public EmployeeTypeAppService( 
            IRepository<EmployeeType> employeeTypeRepository)
        {
            _employeeTypeRepository = employeeTypeRepository;
        }
        public List<EmployeeTypeListingDto> GetData(int? TenantId, string fromCode, string toCode, string description)
        {
            if (TenantId == null)
            {
                TenantId = AbpSession.TenantId;

            }
            var data = from a in _employeeTypeRepository.GetAll().Where(o => o.TenantId == TenantId &&
            o.TypeID >= Convert.ToInt32(fromCode) && o.TypeID <= Convert.ToInt32(toCode) &&
            o.EmpType.Contains(description))
                       select new EmployeeTypeListingDto()
                       {
                           TypeID = a.TypeID.ToString(),
                           EmpType = a.EmpType
                       };


            return data.ToList();
        } 
    }
}
