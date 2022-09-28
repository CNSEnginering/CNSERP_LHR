
using System;
using Abp.Application.Services.Dto;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class GLBOOKSDto : EntityDto
    {
		public string BookID { get; set; }

		public string BookName { get; set; }

		public int NormalEntry { get; set; }

		public bool Integrated { get; set; }

		public bool INACTIVE { get; set; }

		public DateTime? AUDTDATE { get; set; }

		public string AUDTUSER { get; set; }

		public short? Restricted { get; set; }



    }
}