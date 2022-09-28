using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.GeneralLedger.SetupForms.LCExpensesHD.Dtos
{
    public class GetLCExpensesDetailForEditOutput
    {
        public ICollection<CreateOrEditLCExpensesDetailDto> LCExpensesDetail { get; set; }
    }
}
