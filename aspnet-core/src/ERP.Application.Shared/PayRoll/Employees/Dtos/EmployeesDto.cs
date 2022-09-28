
using System;
using Abp.Application.Services.Dto;

namespace ERP.PayRoll.Employees.Dtos
{
    public class EmployeesDto : EntityDto
    {
        public int EmployeeID { get; set; }

        public string EmployeeName { get; set; }
        public virtual bool? lunchStatus { get; set; }
        public string FatherName { get; set; }

        public DateTime? date_of_birth { get; set; }

        public string home_address { get; set; }

        public string PhoneNo { get; set; }

        public string NTN { get; set; }

        public DateTime? apointment_date { get; set; }

        public DateTime? date_of_joining { get; set; }

        public DateTime? date_of_leaving { get; set; }

        public string City { get; set; }

        public string Cnic { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }

        public int? EdID { get; set; }

        public int? DeptID { get; set; }

        public int? DesignationID { get; set; }
        public int? EBankID { get; set; }
        public int? SubDesignationID { get; set; }

        public string Gender { get; set; }

        public bool? Status { get; set; }

        public int? ShiftID { get; set; }

        public int? TypeID { get; set; }

        public int? SecID { get; set; }

        public int? ReligionID { get; set; }

        public bool? social_security { get; set; }

        public float? SScAmt { get; set; }

        public bool? eobi { get; set; }

        public float? EobiAmt { get; set; }

        public bool? wppf { get; set; }

        public string payment_mode { get; set; }

        public string bank_name { get; set; }

        public string account_no { get; set; }

        public string academic_qualification { get; set; }

        public string professional_qualification { get; set; }

        public int? first_rest_days { get; set; }

        public int? second_rest_days { get; set; }

        public int? first_rest_days_w2 { get; set; }

        public int? second_rest_days_w2 { get; set; }

        public string BloodGroup { get; set; }

        public string Reference { get; set; }
        public string Visa_Details { get; set; }
        public string Driving_Licence { get; set; }

        public double? Duty_Hours { get; set; }

        public bool? Active { get; set; }

        public bool? Confirmed { get; set; }

        public string AudtUser { get; set; }

        public DateTime? AudtDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreateDate { get; set; }

        public bool? WaveOff { get; set; }
        public int? type_of_notice_period { get; set; }
        public bool? Reinstate { get; set; }
        public DateTime? ReinstateDate { get; set; }
        public string ReinstateReason { get; set; }

        public short? AllowanceType { get; set; }
        public double? AllowanceAmt { get; set; }
        public double? AllowanceQty { get; set; }
        public DateTime? ContractExpDate { get; set; }
    }
}