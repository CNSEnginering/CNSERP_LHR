using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.GeneralLedger.SetupForms.LCExpensesHD.Dtos
{
    public class GetAllLCExpensesDetailForExcelInput
    {
        public string Filter { get; set; }
        public int? MaxDetIDFilter { get; set; }
        public int? MinDetIDFilter { get; set; }
        public int? MaxLocIDFilter { get; set; }
        public int? MinLocIDFilter { get; set; }
        public int? MaxDocNoFilter { get; set; }
        public int? MinDocNoFilter { get; set; }

        public string ExpDescFilter { get; set; }
        public double? MaxAmountFilter { get; set; }
        public double? MinAmountFilter { get; set; }

    }
}
