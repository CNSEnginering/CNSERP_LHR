using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRD.Dtos;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH.Dtos;
using System.Collections.Generic;

namespace ERP.GeneralLedger.Transaction.VoucherEntry.Dtos
{
    public class VoucherEntryDto
    {
        public CreateOrEditGLTRHeaderDto GLTRHeader { get; set; }

        public ICollection<CreateOrEditGLTRDetailDto> GLTRDetail { get; set; }

    }
}
