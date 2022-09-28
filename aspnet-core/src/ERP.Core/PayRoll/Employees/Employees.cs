using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace ERP.PayRoll.Employees
{
    [Table("Employees")]
    public class Employees : Entity, IMustHaveTenant
    {
        public int TenantId { get; set; }


        [Required]
        public virtual int EmployeeID { get; set; }

        [Required]
        public virtual string EmployeeName { get; set; }

        public virtual string FatherName { get; set; }

        public virtual DateTime? date_of_birth { get; set; }

        public virtual string home_address { get; set; }
        public virtual string PhoneNo { get; set; }
        public virtual string NTN { get; set; }

        public virtual DateTime? apointment_date { get; set; }

        public virtual DateTime? date_of_joining { get; set; }

        public virtual DateTime? date_of_leaving { get; set; }

        public virtual string City { get; set; }

        public virtual string Cnic { get; set; }

        public virtual int? EdID { get; set; }
        public virtual int? LocID { get; set; }

        public virtual int? DeptID { get; set; }

        public virtual int? DesignationID { get; set; }
        public virtual int? SubDesignationID { get; set; }

        public virtual string Gender { get; set; }
        
        public virtual bool? Status { get; set; }

        public virtual int? ShiftID { get; set; }

        public virtual int? TypeID { get; set; }

        public virtual int? SecID { get; set; }

        public virtual int? ReligionID { get; set; }

        public virtual bool? social_security { get; set; }

        public virtual double? SScAmt { get; set; }

        public virtual bool? eobi { get; set; }
        public virtual bool? lunchStatus { get; set; }

        public virtual double? EobiAmt { get; set; }

        public virtual bool? wppf { get; set; }

        public virtual string payment_mode { get; set; }

        public virtual string bank_name { get; set; }

        public virtual string account_no { get; set; }

        public virtual string academic_qualification { get; set; }

        public virtual string professional_qualification { get; set; }

        public virtual int? first_rest_days { get; set; }

        public virtual int? second_rest_days { get; set; }

        public virtual int? first_rest_days_w2 { get; set; }

        public virtual int? second_rest_days_w2 { get; set; }

        public virtual string BloodGroup { get; set; }

        public virtual string Reference { get; set; }
        public virtual string Visa_Details { get; set; }
        public virtual string Driving_Licence { get; set; }

        public virtual double? Duty_Hours { get; set; }

        public virtual bool? Active { get; set; }

        public virtual bool? Confirmed { get; set; }

        public virtual string AudtUser { get; set; }

        public virtual DateTime? AudtDate { get; set; }

        public virtual string CreatedBy { get; set; }

        public virtual DateTime? CreateDate { get; set; }

        public virtual bool? WaveOff { get; set; }
        public virtual int? type_of_notice_period { get; set; }
        public virtual bool? Reinstate { get; set; }
        public virtual DateTime? ReinstateDate { get; set; }
        public virtual string ReinstateReason { get; set; }
        public virtual short? AllowanceType { get; set; }
        public virtual double? AllowanceAmt { get; set; }
        public virtual double? AllowanceQty { get; set; }

        public virtual string EoBiNo { get; set; }
        public virtual string SscNo { get; set; }
        public virtual int? OldEmployeeID { get; set; }
        public virtual string MStatus { get; set; }
        public virtual DateTime? ContractExpDate { get; set; }
        public virtual int? EBankID { get; set; }
        public virtual bool? joningReport { get; set; }
        public virtual bool? signEmplForm { get; set; }
        public virtual bool? academicRec { get; set; }
        public virtual bool? pasPhoto { get; set; }
        public virtual bool? harrasmentComp { get; set; }
        public virtual bool? validCnic { get; set; }
        public virtual bool? cVResume { get; set; }
        public virtual bool? salarySlip { get; set; }
        public virtual bool? serviceCertificate { get; set; }
        public virtual bool? refCninc { get; set; }
        public virtual bool? resPrevEmp { get; set; }
        public virtual bool? disclosureForm { get; set; }
    }
}