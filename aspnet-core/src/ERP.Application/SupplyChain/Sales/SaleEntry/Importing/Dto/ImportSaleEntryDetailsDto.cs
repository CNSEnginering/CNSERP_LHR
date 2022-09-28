using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.SupplyChain.Sales.SaleEntry.Importing.Dto
{
    public class ImportSaleEntryDetailsDto
    {

        public virtual int LocID { get; set; }

        public virtual int DocNo { get; set; }

        public virtual string ItemID { get; set; }

        public virtual string Unit { get; set; }

        public virtual double? Conver { get; set; }

        public virtual double? Qty { get; set; }

        public virtual double? Rate { get; set; }

        public virtual double? Amount { get; set; }

        public virtual double? ExlTaxAmount { get; set; }

        public virtual double? Disc { get; set; }

        public virtual string TaxAuth { get; set; }

        public virtual int? TaxClass { get; set; }

        public virtual double? TaxRate { get; set; }

        public virtual double? TaxAmt { get; set; }

        public virtual string Remarks { get; set; }

        public virtual double? NetAmount { get; set; }

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
