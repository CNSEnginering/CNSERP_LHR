using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Reports.PayRoll.Dtos
{
    public class EmployeeListingDto
    {
        public string EmpID { get; set; }
        public string EmpName { get; set; }
        public string FatherName { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public string Designation { get; set; }
        public string Joining { get; set; }
        public string Leaving { get; set; }
        public string GrossSalary { get; set; }
        public int? LocID { get; set; }
        public string LocationName { get; set; }
        public int? Emptype { get; set; }
    }
}
