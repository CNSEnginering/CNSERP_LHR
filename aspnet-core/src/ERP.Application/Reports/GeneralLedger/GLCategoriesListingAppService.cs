using Abp.Domain.Repositories;
using ERP.GeneralLedger.SetupForms;
using ERP.Reports.GeneralLedger.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Reports.GeneralLedger
{
    class GLCategoriesListingAppService : ERPReportAppServiceBase, IGLCategoriesListingAppService
    {
        private readonly IRepository<GroupCategory> _groupCategoryRepository;

        public GLCategoriesListingAppService(IRepository<GroupCategory> groupCategoryRepository)
        {
            _groupCategoryRepository = groupCategoryRepository;
        }
        public List<GLCategoriesListingDto> GetData(int? TenantId, string fromCode, string toCode)
        {
            if (TenantId == null)
            {
                TenantId = AbpSession.TenantId;
            }
            var data = from a in _groupCategoryRepository.GetAll().Where(o => o.TenantId == TenantId &&
            o.GRPCTCODE >= Convert.ToInt32(fromCode) && o.GRPCTCODE <= Convert.ToInt32(toCode))
                       select new GLCategoriesListingDto()
                       {
                           GRPCTCODE = a.GRPCTCODE.ToString(),
                           GRPCTDESC = a.GRPCTDESC
                       };

            return data.ToList();
        }
    }
}
