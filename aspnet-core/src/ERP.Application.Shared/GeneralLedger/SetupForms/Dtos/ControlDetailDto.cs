
using System;
using Abp.Application.Services.Dto;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class ControlDetailDto : EntityDto<int>
    {
        public string Seg1ID { get; set; }
        public string SegmentName { get; set; }

		public int Family { get; set; }

		public string OldCode { get; set; }
    }
}