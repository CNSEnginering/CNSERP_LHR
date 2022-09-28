using Abp.Domain.Repositories;
using ERP.GeneralLedger.SetupForms;
using ERP.Reports.GeneralLedger.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Reports.GeneralLedger
{
    class GLLocationListingAppService : ERPReportAppServiceBase, IGLLocationListingAppService
    {
        private readonly IRepository<GLLocation> _glLocationRepository;

        public GLLocationListingAppService(IRepository<GLLocation> glLocationRepository)
        {
            _glLocationRepository = glLocationRepository;
        }
        public List<GLLocationListingDto> GetData(int? TenantId, string fromCode, string toCode)
        {
            if (TenantId == null)
            {
                TenantId = AbpSession.TenantId;
            }
            var data = from a in _glLocationRepository.GetAll().Where(o => o.TenantId == TenantId &&
            o.LocId >= Convert.ToInt32(fromCode) && o.LocId <= Convert.ToInt32(toCode))
                       select new GLLocationListingDto()
                       {
                           LocId = a.LocId.ToString(),
                           LocDesc = a.LocDesc
                       };

            return data.ToList();
        }
    }
}
