using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.AccountPayables.CRDRNote.Dtos
{
    public class GetCRDRNoteForEditOutput
    {
		public CreateOrEditCRDRNoteDto CRDRNote { get; set; }


    }
}