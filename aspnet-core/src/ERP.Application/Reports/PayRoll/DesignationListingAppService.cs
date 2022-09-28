using Abp.Domain.Repositories;
using ERP.Reports.PayRoll.Dtos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERP.PayRoll.Designation;
using Abp.Authorization;
using ERP.Authorization;

namespace ERP.Reports.PayRoll
{
    [AbpAuthorize(AppPermissions.PayRollReports_SetupReports)]
    public class DesignationAppService : ERPReportAppServiceBase, IDesignationAppService
    {
        private readonly IRepository<Designations> _designationRepository;
        public DesignationAppService(
            IRepository<Designations> designationRepository)
        {
            _designationRepository = designationRepository;
        }
        public List<DesignationListingDto> GetData(int? TenantId, string fromCode, string toCode, string description)
        {
            if (TenantId == null)
            {
                TenantId = AbpSession.TenantId;

            }
            var data = from a in _designationRepository.GetAll().Where(o => o.TenantId == TenantId &&
            o.DesignationID >= Convert.ToInt32(fromCode) && o.DesignationID <= Convert.ToInt32(toCode) &&
            o.Designation.Contains(description))
                       select new DesignationListingDto()
                       {
                           DesignationID = a.DesignationID.ToString(),
                           Designation = a.Designation
                       };


            return data.ToList();
        }
    }
}
