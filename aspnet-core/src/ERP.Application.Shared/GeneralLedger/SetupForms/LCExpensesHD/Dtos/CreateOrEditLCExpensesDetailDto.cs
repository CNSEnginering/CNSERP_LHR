using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ERP.GeneralLedger.SetupForms.LCExpensesHD.Dtos
{
    public class CreateOrEditLCExpensesDetailDto: EntityDto<int?>
    {
        [Required]
        public int DetID { get; set; }
        [Required]
        public int LocID { get; set; }
        [Required]
        public int DocNo { get; set; }
        [Required]
        public string ExpDesc { get; set; }
        public double? Amount { get; set; }

    }
}
