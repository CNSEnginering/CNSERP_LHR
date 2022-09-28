using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class GetChartofControlForEditOutput
    {
		public CreateOrEditChartofControlDto ChartofControl { get; set; }

		public string ControlDetailSegmentName { get; set;}

		public string SubControlDetailSegmentName { get; set;}

		public string Segmentlevel3SegmentName { get; set;}


    }
}