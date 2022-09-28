using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using Abp.Domain.Repositories;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRD;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH;
using ERP.GeneralLedger.SetupForms;
using System.Threading.Tasks;
using Abp.Collections.Extensions;
using Abp.Linq.Extensions;
using Abp.Application.Services.Dto;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH.Dtos;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace ERP.Reports.Finance
{
    public class BillWiseAgingReportAppService : ERPReportAppServiceBase
    {
        public BillWiseAgingReportAppService()
        {

        }
        public List<CustomerAgingDto> GetAll(CustomerAgingInputs inputs)
        {
            string str = ConfigurationManager.AppSettings["ConnectionString"];
            var result = new List<CustomerAgingDto>();
            using (SqlConnection cn = new SqlConnection(str))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GlAgingBillWiseNew", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var formtedate = inputs.asOnDate.ToString("yyyy-MM-dd");
                    cmd.Parameters.AddWithValue("@AsOfDate", formtedate);
                    cmd.Parameters.AddWithValue("@FromAcc", inputs.fromAccount);
                    cmd.Parameters.AddWithValue("@ToAcc", inputs.toAccount);
                    cmd.Parameters.AddWithValue("@FromSubAcc", inputs.frmSubAcc);
                    cmd.Parameters.AddWithValue("@ToSubAcc", inputs.toSubAcc);
                    cmd.Parameters.AddWithValue("@A1", inputs.agingPeriod1);
                    cmd.Parameters.AddWithValue("@A2", inputs.agingPeriod2);
                    cmd.Parameters.AddWithValue("@A3", inputs.agingPeriod3);
                    cmd.Parameters.AddWithValue("@A4", inputs.agingPeriod4);
                    // cmd.Parameters.AddWithValue("@AGPRD90", inputs.agingPeriod5);
                    cmd.Parameters.AddWithValue("@TenantID", AbpSession.TenantId);
                    cn.Open();
                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {


                        while (dataReader.Read())
                        {
                            decimal TotalAmount;
                            decimal.TryParse(dataReader["BAL"].ToString(), out TotalAmount);

                            decimal dueAmount;
                            decimal.TryParse(dataReader["A1"].ToString(), out dueAmount);

                            decimal Amount0_30;
                            decimal.TryParse(dataReader["A2"].ToString(), out Amount0_30);

                            decimal Amount31_60;
                            decimal.TryParse(dataReader["A3"].ToString(), out Amount31_60);

                            decimal Amount61_90;
                            decimal.TryParse(dataReader["A4"].ToString(), out Amount61_90);

                            decimal AboveAmount;
                            decimal.TryParse(dataReader["A5"].ToString(), out AboveAmount);


                            result.Add(new CustomerAgingDto
                            {
                                InvoiceNo = Convert.ToInt32(dataReader["InvoiceNo"]),
                                subID = int.Parse(dataReader["SUBACCID"].ToString()),
                                CustomerName = dataReader["SUBACCNAME"].ToString(),
                                TotalAmount = TotalAmount,
                                dueAmount = dueAmount,
                                Amount0_30 = Amount0_30,
                                Amount31_60 = Amount31_60,
                                Amount61_90 = Amount61_90,
                                AboveAmount = AboveAmount

                            });
                        }
                    }
                }
             //   // cn.Close();
            }
            return result;

        }
        //public List<BillWiseAgingReport> GetAll(DateTime asOnDate, string fromAccount, string toAccount, int frmSubAcc, int toSubAcc,string typeId)
        //{
        //    var tenantId = AbpSession.TenantId;
        //    string str = ConfigurationManager.AppSettings["ConnectionString"];
        //    SqlConnection cn = new SqlConnection(str);
        //    SqlDataReader rdr = null;
        //    List<BillWiseAgingReport> billWiseAgingReportList = new List<BillWiseAgingReport>();
        //    SqlCommand cmd;
        //    cmd = new SqlCommand("sp_GlAgingBillWiseNew", cn);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@AsOfDate", asOnDate.Date);
        //    cmd.Parameters.AddWithValue("@FromAcc", fromAccount);
        //    cmd.Parameters.AddWithValue("@ToAcc", toAccount);
        //    cmd.Parameters.AddWithValue("@FromSubAcc", frmSubAcc);
        //    cmd.Parameters.AddWithValue("@ToSubAcc", toSubAcc);
        //    cmd.Parameters.AddWithValue("@TenantID", AbpSession.TenantId);
        //    cmd.Parameters.AddWithValue("@TypeId", typeId);

        //    cn.Open();
        //    rdr = cmd.ExecuteReader();
        //    while (rdr.Read())
        //    {
        //        BillWiseAgingReport billWiseAgingReport = new BillWiseAgingReport()
        //        {
        //            DocNo = Convert.ToInt32(rdr["DocNo"]),
        //            SubAccId = Convert.ToInt32(rdr["SubAccId"]),
        //            SubAccName = rdr["SubAccName"].ToString(),
        //            DocDate = Convert.ToDateTime(rdr["DocDate"]),
        //            Amount = Convert.ToDouble(rdr["Amount"]),
        //            Aging = Convert.ToInt32(rdr["Aging"]),
        //            Narration = rdr["Narration"].ToString()
        //        };

        //        billWiseAgingReportList.Add(billWiseAgingReport);
        //    }
        //    // cn.Close();
        //    return billWiseAgingReportList.ToList();

        //}
    }

    public class BillWiseAgingReport
    {
        public int DocNo { get; set; }
        public int SubAccId { get; set; }
        public string SubAccName { get; set; }
        public DateTime DocDate { get; set; }
        public double Amount { get; set; }
        public int Aging { get; set; }
        public string Narration { get; set; }
    }
}
