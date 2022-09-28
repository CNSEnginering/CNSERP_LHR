using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.SupplyChain.Sales.OECSH.Exporting;
using ERP.SupplyChain.Sales.OECSH.Dtos;
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
using ERP.SupplyChain.Sales.OECSD.Dtos;

namespace ERP.SupplyChain.Sales.OECSH
{
    [AbpAuthorize(AppPermissions.Pages_OECSH)]
    public class OECSHAppService : ERPAppServiceBase, IOECSHAppService
    {
        private readonly IRepository<OECSH> _oecshRepository;
        private readonly IOECSHExcelExporter _oecshExcelExporter;

        private readonly IRepository<ICItem> _ICItemRepository;
        private readonly IRepository<OESALEDetail> _oesaleDetailRepository;
        private readonly IRepository<ICLocation> _icLocationRepository;
        private readonly IRepository<OECSD.OECSD> _oecsdRepository;
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

        public OECSHAppService(IRepository<OECSH> oecshRepository, IOECSHExcelExporter oecshExcelExporter, IRepository<ICItem> ICItemRepository, IRepository<OESALEDetail> oesaleDetailRepository, IRepository<TaxClass> taxClassRepository, IRepository<OESALEHeader> oesaleHeaderRepository, IRepository<ICOPT4> icopT4Repository, IRepository<LedgerType> ledgerTypeRepository, IRepository<GLSecChartofControl, string> glSecChartofControlRepository, IRepository<AccountSubLedger> accountSubLedgerRepository, IRepository<OECOLL> oecollRepository, IRepository<TransactionType> transactionTypeRepository, IRepository<ICItem> itemRepository, IRepository<ICLocation> locRepository, IRepository<User, long> userRepository, IRepository<ICLocation> icLocationRepository, IRepository<OECSD.OECSD> oecsdRepository)
        {
            _oecshRepository = oecshRepository;
            _oecshExcelExporter = oecshExcelExporter;
            _ICItemRepository = ICItemRepository;
            _oesaleDetailRepository = oesaleDetailRepository;
            _taxClassRepository = taxClassRepository;
            _glSecChartofControlRepository = glSecChartofControlRepository;
            _oesaleHeaderRepository = oesaleHeaderRepository;
            _oecsdRepository = oecsdRepository;
            _userRepository = userRepository;
            _locRepository = locRepository;
            _ledgerTypeRepository = ledgerTypeRepository;
            _itemRepository = itemRepository;
            _transactionTypeRepository = transactionTypeRepository;
            _oecollRepository = oecollRepository;
            _accountSubLedgerRepository = accountSubLedgerRepository;
            _icopT4Repository = icopT4Repository;
            _icLocationRepository = icLocationRepository;
        }

        public async Task<PagedResultDto<GetOECSHForViewDto>> GetAll(GetAllOECSHInput input)
        {

            var filteredOECSH = _oecshRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.SaleDoc.Contains(input.Filter) || e.MDocNo.Contains(input.Filter) || e.TypeID.Contains(input.Filter) || e.SalesCtrlAcc.Contains(input.Filter) || e.Narration.Contains(input.Filter) || e.NoteText.Contains(input.Filter) || e.PayTerms.Contains(input.Filter) || e.DelvTerms.Contains(input.Filter) || e.ValidityTerms.Contains(input.Filter) || e.ApprovedBy.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter) || e.BasicStyle.Contains(input.Filter) || e.License.Contains(input.Filter))
                        .WhereIf(input.MinLocIDFilter != null, e => e.LocID >= input.MinLocIDFilter)
                        .WhereIf(input.MaxLocIDFilter != null, e => e.LocID <= input.MaxLocIDFilter)
                        .WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
                        .WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
                        .WhereIf(input.MinDocDateFilter != null, e => e.DocDate >= input.MinDocDateFilter)
                        .WhereIf(input.MaxDocDateFilter != null, e => e.DocDate <= input.MaxDocDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SaleDocFilter), e => e.SaleDoc == input.SaleDocFilter)
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
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BasicStyleFilter), e => e.BasicStyle == input.BasicStyleFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LicenseFilter), e => e.License == input.LicenseFilter)
                        .WhereIf(input.MinDirectCostFilter != null, e => e.DirectCost >= input.MinDirectCostFilter)
                        .WhereIf(input.MaxDirectCostFilter != null, e => e.DirectCost <= input.MaxDirectCostFilter)
                        .WhereIf(input.MinCommRateFilter != null, e => e.CommRate >= input.MinCommRateFilter)
                        .WhereIf(input.MaxCommRateFilter != null, e => e.CommRate <= input.MaxCommRateFilter)
                        .WhereIf(input.MinCommAmtFilter != null, e => e.CommAmt >= input.MinCommAmtFilter)
                        .WhereIf(input.MaxCommAmtFilter != null, e => e.CommAmt <= input.MaxCommAmtFilter)
                        .WhereIf(input.MinOHRateFilter != null, e => e.OHRate >= input.MinOHRateFilter)
                        .WhereIf(input.MaxOHRateFilter != null, e => e.OHRate <= input.MaxOHRateFilter)
                        .WhereIf(input.MinOHAmtFilter != null, e => e.OHAmt >= input.MinOHAmtFilter)
                        .WhereIf(input.MaxOHAmtFilter != null, e => e.OHAmt <= input.MaxOHAmtFilter)
                        .WhereIf(input.MinTaxRateFilter != null, e => e.TaxRate >= input.MinTaxRateFilter)
                        .WhereIf(input.MaxTaxRateFilter != null, e => e.TaxRate <= input.MaxTaxRateFilter)
                        .WhereIf(input.MinTaxAmtFilter != null, e => e.TaxAmt >= input.MinTaxAmtFilter)
                        .WhereIf(input.MaxTaxAmtFilter != null, e => e.TaxAmt <= input.MaxTaxAmtFilter)
                        .WhereIf(input.MinTotalCostFilter != null, e => e.TotalCost >= input.MinTotalCostFilter)
                        .WhereIf(input.MaxTotalCostFilter != null, e => e.TotalCost <= input.MaxTotalCostFilter)
                        .WhereIf(input.MinProfRateFilter != null, e => e.ProfRate >= input.MinProfRateFilter)
                        .WhereIf(input.MaxProfRateFilter != null, e => e.ProfRate <= input.MaxProfRateFilter)
                        .WhereIf(input.MinProfAmtFilter != null, e => e.ProfAmt >= input.MinProfAmtFilter)
                        .WhereIf(input.MaxProfAmtFilter != null, e => e.ProfAmt <= input.MaxProfAmtFilter)
                        .WhereIf(input.MinSalePriceFilter != null, e => e.SalePrice >= input.MinSalePriceFilter)
                        .WhereIf(input.MaxSalePriceFilter != null, e => e.SalePrice <= input.MaxSalePriceFilter)
                        .WhereIf(input.MinCostPPFilter != null, e => e.CostPP >= input.MinCostPPFilter)
                        .WhereIf(input.MaxCostPPFilter != null, e => e.CostPP <= input.MaxCostPPFilter)
                        .WhereIf(input.MinSalePPFilter != null, e => e.SalePP >= input.MinSalePPFilter)
                        .WhereIf(input.MaxSalePPFilter != null, e => e.SalePP <= input.MaxSalePPFilter)
                        .WhereIf(input.MinSaleUSFilter != null, e => e.SaleUS >= input.MinSaleUSFilter)
                        .WhereIf(input.MaxSaleUSFilter != null, e => e.SaleUS <= input.MaxSaleUSFilter);

            var pagedAndFilteredOECSH = filteredOECSH
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var oecsh = from o in pagedAndFilteredOECSH
                        select new
                        {

                            o.LocID,
                            o.DocNo,
                            o.DocDate,
                            o.SaleDoc,
                            o.MDocNo,
                            o.MDocDate,
                            o.TypeID,
                            o.SalesCtrlAcc,
                            o.CustID,
                            o.Narration,
                            o.NoteText,
                            o.PayTerms,
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
                            o.BasicStyle,
                            o.License,
                            o.DirectCost,
                            o.CommRate,
                            o.CommAmt,
                            o.OHRate,
                            o.OHAmt,
                            o.TaxRate,
                            o.TaxAmt,
                            o.TotalCost,
                            o.ProfRate,
                            o.ProfAmt,
                            o.SalePrice,
                            o.CostPP,
                            o.SalePP,
                            o.SaleUS,
                            o.USRate,
                            Id = o.Id
                        };

            var totalCount = await filteredOECSH.CountAsync();

            var dbList = await oecsh.ToListAsync();
            var results = new List<GetOECSHForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetOECSHForViewDto()
                {
                    OECSH = new OECSHDto
                    {

                        LocID = o.LocID,
                        DocNo = o.DocNo,
                        DocDate = o.DocDate,
                        SaleDoc = o.SaleDoc,
                        MDocNo = o.MDocNo,
                        MDocDate = o.MDocDate,
                        TypeID = o.TypeID,
                        SalesCtrlAcc = o.SalesCtrlAcc,
                        CustID = o.CustID,
                        Narration = o.Narration,
                        NoteText = o.NoteText,
                        PayTerms = o.PayTerms,
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
                        BasicStyle = o.BasicStyle,
                        License = o.License,
                        DirectCost = o.DirectCost,
                        CommRate = o.CommRate,
                        CommAmt = o.CommAmt,
                        OHRate = o.OHRate,
                        OHAmt = o.OHAmt,
                        TaxRate = o.TaxRate,
                        TaxAmt = o.TaxAmt,
                        TotalCost = o.TotalCost,
                        ProfRate = o.ProfRate,
                        ProfAmt = o.ProfAmt,
                        SalePrice = o.SalePrice,
                        CostPP = o.CostPP,
                        SalePP = o.SalePP,
                        SaleUS = o.SaleUS,
                        USRate = o.USRate,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetOECSHForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetOECSHForViewDto> GetOECSHForView(int id)
        {
            var oecsh = await _oecshRepository.GetAsync(id);

            var output = new GetOECSHForViewDto { OECSH = ObjectMapper.Map<OECSHDto>(oecsh) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_OECSH_Edit)]
        public async Task<GetOECSHForEditOutput> GetOECSHForEdit(EntityDto input)
        {
            var itemList = await _itemRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId).ToListAsync();
            var TaxList = await _taxClassRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId).ToListAsync();
            var oecsh = await _oecshRepository.FirstOrDefaultAsync(x => x.DocNo == input.Id && x.TenantId == AbpSession.TenantId);

            var detailQ = _oecsdRepository.GetAll().Where(x => x.DocNo == oecsh.DocNo && x.TenantId == (int)AbpSession.TenantId);
            var output = new GetOECSHForEditOutput { OECSH = ObjectMapper.Map<CreateOrEditOECSHDto>(oecsh) };

            output.OECSH.QutationDetailDto = ObjectMapper.Map<List<OECSDDto>>(detailQ);

            //Location Description 
            output.OECSH.LocDesc = _locRepository.GetAll().Where(o => o.LocID == oecsh.LocID).Count() > 0
              ?
              _locRepository.GetAll().Where(o => o.LocID == oecsh.LocID).FirstOrDefault().LocName
              : "";
            //sale type 

            output.OECSH.SaleTypeDesc = _transactionTypeRepository.GetAll().Where(x => x.TypeId.ToUpper().Trim() == oecsh.TypeID.ToUpper().Trim()).Count() > 0 ?
                _transactionTypeRepository.GetAll().Where(x => x.TypeId == oecsh.TypeID).FirstOrDefault().Description : "";

            //Customer 
            var accountID = _oecollRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.TypeID.ToUpper().Trim() == oecsh.TypeID.ToUpper().Trim()).Count() > 0 ?
              _oecollRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.TypeID.ToUpper().Trim() == oecsh.TypeID.ToUpper().Trim()).SingleOrDefault().ChAccountID : "";
            if (accountID != "" && accountID != null)
            {
                output.OECSH.CustomerDesc = _accountSubLedgerRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId && x.AccountID == accountID && x.Id == oecsh.CustID).WhereIf(!string.IsNullOrWhiteSpace(""),
                   e => e.Id.ToString().ToUpper().Contains("") || e.SubAccName.ToString().ToUpper().Contains("")).FirstOrDefault().SubAccName;

            }

            var tenantID = (int)AbpSession.TenantId;


            foreach (var data in output.OECSH.QutationDetailDto)
            {
                var item = itemList.Where(o => o.ItemId == data.ItemID);

                output.OECSH.QutationDetailDto.Where(o => o.Id == data.Id).FirstOrDefault().ItemID = item.FirstOrDefault().ItemId + "*" + item.FirstOrDefault().Descp + "*" + data.Unit + "*" + data.Conver;
                output.OECSH.QutationDetailDto.Where(o => o.Id == data.Id).FirstOrDefault().TransName = _icopT4Repository.GetAll().Where(x => x.TenantId == tenantID && x.OptID == data.TransType).Count() > 0 ? _icopT4Repository.GetAll().Where(x => x.TenantId == tenantID && x.OptID == data.TransType).FirstOrDefault().Descp : "";
                output.OECSH.QutationDetailDto.Where(o => o.Id == data.Id).FirstOrDefault().TaxClassDesc = _taxClassRepository.GetAll().Where(x => x.TenantId == tenantID && x.CLASSID == data.TaxClass).Count() > 0 ? _taxClassRepository.GetAll().Where(x => x.TenantId == tenantID && x.CLASSID == data.TaxClass).FirstOrDefault().CLASSDESC : "";
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditOECSHDto input)
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

        [AbpAuthorize(AppPermissions.Pages_OECSH_Create)]
        protected virtual async Task Create(CreateOrEditOECSHDto input)
        {
            var oecsh = ObjectMapper.Map<OECSH>(input);
            if (AbpSession.TenantId != null)
            {
                oecsh.TenantId = (int)AbpSession.TenantId;
                oecsh.CreateDate = DateTime.Now;
                oecsh.CreatedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            }

            
            if (input.SaleDoc != "" && input.SaleDoc != null && input.SaleDoc != "0")
            {
                var SaleDocNo = Convert.ToInt32(input.SaleDoc);
                var tenantID = (int)AbpSession.TenantId;
                var SaleData = from a in _oesaleHeaderRepository.GetAll().Where(x => x.TenantId == tenantID)
                               join
                               b in _oesaleDetailRepository.GetAll().Where(x => x.TenantId == tenantID)
                               on new { A = a.TenantId, B = a.Id } equals new { A = b.TenantId, B = b.DetID }
                               join
                               f in _itemRepository.GetAll().Where(a => a.TenantId == tenantID)
                               on new { A = b.ItemID, B = b.TenantId } equals new { A = f.ItemId, B = f.TenantId }

                               where (a.DocNo == SaleDocNo && a.TenantId == tenantID)
                               select new CreateOrEditOECSHDto() { Narration = a.SalesCtrlAcc, OrderQty = a.TotalQty, CustID = a.CustID, BasicStyle = a.BasicStyle, ItemName = f.Descp, License = a.License };
                var data1 = SaleData.ToList();

                if (data1 != null)
                {

                    data1.FirstOrDefault().PartyName = _accountSubLedgerRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId && x.AccountID == data1.FirstOrDefault().Narration && x.Id == data1.FirstOrDefault().CustID).WhereIf(!string.IsNullOrWhiteSpace(""),
                      e => e.Id.ToString().ToUpper().Contains("") || e.SubAccName.ToString().ToUpper().Contains("")).FirstOrDefault().SubAccName;

                }
                oecsh.PartyName = data1.FirstOrDefault().PartyName;
                oecsh.ItemName = data1.FirstOrDefault().ItemName;
                oecsh.OrderQty = data1.FirstOrDefault().OrderQty;
                oecsh.BasicStyle = data1.FirstOrDefault().BasicStyle;
                oecsh.License = data1.FirstOrDefault().License;
            }
            oecsh.BasicStyle = input.BasicStyle;
            oecsh.License = input.License;

            var getId = await _oecshRepository.InsertAndGetIdAsync(oecsh);

            if (input.QutationDetailDto != null)
            {
                foreach (var data in input.QutationDetailDto)
                {

                    var QDetail = ObjectMapper.Map<OECSD.OECSD>(data);
                    if (AbpSession.TenantId != null)
                    {
                        QDetail.TenantId = (int)AbpSession.TenantId;
                    }
                    QDetail.DetID = getId;
                    QDetail.DocNo = input.DocNo;
                    QDetail.LocID = input.LocID;

                    await _oecsdRepository.InsertAsync(QDetail);
                }
            }

        }

        [AbpAuthorize(AppPermissions.Pages_OECSH_Edit)]
        protected virtual async Task Update(CreateOrEditOECSHDto input)
        {
            input.AudtDate = DateTime.Now;
            input.AudtUser = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            var oecsh = await _oecshRepository.FirstOrDefaultAsync(x => x.DocNo == input.DocNo && x.TenantId == AbpSession.TenantId);
            
            if (input.SaleDoc != "" && input.SaleDoc != null && input.SaleDoc != "0")
            {
                var SaleDocNo = Convert.ToInt32(input.SaleDoc);
                var tenantID = (int)AbpSession.TenantId;
                var SaleData = from a in _oesaleHeaderRepository.GetAll().Where(x => x.TenantId == tenantID)
                               join
                               b in _oesaleDetailRepository.GetAll().Where(x => x.TenantId == tenantID)
                               on new { A = a.TenantId, B = a.DocNo } equals new { A = b.TenantId, B = b.DocNo }
                               join
                               f in _itemRepository.GetAll().Where(a => a.TenantId == tenantID)
                               on new { A = b.ItemID, B = b.TenantId } equals new { A = f.ItemId, B = f.TenantId }

                               where (a.DocNo == SaleDocNo && a.TenantId == tenantID)
                               select new CreateOrEditOECSHDto() { Narration = a.SalesCtrlAcc, OrderQty = a.TotalQty, CustID = a.CustID, BasicStyle = a.BasicStyle, ItemName = f.Descp, License = a.License };
                var data1 = SaleData.ToList();

                if (data1 != null)
                {

                    data1.FirstOrDefault().PartyName = _accountSubLedgerRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId && x.AccountID == data1.FirstOrDefault().Narration && x.Id == data1.FirstOrDefault().CustID).WhereIf(!string.IsNullOrWhiteSpace(""),
                      e => e.Id.ToString().ToUpper().Contains("") || e.SubAccName.ToString().ToUpper().Contains("")).FirstOrDefault().SubAccName;

                }
                oecsh.PartyName = data1.FirstOrDefault().PartyName;
                oecsh.ItemName = data1.FirstOrDefault().ItemName;
                oecsh.OrderQty = data1.FirstOrDefault().OrderQty;
                oecsh.BasicStyle = data1.FirstOrDefault().BasicStyle;
                oecsh.License = data1.FirstOrDefault().License;
            }
            oecsh.BasicStyle = input.BasicStyle;
            oecsh.License = input.License;

            ObjectMapper.Map(input, oecsh);

            //For Qutation Detail
            var oeqDet = await _oecsdRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId && x.DocNo == input.DocNo).ToListAsync();
            if (oeqDet != null)
            {
                foreach (var item in oeqDet)
                {
                    await _oecsdRepository.DeleteAsync(item);
                }
            }


            if (input.QutationDetailDto != null)
            {
                foreach (var data in input.QutationDetailDto)
                {

                    var QDetail = ObjectMapper.Map<OECSD.OECSD>(data);

                    if (AbpSession.TenantId != null)
                    {
                        QDetail.TenantId = (int)AbpSession.TenantId;
                    }
                    QDetail.DetID = oecsh.Id;
                    QDetail.DocNo = input.DocNo;
                    QDetail.LocID = input.LocID;
                    await _oecsdRepository.InsertAsync(QDetail);
                }
            }

        }

        [AbpAuthorize(AppPermissions.Pages_OECSH_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _oecshRepository.DeleteAsync(x => x.DocNo == input.Id && x.TenantId == AbpSession.TenantId);
            var oeqDetailsList = _oecsdRepository.GetAll().Where(x => x.DocNo == input.Id && x.TenantId == AbpSession.TenantId);
            foreach (var item in oeqDetailsList)
            {
                await _oecsdRepository.DeleteAsync(item.Id);
            }
        }

        public async Task<FileDto> GetOECSHToExcel(GetAllOECSHForExcelInput input)
        {

            var filteredOECSH = _oecshRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.SaleDoc.Contains(input.Filter) || e.MDocNo.Contains(input.Filter) || e.TypeID.Contains(input.Filter) || e.SalesCtrlAcc.Contains(input.Filter) || e.Narration.Contains(input.Filter) || e.NoteText.Contains(input.Filter) || e.PayTerms.Contains(input.Filter) || e.DelvTerms.Contains(input.Filter) || e.ValidityTerms.Contains(input.Filter) || e.ApprovedBy.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter) || e.BasicStyle.Contains(input.Filter) || e.License.Contains(input.Filter))
                        .WhereIf(input.MinLocIDFilter != null, e => e.LocID >= input.MinLocIDFilter)
                        .WhereIf(input.MaxLocIDFilter != null, e => e.LocID <= input.MaxLocIDFilter)
                        .WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
                        .WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
                        .WhereIf(input.MinDocDateFilter != null, e => e.DocDate >= input.MinDocDateFilter)
                        .WhereIf(input.MaxDocDateFilter != null, e => e.DocDate <= input.MaxDocDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SaleDocFilter), e => e.SaleDoc == input.SaleDocFilter)
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
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BasicStyleFilter), e => e.BasicStyle == input.BasicStyleFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LicenseFilter), e => e.License == input.LicenseFilter)
                        .WhereIf(input.MinDirectCostFilter != null, e => e.DirectCost >= input.MinDirectCostFilter)
                        .WhereIf(input.MaxDirectCostFilter != null, e => e.DirectCost <= input.MaxDirectCostFilter)
                        .WhereIf(input.MinCommRateFilter != null, e => e.CommRate >= input.MinCommRateFilter)
                        .WhereIf(input.MaxCommRateFilter != null, e => e.CommRate <= input.MaxCommRateFilter)
                        .WhereIf(input.MinCommAmtFilter != null, e => e.CommAmt >= input.MinCommAmtFilter)
                        .WhereIf(input.MaxCommAmtFilter != null, e => e.CommAmt <= input.MaxCommAmtFilter)
                        .WhereIf(input.MinOHRateFilter != null, e => e.OHRate >= input.MinOHRateFilter)
                        .WhereIf(input.MaxOHRateFilter != null, e => e.OHRate <= input.MaxOHRateFilter)
                        .WhereIf(input.MinOHAmtFilter != null, e => e.OHAmt >= input.MinOHAmtFilter)
                        .WhereIf(input.MaxOHAmtFilter != null, e => e.OHAmt <= input.MaxOHAmtFilter)
                        .WhereIf(input.MinTaxRateFilter != null, e => e.TaxRate >= input.MinTaxRateFilter)
                        .WhereIf(input.MaxTaxRateFilter != null, e => e.TaxRate <= input.MaxTaxRateFilter)
                        .WhereIf(input.MinTaxAmtFilter != null, e => e.TaxAmt >= input.MinTaxAmtFilter)
                        .WhereIf(input.MaxTaxAmtFilter != null, e => e.TaxAmt <= input.MaxTaxAmtFilter)
                        .WhereIf(input.MinTotalCostFilter != null, e => e.TotalCost >= input.MinTotalCostFilter)
                        .WhereIf(input.MaxTotalCostFilter != null, e => e.TotalCost <= input.MaxTotalCostFilter)
                        .WhereIf(input.MinProfRateFilter != null, e => e.ProfRate >= input.MinProfRateFilter)
                        .WhereIf(input.MaxProfRateFilter != null, e => e.ProfRate <= input.MaxProfRateFilter)
                        .WhereIf(input.MinProfAmtFilter != null, e => e.ProfAmt >= input.MinProfAmtFilter)
                        .WhereIf(input.MaxProfAmtFilter != null, e => e.ProfAmt <= input.MaxProfAmtFilter)
                        .WhereIf(input.MinSalePriceFilter != null, e => e.SalePrice >= input.MinSalePriceFilter)
                        .WhereIf(input.MaxSalePriceFilter != null, e => e.SalePrice <= input.MaxSalePriceFilter)
                        .WhereIf(input.MinCostPPFilter != null, e => e.CostPP >= input.MinCostPPFilter)
                        .WhereIf(input.MaxCostPPFilter != null, e => e.CostPP <= input.MaxCostPPFilter)
                        .WhereIf(input.MinSalePPFilter != null, e => e.SalePP >= input.MinSalePPFilter)
                        .WhereIf(input.MaxSalePPFilter != null, e => e.SalePP <= input.MaxSalePPFilter)
                        .WhereIf(input.MinSaleUSFilter != null, e => e.SaleUS >= input.MinSaleUSFilter)
                        .WhereIf(input.MaxSaleUSFilter != null, e => e.SaleUS <= input.MaxSaleUSFilter);

            var query = (from o in filteredOECSH
                         select new GetOECSHForViewDto()
                         {
                             OECSH = new OECSHDto
                             {
                                 LocID = o.LocID,
                                 DocNo = o.DocNo,
                                 DocDate = o.DocDate,
                                 SaleDoc = o.SaleDoc,
                                 MDocNo = o.MDocNo,
                                 MDocDate = o.MDocDate,
                                 TypeID = o.TypeID,
                                 SalesCtrlAcc = o.SalesCtrlAcc,
                                 CustID = o.CustID,
                                 Narration = o.Narration,
                                 NoteText = o.NoteText,
                                 PayTerms = o.PayTerms,
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
                                 BasicStyle = o.BasicStyle,
                                 License = o.License,
                                 DirectCost = o.DirectCost,
                                 CommRate = o.CommRate,
                                 CommAmt = o.CommAmt,
                                 OHRate = o.OHRate,
                                 OHAmt = o.OHAmt,
                                 TaxRate = o.TaxRate,
                                 TaxAmt = o.TaxAmt,
                                 TotalCost = o.TotalCost,
                                 ProfRate = o.ProfRate,
                                 ProfAmt = o.ProfAmt,
                                 SalePrice = o.SalePrice,
                                 CostPP = o.CostPP,
                                 SalePP = o.SalePP,
                                 SaleUS = o.SaleUS,
                                 Id = o.Id
                             }
                         });

            var oecshListDtos = await query.ToListAsync();

            return _oecshExcelExporter.ExportToFile(oecshListDtos);
        }

        public int GetDocId()
        {
            var result = _oecshRepository.GetAll().DefaultIfEmpty().Max(o => o.DocNo);
            return result = result + 1;
        }

    }
}