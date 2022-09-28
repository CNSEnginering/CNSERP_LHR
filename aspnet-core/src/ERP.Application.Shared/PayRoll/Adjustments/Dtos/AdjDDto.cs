using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.PayRoll.Adjustments.Dtos
{
    public class AdjDDto
    {
        public int AdjID { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public int adjType { get; set; }
        public string Remarks { get; set; }
        public int SalaryYear { get; set; }
        public int SalaryMonth { get; set; }
        public DateTime AdjDate { get; set; }
        public double Amount { get; set; }
        public bool Active { get; set; }
        public string AudtUser { get; set; }
        public DateTime AudtDate { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }

        public int? DeductionID { get; set; }
        public int DeductionType { get; set; }
        public DateTime DeductionDate { get; set; }
        public int Id { get; set; }





    }
}
