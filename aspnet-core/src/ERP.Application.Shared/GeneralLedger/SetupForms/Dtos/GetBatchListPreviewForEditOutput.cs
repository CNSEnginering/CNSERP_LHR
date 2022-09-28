using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class GetBatchListPreviewForEditOutput
    {
		public CreateOrEditBatchListPreviewDto BatchListPreview { get; set; }


    }
}