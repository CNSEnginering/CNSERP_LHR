using Abp.Domain.Repositories;
using ERP.GeneralLedger.SetupForms;
using ERP.Reports.GeneralLedger.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Reports.GeneralLedger
{
    class GLLevel2ListingAppService : ERPReportAppServiceBase, IGLLevel2ListingAppService
    {
        private readonly IRepository<SubControlDetail> _subControlDetailRepository;
        private readonly IRepository<ControlDetail> _lookup_controlDetailRepository;

        public GLLevel2ListingAppService(IRepository<SubControlDetail> subControlDetailRepository,
            IRepository<ControlDetail> lookup_controlDetailRepository)
        {
            _subControlDetailRepository = subControlDetailRepository;
            _lookup_controlDetailRepository = lookup_controlDetailRepository;
        }
        public List<GLLevel2ListingDto> GetData(int? TenantId, string fromCode, string toCode)
        {
            if (TenantId == null)
            {
                TenantId = AbpSession.TenantId;
            }

            var subControlDetails = _subControlDetailRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId);
            //&&Convert.ToInt32(x.Seg2ID) >= Convert.ToInt32(fromCode) && Convert.ToInt32(x.Seg2ID) <= Convert.ToInt32(toCode));

            var data = from o in subControlDetails
                       join o1 in _lookup_controlDetailRepository.GetAll() on o.Seg2ID.Substring(0, o.Seg2ID.Length - 4) equals o1.Seg1ID into j1
                       from s1 in j1.DefaultIfEmpty().Where(x => x.TenantId == AbpSession.TenantId)
                       select new GLLevel2ListingDto()
                       {
                           Id = o.Seg2ID,
                           Level2Desc = o.SegmentName,
                           Level1 = s1 == null ? "" : s1.SegmentName.ToString()
                       };

            data.OrderBy(o => o.Id);

            return data.ToList();
        }
    }
}
