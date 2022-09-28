
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ERP.SupplyChain.Inventory.Dtos
{
    public class CreateOrEditTransferDto : EntityDto<int?>
    {
        [Required]
        public virtual int FromLocID { get; set; }

        public virtual string FromLocName { get; set; }

        [Required]
        public virtual int DocNo { get; set; }

        public virtual DateTime DocDate { get; set; }

        public virtual string Narration { get; set; }

        public virtual int? ToLocID { get; set; }
        public virtual string ToLocName { get; set; }
        public virtual decimal? TotalQty { get; set; }

        public virtual decimal? TotalAmt { get; set; }

        [Required]
        public virtual bool Posted { get; set; }

        public virtual bool Approved { get; set; }

        public virtual string PostedBy { get; set; }

        public virtual DateTime? PostedDate { get; set; }

        public virtual int? LinkDetID { get; set; }

        public virtual string OrdNo { get; set; }

        [Required]
        public virtual bool HOLD { get; set; }

        public virtual string AudtUser { get; set; }

        public virtual DateTime? AudtDate { get; set; }

        public virtual string CreatedBy { get; set; }

        public virtual DateTime? CreateDate { get; set; }


        public virtual string ReferenceNo { get; set; }
        public virtual string VehicleNo { get; set; }
        public List<TransferDetailDto> transferDetailDto { get; set; }
        public virtual string CCID { get; set; }
        public string ccdesc { get; set; }
    }
}