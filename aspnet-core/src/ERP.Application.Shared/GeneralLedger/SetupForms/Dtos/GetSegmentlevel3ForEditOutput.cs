using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class GetSegmentlevel3ForEditOutput
    {
		public CreateOrEditSegmentlevel3Dto Segmentlevel3 { get; set; }

		public string ControlDetailId { get; set;}
        public string ControlDetailDesc { get; set; }

        public string SubControlDetailId { get; set;}
        public string SubControlDetailDesc { get; set; }


    }
}