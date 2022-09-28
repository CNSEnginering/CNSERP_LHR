using Abp.Domain.Repositories;
using ERP.GeneralLedger.SetupForms;
using ERP.Reports.GeneralLedger.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Reports.GeneralLedger
{
    class GLLevel3ListingAppService : ERPReportAppServiceBase, IGLLevel3ListingAppService
    {
        private readonly IRepository<Segmentlevel3> _segmentlevel3Repository;
        private readonly IRepository<ControlDetail> _lookup_controlDetailRepository;
        private readonly IRepository<SubControlDetail> _lookup_subControlDetailRepository;

        public GLLevel3ListingAppService(IRepository<Segmentlevel3> segmentlevel3Repository,
            IRepository<ControlDetail> lookup_controlDetailRepository, IRepository<SubControlDetail> lookup_subControlDetailRepository)
        {
            _segmentlevel3Repository = segmentlevel3Repository;
            _lookup_controlDetailRepository = lookup_controlDetailRepository;
            _lookup_subControlDetailRepository = lookup_subControlDetailRepository;
        }
        public List<GLLevel3ListingDto> GetData(int? TenantId, string fromCode, string toCode)
        {
            if (TenantId == null)
            {
                TenantId = AbpSession.TenantId;
            }

            var segmentlevel3s = _segmentlevel3Repository.GetAll().Where(x => x.TenantId == AbpSession.TenantId);
            //&& Convert.ToInt32(x.Seg3ID) >= Convert.ToInt32(fromCode) && Convert.ToInt32(x.Seg3ID) <= Convert.ToInt32(toCode));

            var data = from o in segmentlevel3s
                       join o1 in _lookup_controlDetailRepository.GetAll() on new { Seg1ID = o.Seg3ID.Substring(0, o.Seg3ID.Length - 7), o.TenantId } equals new { o1.Seg1ID, o1.TenantId }
                       join o2 in _lookup_subControlDetailRepository.GetAll() on new { Seg2ID = o.Seg3ID.Substring(0, o.Seg3ID.Length - 3), o.TenantId } equals new
                       { o2.Seg2ID, o2.TenantId }
                       select new GLLevel3ListingDto()
                       {
                           Level3ID = o.Seg3ID,
                           Description = o.SegmentName,
                           Level1 = o1.SegmentName,
                           Level2 = o2.SegmentName
                       };

            data.OrderBy(o => o.Level3ID);

            return data.ToList();
        }
    }
}
