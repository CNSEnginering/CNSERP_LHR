using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRD.Dtos;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class GetBatchListPreviewForViewDto
    {
		public BatchListPreviewDto BatchListPreview { get; set; }

        public GLTRDetailDto gLTRDetailDto { get; set; }

        public ChartofControlDto chartofControlDto { get; set; }
    }
}