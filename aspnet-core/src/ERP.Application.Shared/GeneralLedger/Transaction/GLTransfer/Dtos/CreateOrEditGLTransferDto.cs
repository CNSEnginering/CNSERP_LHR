
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace ERP.GeneralLedger.Transaction.GLTransfer.Dtos
{
    public class CreateOrEditGLTransferDto : EntityDto<int?>
    {

        [Required]
        public int DOCID { get; set; }

        [Required]
        public DateTime DOCDATE { get; set; }

        [Required]
        public DateTime TRANSFERDATE { get; set; }

        public string DESCRIPTION { get; set; }
        public int? FROMLOCID { get; set; }
        public string FROMBANKID { get; set; }
        public int? FROMCONFIGID { get; set; }
        public string FROMBANKACCID { get; set; }
        public string FROMACCID { get; set; }
        public int? TOLOCID { get; set; }
        public string TOBANKID { get; set; }
        public int? TOCONFIGID { get; set; }
        public string TOBANKACCID { get; set; }
        public string TOACCID { get; set; }
        public double? TRANSFERAMOUNT { get; set; }
        public bool STATUS { get; set; }
        public int? GLLINKIDFROM { get; set; }
        public int? GLLINKIDTO { get; set; }
        public int? GLDOCIDFROM { get; set; }
        public int? GLDOCIDTO { get; set; }

        public string AUDTUSER { get; set; }

        public DateTime? AUDTDATE { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }
        public int? LinkDetIDBP { get; set; }
        public int? LinkDetIDBR { get; set; }
        public int? LinkDetIDCP { get; set; }
        public int? LinkDetIDCR { get; set; }
        public bool? Posted { get; set; }
        public virtual byte? ChType { get; set; }
        public virtual string ChNumber { get; set; }
    }
}