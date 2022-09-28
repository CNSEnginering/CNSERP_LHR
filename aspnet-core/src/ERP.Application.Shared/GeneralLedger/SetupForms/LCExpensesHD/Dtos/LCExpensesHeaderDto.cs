
using System;
using Abp.Application.Services.Dto;

namespace ERP.GeneralLedger.SetupForms.LCExpensesHD.Dtos
{
    public class LCExpensesHeaderDto : EntityDto
    {
        public int LocID { get; set; }
        public int DocNo { get; set; }
        public DateTime? DocDate { get; set; }
        public string TypeID { get; set; }
        public string AccountID { get; set; }
        public int? SubAccID { get; set; }
        public string PayableAccID { get; set; }
        public string LCNumber { get; set; }
        public bool Active { get; set; }
        public string AudtUser { get; set; }
        public DateTime? AudtDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreateDate { get; set; }

    }
}