using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.GeneralLedger.SetupForms.LCExpensesHD.Dtos
{
    public class GetAllLCExpensesHeaderForExcelInput 
    {
        public string Filter { get; set; }
        public int? MaxLocIDFilter { get; set; }
        public int? MinLocIDFilter { get; set; }
        public int? MaxDocNoFilter { get; set; }
        public int? MinDocNoFilter { get; set; }
        public DateTime? MaxDocDateFilter { get; set; }
        public DateTime? MinDocDateFilter { get; set; }
        public string TypeIDFilter { get; set; }
        public string AccountIDFilter { get; set; }
        public int? MaxSubAccIDFilter { get; set; }
        public int? MinSubAccIDFilter { get; set; }
        public string PayableAccIDFilter { get; set; }
        public string LCNumberFilter { get; set; }
        public int ActiveFilter { get; set; }
        public string AudtUserFilter { get; set; }
        public DateTime? MaxAudtDateFilter { get; set; }

        public DateTime? MinAudtDateFilter { get; set; }
        public string CreatedByFilter { get; set; }
        public DateTime? MaxCreateDateFilter { get; set; }
        public DateTime? MinCreateDateFilter { get; set; }

    }
}
