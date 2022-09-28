using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ERP.GeneralLedger.SetupForms.LCExpensesHD.Dtos
{
    public class CreateOrEditLCExpensesHeaderDto : EntityDto<int?>
    {
        [Required]
        public int LocID { get; set; }
        [Required]
        public int DocNo { get; set; }
        public DateTime? DocDate { get; set; }
        public string Narration { get; set; }
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

        public bool flag { get; set; }
        public int? LinkDetID { get; set; }
        public bool? Posted { get; set; }
        public ICollection<CreateOrEditLCExpensesDetailDto> LCExpensesDetail { get; set; }

    }
}
