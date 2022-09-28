using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.GeneralLedger.Transaction.Dtos;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH;
using ERP.GeneralLedger.SetupForms;
using ERP.Authorization.Users;
using ERP.Configuration.Dto;
using Abp.Net.Mail;
using ERP.Configuration;
using ERP.Configuration.Tenants.Dto;
using Abp.Runtime.Session;
using Abp.Runtime.Security;
using Abp.Configuration;
using System.Net.Mail;
using ERP.Common.AlertLog;
using ERP.Common.AlertLog.Dtos;
using ERP.Common.CSAlertInfo;
using ERP.Common.AuditPostingLogs;
using ERP.Common.AuditPostingLogs.Dtos;
using System.Net;
using System.IO;

namespace ERP.GeneralLedger.Transaction
{
    [AbpAuthorize(AppPermissions.Pages_AccountsPostings)]
    public class AccountsPostingsAppService : ERPAppServiceBase, IAccountsPostingsAppService
    {
        private readonly IRepository<AccountsPosting> _accountsPostingRepository;
        private readonly IRepository<GLTRHeader> _repository;
        private readonly IRepository<GLBOOKS> _glbooksRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly UserManager _userManager;
        private readonly IRepository<GLLocation> _glLocationRepository;
        private readonly IEmailSender _emailSender;
        private readonly ICSAlertLogAppService _iCSAlertLogAppService;
        private readonly IRepository<CSAlert> _csAlertRepository;
        private readonly AuditPostingLogsAppService _auditPostingLogsAppService;
        public AccountsPostingsAppService(IRepository<AccountsPosting> accountsPostingRepository,
              IRepository<GLTRHeader> repository,
              IRepository<User, long> userRepository,
              IRepository<GLBOOKS> glbooksRepository,
               IEmailSender emailSender,
               UserManager userManager,
              IRepository<GLLocation> glLocationRepository,
              ICSAlertLogAppService iCSAlertLogAppService,
              IRepository<CSAlert> csAlertRepository,
              AuditPostingLogsAppService auditPostingLogsAppService
               )

        {
            _accountsPostingRepository = accountsPostingRepository;
            _repository = repository;
            _glbooksRepository = glbooksRepository;
            _userRepository = userRepository;
            _userManager = userManager;
            _glLocationRepository = glLocationRepository;
            _emailSender = emailSender;
            _iCSAlertLogAppService = iCSAlertLogAppService;
            _csAlertRepository = csAlertRepository;
            _auditPostingLogsAppService = auditPostingLogsAppService;
        }

        public async Task<PagedResultDto<GetAccountsPostingForViewDto>> GetAll(GetAllAccountsPostingsInput input)
        {

            var filteredAccountsPostings = _accountsPostingRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.BookID.Contains(input.Filter) || e.AuditUser.Contains(input.Filter) || e.BookName.Contains(input.Filter) || e.AccountID.Contains(input.Filter) || e.Narration.Contains(input.Filter) || e.AccountName.Contains(input.Filter) || e.SubAccName.Contains(input.Filter) || e.ChequeNo.Contains(input.Filter) || e.RegNo.Contains(input.Filter) || e.Reference.Contains(input.Filter))
                        .WhereIf(input.MinDetIDFilter != null, e => e.Id >= input.MinDetIDFilter)
                        .WhereIf(input.MaxDetIDFilter != null, e => e.Id <= input.MaxDetIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BookIDFilter), e => e.BookID.ToLower() == input.BookIDFilter.ToLower().Trim())
                        .WhereIf(input.MinConfigIDFilter != null, e => e.ConfigID >= input.MinConfigIDFilter)
                        .WhereIf(input.MaxConfigIDFilter != null, e => e.ConfigID <= input.MaxConfigIDFilter)
                        .WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
                        .WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
                        .WhereIf(input.MinDocMonthFilter != null, e => e.DocMonth >= input.MinDocMonthFilter)
                        .WhereIf(input.MaxDocMonthFilter != null, e => e.DocMonth <= input.MaxDocMonthFilter)
                        .WhereIf(input.MinDocDateFilter != null, e => e.DocDate >= input.MinDocDateFilter)
                        .WhereIf(input.MaxDocDateFilter != null, e => e.DocDate <= input.MaxDocDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AuditUserFilter), e => e.AuditUser.ToLower() == input.AuditUserFilter.ToLower().Trim())
                        .WhereIf(input.MinAuditTimeFilter != null, e => e.AuditTime >= input.MinAuditTimeFilter)
                        .WhereIf(input.MaxAuditTimeFilter != null, e => e.AuditTime <= input.MaxAuditTimeFilter)
                        .WhereIf(input.PostedFilter > -1, e => Convert.ToInt32(e.Posted) == input.PostedFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BookNameFilter), e => e.BookName.ToLower() == input.BookNameFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AccountIDFilter), e => e.AccountID.ToLower() == input.AccountIDFilter.ToLower().Trim())
                        .WhereIf(input.MinSubAccIDFilter != null, e => e.SubAccID >= input.MinSubAccIDFilter)
                        .WhereIf(input.MaxSubAccIDFilter != null, e => e.SubAccID <= input.MaxSubAccIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NarrationFilter), e => e.Narration.ToLower() == input.NarrationFilter.ToLower().Trim())
                        .WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
                        .WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AccountNameFilter), e => e.AccountName.ToLower() == input.AccountNameFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SubAccNameFilter), e => e.SubAccName.ToLower() == input.SubAccNameFilter.ToLower().Trim())
                        .WhereIf(input.MinDetailIDFilter != null, e => e.DetailID >= input.MinDetailIDFilter)
                        .WhereIf(input.MaxDetailIDFilter != null, e => e.DetailID <= input.MaxDetailIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ChequeNoFilter), e => e.ChequeNo.ToLower() == input.ChequeNoFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RegNoFilter), e => e.RegNo.ToLower() == input.RegNoFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ReferenceFilter), e => e.Reference.ToLower() == input.ReferenceFilter.ToLower().Trim());

            var pagedAndFilteredAccountsPostings = filteredAccountsPostings
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var accountsPostings = from o in pagedAndFilteredAccountsPostings
                                   select new GetAccountsPostingForViewDto()
                                   {
                                       AccountsPosting = new AccountsPostingDto
                                       {
                                           // DetID = o.DetID,
                                           BookID = o.BookID,
                                           ConfigID = o.ConfigID,
                                           DocNo = o.DocNo,
                                           DocMonth = o.DocMonth,
                                           DocDate = o.DocDate,
                                           AuditUser = o.AuditUser,
                                           AuditTime = o.AuditTime,
                                           Posted = o.Posted,
                                           BookName = o.BookName,
                                           AccountID = o.AccountID,
                                           SubAccID = o.SubAccID,
                                           Narration = o.Narration,
                                           Amount = o.Amount,
                                           AccountName = o.AccountName,
                                           SubAccName = o.SubAccName,
                                           DetailID = o.DetailID,
                                           ChequeNo = o.ChequeNo,
                                           RegNo = o.RegNo,
                                           Reference = o.Reference,
                                           Id = o.Id
                                       }
                                   };

            var totalCount = await filteredAccountsPostings.CountAsync();

            return new PagedResultDto<GetAccountsPostingForViewDto>(
                totalCount,
                await accountsPostings.ToListAsync()
            );
        }

        public async Task<GetAccountsPostingForViewDto> GetAccountsPostingForView(int id)
        {
            var accountsPosting = await _accountsPostingRepository.GetAsync(id);

            var output = new GetAccountsPostingForViewDto { AccountsPosting = ObjectMapper.Map<AccountsPostingDto>(accountsPosting) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_AccountsPostings_Edit)]
        public async Task<GetAccountsPostingForEditOutput> GetAccountsPostingForEdit(EntityDto input)
        {
            var accountsPosting = await _accountsPostingRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetAccountsPostingForEditOutput { AccountsPosting = ObjectMapper.Map<CreateOrEditAccountsPostingDto>(accountsPosting) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditAccountsPostingDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_AccountsPostings_Create)]
        protected virtual async Task Create(CreateOrEditAccountsPostingDto input)
        {
            var accountsPosting = ObjectMapper.Map<AccountsPosting>(input);


            if (AbpSession.TenantId != null)
            {
                //accountsPosting.TenantId = (int?) AbpSession.TenantId;
            }


            await _accountsPostingRepository.InsertAsync(accountsPosting);
        }



        [AbpAuthorize(AppPermissions.Pages_AccountsPostings_Edit)]
        protected virtual async Task Update(CreateOrEditAccountsPostingDto input)
        {
            var accountsPosting = await _accountsPostingRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, accountsPosting);
        }

        [AbpAuthorize(AppPermissions.Pages_AccountsPostings_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _accountsPostingRepository.DeleteAsync(input.Id);
        }


        public getDetailForAccountsPostingDto GetDetailForAccountsPosting(DateTime fromDate, DateTime toDate, string Mode, int fromDoc, int ToDoc)
        {
            IQueryable<string> accountPostingDataForBook = null;
            IQueryable<string> accountPostingDataForUser = null;
            if (Mode == "AccountsPosting" || Mode == "AccountsUnApproval")
            {
                accountPostingDataForBook = _repository.GetAll().Where(o => o.DocDate >= Convert.ToDateTime(fromDate).Date
             && o.DocDate <= Convert.ToDateTime(toDate) && o.Approved == true && o.Posted == false && o.TenantId == AbpSession.TenantId && o.DocNo >= fromDoc && o.DocNo <= ToDoc).Select(o => o.BookID).Distinct();
                accountPostingDataForUser = _repository.GetAll().Where(o => o.DocDate >= Convert.ToDateTime(fromDate).Date
             && o.DocDate <= Convert.ToDateTime(toDate) && o.Approved == true && o.Posted == false && o.TenantId == AbpSession.TenantId && o.DocNo >= fromDoc && o.DocNo <= ToDoc).Select(o => o.AuditUser).Distinct();
            }
            else
            {
                accountPostingDataForBook = _repository.GetAll().Where(o => o.DocDate >= Convert.ToDateTime(fromDate).Date
                            && o.DocDate <= Convert.ToDateTime(toDate) && o.Approved == false && o.TenantId == AbpSession.TenantId && o.DocNo >= fromDoc && o.DocNo <= ToDoc).Select(o => o.BookID).Distinct();
                accountPostingDataForUser = _repository.GetAll().Where(o => o.DocDate >= Convert.ToDateTime(fromDate).Date
             && o.DocDate <= Convert.ToDateTime(toDate) && o.Approved == false && o.TenantId == AbpSession.TenantId && o.DocNo >= fromDoc && o.DocNo <= ToDoc).Select(o => o.AuditUser).Distinct();
            }


            var getDetailForAccountsPostingDto = new getDetailForAccountsPostingDto();
            foreach (var data in accountPostingDataForBook)
            {
                getDetailForAccountsPostingDto.getBooksForAccountPostingDto.Add(
                    new getBooksForAccountPostingDto()
                    {
                        bookId = data,
                        bookName = data
                    });
            }
            foreach (var data in accountPostingDataForUser)
            {
                getDetailForAccountsPostingDto.getUsersForAccountPostingDto.Add(

                    new getUsersForAccountPostingDto()
                    {
                        UserId = data
                    });
            }

            return getDetailForAccountsPostingDto;
        }

        public List<AccountsPostingListDto> getAccountsPostingList(
            string[] users,
            string[] books,
            DateTime fromDate, DateTime toDate, string Mode, int fromDoc, int ToDoc
            )
        {
            IQueryable<GLTRHeader> query = null;
            var accountsPostingListDto = new List<AccountsPostingListDto>();
            if (Mode == "AccountsPosting" || Mode == "AccountsUnApproval")
            {
                if (users.Count() > 0 && books.Count() > 0)
                    query = _repository.GetAll().Where(b => books.Contains(b.BookID)).Where(u => users.Contains(u.AuditUser)).Where(p => p.Approved == true && p.Posted == false && p.TenantId == AbpSession.TenantId
                    && p.DocDate >= Convert.ToDateTime(fromDate).Date && p.DocNo >= fromDoc && p.DocNo <= ToDoc
                && p.DocDate <= Convert.ToDateTime(toDate));
                else if (books.Count() > 0)
                    query = _repository.GetAll().Where(b => books.Contains(b.BookID)).Where(p => p.Approved == true && p.Posted == false && p.TenantId == AbpSession.TenantId && p.DocDate >= Convert.ToDateTime(fromDate).Date
                && p.DocDate <= Convert.ToDateTime(toDate) && p.DocNo >= fromDoc && p.DocNo <= ToDoc);
                else if (users.Count() > 0)
                    query = _repository.GetAll().Where(u => users.Contains(u.AuditUser)).Where(p => p.Approved == true && p.Posted == false && p.TenantId == AbpSession.TenantId && p.DocDate >= Convert.ToDateTime(fromDate).Date
                && p.DocDate <= Convert.ToDateTime(toDate) && p.DocNo >= fromDoc && p.DocNo <= ToDoc);
            }
            else
            {
                if (users.Count() > 0 && books.Count() > 0)
                    query = _repository.GetAll().Where(b => books.Contains(b.BookID)).Where(u => users.Contains(u.AuditUser)).Where(p => p.Approved == false && p.TenantId == AbpSession.TenantId
                    && p.DocDate >= Convert.ToDateTime(fromDate).Date
                && p.DocDate <= Convert.ToDateTime(toDate) && p.DocNo >= fromDoc && p.DocNo <= ToDoc);
                else if (books.Count() > 0)
                    query = _repository.GetAll().Where(b => books.Contains(b.BookID)).Where(p => p.Approved == false && p.TenantId == AbpSession.TenantId && p.DocDate >= Convert.ToDateTime(fromDate).Date
                && p.DocDate <= Convert.ToDateTime(toDate) && p.DocNo >= fromDoc && p.DocNo <= ToDoc);
                else if (users.Count() > 0)
                    query = _repository.GetAll().Where(u => users.Contains(u.AuditUser)).Where(p => p.Approved == false && p.TenantId == AbpSession.TenantId && p.DocDate >= Convert.ToDateTime(fromDate).Date
                && p.DocDate <= Convert.ToDateTime(toDate) && p.DocNo >= fromDoc && p.DocNo <= ToDoc);
            }


            if (query.Count() > 0)
            {
                foreach (var qry in query)
                {
                    var bookName = _glbooksRepository.GetAll().Where(q => q.BookID == qry.BookID && qry.DocNo >= fromDoc && qry.DocNo <= ToDoc).Select(x => x.BookName).SingleOrDefault();
                    var locDesc = _glLocationRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.LocId == qry.LocId).Select(x => x.LocDesc).SingleOrDefault();
                    accountsPostingListDto.Add(new AccountsPostingListDto()
                    {
                        BookId = qry.BookID,
                        BookName = bookName,
                        ConfigId = qry.ConfigID,
                        DocDate = qry.DocDate,
                        DocNo = qry.DocNo,
                        Posted = qry.Posted,
                        UserId = qry.AuditUser,
                        DetailId = qry.Id,
                        NARRATION = qry.NARRATION,
                        Amount = qry.Amount,
                        LocDesc = locDesc
                    });
                }
            }

            return accountsPostingListDto;
        }





        public virtual async Task PostingData(int[] postedData, string Mode, bool bit)
        {
            try
            {
                var postedDataIds = postedData.Distinct();
                string type = "";
                if (Mode == "AccountsApproval" && bit == true)
                    type = "Approved";
                else if (Mode == "AccountsApproval" && bit == false)
                    type = "UnApproved";
                else if (Mode == "AccountsPosting")
                    type = "Posted";

                foreach (var data in postedDataIds)
                {
                    var result = _repository.GetAll().Where(o => o.Id == data).ToList();
                    var gltrHeader = await _repository.FirstOrDefaultAsync((int)data);

                    foreach (var res in result)
                    {
                        if (Mode == "AccountsPosting")
                        {

                            res.Posted = bit;
                            res.PostedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                            res.PostedDate = DateTime.Now;
                            _repository.Update(res);

                            //TenantEmailSettingsEditDto emailsettings = new TenantEmailSettingsEditDto();
                            //emailsettings = await GetEmailSettingsAsync();

                            //MailMessage mailMessage = new MailMessage();

                            //var MessageBody =  _csAlertRepository.FirstOrDefaultAsync(x=> x.AlertId == 2).Result.AlertBody;

                            //if (!string.IsNullOrWhiteSpace(MessageBody))
                            //{
                            //    mailMessage.Body = MessageBody;

                            //}
                            //else
                            //{
                            //    mailMessage.Body = res.DocNo + " Posted successfully";

                            //}

                            //var MessageSubject = _csAlertRepository.FirstOrDefaultAsync(x => x.AlertId == 2).Result.AlertSubject;
                            //if (!string.IsNullOrWhiteSpace(MessageBody))
                            //{

                            //    mailMessage.Subject = MessageSubject;
                            //}
                            //else
                            //{

                            //    mailMessage.Subject = "Voucher Posting Notification";
                            //}

                            // var admi = _userManager.GetAdminAsync();

                            //mailMessage.Sender = new MailAddress(admi.Result.EmailAddress);

                            //mailMessage.From = new MailAddress(emailsettings.DefaultFromAddress, emailsettings.DefaultFromDisplayName);
                            //mailMessage.To.Add(new MailAddress(admi.Result.EmailAddress));

                            //await _emailSender.SendAsync(emailsettings.DefaultFromAddress, admi.Result.EmailAddress, mailMessage.Subject, mailMessage.Body);

                            //CreateOrEditCSAlertLogDto input = new CreateOrEditCSAlertLogDto();

                            //input.UserHost = "";
                            //input.LogInUser =  GetCurrentUserAsync().Result.UserName;
                            //input.SentDate = DateTime.Now;
                            //input.CreatedBy = GetCurrentUserAsync().Result.UserName;
                            //input.CreateDate = DateTime.Now;
                            //input.AudtUser = GetCurrentUserAsync().Result.UserName;
                            //input.AudtDate = DateTime.Now;
                            //input.Active = 1;
                            //input.AlertId = 2 ;

                            //await _iCSAlertLogAppService.CreateOrEdit(input);

                            //if (await FeatureChecker.IsEnabledAsync((int)AbpSession.TenantId, "App.EmailAlert"))
                            //{


                            //}
                        }
                        else if (Mode == "AccountsUnApproval")
                        {
                            res.Approved = false;
                            res.AprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                            res.AprovedDate = DateTime.Now;
                            _repository.Update(res);
                        }
                        else
                        {
                            res.Approved = bit;
                            res.AprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                            res.AprovedDate = DateTime.Now;
                            _repository.Update(res);
                        }
                        // await _repository.SaveChangesAsync();
                        await _auditPostingLogsAppService.CreateOrEdit(new CreateOrEditAuditPostingLogsDto()
                        {
                            BookId = res.BookID,
                            DetId = res.Id,
                            IpAddress = GetPublicIP(),
                            Status = type,
                            TenantId = AbpSession.TenantId,
                            AuditDateTime = DateTime.Now,
                            SystemUser = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).Count() > 0 ?
                            _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName :
                             ""
                        });
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }



        private async Task<TenantEmailSettingsEditDto> GetEmailSettingsAsync()
        {
            var useHostDefaultEmailSettings = await SettingManager.GetSettingValueForTenantAsync<bool>(AppSettings.Email.UseHostDefaultEmailSettings, AbpSession.GetTenantId());

            if (useHostDefaultEmailSettings)
            {
                return new TenantEmailSettingsEditDto
                {
                    UseHostDefaultEmailSettings = true
                };
            }

            var smtpPassword = await SettingManager.GetSettingValueForTenantAsync(EmailSettingNames.Smtp.Password, AbpSession.GetTenantId());

            return new TenantEmailSettingsEditDto
            {
                UseHostDefaultEmailSettings = false,
                DefaultFromAddress = await SettingManager.GetSettingValueForTenantAsync(EmailSettingNames.DefaultFromAddress, AbpSession.GetTenantId()),
                DefaultFromDisplayName = await SettingManager.GetSettingValueForTenantAsync(EmailSettingNames.DefaultFromDisplayName, AbpSession.GetTenantId()),
                SmtpHost = await SettingManager.GetSettingValueForTenantAsync(EmailSettingNames.Smtp.Host, AbpSession.GetTenantId()),
                SmtpPort = await SettingManager.GetSettingValueForTenantAsync<int>(EmailSettingNames.Smtp.Port, AbpSession.GetTenantId()),
                SmtpUserName = await SettingManager.GetSettingValueForTenantAsync(EmailSettingNames.Smtp.UserName, AbpSession.GetTenantId()),
                SmtpPassword = SimpleStringCipher.Instance.Decrypt(smtpPassword),
                SmtpDomain = await SettingManager.GetSettingValueForTenantAsync(EmailSettingNames.Smtp.Domain, AbpSession.GetTenantId()),
                SmtpEnableSsl = await SettingManager.GetSettingValueForTenantAsync<bool>(EmailSettingNames.Smtp.EnableSsl, AbpSession.GetTenantId()),
                SmtpUseDefaultCredentials = await SettingManager.GetSettingValueForTenantAsync<bool>(EmailSettingNames.Smtp.UseDefaultCredentials, AbpSession.GetTenantId())
            };
        }
    }
}