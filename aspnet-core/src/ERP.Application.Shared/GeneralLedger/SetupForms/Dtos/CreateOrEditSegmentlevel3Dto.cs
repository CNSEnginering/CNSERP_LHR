
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class CreateOrEditSegmentlevel3Dto : EntityDto<int?>
    {



        public string Seg3ID { get; set; }

        public string SegmentName { get; set; }
		
		
		public string OldCode { get; set; }
		
		
		 public string ControlDetailId { get; set; }
		 
		 		 public string SubControlDetailId { get; set; }
        public bool Flag { get; set; }


    }
}