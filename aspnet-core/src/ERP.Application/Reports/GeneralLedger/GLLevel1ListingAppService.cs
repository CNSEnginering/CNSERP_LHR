using Abp.Domain.Repositories;
using ERP.GeneralLedger.SetupForms;
using ERP.Reports.GeneralLedger.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Reports.GeneralLedger
{
    class GLLevel1ListingAppService : ERPReportAppServiceBase, IGLLevel1ListingAppService
    {
        private readonly IRepository<ControlDetail> _controlDetailRepository;
        private readonly IRepository<GroupCategory, int> _lookup_groupCategoryRepository;

        public GLLevel1ListingAppService(IRepository<ControlDetail> controlDetailRepository,
            IRepository<GroupCategory, int> lookup_groupCategoryRepository)
        {
            _controlDetailRepository = controlDetailRepository;
            _lookup_groupCategoryRepository = lookup_groupCategoryRepository;
        }
        public List<GLLevel1ListingDto> GetData(int? TenantId, string fromCode, string toCode)
        {
            if (TenantId == null)
            {
                TenantId = AbpSession.TenantId;
            }

            var controlDetails = _controlDetailRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId
            && Convert.ToInt32(x.Seg1ID) >= Convert.ToInt32(fromCode) && Convert.ToInt32(x.Seg1ID) <= Convert.ToInt32(toCode));

            var _groupCategory = _lookup_groupCategoryRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId);

            var data = from o in controlDetails
                       join o1 in _groupCategory on
                       o.Family equals o1.GRPCTCODE into j1
                       from s1 in j1.DefaultIfEmpty()
                       select new GLLevel1ListingDto()
                       {
                           ControlAcc = o.Seg1ID,
                           Category = s1 == null ? "" : s1.GRPCTDESC,
                           ControlDesc = o.SegmentName
                       };

            return data.ToList();
        }
    }
}
