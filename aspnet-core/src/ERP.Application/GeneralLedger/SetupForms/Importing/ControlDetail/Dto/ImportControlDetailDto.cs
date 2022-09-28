using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.GeneralLedger.SetupForms.Importing.ControlDetail.Dto
{
    public class ImportControlDetailDto
    {
        public int Id { get; set; }
        public string Seg1ID { get; set; }
        public string SegmentName { get; set; }
        public int Family { get; set; }
        public string OldCode { get; set; }

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
