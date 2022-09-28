using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using ERP.AccountPayables;
using ERP.AccountPayables.Dtos;
using ERP.Authorization;
using ERP.Authorization.Users;
using ERP.CommonServices;
using ERP.CommonServices.UserLoc.CSUserLocD;
using ERP.GeneralLedger.SetupForms;
using ERP.GeneralLedger.SetupForms.Dtos;
using ERP.GeneralLedger.Transaction.VoucherEntry.Dtos;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRD;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRD.Dtos;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH.Dtos;
using ERP.PayRoll.SalaryLock;
using ERP.PayRoll.SalaryLock.Dtos;
using ERP.SupplyChain.Inventory;
using Microsoft.AspNetCore.Mvc;

namespace ERP.GeneralLedger.Transaction.VoucherEntry
{
    public class VoucherEntryAppService : ERPAppServiceBase, IVoucherEntryAppService
    {
        private readonly IRepository<GLLocation> _glLocationRepository;
        private readonly IRepository<GLTRHeader> _gltrHeaderRepository;
        private readonly IRepository<GLOption> _glOptionRepository;
        private readonly IRepository<GLTRDetail> _gltrDetailRepository;
        private readonly IRepository<GLBOOKS> _glbooksRepository;
        private readonly IRepository<ChartofControl, string> _chartofControlRepository;
        private readonly IRepository<AccountSubLedger> _accountSubLedgerRepository;
        private readonly IRepository<APOption> _apOptionRepository;
        private readonly IRepository<CurrencyRate, string> _currencyRateRepository;
        private readonly IRepository<Bank> _bankRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<CSUserLocD> _csUserLocDRepository;
        private readonly CommonAppService _commonappRepository;
        private readonly IRepository<SalaryLock> _salaryLockRepository;

        public VoucherEntryAppService(IRepository<GLTRHeader> gltrHeaderRepository, IRepository<CSUserLocD> csUserLocDRepository, IRepository<GLTRDetail> gltrDetailRepository, 
            IRepository<GLLocation> glLocationRepository, IRepository<GLOption> glOptionRepository, IRepository<GLBOOKS> glbooksRepository, 
            IRepository<ChartofControl, string> chartofControlRepository, IRepository<AccountSubLedger> accountSubLedgerRepository, 
            IRepository<APOption> apOptionRepository, CommonAppService commonappRepository, IRepository<CurrencyRate, string> currencyRateRepository, IRepository<Bank> bankRepository,
            IRepository<User, long> userRepository, IRepository<SalaryLock> salaryLockRepository)
        {
            _gltrDetailRepository = gltrDetailRepository;
            _gltrHeaderRepository = gltrHeaderRepository;
            _glOptionRepository = glOptionRepository;
            _csUserLocDRepository = csUserLocDRepository;
            _glbooksRepository = glbooksRepository;
            _chartofControlRepository = chartofControlRepository;
            _accountSubLedgerRepository = accountSubLedgerRepository;
            _apOptionRepository = apOptionRepository;
            _currencyRateRepository = currencyRateRepository;
            _glLocationRepository = glLocationRepository;
            _bankRepository = bankRepository;
            _userRepository = userRepository;
            _commonappRepository = commonappRepository;
            _salaryLockRepository = salaryLockRepository;
        }

        public bool GetWeatherBankTypeISOverDraftOrNot(string accountId)
        {
            var data = _bankRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.IDACCTBANK == accountId).Count() > 0 ?
                   _bankRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.IDACCTBANK == accountId).FirstOrDefault().DocType : 1;
            return data == 3 ? true : false;
        }
        public double? GetOverDraftLimit(string accountId, int docNo, string bookId)
        {
            double? voucherData, limitData = 0;
            voucherData = (from a in _gltrDetailRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                           join
                           b in _gltrHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId).WhereIf(docNo > 0, p => p.DocNo != docNo)
                           on new { A = a.DetID, B = a.TenantId } equals new { A = b.Id, B = b.TenantId }
                           where (b.Approved == false && b.BookID == bookId && a.AccountID == accountId && a.Amount < 0)
                           //group new { a.AccountID } by new { a.AccountID, a.Amount }
                           // into accountGroup
                           select (a.Amount)).Sum();

            limitData = _bankRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.IDACCTBANK == accountId).Count() > 0 ?
                           _bankRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.IDACCTBANK == accountId).FirstOrDefault().ODLIMIT : 0;

            return (limitData + voucherData) > 0 ? (limitData + voucherData) : 0;
        }

        public async Task CreateOrEditVoucherEntry(VoucherEntryDto input)
        {
            if (input.GLTRHeader.Id == null)
            {
                await CreateVoucherEntry(input);
            }
            else
            {
                await UpdateVoucherEntry(input);
            }
        }

        //private List<GLTRHeaderDto> ProcessVoucherEntry(VoucherEntryDto input)
        // {
        //     var gltrHeader = ObjectMapper.Map<GLTRHeader>(input.GLTRHeader);

        //     if (AbpSession.TenantId != null)
        //     {
        //         gltrHeader.TenantId = (int)AbpSession.TenantId;
        //     }

        //     gltrHeader.DocNo = Convert.ToInt32(GetMaxDocId(input.GLTRHeader.BookID, false, null));
        //     var getGenratedId = _gltrHeaderRepository.InsertAndGetId(gltrHeader);


        //     foreach (var item in input.GLTRDetail)
        //     {

        //         var gltrDetail = ObjectMapper.Map<GLTRDetail>(item);

        //         if (AbpSession.TenantId != null)
        //         {
        //             gltrDetail.TenantId = (int)AbpSession.TenantId;

        //         }
        //         gltrDetail.LocId = input.GLTRHeader.LocId;
        //         gltrDetail.DetID = getGenratedId;
        //         _gltrDetailRepository.InsertAsync(gltrDetail);
        //     }

        //     List<GLTRHeaderDto> returnList = new List<GLTRHeaderDto>();
        //     returnList.Add(new GLTRHeaderDto
        //     {
        //         Id = getGenratedId,
        //         DocNo = gltrHeader.DocNo,
        //         LocId = gltrHeader.LocId
        //     });

        //     return returnList;
        // }

        //[AbpAuthorize(AppPermissions.Transaction_VoucherEntry_Create)]
        private async Task CreateVoucherEntry(VoucherEntryDto input)
        {
            var gltrHeader = ObjectMapper.Map<GLTRHeader>(input.GLTRHeader);
            gltrHeader.AuditUser = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            gltrHeader.CreatedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            gltrHeader.CreatedOn = DateTime.Now;
            if (AbpSession.TenantId != null)
            {
                gltrHeader.TenantId = (int)AbpSession.TenantId;
            }

            var accountNo = "";
            foreach (var item in input.GLTRDetail)
            {
                if (item.IsAuto == true)
                {
                    accountNo = item.AccountID;
                }
            }
            var directPost = _glOptionRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).FirstOrDefault().DirectPost;
            gltrHeader.Posted = GetDirectPostedStatus(accountNo);
            if(directPost == true)
            {
                gltrHeader.Approved = true;
                gltrHeader.AprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                gltrHeader.AprovedDate = DateTime.Now;
            }

            gltrHeader.DocNo = Convert.ToInt32(GetMaxDocId(input.GLTRHeader.BookID, false, gltrHeader.DocDate));
            gltrHeader.FmtDocNo = Convert.ToInt32(GetMaxDocId(input.GLTRHeader.BookID, true, gltrHeader.DocDate));
            gltrHeader.CURID = input.GLTRHeader.BookID != "JV" ? GetBaseCurrency().Id : input.GLTRHeader.CURID;
            var getGenratedId = await _gltrHeaderRepository.InsertAndGetIdAsync(gltrHeader);


            foreach (var item in input.GLTRDetail)
            {

                var gltrDetail = ObjectMapper.Map<GLTRDetail>(item);

                if (AbpSession.TenantId != null)
                {
                    gltrDetail.TenantId = (int)AbpSession.TenantId;

                }
                gltrDetail.LocId = input.GLTRHeader.LocId;
                gltrDetail.DetID = getGenratedId;
                await _gltrDetailRepository.InsertAsync(gltrDetail);
            }
        }

        [AbpAuthorize(AppPermissions.Transaction_VoucherEntry_Edit)]
        private async Task UpdateVoucherEntry(VoucherEntryDto input)
        {
            var gltrHeader = await _gltrHeaderRepository.FirstOrDefaultAsync((int)input.GLTRHeader.Id);
            input.GLTRHeader.CreatedOn = gltrHeader.CreatedOn;

            ObjectMapper.Map(input.GLTRHeader, gltrHeader);
            //delete record when remove in object array
            var deltedRecordsArray = input.GLTRDetail.Select(o => o.Id).ToArray();
            var detailDBRecords = _gltrDetailRepository.GetAll().Where(o => o.DetID == input.GLTRHeader.Id && o.IsAuto == false).Where(o => !deltedRecordsArray.Contains(o.Id));
            foreach (var item in detailDBRecords)
            {
                await _gltrDetailRepository.DeleteAsync(item.Id);
            }

            foreach (var item in input.GLTRDetail)
            {
                if (item.IsAuto == true)
                {
                    var autoEntryId = _gltrDetailRepository.GetAll().Where(o => o.DetID == input.GLTRHeader.Id && o.IsAuto == true).SingleOrDefault().Id;
                    var gltrDetail = await _gltrDetailRepository.FirstOrDefaultAsync(autoEntryId);
                    item.Id = autoEntryId;
                    item.LocId = input.GLTRHeader.LocId;
                    item.DetID = (int)input.GLTRHeader.Id;
                    ObjectMapper.Map(item, gltrDetail);
                }
                else
                {
                    if (item.Id != null)
                    {
                        var gltrDetail = await _gltrDetailRepository.FirstOrDefaultAsync((int)item.Id);
                        gltrDetail.LocId = input.GLTRHeader.LocId;
                        ObjectMapper.Map(item, gltrDetail);
                    }
                    else
                    {
                        var gltrDetail = ObjectMapper.Map<GLTRDetail>(item);

                        if (AbpSession.TenantId != null)
                        {
                            gltrDetail.TenantId = (int)AbpSession.TenantId;

                        }
                        gltrDetail.LocId = input.GLTRHeader.LocId;
                        gltrDetail.DetID = (int)input.GLTRHeader.Id;
                        await _gltrDetailRepository.InsertAsync(gltrDetail);
                    }
                }
            }
        }
        public int GetMaxSrNo()
        {
            var result = _gltrHeaderRepository.GetAll().DefaultIfEmpty().Max(o => o.DocNo);
            return result = result + 1;
        }
        public int GetMaxDocId(string bookId, bool fmtDocNoRequired, DateTime? docDate)
        {
            int maxid = 0;
            //int fmtDocNo = 0;
            ////DateTime docDate = DateTime.Now;
            //int? docFrequency = 0;
            //if (bookId != "")
            //{
            //    maxid = ((from tab1 in _gltrHeaderRepository.GetAll().Where(o => o.BookID == bookId && o.TenantId == AbpSession.TenantId) select (int?)tab1.DocNo).Max() ?? 0) + 1;
            //}

            //if (fmtDocNoRequired == true)
            //{


            //        //var glOptionsData = _glOptionRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);
            //        //if (glOptionsData.Count() > 0)
            //        //{
            //        //    docFrequency = glOptionsData.FirstOrDefault().DocFrequency;
            //        //    if (docFrequency == 1)
            //        //    {
            //        //        fmtDocNo = maxid;
            //        //    }
            //        //    else if (docFrequency == 2)
            //        //    {
            //        //        var maxfmtDocId = ((from a in _gltrHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.BookID == bookId && o.DocDate.Year ==
            //        //        docDate.Value.Year
            //        //        )
            //        //                           //    select (int?)a.DocNo).Max() ?? 0) + 1;
            //        //                            select (int?)Convert.ToInt32(a.FmtDocNo)).Max() ?? 0) + 1;
            //        //        fmtDocNo = maxfmtDocId;
            //        //        //fmtDocNo = maxfmtDocId.ToString("D6");
            //        //        //fmtDocNo = (bookId + "-" + fmtDocNo);
            //        //    }
            //        //    else if (docFrequency == 3)
            //        //    {
            //        //        var maxfmtDocId = ((from a in _gltrHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.BookID == bookId && o.DocDate.Year ==
            //        //        docDate.Value.Year && o.DocDate.Month == docDate.Value.Month
            //        //        )
            //        //                            select (int?)Convert.ToInt32(a.FmtDocNo)).Max() ?? 0) + 1;
            //        //        //fmtDocNo = maxfmtDocId.ToString("D6");
            //        //        //fmtDocNo = (bookId + "-" + fmtDocNo);
            //        //        fmtDocNo = maxfmtDocId;
            //        //    }
            //        //}
            //    }

            string str = ConfigurationManager.AppSettings["ConnectionString"];
            using (SqlConnection cn = new SqlConnection(str))
            {

                SqlCommand cmd;
                cmd = new SqlCommand("sp_GLFMTDocNo", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                var GrandAmt1 = 0.0;
                cmd.Parameters.AddWithValue("@bookid", bookId);
                cmd.Parameters.AddWithValue("@month", docDate.Value.Month);
                cmd.Parameters.AddWithValue("@year", docDate.Value.Year);
                cmd.Parameters.AddWithValue("@TenantID", AbpSession.TenantId);
                cmd.Parameters.AddWithValue("@GetFMT", fmtDocNoRequired);

                cn.Open();
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        maxid = rdr["MaxID"] is DBNull ? 0 : Convert.ToInt32(rdr["MaxID"]);
                    }

                }
            }
            return maxid;
        }

        public int GetExcelMaxDocId(string bookId, bool fmtDocNoRequired, DateTime? docDate)
        {
            int maxid = 0;
            //DateTime docDate = DateTime.Now;
            int? docFrequency = 0;
            if (bookId != "")
            {
                maxid = ((from tab1 in _gltrHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId) select (int?)tab1.DocNo).Max() ?? 0) + 1;
            }

            if (fmtDocNoRequired == true)
            {


                var glOptionsData = _glOptionRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);
                if (glOptionsData.Count() > 0)
                {
                    docFrequency = glOptionsData.FirstOrDefault().DocFrequency;
                    if (docFrequency == 1)
                    {
                      //  maxid = maxid;
                    }
                    else if (docFrequency == 2)
                    {
                        var maxfmtDocId = ((from a in _gltrHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.BookID == bookId && o.DocDate.Year ==
                        docDate.Value.Year
                        )
                                                //    select (int?)a.DocNo).Max() ?? 0) + 1;
                                            select (int?)Convert.ToInt32(a.FmtDocNo)).Max() ?? 0) + 1;
                        maxid = maxfmtDocId;
                        //fmtDocNo = maxfmtDocId.ToString("D6");
                        //fmtDocNo = (bookId + "-" + fmtDocNo);
                    }
                    else if (docFrequency == 3)
                    {
                        var maxfmtDocId = ((from a in _gltrHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.BookID == bookId && o.DocDate.Year ==
                        docDate.Value.Year && o.DocDate.Month == docDate.Value.Month
                        )
                                            select (int?)Convert.ToInt32(a.FmtDocNo)).Max() ?? 0) + 1;
                        //fmtDocNo = maxfmtDocId.ToString("D6");
                        //fmtDocNo = (bookId + "-" + fmtDocNo);
                        maxid = maxfmtDocId;
                    }
                }
            }

            //string str = ConfigurationManager.AppSettings["ConnectionString"];
            //using (SqlConnection cn = new SqlConnection(str))
            //{

            //    SqlCommand cmd;
            //    cmd = new SqlCommand("sp_GLFMTDocNo", cn);
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    var GrandAmt1 = 0.0;
            //    cmd.Parameters.AddWithValue("@bookid", bookId);
            //    cmd.Parameters.AddWithValue("@month", docDate.Value.Month);
            //    cmd.Parameters.AddWithValue("@year", docDate.Value.Year);
            //    cmd.Parameters.AddWithValue("@TenantID", AbpSession.TenantId);
            //    cmd.Parameters.AddWithValue("@GetFMT", fmtDocNoRequired);

            //    cn.Open();
            //    using (SqlDataReader rdr = cmd.ExecuteReader())
            //    {
            //        while (rdr.Read())
            //        {
            //            maxid = rdr["MaxID"] is DBNull ? 0 : Convert.ToInt32(rdr["MaxID"]);
            //        }

            //    }
            //}
            return maxid;
        }



        public int GetMaxDocIDForExcel(string bookId, bool fmtDocNoRequired, DateTime? docDate, int tenantId)
        {
            int maxid = 0;
            int fmtDocNo = 0;
            //DateTime docDate = DateTime.Now;
            int? docFrequency = 0;
            if (bookId != "")
            {
                maxid = ((from tab1 in _gltrHeaderRepository.GetAll().Where(o => o.BookID == bookId && o.TenantId == tenantId) select (int?)tab1.DocNo).Max() ?? 0) + 1;
            }

            if (fmtDocNoRequired == true)
            {
                var glOptionsData = _glOptionRepository.GetAll().Where(o => o.TenantId == tenantId);
                if (glOptionsData.Count() > 0)
                {
                    docFrequency = glOptionsData.FirstOrDefault().DocFrequency;
                    if (docFrequency == 1)
                    {
                        fmtDocNo = maxid;
                    }
                    else if (docFrequency == 2)
                    {
                        var maxfmtDocId = ((from a in _gltrHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.BookID == bookId && o.DocDate.Year ==
                        docDate.Value.Year
                        )
                                            select (int?)a.DocNo).Max() ?? 0) + 1;
                        //fmtDocNo = maxfmtDocId.ToString("D6");
                        //fmtDocNo = (bookId + "-" + fmtDocNo);
                        fmtDocNo = maxfmtDocId;
                    }
                    else if (docFrequency == 3)
                    {
                        var maxfmtDocId = ((from a in _gltrHeaderRepository.GetAll().Where(o => o.TenantId == tenantId && o.BookID == bookId && o.DocDate.Year ==
                        docDate.Value.Year && o.DocDate.Month == docDate.Value.Month
                        )
                                            select (int?)a.DocNo).Max() ?? 0) + 1;

                        fmtDocNo = maxfmtDocId;
                        //fmtDocNo = maxfmtDocId.ToString("D6");
                        //fmtDocNo = (bookId + "-" + fmtDocNo);
                    }
                }
            }

            return fmtDocNoRequired == true ? fmtDocNo : maxid;
        }




        public bool GetDirectPostedStatus(string account)
        {
            var status = false;
            var records = _glOptionRepository.GetAll().Where(o => o.DEFAULTCLACC == account && o.TenantId == AbpSession.TenantId);
            if (records.Count() > 0)
            {
                status = _glOptionRepository.GetAll().Where(o => o.DEFAULTCLACC == account && o.TenantId == AbpSession.TenantId).SingleOrDefault().DirectPost;
            }
            return status;
        }

        public async Task<GLBOOKSDto> GetBookNormalEntry(string bookId)
        {
            //int normalEntry = 0;
            //if (bookId != "")
            //{
            //    var glbooks = _glbooksRepository.GetAll().Where(o => o.BookID == bookId && o.TenantId == AbpSession.TenantId);
            //    normalEntry = glbooks.Count()>0? glbooks.SingleOrDefault().NormalEntry:0;
            //}
            //return normalEntry;

            var glbooks = await _glbooksRepository.FirstOrDefaultAsync(o => o.BookID == bookId && o.TenantId == AbpSession.TenantId);
            var output = ObjectMapper.Map<GLBOOKSDto>(glbooks);
            return output;
        }

        public string GetAccountDesc(string accountId)
        {
            string accountDesc = "";
            if (accountId != "")
            {
                accountDesc = _chartofControlRepository.GetAll().Where(o => o.Id == accountId && o.TenantId == AbpSession.TenantId).SingleOrDefault().AccountName;
            }
            return accountDesc;
        }

        public string GetSubledgerDesc(string accountId, int subledgerId)
        {
            string sublDesc = "";
            if (accountId != "" && subledgerId != 0)
            {
                sublDesc = _accountSubLedgerRepository.GetAll().Where(o => o.AccountID == accountId && o.Id == subledgerId && o.TenantId == AbpSession.TenantId).SingleOrDefault().SubAccName;
            }
            return sublDesc;
        }

        public APOptionCurrencyRateLookupTableDto GetBaseCurrency()
        {
            APOptionCurrencyRateLookupTableDto output = new APOptionCurrencyRateLookupTableDto();
            var baseCurr = _apOptionRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId).Count() > 0 ?
                _apOptionRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId).SingleOrDefault().DEFCURRCODE : "";
            var curr = _currencyRateRepository.GetAll().Where(o => o.Id == baseCurr && o.TenantId == AbpSession.TenantId).SingleOrDefault();
            if (curr != null)
            {
                output.Id = baseCurr;
                output.CurrRate = curr.CURRATE;
                output.Symbol = curr.SYMBOL;
            }
            return output;
        }


        [AbpAuthorize(AppPermissions.Transaction_VoucherEntry_Delete)]
        public async Task Delete(EntityDto input)
        {
            DeleteLog(input.Id);

            await _gltrHeaderRepository.DeleteAsync(input.Id);
            var gltrDetailsList = _gltrDetailRepository.GetAll().Where(e => e.DetID == input.Id);
            foreach (var item in gltrDetailsList)
            {
                await _gltrDetailRepository.DeleteAsync(item.Id);
            }
        }
        public void DeleteLog(int detid)
        {
            var data = _gltrHeaderRepository.FirstOrDefault(c => c.Id == detid);
            LogModel model = new LogModel()
            {
                Action = "Delete",
                Detid = detid,
                DocNo = data.DocNo,
                FormName = data.BookID,
                Status = true,
                ApprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName,
                TenantId = AbpSession.TenantId
            };
            _commonappRepository.ApproveLog(model);
        }

        public List<BooksDetailsDto> GetBooksDetails(bool Jvoucher, string interfaceName)
        {
            IQueryable<GLBOOKS> books = null;

            if (interfaceName == "transactionVoucher")
            {
                books = _glbooksRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.BookID != "JV" && o.Integrated == false && o.INACTIVE == false)
                           //.WhereIf(Jvoucher == true, e => e.NormalEntry == 3)
                           .WhereIf(Jvoucher == false, e => e.NormalEntry != 3);


            }
            else if (interfaceName == "integratedVoucher")
            {
                books = _glbooksRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.BookID != "JV" && o.Integrated == true && o.INACTIVE == false);


            }
            else
            {

                books = _glbooksRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.BookID != "JV" && o.Integrated == false && o.INACTIVE == false)
              .WhereIf(Jvoucher == true, e => e.NormalEntry == 3)
              .WhereIf(Jvoucher == false, e => e.NormalEntry != 3);



            }
            List<BooksDetailsDto> booksList = new List<BooksDetailsDto>();


            booksList = (from o in books
                         select new BooksDetailsDto
                         {
                             BookID = o.BookID,
                             BookName = o.BookName
                         }).ToList();

            if (interfaceName == "BatchListPreviews")
            {
                booksList.Add(new BooksDetailsDto
                {
                    BookID = "JV",
                    BookName = "Journal Voucher"
                });
            }
            return booksList.ToList();
        }

        public List<GLLocationDto> GetGLLocData()
        {
            var gLLoc = _glLocationRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);
            var gLLocList = from o in gLLoc
                            select new GLLocationDto
                            {
                                LocId = o.LocId,
                                LocDesc = o.LocDesc
                            };
            return gLLocList.ToList();
        }
        public List<GLLocationDto> GetUserGLLocData()
        {
            IQueryable<GLLocationDto> lookupTableDto;
               var userid = userInfo();
            if (userid.ToLower() != "admin")
            {
                var query = _csUserLocDRepository.GetAll().Where(c => c.TenantId == AbpSession.TenantId && c.TypeID == 1 && c.UserID == userid && c.Status == true);
                // .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                // e => e.LocId.ToString().ToUpper().Contains(input.Filter.ToUpper()) || e..ToUpper().Contains(input.Filter.ToUpper())).Where(o => o.TenantId == AbpSession.TenantId);
                var locQuery = _glLocationRepository.GetAll().Where(c => c.TenantId == AbpSession.TenantId);

                lookupTableDto = from o in query
                                 join p in locQuery on o.LocId equals p.LocId
                                 select new GLLocationDto
                                 {
                                     LocId = Convert.ToInt32(o.LocId),
                                     LocDesc = p.LocDesc
                                 };

            }
            else
            {
                var gLLoc = _glLocationRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);
                lookupTableDto = from o in gLLoc
                                select new GLLocationDto
                                {
                                    LocId = o.LocId,
                                    LocDesc = o.LocDesc
                                };
            }


            return lookupTableDto.ToList();
           
        }
        public string userInfo()
        {
            var data = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).FirstOrDefault();
            return data.Name;
        }

        public List<GLTRHeaderDto> ProcessVoucherEntry(VoucherEntryDto input)
        {
            //input.GLTRHeader.Posted = true;
            //input.GLTRHeader.PostedBy = input.GLTRHeader.CreatedBy;
            //input.GLTRHeader.PostedDate = DateTime.Now;

            input.GLTRHeader.Approved = true;
            input.GLTRHeader.AprovedBy = input.GLTRHeader.CreatedBy;
            input.GLTRHeader.AprovedDate = DateTime.Now;

            var gltrHeader = ObjectMapper.Map<GLTRHeader>(input.GLTRHeader);

            if (AbpSession.TenantId != null)
            {
                gltrHeader.TenantId = (int)AbpSession.TenantId;
            }


            gltrHeader.DocNo = Convert.ToInt32(GetMaxDocId(input.GLTRHeader.BookID, false, gltrHeader.DocDate));
            gltrHeader.FmtDocNo = Convert.ToInt32(GetMaxDocId(input.GLTRHeader.BookID, true, gltrHeader.DocDate));
            var getGenratedId = _gltrHeaderRepository.InsertAndGetId(gltrHeader);


            foreach (var item in input.GLTRDetail)
            {

                var gltrDetail = ObjectMapper.Map<GLTRDetail>(item);

                if (AbpSession.TenantId != null)
                {
                    gltrDetail.TenantId = (int)AbpSession.TenantId;

                }
                gltrDetail.LocId = input.GLTRHeader.LocId;
                gltrDetail.DetID = getGenratedId;
                _gltrDetailRepository.InsertAsync(gltrDetail);
            }

            List<GLTRHeaderDto> returnList = new List<GLTRHeaderDto>();
            returnList.Add(new GLTRHeaderDto
            {
                Id = getGenratedId,
                DocNo = gltrHeader.DocNo,
                LocId = gltrHeader.LocId
            });

            return returnList;
        }
        public void JVStatusChange(int? month, int? year)
        {
            try
            {
                CreateOrEditSalaryLockDto input = new CreateOrEditSalaryLockDto();
                var salaryLock = _salaryLockRepository.GetAll().Where(c => c.TenantId == AbpSession.TenantId && c.SalaryYear == year && c.SalaryMonth == month);
                if (salaryLock != null)
                {
                    string constr = ConfigurationManager.AppSettings["ConnectionStringHRM"];
                    SqlConnection con = new SqlConnection(constr);
                    SqlCommand cmd1 = new SqlCommand("update SalaryLock set jvlocked='true' where tenantid=" + AbpSession.TenantId + " and salarymonth=" + month + " and salaryyear=" + year + "", con);
                    con.Open();
                    cmd1.ExecuteNonQuery();
                    con.Close();
                }


            }
            catch (Exception ex)
            {

                throw;
            }
        }
        [HttpGet]
        public List<CreateOrEditGLTRDetailDto> FilterGLTRDDataForSalarySheet(int? Years, int? months)
        {
            //GetGLTRDetailForViewDto() List
            // GLTRDetailDto

            List<CreateOrEditGLTRDetailDto> dataList = new List<CreateOrEditGLTRDetailDto>();
            var tenantid = (Int32)AbpSession.TenantId;
            string constr = ConfigurationManager.AppSettings["ConnectionStringHRM"];
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("sp_GlLinkWithJv", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@TenantId", tenantid);
            cmd.Parameters.AddWithValue("@Year", Years);
            cmd.Parameters.AddWithValue("@Month", months);
            con.Open();
            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    CreateOrEditGLTRDetailDto data = new CreateOrEditGLTRDetailDto();
                    data.DetID = 0;
                    data.AccountID = rdr["AccountID"] is DBNull ? "" : rdr["AccountID"].ToString();
                    data.Amount = rdr["Amount"] is DBNull ? 0 : Convert.ToDouble(rdr["Amount"].ToString());
                    data.Narration = rdr["Narration"] is DBNull ? "" : rdr["Narration"].ToString() + " " + months + "And Year is " + Years + ".";
                    data.LocId = 1;
                    data.IsAuto = false;
                    dataList.Add(data);
                }
            }
            con.Close();
            return dataList;
        }
        public string CheckJVStatus(int? year, int? month)
        {
            try
            {
                var check = "";
                var isLocked = _salaryLockRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId && x.SalaryMonth == month && x.SalaryYear == year);
                if (isLocked.FirstOrDefault().Locked == false && isLocked.FirstOrDefault().JVLocked == false)
                {
                    check = "D";
                }
                else if (isLocked.FirstOrDefault().Locked == false && isLocked.FirstOrDefault().JVLocked == true)
                {
                    check = "E";
                }
                else if (isLocked.FirstOrDefault().Locked == true && isLocked.FirstOrDefault().JVLocked == false)
                {
                    check = "L";
                }
                return check;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public string GetJVForsalarySheet(int? Years, int? months)
        {

            try
            {
                var message = "";
                var Islocked = CheckJVStatus(Years, months);
                if (Islocked == "D")
                {

                    DateTime dt = new DateTime((int)Years, (int)months, 1);
                    var DocDate = dt.AddMonths(1).AddDays(-1);
                    //DocDate = new DateTime(DocDate.Year,DocDate.Month,DateTime.DaysInMonth(DocDate.Year,DocDate.Month));
                    var UserName = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                    var Detid = 0;
                    var JVList = FilterGLTRDDataForSalarySheet(Years, months);
                    var TotalAmt = Convert.ToDecimal(JVList.Where(c => c.Amount > 0).Sum(c => c.Amount).Value.ToString());
                    var fmtDocNo = Convert.ToInt32(GetMaxDocId("JV", true, DocDate));
                    var DocxNo = Convert.ToInt32(GetMaxDocId("JV", false, DocDate));
                    string constr = ConfigurationManager.AppSettings["ConnectionString"];
                    SqlConnection con = new SqlConnection(constr);
                    //For JV Header
                    if (JVList.Count > 0)
                    {
                        SqlCommand cmd = new SqlCommand("sp_InsertTrh", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);
                        cmd.Parameters.AddWithValue("@BookID", "JV");
                        cmd.Parameters.AddWithValue("@DocDate", DocDate);
                        cmd.Parameters.AddWithValue("@DocMonth", months);
                        cmd.Parameters.AddWithValue("@LocId", 2);
                        cmd.Parameters.AddWithValue("@chtype", 1);
                        cmd.Parameters.AddWithValue("@ConfigID", 0);
                        cmd.Parameters.AddWithValue("@AuditTime", DateTime.Now);
                        cmd.Parameters.AddWithValue("@AuditUser", UserName);
                        cmd.Parameters.AddWithValue("@CreatedOn", DateTime.Now);
                        cmd.Parameters.AddWithValue("@CreatedBy", UserName);
                        cmd.Parameters.AddWithValue("@CurId", "PKR");
                        cmd.Parameters.AddWithValue("@CurRate", 1);
                        cmd.Parameters.AddWithValue("@DocNo", DocxNo);
                        cmd.Parameters.AddWithValue("@Narration", "Payroll voucher for GL for the month of " + months + " And Year is " + Years);

                        cmd.Parameters.AddWithValue("@FmtDocNo", fmtDocNo);
                        cmd.Parameters.AddWithValue("@Amount", TotalAmt);
                        cmd.Parameters.Add("@DetIds", SqlDbType.BigInt).Direction = ParameterDirection.Output;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        Detid = Convert.ToInt32(cmd.Parameters["@DetIds"].Value.ToString());
                        con.Close();
                        con.Open();
                        foreach (var item in JVList)
                        {

                            SqlCommand cmd1 = new SqlCommand("sp_InsertTrd", con);
                            cmd1.CommandType = CommandType.StoredProcedure;
                            cmd1.Parameters.AddWithValue("@tenantId", AbpSession.TenantId);
                            cmd1.Parameters.AddWithValue("@DetID", Detid);
                            cmd1.Parameters.AddWithValue("@AccountID", item.AccountID);
                            cmd1.Parameters.AddWithValue("@Narration", item.Narration);
                            cmd1.Parameters.AddWithValue("@Amount", item.Amount);
                            cmd1.Parameters.AddWithValue("@CheqNo", item.ChequeNo);
                            cmd1.Parameters.AddWithValue("@locid", 2);
                            cmd1.ExecuteNonQuery();

                        }
                        con.Close();
                        JVStatusChange(months, Years);
                        message = "Done";
                    }
                    else
                    {
                        message = "Gen";
                    }

                }
                else
                {
                    if (Islocked == "E")
                    {
                        message = "Exist";
                    }
                    else
                    {
                        message = "Locked";
                    }

                }
                return message;
            }
            catch (Exception ex)
            {
                throw;

            }

        }
    }
}
