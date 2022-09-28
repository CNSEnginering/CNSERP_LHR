using Abp.Domain.Repositories;
using ERP.GeneralLedger.SetupForms.LedgerType;
using ERP.Reports.GeneralLedger.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Reports.GeneralLedger
{
    class LedgerTypeListingAppService : ERPReportAppServiceBase, ILedgerTypeListingAppService
    {
        private readonly IRepository<LedgerType> _ledgerTypeRepository;


        public LedgerTypeListingAppService(IRepository<LedgerType> ledgerTypeRepository)
        {
            _ledgerTypeRepository = ledgerTypeRepository;
        }
        public List<LedgerTypeListingDto> GetData(int? TenantId, string fromCode, string toCode)
        {
            if (TenantId == null)
            {
                TenantId = AbpSession.TenantId;
            }
            var data = from a in _ledgerTypeRepository.GetAll().Where(o => o.TenantId == TenantId &&
            o.LedgerID >= Convert.ToInt32(fromCode) && o.LedgerID <= Convert.ToInt32(toCode))
                       select new LedgerTypeListingDto()
                       {
                           LedgerID = a.LedgerID.ToString(),
                           LedgerDesc = a.LedgerDesc
                       };

            return data.ToList();
        }
    }
}
