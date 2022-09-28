using Abp.Application.Services.Dto;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class AccountSubLedgerChartofControlLookupTableDto
    {
        public string Id { get; set; }

        public string DisplayName { get; set; }
        public string SlType { get; set; }
        public string SlDesc { get; set; }
    }
}