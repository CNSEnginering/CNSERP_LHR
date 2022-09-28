using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ERP.SupplyChain.Inventory.Dtos
{
    public class GatePassDetailDto : EntityDto<int?>
    {
        public int DetID { get; set; }
        public int TenantId { get; set; }
        [StringLength(14)]
        public virtual string ItemID { get; set; }
        [StringLength(7)]
        public virtual string Unit { get; set; }
        public virtual decimal Conver { get; set; }
        public virtual double Qty { get; set; }
        [StringLength(50)]
        public virtual string Comments { get; set; }
        //[NotMapped]
        //public virtual string ItemDescp { get; set; }
        //[NotMapped]
        //public virtual string ItemUnit { get; set; }
    }
}
