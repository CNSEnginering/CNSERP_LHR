using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.AccountPayables.Dtos;
using ERP.GeneralLedger.SetupForms.Dtos;
using ERP.GeneralLedger.Transaction.VoucherEntry.Dtos;
using System;
using System.Threading.Tasks;

namespace ERP.GeneralLedger.Transaction.VoucherEntry
{
    public interface IVoucherEntryAppService : IApplicationService
    {
        Task CreateOrEditVoucherEntry(VoucherEntryDto input);

        //int GetMaxDocId(string input);
        int GetMaxDocId(string bookId, bool fmtDocNoRequired, DateTime? docDate);

        bool GetDirectPostedStatus(string account);

        Task<GLBOOKSDto> GetBookNormalEntry(string bookId);

        string GetAccountDesc(string accountId);

        string GetSubledgerDesc(string accountId,int subledgerId);

        APOptionCurrencyRateLookupTableDto GetBaseCurrency();

        Task Delete(EntityDto input);
    }
}
