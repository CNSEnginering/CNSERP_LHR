
using System;
using Abp.Application.Services.Dto;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class BatchListPreviewDto : EntityDto
    {
		public DateTime DocDate { get; set; }

        public virtual int DocMonth { get; set; }

        public string Description { get; set; }

		public string BookDesc { get; set; }

		public double Debit { get; set; }

		public double Credit { get; set; }

        public string BookID { get; set; }
        public string LocDesc { get; set; }
        public string Reference { get; set; }

        public bool Approved { get; set; }



    }
}