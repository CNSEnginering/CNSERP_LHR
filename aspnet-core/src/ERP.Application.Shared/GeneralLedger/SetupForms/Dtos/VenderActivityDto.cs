using System;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class VenderActivityDto
    {
        public DateTime DocDate { get; set; }       
        public string Type { get; set; }

        public int DocNo { get; set; }
        public string Narration { get; set; }
        
        public double Debit { get; set; }
        public double Credit { get; set; }

        public double? RunningTotal { get; set; }


    }
}