using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.PayRoll.hrmSetup.Dtos
{
    public class GetHrmSetupForEditOutput
    {
        public CreateOrEditHrmSetupDto HrmSetup { get; set; }

    }
}