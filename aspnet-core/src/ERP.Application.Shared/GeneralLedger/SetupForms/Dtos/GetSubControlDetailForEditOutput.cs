using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class GetSubControlDetailForEditOutput
    {
		public CreateOrEditSubControlDetailDto SubControlDetail { get; set; }

		public string ControlDetailId { get; set;}

        public string ControlDetailDesc { get; set; }


    }
}