using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ERP.GeneralLedger.SetupForms.Importing.ChartofAccount.Dto
{
    public class ImportChartofAccountDto
    {
        

        [Column("AccountID")]
        public string Id { get ; set; }

        [Required]
        public virtual string AccountName { get; set; }

        [Required]
        public virtual bool SubLedger { get; set; }

        public virtual int? OptFld { get; set; }

        public virtual short? SLType { get; set; }

        public virtual bool Inactive { get; set; }

        public virtual DateTime? CreationDate { get; set; }

        public virtual string AuditUser { get; set; }

        public virtual DateTime? AuditTime { get; set; }

        public virtual string OldCode { get; set; }


        [Column("Segment1")]
        public virtual string ControlDetailId { get; set; }


        [Column("Segment2")]
        public virtual string SubControlDetailId { get; set; }


        [Column("Segment3")]
        public virtual string Segmentlevel3Id { get; set; }


        public int GroupCode { get; set; }

        public int TenantId { get; set; }

        /// <summary>
        /// Can be set when reading data from excel or when importing Control Detail
        /// </summary>
        public string Exception { get; set; }

        public bool CanBeImported()
        {
            return string.IsNullOrEmpty(Exception);
        }
    }
}
