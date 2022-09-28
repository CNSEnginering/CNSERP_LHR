using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.SupplyChain.Sales.SaleEntry.Importing.Dto
{
    public class ImportSaleEntryHeaderDto
    {
        //public int Id { get; set; }
        public int TenantId { get; set; }

        public virtual int LocID { get; set; }
        
        public virtual int DocNo { get; set; }
        public virtual DateTime? DocDate { get; set; }

        public virtual DateTime? PaymentDate { get; set; }

        public virtual string TypeID { get; set; }

        public virtual string SalesCtrlAcc { get; set; }

        public virtual int? CustID { get; set; }

        public virtual string PriceList { get; set; }

        public virtual string Narration { get; set; }
        

        public virtual double? TotalQty { get; set; }

        public virtual double? Amount { get; set; }

        public virtual double? Tax { get; set; }

        public virtual double? AddTax { get; set; }

        public virtual double? Disc { get; set; }

        public virtual double? TradeDisc { get; set; }

        public virtual double? Margin { get; set; }

        public virtual double? Freight { get; set; }

        public virtual double? TotAmt { get; set; }

        public virtual bool? Active { get; set; }

        public string License { get; set; }

        public virtual string DriverName { get; set; }

        public virtual string VehicleNo { get; set; }

        public virtual int? VehicleType { get; set; }

        public virtual int? RoutID { get; set; }

        public List<ImportSaleEntryDetailsDto> importSaleEntryDetailsDto { get; set; }

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
