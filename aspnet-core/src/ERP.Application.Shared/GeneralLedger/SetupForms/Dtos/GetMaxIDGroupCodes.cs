using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class GetMaxIDGroupCodes
    {
        public int[] GroupID { get; set; }

        public int MaxID { get; set; }
    }
}
