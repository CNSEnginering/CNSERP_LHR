using Abp.Domain.Repositories;
using ERP.GeneralLedger.SetupForms;
using ERP.Reports.GeneralLedger.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Reports.GeneralLedger
{
    class GLConfigListingAppService : ERPReportAppServiceBase, IGLConfigListingAppService
    {
        private readonly IRepository<GLCONFIG> _glconfigRepository;
        private readonly IRepository<GLBOOKS> _lookup_glbooksRepository;
        private readonly IRepository<ChartofControl, string> _lookup_chartofControlRepository;

        public GLConfigListingAppService(IRepository<GLCONFIG> glconfigRepository, IRepository<GLBOOKS> lookup_glbooksRepository,
            IRepository<ChartofControl, string> lookup_chartofControlRepository)
        {
            _glconfigRepository = glconfigRepository;
            _lookup_glbooksRepository = lookup_glbooksRepository;
            _lookup_chartofControlRepository = lookup_chartofControlRepository;
        }
        public List<GLConfigListingDto> GetData(int? TenantId, string fromCode, string toCode)
        {
            if (TenantId == null)
            {
                TenantId = AbpSession.TenantId;
            }

            var GLCONFIG = _glconfigRepository.GetAll().Where(o => o.TenantId == TenantId);
            var data = from o in GLCONFIG
                       select new GLConfigListingDto()
                       {
                           Book = o.BookID,
                           AccountId = o.AccountID,
                           BookName = o.BookID != "" ? _lookup_glbooksRepository.GetAll().Where(a => a.BookID == o.BookID && a.TenantId == o.TenantId).SingleOrDefault().BookName : "",
                           AccountName = o.AccountID != "" ? _lookup_chartofControlRepository.GetAll().Where(a => a.Id == o.AccountID && a.TenantId == o.TenantId).SingleOrDefault().AccountName : "",
                       };
            return data.ToList();
        }

    }
}
