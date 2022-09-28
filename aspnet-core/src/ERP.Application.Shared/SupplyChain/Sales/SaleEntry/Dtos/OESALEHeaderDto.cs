﻿
using System;
using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Sales.SaleEntry.Dtos
{
    public class OESALEHeaderDto : EntityDto
    {
		public int LocID { get; set; }

		public string LocDesc { get; set; }

		public int DocNo { get; set; }

		public DateTime? DocDate { get; set; }

		public DateTime? PaymentDate { get; set; }

		public string TypeID { get; set; }

		public string TypeDesc { get; set; }

        public string SalesCtrlAcc { get; set; }

        public int? CustID { get; set; }

        public string CustomerName { get; set; }
        public string ItemName { get; set; }

        public string PriceList { get; set; }

		public string Narration { get; set; }
        public int DriverID { get; set; }
        public  string VehicleNo { get; set; }

        public  int? VehicleType { get; set; }

        public int? RoutID { get; set; }
        public string RoutDesc { get; set; }



        public string OGP { get; set; }

		public double? TotalQty { get; set; }

		public double? Amount { get; set; }

        public double? ExlTaxAmount { get; set; }

        public double? Tax { get; set; }

		public double? AddTax { get; set; }

        public double? Disc { get; set; }

        public double? TradeDisc { get; set; }

        public double? Margin { get; set; }

        public double? Freight { get; set; }

        public int? OrdNo { get; set; }

		public double? TotAmt { get; set; }

		public bool Posted { get; set; }

        public string PostedBy { get; set; }

        public DateTime? PostedDate { get; set; }

        public int? LinkDetID { get; set; }

		public bool Active { get; set; }

		public string AudtUser { get; set; }

		public DateTime? AudtDate { get; set; }

		public string CreatedBy { get; set; }
		public string QutationDoc { get; set; }
		public string BasicStyle { get; set; }
		public string License { get; set; }

		public DateTime? CreateDate { get; set; }
        public bool? Approved { get; set; }

        public string DriverName { get; set; }

        public string DriverCtrlAcc { get; set; }
        public int? DriverSubAccID { get; set; }


    }
}