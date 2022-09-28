
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.SupplyChain.Sales.SaleAccounts.Dtos
{
    public class CreateOrEditOECOLLDto : EntityDto<int?>
    {

		[Required]
		public int LocID { get; set; }

        public string LocName { get; set; }
        [Required]
		public string TypeID { get; set; }

        public string typeDesc { get; set; }
        public string SalesACC { get; set; }
        public string SalesACCDesc { get; set; }

        public string SalesRetACC { get; set; }

        public string SalesRetACCDesc { get; set; }
        public string COGSACC { get; set; }
        public string COGSACCDesc { get; set; }

        public string ChAccountID { get; set; }

        public string ChAccountIDDesc { get; set; }
        public string DiscAcc { get; set; }
        public string DiscAccDesc { get; set; }

        public string WriteOffAcc { get; set; }
        public string WriteOffAccDesc { get; set; }
        public virtual string PayableAcc { get; set; }
        public virtual string PayableAccDesc { get; set; }
        public virtual string RefundableAcc { get; set; }
        public virtual string RefundableAccDesc { get; set; }
        public short? Active { get; set; }
		
		
		public string AudtUser { get; set; }
		
		
		public DateTime? AudtDate { get; set; }
		
		
		public string CreatedBy { get; set; }
		
		
		public DateTime? CreateDate { get; set; }
		
		

    }
}