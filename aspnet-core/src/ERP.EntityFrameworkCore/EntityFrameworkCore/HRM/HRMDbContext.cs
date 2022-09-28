using Microsoft.EntityFrameworkCore;
using Abp.EntityFrameworkCore;
using ERP.PayRoll.Designation;
using ERP.PayRoll.Department;
using ERP.PayRoll.Section;
using ERP.PayRoll.Location;
using ERP.PayRoll.Cader;
using ERP.PayRoll.Grades;
using ERP.PayRoll.Shifts;
using ERP.PayRoll.EmployeeType;
using ERP.PayRoll.EmployeeEarnings;
using ERP.PayRoll.Employees;
using ERP.PayRoll.EmployeeArrears;
using ERP.PayRoll.EmployeeDeductions;
using ERP.PayRoll.Education;
using ERP.PayRoll.Religion;
using ERP.PayRoll.EmployeeSalary;
using ERP.PayRoll.Attendance;
using ERP.PayRoll.EmployeeLeaves;
using ERP.PayRoll.SalarySheet;
using ERP.PayRoll.DeductionTypes;
using ERP.PayRoll.EarningTypes;
using ERP.PayRoll.Holidays;
using ERP.PayRoll.AllowanceSetup;
using ERP.PayRoll.Allowances;
using ERP.PayRoll.SubDesignations;
using ERP.PayRoll.EmployeeStopSalary;
using ERP.PayRoll.EmployeeLoans;
using ERP.PayRoll.EmployeeLoansType;
using ERP.PayRoll.EmployeeAdvances;
using ERP.PayRoll.StopSalary;
using ERP.PayRoll.Adjustments;
using ERP.Payroll.EmployeeLeaveBalance;
using ERP.PayRoll;
using ERP.Payroll.SlabSetup;
using ERP.PayRoll.SalaryLock;
using ERP.PayRoll.MonthlyCPR;
using ERP.PayRoll.CaderMaster.cader_link_H;
using ERP.PayRoll.CaderMaster.cader_link_D;
using ERP.PayRoll.hrmSetup;

namespace ERP.EntityFrameworkCore
{
    public class HRMDbContext : AbpDbContext
    {
        public virtual DbSet<HrmSetup> HrmSetup { get; set; }
        public virtual DbSet<SalaryLock> SalaryLock { get; set; }
        public virtual DbSet<MonthlyCPR> MonthlyCPR { get; set; }

        public virtual DbSet<EmployeeSalaryDtl> EmployeeSalaryDtl { get; set; }
        public virtual DbSet<EmployerBank> EmployerBank { get; set; }
        public virtual DbSet<Cader_link_H> Cader_link_H { get; set; }
        public virtual DbSet<Cader_link_D> Cader_link_D { get; set; }

        public virtual DbSet<EmployeeLeavesTotal> EmployeeLeavesTotal { get; set; }

        public virtual DbSet<AdjH> AdjH { get; set; }
        public virtual DbSet<SlabSetup> SlabSetup { get; set; }
        public virtual DbSet<PayRoll.StopSalary.StopSalary> StopSalary { get; set; }
        public virtual DbSet<EmployeeLoansTypes> EmployeeLoansType { get; set; }

        public virtual DbSet<EmployeeAdvances> EmployeeAdvances { get; set; }

        public virtual DbSet<EmployeeLoans> EmployeeLoans { get; set; }
        public virtual DbSet<EmployeeStopSalary> EmployeeStopSalary { get; set; }

        public virtual DbSet<SubDesignations> SubDesignations { get; set; }
        public virtual DbSet<PayRoll.Cader.Cader> Cader { get; set; }

        public virtual DbSet<AllowancesDetail> AllowancesDetails { get; set; }

        public virtual DbSet<Allowances> Allowances { get; set; }

        public virtual DbSet<AllowanceSetup> AllowanceSetup { get; set; }

        public virtual DbSet<Holidays> Holidays { get; set; }

        public virtual DbSet<EarningTypes> EarningTypes { get; set; }

        public virtual DbSet<DeductionTypes> DeductionTypes { get; set; }

        public virtual DbSet<Designations> Designations { get; set; }

        public virtual DbSet<Religions> Religions { get; set; }

        public virtual DbSet<Department> Department { get; set; }

        public virtual DbSet<Locations> Locations { get; set; }

        public virtual DbSet<Grade> Grade { get; set; }

        public virtual DbSet<Shift> Shift { get; set; }

        public virtual DbSet<Section> Sections { get; set; }

        public virtual DbSet<EmployeeType> EmployeeType { get; set; }

        public virtual DbSet<EmployeeEarnings> EmployeeEarnings { get; set; }

        public virtual DbSet<EmployeeArrears> EmployeeArrears { get; set; }

        public virtual DbSet<EmployeeLeaves> EmployeeLeaves { get; set; }

        public virtual DbSet<Employees> Employees { get; set; }

        public virtual DbSet<EmployeeDeductions> EmployeeDeductions { get; set; }

        public virtual DbSet<Education> Education { get; set; }

        public virtual DbSet<EmployeeSalary> EmployeeSalary { get; set; }

        public virtual DbSet<AttendanceDetail> AttendanceDetail { get; set; }

        public virtual DbSet<AttendanceHeader> AttendanceHeader { get; set; }

        public virtual DbSet<SalarySheet> SalarySheet { get; set; }

        public virtual DbSet<Attendance> Attendance { get; set; }



        public HRMDbContext(DbContextOptions<HRMDbContext> options)
            : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<HrmSetup>(h =>
            {
                h.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<MonthlyCPR>(m =>
            {
                m.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<SalaryLock>(s =>
            {
                s.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<StopSalary>(s =>
            {
                s.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<EmployeeLoans>(x =>
            {
                x.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<EmployeeLoansTypes>(x =>
            {
                x.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<EmployeeStopSalary>(x =>
            {
                x.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<SubDesignations>(s =>
            {
                s.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<AllowancesDetail>(a =>
            {
                a.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<Allowances>(a =>
            {
                a.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<Cader>(c =>
            {
                c.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<AllowanceSetup>(a =>
            {
                a.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<Holidays>(h =>
            {
                h.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<EarningTypes>(x =>
            {
                x.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<DeductionTypes>(d =>
            {
                d.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<Cader_link_D>(c =>
            {
                c.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<Cader_link_H>(c =>
            {
                c.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<Attendance>(a =>
            {
                a.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<SalarySheet>(s =>
            {
                s.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<AttendanceHeader>(x =>
            {
                x.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<AttendanceDetail>(a =>
            {
                a.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<EmployeeSalary>(x =>
            {
                x.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<Education>(x =>
            {
                x.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<EmployeeDeductions>(x =>
            {
                x.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<Employees>(x =>
            {
                x.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<EmployeeEarnings>(x =>
            {
                x.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<EmployeeArrears>(x =>
            {
                x.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<EmployeeLeaves>(x =>
            {
                x.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<EmployeeType>(x =>
            {
                x.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<Section>(s =>
            {
                s.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<Department>(i =>
            {
                i.HasIndex(e => new { e.TenantId });
            });

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Designations>(i =>
            {
                i.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<Religions>(i =>
            {
                i.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<Locations>(i =>
            {
                i.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<Grade>(i =>
            {
                i.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<Shift>(i =>
            {
                i.HasIndex(e => new { e.TenantId });
            });
            modelBuilder.Entity<EmployeeType>(x =>
            {
                x.HasIndex(e => new { e.TenantId });
            });

        }
    }
}
