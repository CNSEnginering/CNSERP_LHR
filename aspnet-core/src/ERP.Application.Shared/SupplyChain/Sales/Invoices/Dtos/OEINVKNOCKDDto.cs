using System;
using Abp.Application.Services.Dto;

namespace ERP.SupplyChain.Sales.Invoices.Dtos
{
    public class OEINVKNOCKDDto : EntityDto
    {
        public int? DetID { get; set; }

        public int? DocNo { get; set; }

        public int InvNo { get; set; }

        public string InvDate { get; set; }
        public string Date { get; set; }
        public double? Amount { get; set; }

        public double? AlreadyPaid { get; set; }

        public double? Pending { get; set; }

        public double? Adjust { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string AudtUser { get; set; }

        public DateTime? AudtDate { get; set; }

    }
}