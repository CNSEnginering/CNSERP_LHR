using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH.Dtos
{
    public class GetGLTRHeaderForEditOutput
    {
		public CreateOrEditGLTRHeaderDto GLTRHeader { get; set; }

		public string GLCONFIGConfigID { get; set;}


    }
}