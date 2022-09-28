
using System;
using Abp.Application.Services.Dto;

namespace ERP.GeneralLedger.SetupForms.LCExpensesHD.Dtos
{
    public class LCExpensesDetailDto : EntityDto
    {
        public int DetID { get; set; }
        public int LocID { get; set; }
        public int DocNo { get; set; }
        public string ExpDesc { get; set; }
        public double? Amount { get; set; }

    }
}