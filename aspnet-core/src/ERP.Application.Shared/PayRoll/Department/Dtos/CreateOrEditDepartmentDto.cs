
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.PayRoll.Department.Dtos
{
    public class CreateOrEditDepartmentDto : EntityDto<int?>
    {

		[Required]
		public int DeptID { get; set; }
		public int SortId { get; set; }
		
		
		public string DeptName { get; set; }
		
		
		public bool? Active { get; set; }
		
		
		public string AudtUser { get; set; }
		
		
		public DateTime? AudtDate { get; set; }
		
		
		public string CreatedBy { get; set; }
		
		
		public DateTime? CreateDate { get; set; }

        public int? Cader_Id { get; set; }
        public string CaderName { get; set; }

        public string ExpenseAcc { get; set; }
        public string ExpenseAccName { get; set; }



    }
}