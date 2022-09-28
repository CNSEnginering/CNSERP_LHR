using Abp.Domain.Repositories;
using ERP.GeneralLedger.SetupForms;
using ERP.Reports.GeneralLedger.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Reports.GeneralLedger
{
    class GLBooksListingAppService : ERPReportAppServiceBase, IGLBooksListingAppService
    {
        private readonly IRepository<GLBOOKS> _glbooksRepository;

        public GLBooksListingAppService(IRepository<GLBOOKS> glbooksRepository)
        {
            _glbooksRepository = glbooksRepository;
        }
        public List<GLBooksListingDto> GetData(int? TenantId, string fromCode, string toCode)
        {
            if (TenantId == null)
            {
                TenantId = AbpSession.TenantId;
            }
            var data = from a in _glbooksRepository.GetAll().Where(o => o.TenantId == TenantId)
                       select new GLBooksListingDto()
                       {
                           BookID = a.BookID,
                           BookName = a.BookName,
                           NormalEntry = (a.NormalEntry == 1) ? "Credit" : (a.NormalEntry == 2) ? "Debit" : "Open Transaction"
                       };

            return data.ToList();
        }
    }
}