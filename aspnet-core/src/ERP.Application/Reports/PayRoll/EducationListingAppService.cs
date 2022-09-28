using Abp.Domain.Repositories;
using ERP.Reports.PayRoll.Dtos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERP.PayRoll.Education;
using Abp.Authorization;
using ERP.Authorization;

namespace ERP.Reports.PayRoll
{
    [AbpAuthorize(AppPermissions.PayRollReports_SetupReports)]
    public class EducationAppService : ERPReportAppServiceBase, IEducationAppService
    {
        private readonly IRepository<Education> _educationRepository;
        public EducationAppService( 
            IRepository<Education> educationRepository)
        {
            _educationRepository = educationRepository;
        }
        public List<EducationListingDto> GetData(int? TenantId, string fromCode, string toCode, string description)
        {
            if (TenantId == null)
            {
                TenantId = AbpSession.TenantId;

            }
            var data = from a in _educationRepository.GetAll().Where(o => o.TenantId == TenantId &&
            o.EdID >= Convert.ToInt32(fromCode) && o.EdID <= Convert.ToInt32(toCode) &&
            o.Eduction.Contains(description))
                       select new EducationListingDto()
                       {
                           EdID = a.EdID.ToString(),
                           Eduction = a.Eduction
                       };


            return data.ToList();
        } 
    }
}
