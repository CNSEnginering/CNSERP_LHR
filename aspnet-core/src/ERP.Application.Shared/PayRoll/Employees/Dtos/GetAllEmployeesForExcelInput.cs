using Abp.Application.Services.Dto;
using System;

namespace ERP.PayRoll.Employees.Dtos
{
    public class GetAllEmployeesForExcelInput
    {
		public string Filter { get; set; }

		public int? MaxEmployeeIDFilter { get; set; }
		public int? MinEmployeeIDFilter { get; set; }

		public string EmployeeNameFilter { get; set; }

		public string FatherNameFilter { get; set; }

        public DateTime? Maxdate_of_birthFilter { get; set; }
		public DateTime? Mindate_of_birthFilter { get; set; }

		public string home_addressFilter { get; set; }

        public string PhoneNoFilter { get; set; }

        public string NTN_Filter { get; set; }

        public DateTime? Maxapointment_dateFilter { get; set; }
		public DateTime? Minapointment_dateFilter { get; set; }

		public DateTime? Maxdate_of_joiningFilter { get; set; }
		public DateTime? Mindate_of_joiningFilter { get; set; }

		public DateTime? Maxdate_of_leavingFilter { get; set; }
		public DateTime? Mindate_of_leavingFilter { get; set; }

		public string CityFilter { get; set; }

		public string CnicFilter { get; set; }

		public int? MaxEdIDFilter { get; set; }
		public int? MinEdIDFilter { get; set; }

		public int? MaxDeptIDFilter { get; set; }
		public int? MinDeptIDFilter { get; set; }

		public int? MaxDesignationIDFilter { get; set; }
		public int? MinDesignationIDFilter { get; set; }

		public string GenderFilter { get; set; }

		public int StatusFilter { get; set; }

		public int? MaxShiftIDFilter { get; set; }
		public int? MinShiftIDFilter { get; set; }

		public int? MaxTypeIDFilter { get; set; }
		public int? MinTypeIDFilter { get; set; }

		public int? MaxSecIDFilter { get; set; }
		public int? MinSecIDFilter { get; set; }

		public int? MaxReligionIDFilter { get; set; }
		public int? MinReligionIDFilter { get; set; }

		public int social_securityFilter { get; set; }

		public int eobiFilter { get; set; }

		public int wppfFilter { get; set; }

		public string payment_modeFilter { get; set; }

		public string bank_nameFilter { get; set; }

		public string account_noFilter { get; set; }

		public string academic_qualificationFilter { get; set; }

		public string professional_qualificationFilter { get; set; }

		public int? Maxfirst_rest_daysFilter { get; set; }
		public int? Minfirst_rest_daysFilter { get; set; }

		public int? Maxsecond_rest_daysFilter { get; set; }
		public int? Minsecond_rest_daysFilter { get; set; }

		public int? Maxfirst_rest_days_w2Filter { get; set; }
		public int? Minfirst_rest_days_w2Filter { get; set; }

		public int? Maxsecond_rest_days_w2Filter { get; set; }
		public int? Minsecond_rest_days_w2Filter { get; set; }

		public string BloodGroupFilter { get; set; }

		public string ReferenceFilter { get; set; }

		public int ActiveFilter { get; set; }

		public int ConfirmedFilter { get; set; }

        public string Visa_DetailsFilter { get; set; }
        public string Driving_LicenceFilter { get; set; }

        public double? MaxDuty_HoursFilter { get; set; }
        public double? MinDuty_HoursFilter { get; set; }

        public string AudtUserFilter { get; set; }

		public DateTime? MaxAudtDateFilter { get; set; }
		public DateTime? MinAudtDateFilter { get; set; }

		public string CreatedByFilter { get; set; }

		public DateTime? MaxCreateDateFilter { get; set; }
		public DateTime? MinCreateDateFilter { get; set; }



    }
}