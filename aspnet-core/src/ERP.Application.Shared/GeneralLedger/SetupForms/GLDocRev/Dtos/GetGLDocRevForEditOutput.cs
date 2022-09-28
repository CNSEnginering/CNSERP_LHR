using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.GeneralLedger.SetupForms.GLDocRev.Dtos
{
    public class GetGLDocRevForEditOutput
    {
        public CreateOrEditGLDocRevDto GLDocRev { get; set; }

    }
}