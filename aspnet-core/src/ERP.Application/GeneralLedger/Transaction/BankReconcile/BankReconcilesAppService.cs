
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.GeneralLedger.Transaction.BankReconcile.Exporting;
using ERP.GeneralLedger.Transaction.BankReconcile.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ERP.SupplyChain.Inventory;
using ERP.Authorization.Users;

namespace ERP.GeneralLedger.Transaction.BankReconcile
{
    [AbpAuthorize(AppPermissions.Pages_BankReconciles)]
    public class BankReconcilesAppService : ERPAppServiceBase, IBankReconcilesAppService
    {
        private readonly IRepository<BankReconcile> _bankReconcileRepository;
        private readonly IBankReconcilesExcelExporter _bankReconcilesExcelExporter;
        private readonly IBankReconcileDetailsAppService _bankReconcileDetailsAppService;
        private readonly CommonAppService _commonappRepository;
        private readonly IRepository<User, long> _userRepository;





        public BankReconcilesAppService(IRepository<BankReconcile> bankReconcileRepository, IRepository<User, long> userRepository, CommonAppService commonappRepository, IBankReconcilesExcelExporter bankReconcilesExcelExporter, IBankReconcileDetailsAppService bankReconcileDetailsAppService)
        {
            _bankReconcileRepository = bankReconcileRepository;
            _bankReconcilesExcelExporter = bankReconcilesExcelExporter;
            _bankReconcileDetailsAppService = bankReconcileDetailsAppService;
            _commonappRepository = commonappRepository;
            _userRepository = userRepository;

        }
        public async Task<string> GetCurrentUserName()
        {
            var user = await UserManager.FindByIdAsync(AbpSession.UserId.ToString());
            return user.UserName;
        }
        public async Task<PagedResultDto<GetBankReconcileForViewDto>> GetAll(GetAllBankReconcilesInput input)
        {
            string userName = GetCurrentUserName().Result;

            var filteredBankReconciles = _bankReconcileRepository.GetAll().WhereIf(userName != "admin", e => e.CreatedBy == userName)
               .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.DocID.Contains(input.Filter) || e.BankID.Contains(input.Filter) || e.BankName.Contains(input.Filter) || e.Narration.Contains(input.Filter))
               .WhereIf(!string.IsNullOrWhiteSpace(input.DocNoFilter), e => e.DocID == input.DocNoFilter)
               .WhereIf(input.MinDocDateFilter != null, e => DateTime.Parse(e.DocDate.ToString()).Date >= DateTime.Parse(input.MinDocDateFilter.ToString()).Date)
               .WhereIf(input.MaxDocDateFilter != null, e => DateTime.Parse(e.DocDate.ToString()).Date <= DateTime.Parse(input.MaxDocDateFilter.ToString()).Date)
               .WhereIf(!string.IsNullOrWhiteSpace(input.BankIDFilter), e => e.BankID == input.BankIDFilter)
               .WhereIf(!string.IsNullOrWhiteSpace(input.BankNameFilter), e => e.BankName == input.BankNameFilter);


            var pagedAndFilteredBankReconciles = filteredBankReconciles
            .OrderBy(input.Sorting ?? "id asc")
            .PageBy(input);

            var bankReconciles = from o in pagedAndFilteredBankReconciles
                                 select new GetBankReconcileForViewDto()
                                 {
                                     BankReconcile = new BankReconcileDto
                                     {
                                         DocNo = o.DocNo,
                                         DocID = o.DocID,
                                         DocDate = o.DocDate,
                                         BankID = o.BankID,
                                         BankName = o.BankName,
                                         BeginBalance = o.BeginBalance,
                                         EndBalance = o.EndBalance,
                                         ReconcileAmt = o.ReconcileAmt,
                                         DiffAmount = o.DiffAmount,
                                         StatementAmt = o.StatementAmt,
                                         ClDepAmt = o.ClDepAmt,
                                         ClPayAmt = o.ClPayAmt,
                                         UnClDepAmt = o.UnClDepAmt,
                                         UnClPayAmt = o.UnClPayAmt,
                                         ClItems = o.ClItems,
                                         UnClItems = o.UnClItems,
                                         Narration = o.Narration,
                                         Completed = o.Completed,
                                         AudtUser = o.AudtUser,
                                         AudtDate = o.AudtDate,
                                         CreatedBy = o.CreatedBy,
                                         CreatedDate = o.CreatedDate,
                                         Id = o.Id,
                                         Approved = o.Approved
                                     }
                                 };

            var totalCount = await filteredBankReconciles.CountAsync();

            return new PagedResultDto<GetBankReconcileForViewDto>(
            totalCount,
            await bankReconciles.ToListAsync()
            );
        }

        [AbpAuthorize(AppPermissions.Pages_BankReconciles_Edit)]
        public async Task<GetBankReconcileForEditOutput> GetBankReconcileForEdit(EntityDto input)
        {
            var bankReconcile = await _bankReconcileRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetBankReconcileForEditOutput { BankReconcile = ObjectMapper.Map<CreateOrEditBankReconcileDto>(bankReconcile) };

            var bankReconcileDetails = await _bankReconcileDetailsAppService.GetBankReconcileDetailForEdit((int)output.BankReconcile.Id);
            output.BankReconcile.BankReconcileDetail = bankReconcileDetails.BankReconcileDetail;
            output.BankReconcile.BankReconcileDetail.ToList().ForEach(
                s =>
                {
                    s.Cr = s.Cr * -1;
                });
            return output;
        }

        public async Task CreateOrEdit(CreateOrEditBankReconcileDto input)
        {

            if (!input.flag)
            {
                await Create(input);


            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_BankReconciles_Create)]
        protected virtual async Task Create(CreateOrEditBankReconcileDto input)
        {
            var bankReconcile = ObjectMapper.Map<BankReconcile>(input);


            if (AbpSession.TenantId != null)
            {
                bankReconcile.TenantId = (int)AbpSession.TenantId;
            }


            var Hid = await _bankReconcileRepository.InsertAndGetIdAsync(bankReconcile);

            foreach (var item in input.BankReconcileDetail)
            {
                item.DetID = Hid;
            }

            await _bankReconcileDetailsAppService.CreateOrEdit(input.BankReconcileDetail);
        }

        [AbpAuthorize(AppPermissions.Pages_BankReconciles_Edit)]
        protected virtual async Task Update(CreateOrEditBankReconcileDto input)
        {
            var bankReconcile = await _bankReconcileRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, bankReconcile);

            foreach (var item in input.BankReconcileDetail)
            {
                item.DetID = (int)input.Id;
            }

            await _bankReconcileDetailsAppService.Delete((int)input.Id);
            await _bankReconcileDetailsAppService.CreateOrEdit(input.BankReconcileDetail);
        }

        [AbpAuthorize(AppPermissions.Pages_BankReconciles_Delete)]
        public async Task Delete(EntityDto input)
        {
            DeleteLog(input.Id);
            await _bankReconcileRepository.DeleteAsync(input.Id);
            await _bankReconcileDetailsAppService.Delete(input.Id);
        }
        public void DeleteLog(int detid)
        {
            var data = _bankReconcileRepository.FirstOrDefault(c => c.Id == detid);
            LogModel model = new LogModel()
            {
                Action = "Delete",
                Detid = detid,
                DocNo = data.DocNo,
                FormName = "BankReconcile",
                Status = true,
                ApprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName,
                TenantId = AbpSession.TenantId
            };
            _commonappRepository.ApproveLog(model);
        }
        public async Task<FileDto> GetBankReconcilesToExcel(GetAllBankReconcilesForExcelInput input)
        {
            string userName = GetCurrentUserName().Result;

            var filteredBankReconciles = (IQueryable<BankReconcile>)null;
            if (userName == "admin")
            {
                filteredBankReconciles = _bankReconcileRepository.GetAll()
                   .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.DocID.Contains(input.Filter) || e.BankID.Contains(input.Filter) || e.BankName.Contains(input.Filter) || e.Narration.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                   .WhereIf(!string.IsNullOrWhiteSpace(input.DocIDFilter), e => e.DocID == input.DocIDFilter)
                   .WhereIf(input.MinDocDateFilter != null, e => e.DocDate >= input.MinDocDateFilter)
                   .WhereIf(input.MaxDocDateFilter != null, e => e.DocDate <= input.MaxDocDateFilter)
                   .WhereIf(!string.IsNullOrWhiteSpace(input.BankIDFilter), e => e.BankID == input.BankIDFilter)
                   .WhereIf(!string.IsNullOrWhiteSpace(input.BankNameFilter), e => e.BankName == input.BankNameFilter)
                   .WhereIf(input.MinBeginBalanceFilter != null, e => e.BeginBalance >= input.MinBeginBalanceFilter)
                   .WhereIf(input.MaxBeginBalanceFilter != null, e => e.BeginBalance <= input.MaxBeginBalanceFilter)
                   .WhereIf(input.MinEndBalanceFilter != null, e => e.EndBalance >= input.MinEndBalanceFilter)
                   .WhereIf(input.MaxEndBalanceFilter != null, e => e.EndBalance <= input.MaxEndBalanceFilter)
                   .WhereIf(input.MinReconcileAmtFilter != null, e => e.ReconcileAmt >= input.MinReconcileAmtFilter)
                   .WhereIf(input.MaxReconcileAmtFilter != null, e => e.ReconcileAmt <= input.MaxReconcileAmtFilter)
                   .WhereIf(input.MinDiffAmountFilter != null, e => e.DiffAmount >= input.MinDiffAmountFilter)
                   .WhereIf(input.MaxDiffAmountFilter != null, e => e.DiffAmount <= input.MaxDiffAmountFilter)
                   .WhereIf(input.MinClDepAmt != null, e => e.ClDepAmt >= input.MinClDepAmt)
                   .WhereIf(input.MaxClDepAmt != null, e => e.ClDepAmt <= input.MaxClDepAmt)
                   .WhereIf(input.MinClPayAmt != null, e => e.ClPayAmt >= input.MinClPayAmt)
                   .WhereIf(input.MaxClPayAmt != null, e => e.ClPayAmt <= input.MaxClPayAmt)
                   .WhereIf(input.MinUnClDepAmt != null, e => e.UnClDepAmt >= input.MinUnClDepAmt)
                   .WhereIf(input.MaxUnClDepAmt != null, e => e.UnClDepAmt <= input.MaxUnClDepAmt)
                   .WhereIf(input.MinUnClPayAmt != null, e => e.UnClPayAmt >= input.MinUnClPayAmt)
                   .WhereIf(input.MaxUnClPayAmt != null, e => e.UnClPayAmt <= input.MaxUnClPayAmt)
                   .WhereIf(input.MinClItems != null, e => e.ClItems >= input.MinClItems)
                   .WhereIf(input.MaxClItems != null, e => e.ClItems <= input.MaxClItems)
                   .WhereIf(input.MinUnClItems != null, e => e.UnClItems >= input.MinUnClItems)
                   .WhereIf(input.MaxUnClItems != null, e => e.UnClItems <= input.MaxUnClItems)
                   .WhereIf(!string.IsNullOrWhiteSpace(input.NarrationFilter), e => e.Narration == input.NarrationFilter)
                   .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                   .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                   .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                   .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                   .WhereIf(input.MinCreatedDateFilter != null, e => e.CreatedDate >= input.MinCreatedDateFilter)
                   .WhereIf(input.MaxCreatedDateFilter != null, e => e.CreatedDate <= input.MaxCreatedDateFilter);
            }
            else
            {
                filteredBankReconciles = _bankReconcileRepository.GetAll().Where(e => e.CreatedBy == userName)
                   .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.DocID.Contains(input.Filter) || e.BankID.Contains(input.Filter) || e.BankName.Contains(input.Filter) || e.Narration.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                   .WhereIf(!string.IsNullOrWhiteSpace(input.DocIDFilter), e => e.DocID == input.DocIDFilter)
                   .WhereIf(input.MinDocDateFilter != null, e => e.DocDate >= input.MinDocDateFilter)
                   .WhereIf(input.MaxDocDateFilter != null, e => e.DocDate <= input.MaxDocDateFilter)
                   .WhereIf(!string.IsNullOrWhiteSpace(input.BankIDFilter), e => e.BankID == input.BankIDFilter)
                   .WhereIf(!string.IsNullOrWhiteSpace(input.BankNameFilter), e => e.BankName == input.BankNameFilter)
                   .WhereIf(input.MinBeginBalanceFilter != null, e => e.BeginBalance >= input.MinBeginBalanceFilter)
                   .WhereIf(input.MaxBeginBalanceFilter != null, e => e.BeginBalance <= input.MaxBeginBalanceFilter)
                   .WhereIf(input.MinEndBalanceFilter != null, e => e.EndBalance >= input.MinEndBalanceFilter)
                   .WhereIf(input.MaxEndBalanceFilter != null, e => e.EndBalance <= input.MaxEndBalanceFilter)
                   .WhereIf(input.MinReconcileAmtFilter != null, e => e.ReconcileAmt >= input.MinReconcileAmtFilter)
                   .WhereIf(input.MaxReconcileAmtFilter != null, e => e.ReconcileAmt <= input.MaxReconcileAmtFilter)
                   .WhereIf(input.MinDiffAmountFilter != null, e => e.DiffAmount >= input.MinDiffAmountFilter)
                   .WhereIf(input.MaxDiffAmountFilter != null, e => e.DiffAmount <= input.MaxDiffAmountFilter)
                   .WhereIf(input.MinClDepAmt != null, e => e.ClDepAmt >= input.MinClDepAmt)
                   .WhereIf(input.MaxClDepAmt != null, e => e.ClDepAmt <= input.MaxClDepAmt)
                   .WhereIf(input.MinClPayAmt != null, e => e.ClPayAmt >= input.MinClPayAmt)
                   .WhereIf(input.MaxClPayAmt != null, e => e.ClPayAmt <= input.MaxClPayAmt)
                   .WhereIf(input.MinUnClDepAmt != null, e => e.UnClDepAmt >= input.MinUnClDepAmt)
                   .WhereIf(input.MaxUnClDepAmt != null, e => e.UnClDepAmt <= input.MaxUnClDepAmt)
                   .WhereIf(input.MinUnClPayAmt != null, e => e.UnClPayAmt >= input.MinUnClPayAmt)
                   .WhereIf(input.MaxUnClPayAmt != null, e => e.UnClPayAmt <= input.MaxUnClPayAmt)
                   .WhereIf(input.MinClItems != null, e => e.ClItems >= input.MinClItems)
                   .WhereIf(input.MaxClItems != null, e => e.ClItems <= input.MaxClItems)
                   .WhereIf(input.MinUnClItems != null, e => e.UnClItems >= input.MinUnClItems)
                   .WhereIf(input.MaxUnClItems != null, e => e.UnClItems <= input.MaxUnClItems)
                   .WhereIf(!string.IsNullOrWhiteSpace(input.NarrationFilter), e => e.Narration == input.NarrationFilter)
                   .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                   .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                   .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                   .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                   .WhereIf(input.MinCreatedDateFilter != null, e => e.CreatedDate >= input.MinCreatedDateFilter)
                   .WhereIf(input.MaxCreatedDateFilter != null, e => e.CreatedDate <= input.MaxCreatedDateFilter);
            }



            var query = (from o in filteredBankReconciles
                         select new GetBankReconcileForViewDto()
                         {
                             BankReconcile = new BankReconcileDto
                             {
                                 DocID = o.DocID,
                                 DocDate = o.DocDate,
                                 BankID = o.BankID,
                                 BankName = o.BankName,
                                 BeginBalance = o.BeginBalance,
                                 EndBalance = o.EndBalance,
                                 ReconcileAmt = o.ReconcileAmt,
                                 DiffAmount = o.DiffAmount,
                                 StatementAmt = o.StatementAmt,
                                 ClDepAmt = o.ClDepAmt,
                                 ClPayAmt = o.ClPayAmt,
                                 UnClDepAmt = o.UnClDepAmt,
                                 UnClPayAmt = o.UnClPayAmt,
                                 ClItems = o.ClItems,
                                 UnClItems = o.UnClItems,
                                 Narration = o.Narration,
                                 Completed = o.Completed,
                                 AudtUser = o.AudtUser,
                                 AudtDate = o.AudtDate,
                                 CreatedBy = o.CreatedBy,
                                 CreatedDate = o.CreatedDate,
                                 Id = o.Id
                             }
                         });


            var bankReconcileListDtos = await query.ToListAsync();

            return _bankReconcilesExcelExporter.ExportToFile(bankReconcileListDtos);
        }
        public int GetMaxDocNo()
        {
            var maxid = ((from tab1 in _bankReconcileRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId) select (int?)tab1.DocNo).Max() ?? 0) + 1;
            return maxid;
        }


        public double? GetBeginningBalance(string bankID)
        {
            double? checkBankID = _bankReconcileRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.BankID == bankID).Count() > 0 ?
                _bankReconcileRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.BankID == bankID).LastOrDefault().StatementAmt : 0;
            return checkBankID;
        }
        public virtual void ApproveBankReconcile(int id, bool IsApproved)
        {
            var bankReconcile = _bankReconcileRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == id).FirstOrDefault();
            var DocNo = 0;
            DocNo = (int)bankReconcile.DocNo;
            bankReconcile.Approved = IsApproved;
            bankReconcile.ApprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            bankReconcile.ApprovedDatetime = DateTime.Now;
            _bankReconcileRepository.Update(bankReconcile);

            LogModel Log = new LogModel()
            {
                Status = IsApproved,
                ApprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName,
                Detid = Convert.ToInt32(id),
                DocNo = DocNo,
                Action = IsApproved == true ? "Approval" : "UnApproval",
                FormName = "BankReconcile",
                TenantId = AbpSession.TenantId
            };
            _commonappRepository.ApproveLog(Log);
        }

        //public virtual void ApproveBankReconcile(int id, bool IsApproved)
        //{
        //    var bankReconcile = _bankReconcileRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == id).FirstOrDefault();

        //    bankReconcile.Approved = IsApproved;
        //    _bankReconcileRepository.Update(bankReconcile);

        //}

        public BankReconcile GetUnapprovedCount(string bankId)
        {

            var unapproved = _bankReconcileRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.BankID == bankId && (o.Approved == false || o.Approved == null)).FirstOrDefault();

            return unapproved;
        }
    }
}