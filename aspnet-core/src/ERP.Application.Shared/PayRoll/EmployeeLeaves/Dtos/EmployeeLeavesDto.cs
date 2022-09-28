
using System;
using Abp.Application.Services.Dto;

namespace ERP.PayRoll.EmployeeLeaves.Dtos
{
    public class EmployeeLeavesDto : EntityDto
    {

        public int LeaveID { get; set; }

        public int EmployeeID { get; set; }

        public string EmployeeName { get; set; }
        public string DeptName { get; set; }

        public int SalaryYear { get; set; }


        public short? SalaryMonth { get; set; }

        public DateTime? StartDate { get; set; }

        public double? LeaveType { get; set; }

        public double? Casual { get; set; }

        public double? Sick { get; set; }

        public double? Annual { get; set; }

        public string PayType { get; set; }

        public string Remarks { get; set; }

        public string AudtUser { get; set; }

        public DateTime? AudtDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }



    }
}