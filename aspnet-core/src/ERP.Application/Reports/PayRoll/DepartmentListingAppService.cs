using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Domain.Repositories;
using ERP.PayRoll.Department;
using ERP.Reports.PayRoll.Dtos;
using Abp.Authorization;
using ERP.Authorization;

namespace ERP.Reports.PayRoll
{
    [AbpAuthorize(AppPermissions.PayRollReports_SetupReports)]
    public class DepartmentListingAppService : ERPReportAppServiceBase, IDepartmentListingAppService
    {
        private readonly IRepository<Department> _departmentRepository;

        public DepartmentListingAppService(IRepository<Department> departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }
        public List<DepartmentListingDto> GetData(int? TenantId, string fromCode, string toCode, string description)
        {
            if (TenantId == null)
            {
                TenantId = AbpSession.TenantId;

            }
            var data = from a in _departmentRepository.GetAll().Where(o => o.TenantId == TenantId &&
            o.DeptID >= Convert.ToInt32(fromCode) && o.DeptID <= Convert.ToInt32(toCode) &&
            o.DeptName.Contains(description))
                       select new DepartmentListingDto()
                       {
                           DeptID = a.DeptID.ToString(),
                           DeptName = a.DeptName
                       };

            return data.ToList();
        }

    }
}
