using Abp.Domain.Repositories;
using ERP.GeneralLedger.SetupForms;
using ERP.Reports.GeneralLedger.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Reports.GeneralLedger
{
    class GLGroupListingAppService : ERPReportAppServiceBase, IGLGroupListingAppService
    {
        private readonly IRepository<GroupCode> _groupCodeRepository;
        private readonly IRepository<GroupCategory> _lookup_groupCategoryRepository;
        public GLGroupListingAppService(IRepository<GroupCode> groupCodeRepository, IRepository<GroupCategory> lookup_groupCategoryRepository)
        {
            _groupCodeRepository = groupCodeRepository;
            _lookup_groupCategoryRepository = lookup_groupCategoryRepository;
        }
        public List<GLGroupListingDto> GetData(int? TenantId, string fromCode, string toCode)
        {
            if (TenantId == null)
            {
                TenantId = AbpSession.TenantId;
            }
            var groupCodes = _groupCodeRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId &&
            x.GRPCTCODE >= Convert.ToInt32(fromCode) && x.GRPCTCODE <= Convert.ToInt32(toCode));

            var data = from o in groupCodes
                       join o1 in _lookup_groupCategoryRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId)
                   on o.GRPCTCODE equals o1.GRPCTCODE into j1
                       from s1 in j1.DefaultIfEmpty()
                       select new GLGroupListingDto()
                       {
                           GRPCODE = o.GRPCODE.ToString(),
                           GRPDESC = o.GRPDESC,
                           GRPCTCODE = s1 == null ? "" : s1.GRPCTDESC,
                       };

            return data.ToList();
        }
    }
}
