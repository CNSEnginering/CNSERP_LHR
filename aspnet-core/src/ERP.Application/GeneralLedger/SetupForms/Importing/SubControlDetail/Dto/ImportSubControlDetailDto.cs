using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.GeneralLedger.SetupForms.Importing.SubControlDetail.Dto
{
    public class ImportSubControlDetailDto
    {
        public int Id { get; set; }

        public virtual string Seg1ID { get; set; }
        public virtual string Seg2ID { get; set; }

        public virtual string SegmentName { get; set; }

        public virtual string OldCode { get; set; }

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
