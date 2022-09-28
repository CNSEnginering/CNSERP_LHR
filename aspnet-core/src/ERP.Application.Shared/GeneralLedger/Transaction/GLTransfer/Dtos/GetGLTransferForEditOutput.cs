using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.GeneralLedger.Transaction.GLTransfer.Dtos
{
    public class GetGLTransferForEditOutput
    {
		public CreateOrEditGLTransferDto GLTransfer { get; set; }


    }
}