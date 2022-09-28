using Abp.Domain.Repositories;
using ERP.Reports.PayRoll.Dtos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERP.PayRoll.Religion;
using Abp.Authorization;
using ERP.Authorization;

namespace ERP.Reports.PayRoll
{
    [AbpAuthorize(AppPermissions.PayRollReports_SetupReports)]
    public class ReligionAppService : ERPReportAppServiceBase, IReligionAppService
    {
        private readonly IRepository<Religions> _religionRepository;
        public ReligionAppService(
            IRepository<Religions> religionRepository)
        {
            _religionRepository = religionRepository;
        }
        public List<ReligionListingDto> GetData(int? TenantId, string fromCode, string toCode, string description)
        {
            if (TenantId == null)
            {
                TenantId = AbpSession.TenantId;

            }
            var data = from a in _religionRepository.GetAll().Where(o => o.TenantId == TenantId &&
            o.ReligionID >= Convert.ToInt32(fromCode) && o.ReligionID <= Convert.ToInt32(toCode) &&
            o.Religion.Contains(description))
                       select new ReligionListingDto()
                       {
                           ReligionID = a.ReligionID.ToString(),
                           Religion = a.Religion
                       };


            return data.ToList();
        }
    }
}
