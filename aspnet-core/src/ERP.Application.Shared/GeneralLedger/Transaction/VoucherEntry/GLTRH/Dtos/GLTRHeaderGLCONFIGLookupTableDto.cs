using Abp.Application.Services.Dto;

namespace ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH.Dtos
{
    public class GLTRHeaderGLCONFIGLookupTableDto
    {
		public string ConfigId { get; set; }

		public string AccountId { get; set; }

        public string AccountDesc { get; set; }
    }
}