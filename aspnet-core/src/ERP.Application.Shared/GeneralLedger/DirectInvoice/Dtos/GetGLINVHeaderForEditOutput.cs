using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.GeneralLedger.DirectInvoice.Dtos
{
    public class GetGLINVHeaderForEditOutput
    {
		public CreateOrEditGLINVHeaderDto GLINVHeader { get; set; }


    }
}