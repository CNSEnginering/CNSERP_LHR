using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.GeneralLedger.SetupForms.Importing.SegmentLevel3.Dto
{
    public class ImportSegmentLevel3Dto
    {
        public int Id { get; set; }
        public virtual string Seg3ID { get; set; }
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
