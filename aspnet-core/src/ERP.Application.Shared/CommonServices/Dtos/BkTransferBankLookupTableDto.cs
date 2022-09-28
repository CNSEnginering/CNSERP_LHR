using Abp.Application.Services.Dto;

namespace ERP.CommonServices.Dtos
{
    public class BkTransferBankLookupTableDto
    {
		public int Id { get; set; }
		public string DisplayName { get; set; }
        public string Address { get; set; }
        public string BankAccount { get; set; }
        public double AvailableLimit { get; set; }
    }
}