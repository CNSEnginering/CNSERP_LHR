using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.SupplyChain.Sales.SaleQutation.Exporting;
using ERP.SupplyChain.Sales.SaleQutation.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using ERP.Storage;
using ERP.Authorization.Users;
using ERP.SupplyChain.Inventory;
using ERP.SupplyChain.Inventory.IC_Item;
using ERP.GeneralLedger.SetupForms.LedgerType;
using ERP.GeneralLedger.SetupForms;
using ERP.SupplyChain.Sales.SaleAccounts;
using ERP.SupplyChain.Inventory.ICOPT4;
using ERP.CommonServices;
using ERP.SupplyChain.Sales.SaleEntry.Dtos;
using ERP.SupplyChain.Sales.SaleEntry;
using ERP.SupplyChain.Sales.OECSH.Dtos;

namespace ERP.SupplyChain.Sales.SaleQutation
{
    [AbpAuthorize(AppPermissions.Pages_OEQH)]
    public class OEQHAppService : ERPAppServiceBase, IOEQHAppService
    {
        private readonly IRepository<ICItem> _ICItemRepository;
        private readonly IRepository<OESALEDetail> _oesaleDetailRepository;
        private readonly IRepository<OEQH> _oeqhRepository;
        private readonly IOEQHExcelExporter _oeqhExcelExporter;
        private readonly IRepository<ICLocation> _icLocationRepository;
        private readonly IRepository<OEQD> _oeqDRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<ICLocation> _locRepository;
        private readonly IRepository<OESALEHeader> _oesaleHeaderRepository;
        private readonly IRepository<ICItem> _itemRepository;
        private readonly IRepository<TransactionType> _transactionTypeRepository;
        private readonly IRepository<LedgerType> _ledgerTypeRepository;
        private readonly IRepository<GLSecChartofControl, string> _glSecChartofControlRepository;
        private readonly IRepository<OECOLL> _oecollRepository;
        private readonly IRepository<ICOPT4> _icopT4Repository;
        private readonly IRepository<TaxClass> _taxClassRepository;
        private readonly IRepository<AccountSubLedger> _accountSubLedgerRepository;
        private readonly CommonAppService _commonappRepository;


        public OEQHAppService(IRepository<ICItem> ICItemRepository, CommonAppService commonappRepository, IRepository<OESALEDetail> oesaleDetailRepository,IRepository<OEQH> oeqhRepository, IRepository<TaxClass> taxClassRepository, IRepository<OESALEHeader> oesaleHeaderRepository, IRepository<ICOPT4> icopT4Repository, IRepository<LedgerType> ledgerTypeRepository, IRepository<GLSecChartofControl, string> glSecChartofControlRepository, IRepository<AccountSubLedger> accountSubLedgerRepository, IRepository<OECOLL> oecollRepository, IRepository<TransactionType> transactionTypeRepository, IRepository<ICItem> itemRepository, IRepository<ICLocation> locRepository, IRepository<User, long> userRepository, IOEQHExcelExporter oeqhExcelExporter, IRepository<OEQD> oeqDRepository, IRepository<ICLocation> icLocationRepository)
        {
            _ICItemRepository = ICItemRepository;
            _oesaleDetailRepository = oesaleDetailRepository;
            _oeqhRepository = oeqhRepository;
            _taxClassRepository = taxClassRepository;
            _oeqhExcelExporter = oeqhExcelExporter;
            _glSecChartofControlRepository = glSecChartofControlRepository;
            _oesaleHeaderRepository = oesaleHeaderRepository;
            _oeqDRepository = oeqDRepository;
            _userRepository = userRepository;
            _locRepository = locRepository;
            _ledgerTypeRepository = ledgerTypeRepository;
            _itemRepository = itemRepository;
            _transactionTypeRepository = transactionTypeRepository;
            _oecollRepository = oecollRepository;
            _accountSubLedgerRepository = accountSubLedgerRepository;
            _icopT4Repository = icopT4Repository;
            _icLocationRepository = icLocationRepository;

            _commonappRepository = commonappRepository;

        }
        public int GetDocId()
        {
            var result = _oeqhRepository.GetAll().DefaultIfEmpty().Max(o => o.DocNo);
            return result = result + 1;
        }
        public async Task<PagedResultDto<GetOEQHForViewDto>> GetAll(GetAllOEQHInput input)
        {

            var filteredOEQH = _oeqhRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.MDocNo.Contains(input.Filter) || e.TypeID.Contains(input.Filter) || e.SalesCtrlAcc.Contains(input.Filter) || e.Narration.Contains(input.Filter) || e.NoteText.Contains(input.Filter) || e.PayTerms.Contains(input.Filter) || e.DelvTerms.Contains(input.Filter) || e.ValidityTerms.Contains(input.Filter) || e.ApprovedBy.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter) || e.TaxAuth1.Contains(input.Filter) || e.TaxClassDesc1.Contains(input.Filter) || e.TaxAuth2.Contains(input.Filter) || e.TaxClassDesc2.Contains(input.Filter) || e.TaxAuth3.Contains(input.Filter) || e.TaxClassDesc3.Contains(input.Filter) || e.TaxAuth4.Contains(input.Filter) || e.TaxClassDesc4.Contains(input.Filter) || e.TaxAuth5.Contains(input.Filter) || e.TaxClassDesc5.Contains(input.Filter))
                        .WhereIf(input.MinLocIDFilter != null, e => e.LocID >= input.MinLocIDFilter)
                        .WhereIf(input.MaxLocIDFilter != null, e => e.LocID <= input.MaxLocIDFilter)
                        .WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
                        .WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
                        .WhereIf(input.MinDocDateFilter != null, e => e.DocDate >= input.MinDocDateFilter)
                        .WhereIf(input.MaxDocDateFilter != null, e => e.DocDate <= input.MaxDocDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MDocNoFilter), e => e.MDocNo == input.MDocNoFilter)
                        .WhereIf(input.MinMDocDateFilter != null, e => e.MDocDate >= input.MinMDocDateFilter)
                        .WhereIf(input.MaxMDocDateFilter != null, e => e.MDocDate <= input.MaxMDocDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TypeIDFilter), e => e.TypeID == input.TypeIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SalesCtrlAccFilter), e => e.SalesCtrlAcc == input.SalesCtrlAccFilter)
                        .WhereIf(input.MinCustIDFilter != null, e => e.CustID >= input.MinCustIDFilter)
                        .WhereIf(input.MaxCustIDFilter != null, e => e.CustID <= input.MaxCustIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NarrationFilter), e => e.Narration == input.NarrationFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NoteTextFilter), e => e.NoteText == input.NoteTextFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PayTermsFilter), e => e.PayTerms == input.PayTermsFilter)
                        .WhereIf(input.MinNetAmountFilter != null, e => e.NetAmount >= input.MinNetAmountFilter)
                        .WhereIf(input.MaxNetAmountFilter != null, e => e.NetAmount <= input.MaxNetAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DelvTermsFilter), e => e.DelvTerms == input.DelvTermsFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ValidityTermsFilter), e => e.ValidityTerms == input.ValidityTermsFilter)
                        .WhereIf(input.ApprovedFilter.HasValue && input.ApprovedFilter > -1, e => (input.ApprovedFilter == 1 && e.Approved) || (input.ApprovedFilter == 0 && !e.Approved))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ApprovedByFilter), e => e.ApprovedBy == input.ApprovedByFilter)
                        .WhereIf(input.MinApprovedDateFilter != null, e => e.ApprovedDate >= input.MinApprovedDateFilter)
                        .WhereIf(input.MaxApprovedDateFilter != null, e => e.ApprovedDate <= input.MaxApprovedDateFilter)
                        .WhereIf(input.ActiveFilter.HasValue && input.ActiveFilter > -1, e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaxAuth1Filter), e => e.TaxAuth1 == input.TaxAuth1Filter)
                        .WhereIf(input.MinTaxClass1Filter != null, e => e.TaxClass1 >= input.MinTaxClass1Filter)
                        .WhereIf(input.MaxTaxClass1Filter != null, e => e.TaxClass1 <= input.MaxTaxClass1Filter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaxClassDesc1Filter), e => e.TaxClassDesc1 == input.TaxClassDesc1Filter)
                        .WhereIf(input.MinTaxRate1Filter != null, e => e.TaxRate1 >= input.MinTaxRate1Filter)
                        .WhereIf(input.MaxTaxRate1Filter != null, e => e.TaxRate1 <= input.MaxTaxRate1Filter)
                        .WhereIf(input.MinTaxAmt1Filter != null, e => e.TaxAmt1 >= input.MinTaxAmt1Filter)
                        .WhereIf(input.MaxTaxAmt1Filter != null, e => e.TaxAmt1 <= input.MaxTaxAmt1Filter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaxAuth2Filter), e => e.TaxAuth2 == input.TaxAuth2Filter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaxClassDesc2Filter), e => e.TaxClassDesc2 == input.TaxClassDesc2Filter)
                        .WhereIf(input.MinTaxClass2Filter != null, e => e.TaxClass2 >= input.MinTaxClass2Filter)
                        .WhereIf(input.MaxTaxClass2Filter != null, e => e.TaxClass2 <= input.MaxTaxClass2Filter)
                        .WhereIf(input.MinTaxRate2Filter != null, e => e.TaxRate2 >= input.MinTaxRate2Filter)
                        .WhereIf(input.MaxTaxRate2Filter != null, e => e.TaxRate2 <= input.MaxTaxRate2Filter)
                        .WhereIf(input.MinTaxAmt2Filter != null, e => e.TaxAmt2 >= input.MinTaxAmt2Filter)
                        .WhereIf(input.MaxTaxAmt2Filter != null, e => e.TaxAmt2 <= input.MaxTaxAmt2Filter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaxAuth3Filter), e => e.TaxAuth3 == input.TaxAuth3Filter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaxClassDesc3Filter), e => e.TaxClassDesc3 == input.TaxClassDesc3Filter)
                        .WhereIf(input.MinTaxClass3Filter != null, e => e.TaxClass3 >= input.MinTaxClass3Filter)
                        .WhereIf(input.MaxTaxClass3Filter != null, e => e.TaxClass3 <= input.MaxTaxClass3Filter)
                        .WhereIf(input.MinTaxRate3Filter != null, e => e.TaxRate3 >= input.MinTaxRate3Filter)
                        .WhereIf(input.MaxTaxRate3Filter != null, e => e.TaxRate3 <= input.MaxTaxRate3Filter)
                        .WhereIf(input.MinTaxAmt3Filter != null, e => e.TaxAmt3 >= input.MinTaxAmt3Filter)
                        .WhereIf(input.MaxTaxAmt3Filter != null, e => e.TaxAmt3 <= input.MaxTaxAmt3Filter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaxAuth4Filter), e => e.TaxAuth4 == input.TaxAuth4Filter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaxClassDesc4Filter), e => e.TaxClassDesc4 == input.TaxClassDesc4Filter)
                        .WhereIf(input.MinTaxClass4Filter != null, e => e.TaxClass4 >= input.MinTaxClass4Filter)
                        .WhereIf(input.MaxTaxClass4Filter != null, e => e.TaxClass4 <= input.MaxTaxClass4Filter)
                        .WhereIf(input.MinTaxRate4Filter != null, e => e.TaxRate4 >= input.MinTaxRate4Filter)
                        .WhereIf(input.MaxTaxRate4Filter != null, e => e.TaxRate4 <= input.MaxTaxRate4Filter)
                        .WhereIf(input.MinTaxAmt4Filter != null, e => e.TaxAmt4 >= input.MinTaxAmt4Filter)
                        .WhereIf(input.MaxTaxAmt4Filter != null, e => e.TaxAmt4 <= input.MaxTaxAmt4Filter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaxAuth5Filter), e => e.TaxAuth5 == input.TaxAuth5Filter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaxClassDesc5Filter), e => e.TaxClassDesc5 == input.TaxClassDesc5Filter)
                        .WhereIf(input.MinTaxClass5Filter != null, e => e.TaxClass5 >= input.MinTaxClass5Filter)
                        .WhereIf(input.MaxTaxClass5Filter != null, e => e.TaxClass5 <= input.MaxTaxClass5Filter)
                        .WhereIf(input.MinTaxRate5Filter != null, e => e.TaxRate5 >= input.MinTaxRate5Filter)
                        .WhereIf(input.MaxTaxRate5Filter != null, e => e.TaxRate5 <= input.MaxTaxRate5Filter)
                        .WhereIf(input.MinTaxAmt5Filter != null, e => e.TaxAmt5 >= input.MinTaxAmt5Filter)
                        .WhereIf(input.MaxTaxAmt5Filter != null, e => e.TaxAmt5 <= input.MaxTaxAmt5Filter);

            var pagedAndFilteredOEQH = filteredOEQH
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var oeqh = from o in pagedAndFilteredOEQH
                       select new
                       {

                           o.LocID,
                           o.DocNo,
                           o.DocDate,
                           o.MDocNo,
                           o.MDocDate,
                           o.TypeID,
                           o.SalesCtrlAcc,
                           o.CustID,
                           o.Narration,
                           o.NoteText,
                           o.PayTerms,
                           o.NetAmount,
                           o.DelvTerms,
                           o.ValidityTerms,
                           o.Approved,
                           o.ApprovedBy,
                           o.ApprovedDate,
                           o.Active,
                           o.AudtUser,
                           o.AudtDate,
                           o.CreatedBy,
                           o.CreateDate,
                           o.TaxAuth1,
                           o.TaxClass1,
                           o.TaxClassDesc1,
                           o.TaxRate1,
                           o.TaxAmt1,
                           o.TaxAuth2,
                           o.TaxClassDesc2,
                           o.TaxClass2,
                           o.TaxRate2,
                           o.TaxAmt2,
                           o.TaxAuth3,
                           o.TaxClassDesc3,
                           o.TaxClass3,
                           o.TaxRate3,
                           o.TaxAmt3,
                           o.TaxAuth4,
                           o.TaxClassDesc4,
                           o.TaxClass4,
                           o.TaxRate4,
                           o.TaxAmt4,
                           o.TaxAuth5,
                           o.TaxClassDesc5,
                           o.TaxClass5,
                           o.TaxRate5,
                           o.TaxAmt5,
                           Id = o.Id
                       };

            var totalCount = await filteredOEQH.CountAsync();

            var dbList = await oeqh.ToListAsync();
            var results = new List<GetOEQHForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetOEQHForViewDto()
                {
                    OEQH = new OEQHDto
                    {

                        LocID = o.LocID,
                        DocNo = o.DocNo,
                        DocDate = o.DocDate,
                        MDocNo = o.MDocNo,
                        MDocDate = o.MDocDate,
                        TypeID = o.TypeID,
                        SalesCtrlAcc = o.SalesCtrlAcc,
                        CustID = o.CustID,
                        Narration = o.Narration,
                        NoteText = o.NoteText,
                        PayTerms = o.PayTerms,
                        NetAmount = o.NetAmount,
                        DelvTerms = o.DelvTerms,
                        ValidityTerms = o.ValidityTerms,
                        Approved = o.Approved,
                        ApprovedBy = o.ApprovedBy,
                        ApprovedDate = o.ApprovedDate,
                        Active = o.Active,
                        AudtUser = o.AudtUser,
                        AudtDate = o.AudtDate,
                        CreatedBy = o.CreatedBy,
                        CreateDate = o.CreateDate,
                        TaxAuth1 = o.TaxAuth1,
                        TaxClass1 = o.TaxClass1,
                        TaxClassDesc1 = o.TaxClassDesc1,
                        TaxRate1 = o.TaxRate1,
                        TaxAmt1 = o.TaxAmt1,
                        TaxAuth2 = o.TaxAuth2,
                        TaxClassDesc2 = o.TaxClassDesc2,
                        TaxClass2 = o.TaxClass2,
                        TaxRate2 = o.TaxRate2,
                        TaxAmt2 = o.TaxAmt2,
                        TaxAuth3 = o.TaxAuth3,
                        TaxClassDesc3 = o.TaxClassDesc3,
                        TaxClass3 = o.TaxClass3,
                        TaxRate3 = o.TaxRate3,
                        TaxAmt3 = o.TaxAmt3,
                        TaxAuth4 = o.TaxAuth4,
                        TaxClassDesc4 = o.TaxClassDesc4,
                        TaxClass4 = o.TaxClass4,
                        TaxRate4 = o.TaxRate4,
                        TaxAmt4 = o.TaxAmt4,
                        TaxAuth5 = o.TaxAuth5,
                        TaxClassDesc5 = o.TaxClassDesc5,
                        TaxClass5 = o.TaxClass5,
                        TaxRate5 = o.TaxRate5,
                        TaxAmt5 = o.TaxAmt5,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetOEQHForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetOEQHForViewDto> GetOEQHForView(int id)
        {
            var oeqh = await _oeqhRepository.GetAsync(id);

            var output = new GetOEQHForViewDto { OEQH = ObjectMapper.Map<OEQHDto>(oeqh) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_OEQH_Edit)]
        public async Task<GetOEQHForEditOutput> GetOEQHForEdit(EntityDto input)
        {
            try
            {
                var itemList = await _itemRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId).ToListAsync();
                var TaxList = await _taxClassRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId).ToListAsync();
                var oeqh = await _oeqhRepository.FirstOrDefaultAsync(x => x.DocNo == input.Id && x.TenantId == AbpSession.TenantId);

                var detailQ = _oeqDRepository.GetAll().Where(x => x.DocNo == oeqh.DocNo && x.TenantId == AbpSession.TenantId);

                var output = new GetOEQHForEditOutput { OEQH = ObjectMapper.Map<CreateOrEditOEQHDto>(oeqh) };
                output.OEQH.QutationDetailDto = ObjectMapper.Map<List<OEQDDto>>(detailQ);

                //Location Description 
                output.OEQH.LocDesc = _locRepository.GetAll().Where(o => o.LocID == oeqh.LocID).Count() > 0
                  ?
                  _locRepository.GetAll().Where(o => o.LocID == oeqh.LocID).FirstOrDefault().LocName
                  : "";
                //sale type 

                output.OEQH.SaleTypeDesc = _transactionTypeRepository.GetAll().Where(x => x.TypeId.ToUpper().Trim() == oeqh.TypeID.ToUpper().Trim()).Count() > 0 ?
                    _transactionTypeRepository.GetAll().Where(x => x.TypeId == oeqh.TypeID).FirstOrDefault().Description : "";

                //Customer 
                var accountID = _oecollRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.TypeID.ToUpper().Trim() == oeqh.TypeID.ToUpper().Trim()).Count() > 0 ?
                  _oecollRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.TypeID.ToUpper().Trim() == oeqh.TypeID.ToUpper().Trim()).SingleOrDefault().ChAccountID : "";
                if (accountID != "" && accountID != null)
                {
                    output.OEQH.CustomerDesc = _accountSubLedgerRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId && x.AccountID == accountID && x.Id == oeqh.CustID).WhereIf(!string.IsNullOrWhiteSpace(""),
                      e => e.Id.ToString().ToUpper().Contains("") || e.SubAccName.ToString().ToUpper().Contains("")).FirstOrDefault().SubAccName;

                }
                //Chart Of Account
                //var query = from o in _glSecChartofControlRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                //            join o1 in _ledgerTypeRepository.GetAll()
                //            on new { SLType = (int)o.SLType, o.TenantId } equals new { SLType = o1.LedgerID, o1.TenantId }
                //            into f
                //            from o1 in f.DefaultIfEmpty()
                //            select new { o.Id, o.AccountName, o.SubLedger, o.SLType, LedgerDesc = o1 != null ? o1.LedgerDesc : "" };
                //output.OEQH.ChartofAccountDesc = query.Where(x => x.Id == oeqh.SalesCtrlAcc).FirstOrDefault().AccountName;
                var tenantID = (int)AbpSession.TenantId;


                foreach (var data in output.OEQH.QutationDetailDto)
                {
                    var item = itemList.Where(o => o.ItemId == data.ItemID);

                    output.OEQH.QutationDetailDto.Where(o => o.Id == data.Id).FirstOrDefault().ItemID = item.FirstOrDefault().ItemId + "*" + item.FirstOrDefault().Descp + "*" + data.Unit + "*" + data.Conver;
                    output.OEQH.QutationDetailDto.Where(o => o.Id == data.Id).FirstOrDefault().TransName = _icopT4Repository.GetAll().Where(x => x.TenantId == tenantID && x.OptID == data.TransType).Count() > 0 ? _icopT4Repository.GetAll().Where(x => x.TenantId == tenantID && x.OptID == data.TransType).FirstOrDefault().Descp : "";
                    output.OEQH.QutationDetailDto.Where(o => o.Id == data.Id).FirstOrDefault().TaxClassDesc = _taxClassRepository.GetAll().Where(x => x.TenantId == tenantID && x.CLASSID == data.TaxClass).Count() > 0 ? _taxClassRepository.GetAll().Where(x => x.TenantId == tenantID && x.CLASSID == data.TaxClass).FirstOrDefault().CLASSDESC: "";
                }

                return output;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task CreateOrEdit(CreateOrEditOEQHDto input)
        {
            if (input.Id == null || input.Id == 0)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_OEQH_Create)]
        protected virtual async Task Create(CreateOrEditOEQHDto input)
        {
            try
            {
                var oeqh = ObjectMapper.Map<OEQH>(input);

                if (AbpSession.TenantId != null)
                {
                    oeqh.TenantId = (int)AbpSession.TenantId;
                    oeqh.CreateDate = DateTime.Now;
                    oeqh.CreatedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                }
                var getId = await _oeqhRepository.InsertAndGetIdAsync(oeqh);

                if (input.QutationDetailDto != null)
                {
                    foreach (var data in input.QutationDetailDto)
                    {

                        var QDetail = ObjectMapper.Map<OEQD>(data);
                        if (AbpSession.TenantId != null)
                        {
                            QDetail.TenantId = (int)AbpSession.TenantId;
                        }
                        QDetail.DetID = getId;
                        QDetail.DocNo = input.DocNo;
                        QDetail.LocID = input.LocID;

                        await _oeqDRepository.InsertAsync(QDetail);


                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }


        }

        [AbpAuthorize(AppPermissions.Pages_OEQH_Edit)]
        protected virtual async Task Update(CreateOrEditOEQHDto input)
        {
            try
            {
                input.AudtDate = DateTime.Now;
                input.AudtUser = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                var oeqh = await _oeqhRepository.FirstOrDefaultAsync(x => x.DocNo == input.DocNo && x.TenantId == AbpSession.TenantId);
                ObjectMapper.Map(input, oeqh);
                //await _oeqDRepository.Del(o => o.TenantId == AbpSession.TenantId && o.DetID == input.Id,);

                //For Qutation Detail
                var oeqDet = await _oeqDRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId && x.DocNo == input.DocNo).ToListAsync();
                if (oeqDet != null)
                {
                    foreach (var item in oeqDet)
                    {
                        await _oeqDRepository.DeleteAsync(item);
                    }
                }


                if (input.QutationDetailDto != null)
                {
                    foreach (var data in input.QutationDetailDto)
                    {

                        var QDetail = ObjectMapper.Map<OEQD>(data);

                        if (AbpSession.TenantId != null)
                        {
                            QDetail.TenantId = (int)AbpSession.TenantId;
                        }
                        QDetail.DetID = oeqh.Id;
                        QDetail.DocNo = input.DocNo;
                        QDetail.LocID = input.LocID;
                        await _oeqDRepository.InsertAsync(QDetail);
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }

        [AbpAuthorize(AppPermissions.Pages_OEQH_Delete)]
        public async Task Delete(EntityDto input)
        {
            DeleteLog(input.Id);
            await _oeqhRepository.DeleteAsync(x => x.DocNo == input.Id && x.TenantId == AbpSession.TenantId);
            var oeqDetailsList = _oeqDRepository.GetAll().Where(e => e.DocNo == input.Id && e.TenantId == AbpSession.TenantId);
            foreach (var item in oeqDetailsList)
            {
                await _oeqDRepository.DeleteAsync(item.Id);
            }
        }
        public void DeleteLog(int detid)
        {
            var data = _oeqhRepository.FirstOrDefault(c => c.DocNo == detid && c.TenantId == AbpSession.TenantId);
            LogModel model = new LogModel()
            {
                Action = "Delete",
                Detid = data.Id,
                DocNo = detid,
                FormName = "Quotation",
                Status = true,
                ApprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName,
                TenantId = AbpSession.TenantId
            };
            _commonappRepository.ApproveLog(model);
        }

        public async Task<FileDto> GetOEQHToExcel(GetAllOEQHForExcelInput input)
        {

            var filteredOEQH = _oeqhRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.MDocNo.Contains(input.Filter) || e.TypeID.Contains(input.Filter) || e.SalesCtrlAcc.Contains(input.Filter) || e.Narration.Contains(input.Filter) || e.NoteText.Contains(input.Filter) || e.PayTerms.Contains(input.Filter) || e.DelvTerms.Contains(input.Filter) || e.ValidityTerms.Contains(input.Filter) || e.ApprovedBy.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter) || e.TaxAuth1.Contains(input.Filter) || e.TaxClassDesc1.Contains(input.Filter) || e.TaxAuth2.Contains(input.Filter) || e.TaxClassDesc2.Contains(input.Filter) || e.TaxAuth3.Contains(input.Filter) || e.TaxClassDesc3.Contains(input.Filter) || e.TaxAuth4.Contains(input.Filter) || e.TaxClassDesc4.Contains(input.Filter) || e.TaxAuth5.Contains(input.Filter) || e.TaxClassDesc5.Contains(input.Filter))
                        .WhereIf(input.MinLocIDFilter != null, e => e.LocID >= input.MinLocIDFilter)
                        .WhereIf(input.MaxLocIDFilter != null, e => e.LocID <= input.MaxLocIDFilter)
                        .WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
                        .WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
                        .WhereIf(input.MinDocDateFilter != null, e => e.DocDate >= input.MinDocDateFilter)
                        .WhereIf(input.MaxDocDateFilter != null, e => e.DocDate <= input.MaxDocDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MDocNoFilter), e => e.MDocNo == input.MDocNoFilter)
                        .WhereIf(input.MinMDocDateFilter != null, e => e.MDocDate >= input.MinMDocDateFilter)
                        .WhereIf(input.MaxMDocDateFilter != null, e => e.MDocDate <= input.MaxMDocDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TypeIDFilter), e => e.TypeID == input.TypeIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SalesCtrlAccFilter), e => e.SalesCtrlAcc == input.SalesCtrlAccFilter)
                        .WhereIf(input.MinCustIDFilter != null, e => e.CustID >= input.MinCustIDFilter)
                        .WhereIf(input.MaxCustIDFilter != null, e => e.CustID <= input.MaxCustIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NarrationFilter), e => e.Narration == input.NarrationFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NoteTextFilter), e => e.NoteText == input.NoteTextFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PayTermsFilter), e => e.PayTerms == input.PayTermsFilter)
                        .WhereIf(input.MinNetAmountFilter != null, e => e.NetAmount >= input.MinNetAmountFilter)
                        .WhereIf(input.MaxNetAmountFilter != null, e => e.NetAmount <= input.MaxNetAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DelvTermsFilter), e => e.DelvTerms == input.DelvTermsFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ValidityTermsFilter), e => e.ValidityTerms == input.ValidityTermsFilter)
                        .WhereIf(input.ApprovedFilter.HasValue && input.ApprovedFilter > -1, e => (input.ApprovedFilter == 1 && e.Approved) || (input.ApprovedFilter == 0 && !e.Approved))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ApprovedByFilter), e => e.ApprovedBy == input.ApprovedByFilter)
                        .WhereIf(input.MinApprovedDateFilter != null, e => e.ApprovedDate >= input.MinApprovedDateFilter)
                        .WhereIf(input.MaxApprovedDateFilter != null, e => e.ApprovedDate <= input.MaxApprovedDateFilter)
                        .WhereIf(input.ActiveFilter.HasValue && input.ActiveFilter > -1, e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaxAuth1Filter), e => e.TaxAuth1 == input.TaxAuth1Filter)
                        .WhereIf(input.MinTaxClass1Filter != null, e => e.TaxClass1 >= input.MinTaxClass1Filter)
                        .WhereIf(input.MaxTaxClass1Filter != null, e => e.TaxClass1 <= input.MaxTaxClass1Filter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaxClassDesc1Filter), e => e.TaxClassDesc1 == input.TaxClassDesc1Filter)
                        .WhereIf(input.MinTaxRate1Filter != null, e => e.TaxRate1 >= input.MinTaxRate1Filter)
                        .WhereIf(input.MaxTaxRate1Filter != null, e => e.TaxRate1 <= input.MaxTaxRate1Filter)
                        .WhereIf(input.MinTaxAmt1Filter != null, e => e.TaxAmt1 >= input.MinTaxAmt1Filter)
                        .WhereIf(input.MaxTaxAmt1Filter != null, e => e.TaxAmt1 <= input.MaxTaxAmt1Filter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaxAuth2Filter), e => e.TaxAuth2 == input.TaxAuth2Filter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaxClassDesc2Filter), e => e.TaxClassDesc2 == input.TaxClassDesc2Filter)
                        .WhereIf(input.MinTaxClass2Filter != null, e => e.TaxClass2 >= input.MinTaxClass2Filter)
                        .WhereIf(input.MaxTaxClass2Filter != null, e => e.TaxClass2 <= input.MaxTaxClass2Filter)
                        .WhereIf(input.MinTaxRate2Filter != null, e => e.TaxRate2 >= input.MinTaxRate2Filter)
                        .WhereIf(input.MaxTaxRate2Filter != null, e => e.TaxRate2 <= input.MaxTaxRate2Filter)
                        .WhereIf(input.MinTaxAmt2Filter != null, e => e.TaxAmt2 >= input.MinTaxAmt2Filter)
                        .WhereIf(input.MaxTaxAmt2Filter != null, e => e.TaxAmt2 <= input.MaxTaxAmt2Filter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaxAuth3Filter), e => e.TaxAuth3 == input.TaxAuth3Filter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaxClassDesc3Filter), e => e.TaxClassDesc3 == input.TaxClassDesc3Filter)
                        .WhereIf(input.MinTaxClass3Filter != null, e => e.TaxClass3 >= input.MinTaxClass3Filter)
                        .WhereIf(input.MaxTaxClass3Filter != null, e => e.TaxClass3 <= input.MaxTaxClass3Filter)
                        .WhereIf(input.MinTaxRate3Filter != null, e => e.TaxRate3 >= input.MinTaxRate3Filter)
                        .WhereIf(input.MaxTaxRate3Filter != null, e => e.TaxRate3 <= input.MaxTaxRate3Filter)
                        .WhereIf(input.MinTaxAmt3Filter != null, e => e.TaxAmt3 >= input.MinTaxAmt3Filter)
                        .WhereIf(input.MaxTaxAmt3Filter != null, e => e.TaxAmt3 <= input.MaxTaxAmt3Filter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaxAuth4Filter), e => e.TaxAuth4 == input.TaxAuth4Filter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaxClassDesc4Filter), e => e.TaxClassDesc4 == input.TaxClassDesc4Filter)
                        .WhereIf(input.MinTaxClass4Filter != null, e => e.TaxClass4 >= input.MinTaxClass4Filter)
                        .WhereIf(input.MaxTaxClass4Filter != null, e => e.TaxClass4 <= input.MaxTaxClass4Filter)
                        .WhereIf(input.MinTaxRate4Filter != null, e => e.TaxRate4 >= input.MinTaxRate4Filter)
                        .WhereIf(input.MaxTaxRate4Filter != null, e => e.TaxRate4 <= input.MaxTaxRate4Filter)
                        .WhereIf(input.MinTaxAmt4Filter != null, e => e.TaxAmt4 >= input.MinTaxAmt4Filter)
                        .WhereIf(input.MaxTaxAmt4Filter != null, e => e.TaxAmt4 <= input.MaxTaxAmt4Filter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaxAuth5Filter), e => e.TaxAuth5 == input.TaxAuth5Filter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaxClassDesc5Filter), e => e.TaxClassDesc5 == input.TaxClassDesc5Filter)
                        .WhereIf(input.MinTaxClass5Filter != null, e => e.TaxClass5 >= input.MinTaxClass5Filter)
                        .WhereIf(input.MaxTaxClass5Filter != null, e => e.TaxClass5 <= input.MaxTaxClass5Filter)
                        .WhereIf(input.MinTaxRate5Filter != null, e => e.TaxRate5 >= input.MinTaxRate5Filter)
                        .WhereIf(input.MaxTaxRate5Filter != null, e => e.TaxRate5 <= input.MaxTaxRate5Filter)
                        .WhereIf(input.MinTaxAmt5Filter != null, e => e.TaxAmt5 >= input.MinTaxAmt5Filter)
                        .WhereIf(input.MaxTaxAmt5Filter != null, e => e.TaxAmt5 <= input.MaxTaxAmt5Filter);

            var query = (from o in filteredOEQH
                         select new GetOEQHForViewDto()
                         {
                             OEQH = new OEQHDto
                             {
                                 LocID = o.LocID,
                                 DocNo = o.DocNo,
                                 DocDate = o.DocDate,
                                 MDocNo = o.MDocNo,
                                 MDocDate = o.MDocDate,
                                 TypeID = o.TypeID,
                                 SalesCtrlAcc = o.SalesCtrlAcc,
                                 CustID = o.CustID,
                                 Narration = o.Narration,
                                 NoteText = o.NoteText,
                                 PayTerms = o.PayTerms,
                                 NetAmount = o.NetAmount,
                                 DelvTerms = o.DelvTerms,
                                 ValidityTerms = o.ValidityTerms,
                                 Approved = o.Approved,
                                 ApprovedBy = o.ApprovedBy,
                                 ApprovedDate = o.ApprovedDate,
                                 Active = o.Active,
                                 AudtUser = o.AudtUser,
                                 AudtDate = o.AudtDate,
                                 CreatedBy = o.CreatedBy,
                                 CreateDate = o.CreateDate,
                                 TaxAuth1 = o.TaxAuth1,
                                 TaxClass1 = o.TaxClass1,
                                 TaxClassDesc1 = o.TaxClassDesc1,
                                 TaxRate1 = o.TaxRate1,
                                 TaxAmt1 = o.TaxAmt1,
                                 TaxAuth2 = o.TaxAuth2,
                                 TaxClassDesc2 = o.TaxClassDesc2,
                                 TaxClass2 = o.TaxClass2,
                                 TaxRate2 = o.TaxRate2,
                                 TaxAmt2 = o.TaxAmt2,
                                 TaxAuth3 = o.TaxAuth3,
                                 TaxClassDesc3 = o.TaxClassDesc3,
                                 TaxClass3 = o.TaxClass3,
                                 TaxRate3 = o.TaxRate3,
                                 TaxAmt3 = o.TaxAmt3,
                                 TaxAuth4 = o.TaxAuth4,
                                 TaxClassDesc4 = o.TaxClassDesc4,
                                 TaxClass4 = o.TaxClass4,
                                 TaxRate4 = o.TaxRate4,
                                 TaxAmt4 = o.TaxAmt4,
                                 TaxAuth5 = o.TaxAuth5,
                                 TaxClassDesc5 = o.TaxClassDesc5,
                                 TaxClass5 = o.TaxClass5,
                                 TaxRate5 = o.TaxRate5,
                                 TaxAmt5 = o.TaxAmt5,
                                 Id = o.Id
                             }
                         });

            var oeqhListDtos = await query.ToListAsync();

            return _oeqhExcelExporter.ExportToFile(oeqhListDtos);
        }

        public void ApprovalData(int[] postedData, string Mode, bool bit)
        {
            try
            {
                var postedDataIds = postedData.Distinct();
                // foreach (var data in postedDataIds)
                //  {
                //   var result = _icOPNHeaderRepository.GetAll().Where(o => o.Id == data).ToList();
                // var gltrHeader = await _icOPNHeaderRepository.FirstOrDefaultAsync((int)data);

                // foreach (var res in result)
                // {
                var DocNo = 0;
                if (Mode == "Posting")
                {
                    //res.Posted = bit;
                    //res.PostedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                    //res.PostedDate = DateTime.Now;
                    //_repository.Update(res);
                }
                else if (Mode == "UnApproval")
                {
                    (from a in _oeqhRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && postedDataIds.Contains(o.DocNo))
                     select a).ToList().ForEach(x =>
                     {
                         x.Approved = false;
                         x.ApprovedDate = DateTime.Now;
                         x.ApprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                         DocNo = x.DocNo;
                     });
                    //res.Approved = false;
                    //res.AprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                    //res.AprovedDate = DateTime.Now;
                    //_icOPNHeaderRepository.Update(res);
                }
                else
                {
                    (from a in _oeqhRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && postedDataIds.Contains(o.DocNo))
                     select a).ToList().ForEach(x =>
                     {
                         x.Approved = true;
                         x.ApprovedDate = DateTime.Now;
                         x.ApprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                         DocNo = x.DocNo;
                     });
                    //res.Approved = bit;
                    //res.AprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                    //res.AprovedDate = DateTime.Now;
                    //_icOPNHeaderRepository.Update(res);
                }
                LogModel Log = new LogModel()
                {
                    Status = bit,
                    ApprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName,
                    Detid = Convert.ToInt32(postedDataIds.FirstOrDefault().ToString()),
                    DocNo = DocNo,
                    FormName = "Quotation",
                    Action = Mode,
                    TenantId = AbpSession.TenantId
                };
                _commonappRepository.ApproveLog(Log);
                //  await _repository.SaveChangesAsync();
                //  }
                // }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        [AbpAuthorize(AppPermissions.Sales_OESALEHeaders_Edit)]
        public OESALEHeaderDto GetSaleNoHeaderData(int locID, int saleNo)
        {
            var oesaleHeader = _oesaleHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.DocNo == saleNo && o.LocID == locID).Count() > 0 ?
                _oesaleHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.DocNo == saleNo && o.LocID == locID).SingleOrDefault() : null;

            OESALEHeaderDto output = null;
            if (oesaleHeader != null)
            {

                output = ObjectMapper.Map<OESALEHeaderDto>(oesaleHeader);
                
                var tenantID = (int)AbpSession.TenantId;
                var SaleData = from a in _oesaleHeaderRepository.GetAll().Where(x => x.TenantId == tenantID)
                               join
                               b in _oesaleDetailRepository.GetAll().Where(x => x.TenantId == tenantID)
                               on new { A = a.TenantId, B = a.Id } equals new { A = b.TenantId, B = b.DetID }
                               join
                               f in _itemRepository.GetAll().Where(a => a.TenantId == tenantID)
                               on new { A = b.ItemID, B = b.TenantId } equals new { A = f.ItemId, B = f.TenantId }

                               where (a.DocNo == output.DocNo && a.TenantId == tenantID)
                               select new CreateOrEditOECSHDto() { Narration = a.SalesCtrlAcc, OrderQty = a.TotalQty, CustID = a.CustID, BasicStyle = a.BasicStyle, ItemName = f.Descp, License = a.License };
                var data1 = SaleData.ToList();

                output.ItemName = data1.FirstOrDefault().ItemName;
                output.TotalQty = data1.FirstOrDefault().OrderQty;



                if (output.LocID != 0)
                {
                    output.LocDesc = _icLocationRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LocID == output.LocID).Count() > 0 ?
                        _icLocationRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LocID == output.LocID).SingleOrDefault().LocName : "";
                }
                if (output.TypeID != null)
                {
                    output.TypeDesc = _transactionTypeRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.TypeId.Trim() == output.TypeID.Trim()).Count() > 0 ?
                        _transactionTypeRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.TypeId.Trim() == output.TypeID.Trim()).SingleOrDefault().Description : "";
                }
                if (output.CustID != null)
                {
                    var accountID = _oecollRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.TypeID.ToUpper().Trim() == output.TypeID.ToUpper().Trim()).Count() > 0 ?
                        _oecollRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.TypeID.ToUpper().Trim() == output.TypeID.ToUpper().Trim()).SingleOrDefault().ChAccountID : "";
                    if (accountID != "")
                    {
                        output.CustomerName = _accountSubLedgerRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == output.CustID && o.AccountID == accountID).Count() > 0 ?
                        _accountSubLedgerRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == output.CustID && o.AccountID == accountID).SingleOrDefault().SubAccName : "";
                    }
                }
            }
            return output;
        }

        public async Task<PagedResultDto<OEQDDto>> GetOESALEDData(int detId)
        {

            var filteredOESALEDetails = _oesaleDetailRepository.GetAll().Where(e => e.DetID == detId && e.TenantId == AbpSession.TenantId);

            var oesaleDetails = from o in filteredOESALEDetails
                                select new OEQDDto
                                {
                                    DetID = o.DetID,
                                    LocID = o.LocID,
                                    DocNo = o.DocNo,
                                    ItemID = o.ItemID +"*" + (_ICItemRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.ItemId == o.ItemID).Count() > 0 ? _ICItemRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.ItemId == o.ItemID).SingleOrDefault().Descp : "") + "*" + o.Unit + "*" + o.Conver,
                                    Unit = o.Unit,
                                    Conver = o.Conver==null? 0 : Convert.ToDecimal(o.Conver),
                                    Qty = o.Qty==null?0:Convert.ToDecimal(o.Qty),
                                    Rate = o.Rate==null?0: Convert.ToDecimal(o.Rate),
                                    Amount = o.Amount==null?0: Convert.ToDecimal(o.Amount),
                                    TaxAuth = o.TaxAuth,
                                    TaxClass = o.TaxClass,
                                    TaxClassDesc = _taxClassRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.CLASSID == o.TaxClass).Count() > 0 ? _taxClassRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.Id == o.TaxClass).SingleOrDefault().CLASSDESC : "",
                                    TaxRate = o.TaxRate,
                                    TaxAmt = o.TaxAmt,
                                    Remarks = o.Remarks,
                                    NetAmount = o.NetAmount,
                                    Id = o.Id
                                };

            var totalCount = await filteredOESALEDetails.CountAsync();

            return new PagedResultDto<OEQDDto>(
                totalCount,
                await oesaleDetails.ToListAsync()
            );
        }

    }
}