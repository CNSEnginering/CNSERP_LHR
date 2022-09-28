using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.GeneralLedger.DirectInvoice.Dtos
{
    public class GetGLINVDetailForEditOutput
    {
		public CreateOrEditGLINVDetailDto GLINVDetail { get; set; }


    }
}