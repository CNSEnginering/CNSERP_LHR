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
    public class CustomerAgingReportAppService : ERPReportAppServiceBase, ICustomerAgingReportAppService
    {
        //  private readonly ICustomerAgingRepository _customerAgingRepository;

        private readonly IRepository<GLCONFIG> _glConfigRepository;
        private readonly IRepository<GLBOOKS> _glBooksRepository;
        private readonly IRepository<GLTRHeader> _gltrHeaderRepository;
        private readonly IRepository<GLTRDetail> _gltrDetailRepository;
        private readonly IRepository<ChartofControl, string> _ChartofAccountRepository;
        public CustomerAgingReportAppService(IRepository<GLTRHeader> gLTRHeaderrepository,
            IRepository<GLTRDetail> gLTRdetailRepository,
            IRepository<GLCONFIG> glconfigRepository,
            IRepository<GLBOOKS> glBooksRepository,
            IRepository<ChartofControl, string> chartofAccountRepository)
        {
            _gltrHeaderRepository = gLTRHeaderrepository;
            _gltrDetailRepository = gLTRdetailRepository;
            _glConfigRepository = glconfigRepository;
            _glBooksRepository = glBooksRepository;
            _ChartofAccountRepository = chartofAccountRepository;
        }




        public List<CustomerAgingDto> GetAll(CustomerAgingInputs inputs)
        {
            string str = ConfigurationManager.AppSettings["ConnectionString"];
            var result = new List<CustomerAgingDto>();

            using (SqlConnection cn = new SqlConnection(str))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GlAging", cn))
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
                // cn.Close();
            }
            return result;

        }


    }
}
