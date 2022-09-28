
using System;
using Abp.Application.Services.Dto;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class SubControlDetailDto : EntityDto

    {
        public string Seg2ID { get; set; }
        public string SegmentName { get; set; }
		public string OldCode { get; set; }
    }
}