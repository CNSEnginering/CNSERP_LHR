using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.PayRoll.EmployeeSalary.Importing.Dto
{
   public class ImportEmployeeSalaryDto
    {
        public int EmployeeID { get; set; }

        public string EmployeeName { get; set; }
        public double Bank_Amount { get; set; }

        public DateTime StartDate { get; set; }

        public double? Gross_Salary { get; set; }

        public double? Basic_Salary { get; set; }

        public double? Tax { get; set; }

        public double? House_Rent { get; set; }

        public double? Net_Salary { get; set; }

        public string AudtUser { get; set; }

        public DateTime? AudtDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }
        public int TenantId { get; set; }
        /// <summary>
        /// Can be set when reading data from excel or when importing IC Segemnt 3
        /// </summary>
        public string Exception { get; set; }

        public bool CanBeImported()
        {
            return string.IsNullOrEmpty(Exception);
        }

    }
}
