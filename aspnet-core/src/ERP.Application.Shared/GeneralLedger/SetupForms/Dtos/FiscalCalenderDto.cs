
using System;
using Abp.Application.Services.Dto;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class FiscalCalenderDto : EntityDto
    {
		public int Period { get; set; }

		public DateTime StartDate { get; set; }

		public DateTime EndDate { get; set; }

		public bool GL { get; set; }

		public bool AP { get; set; }

		public bool AR { get; set; }

		public bool IN { get; set; }

		public bool PO { get; set; }

		public bool OE { get; set; }

		public bool BK { get; set; }

		public bool HR { get; set; }

		public bool PR { get; set; }

		public int? CreatedBy { get; set; }

		public DateTime? CreatedDate { get; set; }

		public DateTime? EditDate { get; set; }

		public int? EditUser { get; set; }
       
        public bool IsActive { get; set; }
       
        public bool IsLocked { get; set; }




    }
}