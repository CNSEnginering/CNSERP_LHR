using Abp.Domain.Repositories;
using Abp.Web.Models;
using ERP.CommonServices;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using ERP.GeneralLedger.SetupForms;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRD;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using ERP.Authorization.Users;
using ERP.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ERP.AccountPayables.Dtos;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;
using ERP.Reports.Finance.Dto;
using ERP.GeneralLedger.Transaction.BankReconcile;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace ERP.Reports.Finance
{
    public class BankReconcilationReportAppService : ERPReportAppServiceBase, IBankReconcilationReportAppService
    {
        private readonly IRepository<BankReconcile> _bankReconcileHeaderRepository;
        private readonly IRepository<BankReconcileDetail> _bankReconcileDetailRepository;
        public BankReconcilationReportAppService(
            IRepository<BankReconcile> bankReconcileHeaderRepository,
            IRepository<BankReconcileDetail> bankReconcileDetailRepository
            )
        {
            _bankReconcileHeaderRepository = bankReconcileHeaderRepository;
            _bankReconcileDetailRepository = bankReconcileDetailRepository;
        }
        public List<BankReconcilationReportDetailDto> GetDetailData(int? TenantId, string bankID, DateTime fromDate, DateTime toDate)
        {
            if (TenantId == null)
            {
                TenantId = AbpSession.TenantId;

            }
            string str = ConfigurationManager.AppSettings["ConnectionString"];
            List<BankReconcilationReportDetailDto> bankRecon = new List<BankReconcilationReportDetailDto>();
            using (SqlConnection cn = new SqlConnection(str))
            {
                using (SqlCommand cmd = new SqlCommand("SP_BankReconsileReportDetail", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@tenantId", TenantId);
                    cmd.Parameters.AddWithValue("@bankId", bankID);
                    cmd.Parameters.AddWithValue("@fromDate", fromDate.Date);
                    cmd.Parameters.AddWithValue("@toDate", toDate.Date);
                    cn.Open();

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            BankReconcilationReportDetailDto bank = new BankReconcilationReportDetailDto
                            {
                                Date = Convert.ToDateTime(rdr["VoucherDate"]),
                                Amount = Convert.ToInt32(rdr["Amount"]),
                                SubType = rdr["SubType"].ToString(),
                                Type = rdr["Type"].ToString(),
                                Id = Convert.ToInt32(rdr["voucherID"]),
                                Reference = rdr["Reference"].ToString(),
                                Narration = rdr["Narration"].ToString()
                            };
                            bankRecon.Add(bank);
                        }
                    }
                    //  // cn.Close();
                }
            }
            return bankRecon;
        }


        public List<BankReconcilationReportDto> GetData(int? TenantId, string bankID, DateTime fromDate, DateTime toDate)
        {
            if (TenantId == null)
            {
                TenantId = AbpSession.TenantId;

            }
            string str = ConfigurationManager.AppSettings["ConnectionString"];
            List<BankReconcilationReportDto> bankRecon = new List<BankReconcilationReportDto>();
            using (SqlConnection cn = new SqlConnection(str))
            {
                using (SqlCommand cmd = new SqlCommand("SP_BankReconsileReport", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@tenantId", TenantId);
                    cmd.Parameters.AddWithValue("@bankId", bankID);
                    cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    cmd.Parameters.AddWithValue("@toDate", toDate);
                    cn.Open();

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            BankReconcilationReportDto bank = new BankReconcilationReportDto
                            {
                                Opening = Convert.ToDouble(rdr["Opening"]),
                                ClearedDepositItems = Convert.ToInt32(rdr["ClearedDepositItems"]),
                                ClearedDepositPayments = Convert.ToDouble(rdr["ClearedDepositPayments"]),
                                ClearedItems = Convert.ToInt32(rdr["ClearedItems"]),
                                ClearedPayments = Convert.ToDouble(rdr["ClearedPayments"]),
                                UnClearedDepositItems = Convert.ToDouble(rdr["UnClearedDepositItems"]),
                                UnClearedDeposits = Convert.ToDouble(rdr["UnClearedDeposits"]),
                                UnClearedItems = Convert.ToInt32(rdr["UnClearedItems"]),
                                UnClearedPayments = Convert.ToDouble(rdr["UnClearedPayments"])
                            };
                            bankRecon.Add(bank);
                        }
                    }
                }
                // cn.Close();
            }
            return bankRecon;
        }
    }
}