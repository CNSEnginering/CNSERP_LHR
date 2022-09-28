using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.GeneralLedger.SetupForms;
using ERP.GeneralLedger.SetupForms.GLPLCategory;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRD;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH;
using ERP.Reports.Finance.Dto;

namespace ERP.Reports.Finance
{
    public class BalanceSheetAppService : ERPAppServiceBase
    {

        public BalanceSheetAppService()
        {

        }
        public List<ProfitAndLossStatmentDto> GetBalanceSheet(DateTime fromDate, DateTime toDate, int loc)
        {

            string str = ConfigurationManager.AppSettings["ConnectionString"];
            var result = new List<ProfitAndLossStatmentDto>();

            using (SqlConnection cn = new SqlConnection(str))
            {
                using (SqlCommand cmd = new SqlCommand("SP_BSYear", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@fromDate", fromDate.Date);
                    cmd.Parameters.AddWithValue("@toDate", toDate.Date);
                    cmd.Parameters.AddWithValue("@prevFromDate", fromDate.Date.AddYears(-1));
                    cmd.Parameters.AddWithValue("@prevToDate", toDate.Date.AddYears(-1));
                    cmd.Parameters.AddWithValue("@Loc", 1);
                    cmd.Parameters.AddWithValue("@TenantID", AbpSession.TenantId);
                    cn.Open();
                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {

                        while (dataReader.Read())
                        {

                            result.Add(new ProfitAndLossStatmentDto
                            {
                                HeadingText = dataReader["BSAccDesc"] is DBNull ? "" : dataReader["BSAccDesc"].ToString(),
                                Amount = dataReader["Amount"] is DBNull ? 0 : Convert.ToDouble(dataReader["Amount"]),
                                PrevAmount = dataReader["PrevAmount"] is DBNull ? 0 : Convert.ToInt32(dataReader["PrevAmount"]),
                                TypeId = dataReader["BSAccType"] is DBNull ? "" : (dataReader["BSAccType"].ToString() == "EQUITY AND LIABILITIES" ? "SHARE CAPITAL AND RESERVES" : dataReader["BSAccType"].ToString()),
                                GLPLCtGId = dataReader["BSGID"] is DBNull ? 0 : Convert.ToInt32(dataReader["BSGID"]),
                                Type = dataReader["BSType"] is DBNull ? "" : (dataReader["BSType"].ToString() == "LIABILITIES" ? "EQUITY AND LIABILITIES" : dataReader["BSType"].ToString()),
                            });
                        }
                     
                    }
                    //// cn.Close();
                }
            }
            return result;

        }
    }
}
