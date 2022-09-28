
using System;
using Abp.Application.Services.Dto;

namespace ERP.GeneralLedger.DirectInvoice.Dtos
{
    public class GLINVDetailDto : EntityDto<int?>
    {
		public int DetID { get; set; }

		public string AccountID { get; set; }

		public string AccountDesc { get; set; }

		public int? SubAccID { get; set; }

        public string SubAccDesc { get; set; }

        public string Narration { get; set; }

		public double? Amount { get; set; }

		public bool IsAuto { get; set; }



    }
}