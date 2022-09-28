using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ERP.AccountPayables.Importing.DirectInvoice.Dto
{
    public class ImportDirectInvoiceDto
    {
        [Required]
        public int DocNo { get; set; }

        public  int? CprID { get; set; }

        public string CprNo { get; set; }

        public DateTime? CprDate { get; set; }
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
