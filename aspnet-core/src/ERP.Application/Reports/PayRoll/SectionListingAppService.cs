using Abp.Domain.Repositories;
using ERP.PayRoll.Section;
using ERP.Reports.PayRoll.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Authorization;
using ERP.Authorization;

namespace ERP.Reports.PayRoll
{
    [AbpAuthorize(AppPermissions.PayRollReports_SetupReports)]
    public class SectionListingAppService : ERPReportAppServiceBase, ISectionListingAppService
    {
        private readonly IRepository<Section> _sectionRepository;

        public SectionListingAppService(IRepository<Section> sectionRepository)
        {
            _sectionRepository = sectionRepository;
        }
        public List<SectionListingDto> GetData(int? TenantId, string fromCode, string toCode, string description)
        {
            if (TenantId == null)
            {
                TenantId = AbpSession.TenantId;

            }
            var data = from a in _sectionRepository.GetAll().Where(o => o.TenantId == TenantId &&
            o.SecID >= Convert.ToInt32(fromCode) && o.SecID <= Convert.ToInt32(toCode) &&
            o.SecName.Contains(description))
                       select new SectionListingDto()
                       {
                           SecID = a.SecID.ToString(),
                           SecName = a.SecName
                       };

            return data.ToList();
        }

    }
}
