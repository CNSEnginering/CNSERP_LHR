using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.GeneralLedger.DirectInvoice.Dtos
{
    public class DirectInvoiceDto
    {
        public CreateOrEditGLINVHeaderDto GLINVHeader { get; set; }

        public ICollection<CreateOrEditGLINVDetailDto> GLINVDetail { get; set; }

        public string Target { get; set; }
    }
}
