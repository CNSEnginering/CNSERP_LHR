using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.Manufacturing.SetupForms.Dtos
{
    public class GetMFAREAForEditOutput
    {
        public CreateOrEditMFAREADto MFAREA { get; set; }

    }
}