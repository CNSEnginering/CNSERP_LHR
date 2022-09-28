using Abp.Application.Services.Dto;
using System;

namespace ERP.GeneralLedger.SetupForms.GLSLGroups.Dtos
{
    public class GetAllGLSLGroupsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? MaxSLGrpIDFilter { get; set; }
        public int? MinSLGrpIDFilter { get; set; }

        public string SLGRPDESCFilter { get; set; }

        public int? ActiveFilter { get; set; }

        public string AudtUserFilter { get; set; }

        public DateTime? MaxAudtDateFilter { get; set; }
        public DateTime? MinAudtDateFilter { get; set; }

        public string CreatedByFilter { get; set; }

        public DateTime? MaxCreateDateFilter { get; set; }
        public DateTime? MinCreateDateFilter { get; set; }

    }
}