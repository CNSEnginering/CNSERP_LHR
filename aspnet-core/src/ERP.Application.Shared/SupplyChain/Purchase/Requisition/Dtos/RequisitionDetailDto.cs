
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.SupplyChain.Purchase.Requisition.Dtos
{
    public class RequisitionDetailDto : EntityDto<int?>
    {

        //[Required]
        //public int DetailID { get; set; }
        public int TenantId { get; set; }
        [Required]
        public int DetID { get; set; }
        public int LocID { get; set; }
        public int DocNo { get; set; }
        [StringLength(14)]
        public virtual string ItemID { get; set; }
        [StringLength(7)]
        public virtual string Unit { get; set; }
        public virtual int? TransId { get; set; }
        public string TransName { get; set; }

        public virtual double Conver { get; set; }
        public virtual double Qty { get; set; }
        [StringLength(50)]
        public virtual string Remarks { get; set; }
        public double QIH { get; set; }
        public double? QtyInPo { get; set; }
        public int SUBCCID { get; set; }


    }
}