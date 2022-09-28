using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.GeneralLedger.SetupForms.GLSLGroups.Dtos
{
    public class GetGLSLGroupsForEditOutput
    {
        public CreateOrEditGLSLGroupsDto GLSLGroups { get; set; }

    }
}