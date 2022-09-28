
using System.Collections.Generic;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class GetCurrencyRateForEditOutput
    {
        public CreateOrEditCurrencyRateDto CurrencyRate { get; set; }

        public string CompanyProfileCompanyName { get; set; }

        public IEnumerable<CurrencyRateHistoryDto> currencyHistory { get; set; }

    }
}