
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ERP.GeneralLedger.Transaction.BankReconcile.Dtos
{
    public class CreateOrEditBankReconcileDto : EntityDto<int?>
    {
        public int? DocNo { get; set; }
        public string DocID { get; set; }


        public DateTime? DocDate { get; set; }


        public string BankID { get; set; }


        public string BankName { get; set; }


        public double? BeginBalance { get; set; }


        public double? EndBalance { get; set; }


        public double? ReconcileAmt { get; set; }


        public double? DiffAmount { get; set; }
        public double? StatementAmt { get; set; }

        public double? ClDepAmt { get; set; }

        public double? ClPayAmt { get; set; }

        public double? UnClDepAmt { get; set; }

        public double? UnClPayAmt { get; set; }

        public int? ClItems { get; set; }

        public int? UnClItems { get; set; }


        public string Narration { get; set; }


        public bool Completed { get; set; }


        public string AudtUser { get; set; }


        public DateTime? AudtDate { get; set; }


        public string CreatedBy { get; set; }


        public DateTime? CreatedDate { get; set; }

        public bool flag { get; set; }


        public ICollection<CreateOrEditBankReconcileDetailDto> BankReconcileDetail { get; set; }

        public bool? Approved { get; set; }

    }
}