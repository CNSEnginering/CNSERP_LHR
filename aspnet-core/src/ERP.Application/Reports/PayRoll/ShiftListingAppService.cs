using Abp.Domain.Repositories;
using ERP.PayRoll.Shifts;
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
    public class ShiftListingAppService : ERPReportAppServiceBase, IShiftListingAppService
    {
        private readonly IRepository<Shift> _shiftRepository;

        public ShiftListingAppService(IRepository<Shift> shiftRepository)
        {
            _shiftRepository = shiftRepository;
        }
        public List<ShiftListingDto> GetData(int? TenantId, string fromCode, string toCode, string description)
        {
            if (TenantId == null)
            {
                TenantId = AbpSession.TenantId;

            }
            var data = from a in _shiftRepository.GetAll().Where(o => o.TenantId == TenantId &&
            o.ShiftID >= Convert.ToInt32(fromCode) && o.ShiftID <= Convert.ToInt32(toCode) &&
            o.ShiftName.Contains(description))
                       select new ShiftListingDto()
                       {
                           ShiftID = a.ShiftID.ToString(),
                           ShiftName = a.ShiftName
                       };

            return data.ToList();
        }

    }
}
