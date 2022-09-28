using Abp.Application.Services.Dto;

namespace ERP.AccountPayables.Dtos
{
    public class APOptionBankLookupTableDto
    {
		public string Id { get; set; }

		public string DisplayName { get; set; }

        public string AccountID { get; set; }
    }
}