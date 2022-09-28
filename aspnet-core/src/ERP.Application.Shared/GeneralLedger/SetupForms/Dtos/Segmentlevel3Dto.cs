
using System;
using Abp.Application.Services.Dto;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class Segmentlevel3Dto : EntityDto
    {
        public string Seg3ID { get; set; }

        public string SegmentName { get; set; }

		public string OldCode { get; set; } 
    }
}