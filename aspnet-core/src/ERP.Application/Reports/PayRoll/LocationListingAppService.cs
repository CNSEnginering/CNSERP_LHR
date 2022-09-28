using Abp.Domain.Repositories;
using ERP.Reports.PayRoll.Dtos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERP.PayRoll.Location;
using Abp.Authorization;
using ERP.Authorization;

namespace ERP.Reports.PayRoll
{
    [AbpAuthorize(AppPermissions.PayRollReports_SetupReports)]
    public class LocationAppService : ERPReportAppServiceBase, ILocationAppService
    {
        private readonly IRepository<Locations> _locationRepository;
        public LocationAppService(
            IRepository<Locations> locationRepository)
        {
            _locationRepository = locationRepository;
        }
        public List<LocationListingDto> GetData(int? TenantId, string fromCode, string toCode, string description)
        {
            if (TenantId == null)
            {
                TenantId = AbpSession.TenantId;

            }
            var data = from a in _locationRepository.GetAll().Where(o => o.TenantId == TenantId &&
            o.LocID >= Convert.ToInt32(fromCode) && o.LocID <= Convert.ToInt32(toCode) &&
            o.Location.Contains(description))
                       select new LocationListingDto()
                       {
                           LocID = a.LocID.ToString(),
                           Location = a.Location
                       };


            return data.ToList();
        }
    }
}
