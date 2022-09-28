using Abp.Application.Services.Dto;
using System;

namespace ERP.PayRoll.EmployeeSalary.Dtos
{
    public class GetAllEmployeeSalaryForExcelInput
    {
        public string Filter { get; set; }

        public int? MaxEmployeeIDFilter { get; set; }
        public int? MinEmployeeIDFilter { get; set; }

        public string EmployeeNameFilter { get; set; }

        public double? MaxBank_AmountFilter { get; set; }
        public double? MinBank_AmountFilter { get; set; }

        public DateTime? MaxStartDateFilter { get; set; }
        public DateTime? MinStartDateFilter { get; set; }

        public double? MaxGross_SalaryFilter { get; set; }
        public double? MinGross_SalaryFilter { get; set; }

        public double? MaxBasic_SalaryFilter { get; set; }
        public double? MinBasic_SalaryFilter { get; set; }

        public double? MaxTaxFilter { get; set; }
        public double? MinTaxFilter { get; set; }

        public double? MaxHouse_RentFilter { get; set; }
        public double? MinHouse_RentFilter { get; set; }

        public double? MaxNet_SalaryFilter { get; set; }
        public double? MinNet_SalaryFilter { get; set; }

        public string AudtUserFilter { get; set; }

        public DateTime? MaxAudtDateFilter { get; set; }
        public DateTime? MinAudtDateFilter { get; set; }

        public string CreatedByFilter { get; set; }

        public DateTime? MaxCreateDateFilter { get; set; }
        public DateTime? MinCreateDateFilter { get; set; }



    }
}