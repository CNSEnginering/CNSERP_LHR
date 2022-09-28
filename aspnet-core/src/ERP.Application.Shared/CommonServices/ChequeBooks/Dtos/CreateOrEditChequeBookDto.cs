
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ERP.CommonServices.ChequeBooks.Dtos
{
    public class CreateOrEditChequeBookDto : EntityDto<int?>
    {

        public int? DocNo { get; set; }


        public DateTime? DocDate { get; set; }


        [Required]
        [StringLength(ChequeBookConsts.MaxBANKIDLength, MinimumLength = ChequeBookConsts.MinBANKIDLength)]
        public string BANKID { get; set; }

        public string BankName { get; set; }

        [StringLength(ChequeBookConsts.MaxBankAccNoLength, MinimumLength = ChequeBookConsts.MinBankAccNoLength)]
        public string BankAccNo { get; set; }


        [StringLength(ChequeBookConsts.MaxFromChNoLength, MinimumLength = ChequeBookConsts.MinFromChNoLength)]
        public string FromChNo { get; set; }


        [StringLength(ChequeBookConsts.MaxToChNoLength, MinimumLength = ChequeBookConsts.MinToChNoLength)]
        public string ToChNo { get; set; }


        public int? NoofCh { get; set; }


        public bool Active { get; set; }


        [StringLength(ChequeBookConsts.MaxAudtUserLength, MinimumLength = ChequeBookConsts.MinAudtUserLength)]
        public string AudtUser { get; set; }


        public DateTime? AudtDate { get; set; }


        [StringLength(ChequeBookConsts.MaxCreatedByLength, MinimumLength = ChequeBookConsts.MinCreatedByLength)]
        public string CreatedBy { get; set; }


        public DateTime? CreateDate { get; set; }

        public bool flag { get; set; }

        public ICollection<CreateOrEditChequeBookDetailDto> ChequeBookDetail { get; set; }


    }
}