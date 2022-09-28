using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using ERP.Authorization;
using ERP.Authorization.Users;
using ERP.CommonServices.UserLoc.CSUserLocD;
using ERP.GeneralLedger.SetupForms;
using ERP.SupplyChain.Inventory.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.SupplyChain.Inventory
{
    [AbpAuthorize(AppPermissions.Inventory_ICSetups)]
    public class CommonAppService : ERPAppServiceBase
    {
        private readonly IRepository<ICSetup> _icSetupRepository;
        private readonly IRepository<CurrencyRate, string> _currencyRateRepository;
        private readonly IRepository<ICLocation> _icLocRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<TransactionType> _TransRepository;
        private readonly IRepository<CSUserLocD> _csUserLocDRepository;
        public CommonAppService(IRepository<ICSetup> icsetuprepositroy, IRepository<CSUserLocD> csUserLocDRepository, IRepository<User, long> userRepository, IRepository<CurrencyRate, string> currencyRateRepository, IRepository<TransactionType> TransRepository, IRepository<ICLocation> icLocRepository)
        {
            _icSetupRepository = icsetuprepositroy;
            _icLocRepository = icLocRepository;
            _currencyRateRepository = currencyRateRepository;
            _TransRepository = TransRepository;
            _userRepository = userRepository;
            _csUserLocDRepository = csUserLocDRepository;
        }
        public async Task<ICSetupDto> GetICSetupDetail(EntityDto input)
        {

            var icSetups = await _icSetupRepository.FirstOrDefaultAsync(x => x.TenantId == AbpSession.TenantId);
            var output = ObjectMapper.Map<ICSetupDto>(icSetups);
            if (output.CurrentLocID > 0)
            {
                if (userInfo(output.CurrentLocID)==true)
                {
                    var LocNameRec = await _icLocRepository.FirstOrDefaultAsync(x => x.LocID == output.CurrentLocID);
                    output.CurrentLocName = LocNameRec == null ? "" : LocNameRec.LocName;
                }
                else
                {
                    output.CurrentLocID = null;
                }
                
            }
            if (output.TransType != null && output.TransType != "")
            {
                var NameRec = await _TransRepository.FirstOrDefaultAsync(x => x.TypeId == output.TransType);
                output.TransTypeName = NameRec==null?"":NameRec.Description;
            }
            if (output.Currency != null && output.Currency != "")
            {
                var NameRec = await _currencyRateRepository.FirstOrDefaultAsync(x => x.Id == output.Currency);
                output.CURRATE = NameRec == null ? 0 : NameRec.CURRATE;
            }
            return output;
        }
        public bool? userInfo(int? locId)
        {
            var st = false;
            var data = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            if (data!=null)
            {
                var query = _csUserLocDRepository.GetAll().Where(c => c.TenantId == AbpSession.TenantId && c.TypeID == 2 && c.UserID == data && c.Status == true && c.LocId== locId).FirstOrDefault();
                if (query!=null)
                {
                    st = true;
                }
            }
            return st;
        }

        [HttpGet]
        public async Task<List<LogModel>> FormLog(LogModel Models)
        {
            List<LogModel> logList = new List<LogModel>();
            string constr = ConfigurationManager.AppSettings["ConnectionString"];
            SqlConnection con = new SqlConnection(constr);
            //For JV Header
            SqlCommand cmd = new SqlCommand("spFormLog", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@TenantID", AbpSession.TenantId);
            cmd.Parameters.AddWithValue("@FormName", Models.FormName);
            cmd.Parameters.AddWithValue("@DocNo", Models.DocNo);
            con.Open();
            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    LogModel QutationData = new LogModel()
                    {
                        ApprovedBy = rdr["approvedby"] is DBNull ? "" : rdr["approvedby"].ToString(),
                        ApprovedDate = rdr["approveddatetime"] is DBNull ? "" : rdr["approveddatetime"].ToString(),
                        DocNo = rdr["DocNo"] is DBNull ? 0 : Convert.ToInt32(rdr["DocNo"]),
                        Status = rdr["Status"] is DBNull ? false : Convert.ToBoolean(rdr["Status"].ToString()),
                    };

                    logList.Add(QutationData);

                }

            }
            con.Close();
            return logList;
        }

        public void ApproveLog(LogModel Models)
        {
            string constr = ConfigurationManager.AppSettings["ConnectionString"];
            SqlConnection con = new SqlConnection(constr);
            //For JV Header//////////////////////
            SqlCommand cmd = new SqlCommand("sp_approveLog", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@TenantID", Models.TenantId);
            cmd.Parameters.AddWithValue("@ApproveBy", Models.ApprovedBy);
            cmd.Parameters.AddWithValue("@Status", Models.Status);
            cmd.Parameters.AddWithValue("@Action", Models.Action);
            cmd.Parameters.AddWithValue("@FormName", Models.FormName);
            cmd.Parameters.AddWithValue("@DocNo", Models.DocNo);
            cmd.Parameters.AddWithValue("@DetID", Models.Detid);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

        }
    }
    
    public class LogModel
    {
        public string FormName { get; set; }
        public int Detid { get; set; }
        public int DocNo { get; set; }
        public string Action { get; set; }
        public bool? Status { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedDate { get; set; }
        public int? TenantId { get; set; }
    }
}
