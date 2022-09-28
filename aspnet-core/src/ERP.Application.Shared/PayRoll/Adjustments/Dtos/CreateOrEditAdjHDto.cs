
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using ERP.PayRoll.EmployeeDeductions.Dtos;
using ERP.PayRoll.EmployeeEarnings.Dtos;

namespace ERP.PayRoll.Adjustments.Dtos
{
    public class CreateOrEditAdjHDto : EntityDto<int?>
    {

        public int? TenantID { get; set; }


        public int? DocType { get; set; }


        public int? TypeID { get; set; }


        public int? DocID { get; set; }


        public DateTime? Docdate { get; set; }


        public short SalaryYear { get; set; }


        public short SalaryMonth { get; set; }


        public string AudtUser { get; set; }


        public DateTime? AudtDate { get; set; }

        public string CreatedBy { get; set; }


        public DateTime? CreateDate { get; set; }

        public AdjTypeDetail AdjDetails { get; set; }


    }

    public class AdjTypeDetail
    {
        public ICollection<CreateOrEditEmployeeDeductionsDto> DeductionDetail { get; set; }

        public ICollection<CreateOrEditEmployeeEarningsDto> EarningDetail { get; set; }
    }
}