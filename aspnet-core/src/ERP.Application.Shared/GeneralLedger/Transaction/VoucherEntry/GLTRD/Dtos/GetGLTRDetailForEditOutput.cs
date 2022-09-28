using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.GeneralLedger.Transaction.VoucherEntry.GLTRD.Dtos
{
    public class GetGLTRDetailForEditOutput
    {
		public CreateOrEditGLTRDetailDto GLTRDetail { get; set; }


    }
}