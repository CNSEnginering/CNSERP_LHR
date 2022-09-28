
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.GeneralLedger.SetupForms.Dtos
{
    public class CreateOrEditSubControlDetailDto : EntityDto<int?>
    {

        public virtual string Seg2ID { get; set; }

        public string SegmentName { get; set; }

        public string SegmantID1 { get; set; }

        public string OldCode { get; set; }

        public bool Flag { get; set; }
        public string AccountType { get; set; }

        public int? AccountHeader { get; set; }

        public int? SortOrder { get; set; }
        public virtual string Acctype { get; set; }
        public virtual string AccountBSType { get; set; }

        public virtual int? AccountBSHeader { get; set; }

        public virtual int? SortBSOrder { get; set; }
        public virtual string AccountCFType { get; set; }

        public virtual int? AccountCFHeader { get; set; }

        public virtual int? SortCFOrder { get; set; }



    }
}