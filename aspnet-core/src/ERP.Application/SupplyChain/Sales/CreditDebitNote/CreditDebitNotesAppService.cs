

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.SupplyChain.Sales.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ERP.SupplyChain.Sales.CreditDebitNote;
using ERP.Authorization.Users;
using ERP.SupplyChain.Inventory;
using ERP.GeneralLedger.SetupForms;
using ERP.SupplyChain.Inventory.IC_Item;
using ERP.SupplyChain.Sales.SaleAccounts;

namespace ERP.SupplyChain.Sales
{
    [AbpAuthorize(AppPermissions.Sales_CreditDebitNotes)]
    public class CreditDebitNotesAppService : ERPAppServiceBase
    //, ICreditDebitNotesAppService
    {
        private readonly IRepository<CreditDebitNoteHeader> _creditDebitNoteRepository;
        private readonly IRepository<CreditDebitNoteDetail> _creditDebitNoteDetailRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<ICLocation> _locationRepository;
        private readonly IRepository<ChartofControl, string> _chartofControlRepository;
        private readonly IRepository<AccountSubLedger> _SubLedgerRepository;
        private readonly IRepository<ICItem> _itemRepository;
        private readonly IRepository<OECOLL> _oecllRepository;
        private readonly IRepository<TransactionType> _transTypeRepository;
        public CreditDebitNotesAppService(IRepository<CreditDebitNoteHeader> creditDebitNoteRepository,
            IRepository<CreditDebitNoteDetail> creditDebitNoteDetailRepository,
            IRepository<ICLocation> locationRepository,
            IRepository<User, long> userRepository,
            IRepository<ChartofControl, string> chartofControlRepository,
            IRepository<AccountSubLedger> SubLedgerRepository,
            IRepository<ICItem> itemRepository,
            IRepository<OECOLL> oecllRepository,
            IRepository<TransactionType> transTypeRepository)
        {
            _creditDebitNoteRepository = creditDebitNoteRepository;
            _creditDebitNoteDetailRepository = creditDebitNoteDetailRepository;
            _userRepository = userRepository;
            _locationRepository = locationRepository;
            _chartofControlRepository = chartofControlRepository;
            _SubLedgerRepository = SubLedgerRepository;
            _itemRepository = itemRepository;
            _oecllRepository = oecllRepository;
            _transTypeRepository = transTypeRepository;
        }

        public async Task<PagedResultDto<GetCreditDebitNoteForViewDto>> GetAllCreditNote(GetAllCreditDebitNotesInput input)
        {
            IQueryable<CreditDebitNoteDto> filteredQry;
            if (input.Filter == null)
            {
                filteredQry = from a in _creditDebitNoteRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                              join
                              b in _locationRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                              on new { A = a.LocID, B = a.TenantId } equals new { A = b.LocID, B = b.TenantId }
                              where (a.TypeID == 1)
                              select new CreditDebitNoteDto()
                              {
                                  LocID = b.LocID,
                                  LocName = b.LocName,
                                  DocDate = a.DocDate,
                                  Id = a.Id,
                                  DocNo = a.DocNo,
                                  TypeID = a.TypeID,
                                  TransType = a.TransType
                              };
            }
            else
            {
                filteredQry = from a in _creditDebitNoteRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                              join
                              b in _locationRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                              on new { A = a.LocID, B = a.TenantId } equals new { A = b.LocID, B = b.TenantId }
                              where (a.TypeID == 1 && (b.LocName.Contains(input.Filter) || b.LocID.ToString() == input.Filter
                             || a.DocNo.ToString() == input.Filter)
                              )
                              select new CreditDebitNoteDto()
                              {
                                  LocID = b.LocID,
                                  LocName = b.LocName,
                                  DocDate = a.DocDate,
                                  Id = a.Id,
                                  DocNo = a.DocNo,
                                  TypeID = a.TypeID,
                                  TransType = a.TransType
                              };
            }

            //var filteredCreditDebitNotes = _creditDebitNoteRepository.GetAll()
            //			.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.AccountID.Contains(input.Filter) || e.Reason.Contains(input.Filter) || e.Narration.Contains(input.Filter) || e.OGP.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
            //			.WhereIf(input.MinLocIDFilter != null, e => e.LocID >= input.MinLocIDFilter)
            //			.WhereIf(input.MaxLocIDFilter != null, e => e.LocID <= input.MaxLocIDFilter)
            //			.WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
            //			.WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
            //			.WhereIf(input.MinDocDateFilter != null, e => e.DocDate >= input.MinDocDateFilter)
            //			.WhereIf(input.MaxDocDateFilter != null, e => e.DocDate <= input.MaxDocDateFilter)
            //			.WhereIf(input.MinPostingDateFilter != null, e => e.PostingDate >= input.MinPostingDateFilter)
            //			.WhereIf(input.MaxPostingDateFilter != null, e => e.PostingDate <= input.MaxPostingDateFilter)
            //			.WhereIf(input.MinPaymentDateFilter != null, e => e.PaymentDate >= input.MinPaymentDateFilter)
            //			.WhereIf(input.MaxPaymentDateFilter != null, e => e.PaymentDate <= input.MaxPaymentDateFilter)
            //			.WhereIf(input.MinTypeIDFilter != null, e => e.TypeID >= input.MinTypeIDFilter)
            //			.WhereIf(input.MaxTypeIDFilter != null, e => e.TypeID <= input.MaxTypeIDFilter)
            //			.WhereIf(!string.IsNullOrWhiteSpace(input.AccountIDFilter),  e => e.AccountID == input.AccountIDFilter)
            //			.WhereIf(input.MinSubAccIDFilter != null, e => e.SubAccID >= input.MinSubAccIDFilter)
            //			.WhereIf(input.MaxSubAccIDFilter != null, e => e.SubAccID <= input.MaxSubAccIDFilter)
            //			.WhereIf(!string.IsNullOrWhiteSpace(input.ReasonFilter),  e => e.Reason == input.ReasonFilter)
            //			.WhereIf(!string.IsNullOrWhiteSpace(input.NarrationFilter),  e => e.Narration == input.NarrationFilter)
            //			.WhereIf(!string.IsNullOrWhiteSpace(input.OGPFilter),  e => e.OGP == input.OGPFilter)
            //			.WhereIf(input.MinTotalQtyFilter != null, e => e.TotalQty >= input.MinTotalQtyFilter)
            //			.WhereIf(input.MaxTotalQtyFilter != null, e => e.TotalQty <= input.MaxTotalQtyFilter)
            //			.WhereIf(input.MinTotAmtFilter != null, e => e.TotAmt >= input.MinTotAmtFilter)
            //			.WhereIf(input.MaxTotAmtFilter != null, e => e.TotAmt <= input.MaxTotAmtFilter)
            //			.WhereIf(input.PostedFilter > -1,  e => (input.PostedFilter == 1 && e.Posted) || (input.PostedFilter == 0 && !e.Posted) )
            //			.WhereIf(input.MinLinkDetIDFilter != null, e => e.LinkDetID >= input.MinLinkDetIDFilter)
            //			.WhereIf(input.MaxLinkDetIDFilter != null, e => e.LinkDetID <= input.MaxLinkDetIDFilter)
            //			.WhereIf(input.ActiveFilter > -1,  e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active) )
            //			.WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter),  e => e.AudtUser == input.AudtUserFilter)
            //			.WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
            //			.WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
            //			.WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter),  e => e.CreatedBy == input.CreatedByFilter)
            //			.WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
            //			.WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var pagedAndFilteredCreditDebitNotes = filteredQry
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var creditDebitNotes = from o in pagedAndFilteredCreditDebitNotes
                                   select new GetCreditDebitNoteForViewDto()
                                   {
                                       CreditDebitNote = new CreditDebitNoteDto
                                       {
                                           LocID = o.LocID,
                                           LocName = o.LocName,
                                           DocNo = o.DocNo,
                                           DocDate = o.DocDate,
                                           PostingDate = o.PostingDate,
                                           PaymentDate = o.PaymentDate,
                                           TypeID = o.TypeID,
                                           AccountID = o.AccountID,
                                           SubAccID = o.SubAccID,
                                           Reason = o.Reason,
                                           Narration = o.Narration,
                                           OGP = o.OGP,
                                           TotalQty = o.TotalQty,
                                           TotAmt = o.TotAmt,
                                           Posted = o.Posted,
                                           LinkDetID = o.LinkDetID,
                                           Active = o.Active,
                                           AudtUser = o.AudtUser,
                                           AudtDate = o.AudtDate,
                                           CreatedBy = o.CreatedBy,
                                           CreateDate = o.CreateDate,
                                           TransType = o.TransType,
                                           Id = o.Id
                                       }
                                   };

            var totalCount = await filteredQry.CountAsync();

            return new PagedResultDto<GetCreditDebitNoteForViewDto>(
                totalCount,
                await creditDebitNotes.ToListAsync()
            );
        }
        public async Task<PagedResultDto<GetCreditDebitNoteForViewDto>> GetAllDebitNote(GetAllCreditDebitNotesInput input)
        {
            IQueryable<CreditDebitNoteDto> filteredQry;
            if (input.Filter == null)
            {
                filteredQry = from a in _creditDebitNoteRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                              join
                              b in _locationRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                              on new { A = a.LocID, B = a.TenantId } equals new { A = b.LocID, B = b.TenantId }
                              where (a.TypeID == 2)
                              select new CreditDebitNoteDto()
                              {
                                  LocID = b.LocID,
                                  LocName = b.LocName,
                                  DocDate = a.DocDate,
                                  Id = a.Id,
                                  DocNo = a.DocNo,
                                  TypeID = a.TypeID,
                                  TransType = a.TransType
                              };
            }
            else
            {
                filteredQry = from a in _creditDebitNoteRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                              join
                              b in _locationRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                              on new { A = a.LocID, B = a.TenantId } equals new { A = b.LocID, B = b.TenantId }
                              where ((a.TypeID == 2) && (b.LocName.Contains(input.Filter) || b.LocID.ToString() == input.Filter
                              || a.DocNo.ToString() == input.Filter)
                              )
                              select new CreditDebitNoteDto()
                              {
                                  LocID = b.LocID,
                                  LocName = b.LocName,
                                  DocDate = a.DocDate,
                                  Id = a.Id,
                                  DocNo = a.DocNo,
                                  TypeID = a.TypeID,
                                  TransType = a.TransType
                              };
            }

            //var filteredCreditDebitNotes = _creditDebitNoteRepository.GetAll()
            //			.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.AccountID.Contains(input.Filter) || e.Reason.Contains(input.Filter) || e.Narration.Contains(input.Filter) || e.OGP.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
            //			.WhereIf(input.MinLocIDFilter != null, e => e.LocID >= input.MinLocIDFilter)
            //			.WhereIf(input.MaxLocIDFilter != null, e => e.LocID <= input.MaxLocIDFilter)
            //			.WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
            //			.WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
            //			.WhereIf(input.MinDocDateFilter != null, e => e.DocDate >= input.MinDocDateFilter)
            //			.WhereIf(input.MaxDocDateFilter != null, e => e.DocDate <= input.MaxDocDateFilter)
            //			.WhereIf(input.MinPostingDateFilter != null, e => e.PostingDate >= input.MinPostingDateFilter)
            //			.WhereIf(input.MaxPostingDateFilter != null, e => e.PostingDate <= input.MaxPostingDateFilter)
            //			.WhereIf(input.MinPaymentDateFilter != null, e => e.PaymentDate >= input.MinPaymentDateFilter)
            //			.WhereIf(input.MaxPaymentDateFilter != null, e => e.PaymentDate <= input.MaxPaymentDateFilter)
            //			.WhereIf(input.MinTypeIDFilter != null, e => e.TypeID >= input.MinTypeIDFilter)
            //			.WhereIf(input.MaxTypeIDFilter != null, e => e.TypeID <= input.MaxTypeIDFilter)
            //			.WhereIf(!string.IsNullOrWhiteSpace(input.AccountIDFilter),  e => e.AccountID == input.AccountIDFilter)
            //			.WhereIf(input.MinSubAccIDFilter != null, e => e.SubAccID >= input.MinSubAccIDFilter)
            //			.WhereIf(input.MaxSubAccIDFilter != null, e => e.SubAccID <= input.MaxSubAccIDFilter)
            //			.WhereIf(!string.IsNullOrWhiteSpace(input.ReasonFilter),  e => e.Reason == input.ReasonFilter)
            //			.WhereIf(!string.IsNullOrWhiteSpace(input.NarrationFilter),  e => e.Narration == input.NarrationFilter)
            //			.WhereIf(!string.IsNullOrWhiteSpace(input.OGPFilter),  e => e.OGP == input.OGPFilter)
            //			.WhereIf(input.MinTotalQtyFilter != null, e => e.TotalQty >= input.MinTotalQtyFilter)
            //			.WhereIf(input.MaxTotalQtyFilter != null, e => e.TotalQty <= input.MaxTotalQtyFilter)
            //			.WhereIf(input.MinTotAmtFilter != null, e => e.TotAmt >= input.MinTotAmtFilter)
            //			.WhereIf(input.MaxTotAmtFilter != null, e => e.TotAmt <= input.MaxTotAmtFilter)
            //			.WhereIf(input.PostedFilter > -1,  e => (input.PostedFilter == 1 && e.Posted) || (input.PostedFilter == 0 && !e.Posted) )
            //			.WhereIf(input.MinLinkDetIDFilter != null, e => e.LinkDetID >= input.MinLinkDetIDFilter)
            //			.WhereIf(input.MaxLinkDetIDFilter != null, e => e.LinkDetID <= input.MaxLinkDetIDFilter)
            //			.WhereIf(input.ActiveFilter > -1,  e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active) )
            //			.WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter),  e => e.AudtUser == input.AudtUserFilter)
            //			.WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
            //			.WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
            //			.WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter),  e => e.CreatedBy == input.CreatedByFilter)
            //			.WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
            //			.WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var pagedAndFilteredCreditDebitNotes = filteredQry
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var creditDebitNotes = from o in pagedAndFilteredCreditDebitNotes
                                   select new GetCreditDebitNoteForViewDto()
                                   {
                                       CreditDebitNote = new CreditDebitNoteDto
                                       {
                                           LocID = o.LocID,
                                           LocName = o.LocName,
                                           DocNo = o.DocNo,
                                           DocDate = o.DocDate,
                                           PostingDate = o.PostingDate,
                                           PaymentDate = o.PaymentDate,
                                           TypeID = o.TypeID,
                                           AccountID = o.AccountID,
                                           SubAccID = o.SubAccID,
                                           Reason = o.Reason,
                                           Narration = o.Narration,
                                           OGP = o.OGP,
                                           TotalQty = o.TotalQty,
                                           TotAmt = o.TotAmt,
                                           Posted = o.Posted,
                                           LinkDetID = o.LinkDetID,
                                           Active = o.Active,
                                           AudtUser = o.AudtUser,
                                           AudtDate = o.AudtDate,
                                           CreatedBy = o.CreatedBy,
                                           CreateDate = o.CreateDate,
                                           TransType = o.TransType,
                                           Id = o.Id
                                       }
                                   };

            var totalCount = await filteredQry.CountAsync();

            return new PagedResultDto<GetCreditDebitNoteForViewDto>(
                totalCount,
                await creditDebitNotes.ToListAsync()
            );
        }

        [AbpAuthorize(AppPermissions.Sales_CreditDebitNotes_Edit)]
        public async Task<GetCreditDebitNoteForEditOutput> GetCreditDebitNoteForEdit(EntityDto input)
        {
            var creditDebitNote = await _creditDebitNoteRepository.FirstOrDefaultAsync(input.Id);
            var itemList = await _itemRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId).ToListAsync();
            var output = new GetCreditDebitNoteForEditOutput { CreditDebitNote = ObjectMapper.Map<CreateOrEditCreditDebitNoteDto>(creditDebitNote) };
            var Detail = await _creditDebitNoteDetailRepository.GetAll().Where(o => o.DetID == input.Id).ToListAsync();
            output.CreditDebitNote.LocDesc = _locationRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LocID == creditDebitNote.LocID
            ).Count() > 0 ? _locationRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LocID == creditDebitNote.LocID
            ).FirstOrDefault().LocName : "";
            output.CreditDebitNote.AccountDesc = _chartofControlRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == creditDebitNote.AccountID
          ).Count() > 0 ? _chartofControlRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == creditDebitNote.AccountID
          ).FirstOrDefault().AccountName : "";
            output.CreditDebitNote.SubAccDesc = _SubLedgerRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == creditDebitNote.SubAccID
          ).Count() > 0 ? _SubLedgerRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == creditDebitNote.SubAccID
          ).FirstOrDefault().SubAccName : "";
            output.CreditDebitNote.CreditDebitNoteDetailDto = ObjectMapper.Map<List<CreditDebitNoteDetailDto>>(Detail);

            output.CreditDebitNote.TRTypeDesc = _transTypeRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId &&
            o.TypeId == output.CreditDebitNote.TRTypeID
            ).Count() > 0 ? _transTypeRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId &&
            o.TypeId == output.CreditDebitNote.TRTypeID
            ).FirstOrDefault().Description : "";
            foreach (var data in output.CreditDebitNote.CreditDebitNoteDetailDto)
            {
                var item = itemList.Where(o => o.ItemId == data.ItemID);
                // data.MaxQty = Convert.ToInt32(GetQtyInHand(data.ItemID, data.FromLocId, data.DocNo));
                output.CreditDebitNote.CreditDebitNoteDetailDto.Where(o => o.Id == data.Id).FirstOrDefault().ItemID = item.FirstOrDefault().ItemId + "*" + item.FirstOrDefault().Descp + "*" + data.Unit + "*" + data.Conver;
            }
            return output;
        }
        //public Task CreateOrEdit(List<CreditDebitNoteDetailDto> CreditDebitNoteDetailDtolist)
        //{
        //    // if (input.Id == null)
        //    // {
        //    //await Create(input);
        //    //  }
        //    //  else
        //    //{
        //    //await Update(input);
        //    // }
        //    return null;
        //}
        public async Task CreateOrEdit(CreateOrEditCreditDebitNoteDto input)
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

        [AbpAuthorize(AppPermissions.Sales_CreditDebitNotes_Create)]
        protected virtual async Task Create(CreateOrEditCreditDebitNoteDto input)
        {
            var creditDebitNote = ObjectMapper.Map<CreditDebitNoteHeader>(input);
            creditDebitNote.AudtUser = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            creditDebitNote.CreatedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            creditDebitNote.AudtDate = DateTime.Now;
            creditDebitNote.CreateDate = DateTime.Now;
            creditDebitNote.DocNo = GetDocId();
            if (AbpSession.TenantId != null)
            {
                creditDebitNote.TenantId = (int)AbpSession.TenantId;
            }
            var getId = await _creditDebitNoteRepository.InsertAndGetIdAsync(creditDebitNote);
            foreach (var data in input.CreditDebitNoteDetailDto)
            {
                var detail = ObjectMapper.Map<CreditDebitNoteDetail>(data);
                if (AbpSession.TenantId != null)
                {
                    detail.TenantId = (int)AbpSession.TenantId;
                }
                detail.DetID = getId;
                detail.DocNo = creditDebitNote.DocNo;
                detail.LocID = creditDebitNote.LocID;
                detail.Unit = data.Unit;
                await _creditDebitNoteDetailRepository.InsertAsync(detail);
            }
        }

        [AbpAuthorize(AppPermissions.Sales_CreditDebitNotes_Edit)]
        protected virtual async Task Update(CreateOrEditCreditDebitNoteDto input)
        {
            var creditDebitNote = await _creditDebitNoteRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, creditDebitNote);
            await _creditDebitNoteDetailRepository.DeleteAsync(o => o.TenantId == AbpSession.TenantId && o.DetID == input.Id);
            foreach (var data in input.CreditDebitNoteDetailDto)
            {
                var detail = ObjectMapper.Map<CreditDebitNoteDetail>(data);
                if (AbpSession.TenantId != null)
                {
                    detail.TenantId = (int)AbpSession.TenantId;
                }
                detail.DetID = (int)input.Id;
                detail.DocNo = input.DocNo;
                detail.ItemID = data.ItemID;
                detail.LocID = input.LocID;
                detail.Remarks = data.Remarks;
                detail.Unit = data.Unit;
                detail.Amount = data.Amount;
                detail.Qty = data.Qty;
                detail.Rate = data.Rate;
                await _creditDebitNoteDetailRepository.InsertAsync(detail);
            }
        }

        [AbpAuthorize(AppPermissions.Sales_CreditDebitNotes_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _creditDebitNoteRepository.DeleteAsync(input.Id);
            await _creditDebitNoteDetailRepository.DeleteAsync(o => o.DetID == input.Id);
        }
        public int GetDocId()
        {
            var result = _creditDebitNoteRepository.GetAll().DefaultIfEmpty().Max(o => o.DocNo);
            return result = result + 1;
        }

        public string GetAccIdAgainstTransTypeAndLoc(int locId, string type)
        {
            var data = _oecllRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId
            && o.LocID == locId && o.TypeID == type
            );
            var accId = data.Count() > 0 ? data.FirstOrDefault().SalesACC : "";
            var accName = _chartofControlRepository.GetAll().Where(o => o.Id == accId && o.TenantId == AbpSession.TenantId).Count() > 0 ?
                _chartofControlRepository.GetAll().Where(o => o.Id == accId && o.TenantId == AbpSession.TenantId).FirstOrDefault().AccountName : "";
            accId = accId + "*" + accName;
            return accId;
        }
    }
}