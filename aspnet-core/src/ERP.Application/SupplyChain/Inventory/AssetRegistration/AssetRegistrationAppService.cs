

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.SupplyChain.Inventory.AssetRegistration.Exporting;
using ERP.SupplyChain.Inventory.AssetRegistration.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ERP.SupplyChain.Inventory.IC_Item;
using ERP.GeneralLedger.SetupForms;
using ERP.SupplyChain.Sales.SalesReference;

namespace ERP.SupplyChain.Inventory.AssetRegistration
{
    [AbpAuthorize(AppPermissions.Pages_AssetRegistration)]
    public class AssetRegistrationAppService : ERPAppServiceBase, IAssetRegistrationAppService
    {

        private readonly IRepository<AssetRegistration> _assetRegistrationRepository;
        private readonly IRepository<SalesReference> _salesReferenceRepository;

        private readonly IAssetRegistrationExcelExporter _assetRegistrationExcelExporter;
        private readonly IICItemAppService _ICItemAppService;
        private readonly IICLocationsAppService _iCLocationsAppService;
        private readonly IChartofControlsAppService _chartofControlsAppService;
        private readonly IAssetRegistrationDetailsAppService _assetRegistrationDetailsAppService;


        public AssetRegistrationAppService(IRepository<AssetRegistration> assetRegistrationRepository, IAssetRegistrationExcelExporter assetRegistrationExcelExporter, IICItemAppService ICItemAppService, IICLocationsAppService iCLocationsAppService, IChartofControlsAppService chartofControlsAppService, IAssetRegistrationDetailsAppService assetRegistrationDetailsAppService,
            IRepository<SalesReference> salesReferenceRepository)
        {
            _assetRegistrationRepository = assetRegistrationRepository;
            _assetRegistrationExcelExporter = assetRegistrationExcelExporter;
            _ICItemAppService = ICItemAppService;
            _iCLocationsAppService = iCLocationsAppService;
            _chartofControlsAppService = chartofControlsAppService;
            _assetRegistrationDetailsAppService = assetRegistrationDetailsAppService;
            _salesReferenceRepository = salesReferenceRepository;

        }

        public async Task<PagedResultDto<GetAssetRegistrationForViewDto>> GetAll(GetAllAssetRegistrationInput input)
        {

            var filteredAssetRegistration = _assetRegistrationRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.FmtAssetID.Contains(input.Filter) || e.AssetName.Contains(input.Filter) || e.ItemID.Contains(input.Filter) || e.SerialNumber.Contains(input.Filter) || e.Narration.Contains(input.Filter) || e.AccAsset.Contains(input.Filter) || e.AccDepr.Contains(input.Filter) || e.AccExp.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinAssetIDFilter != null, e => e.AssetID >= input.MinAssetIDFilter)
                        .WhereIf(input.MaxAssetIDFilter != null, e => e.AssetID <= input.MaxAssetIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FmtAssetIDFilter), e => e.FmtAssetID == input.FmtAssetIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AssetNameFilter), e => e.AssetName == input.AssetNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ItemIDFilter), e => e.ItemID == input.ItemIDFilter)
                        .WhereIf(input.MinLocIDFilter != null, e => e.LocID >= input.MinLocIDFilter)
                        .WhereIf(input.MaxLocIDFilter != null, e => e.LocID <= input.MaxLocIDFilter)
                        .WhereIf(input.MinRegDateFilter != null, e => e.RegDate >= input.MinRegDateFilter)
                        .WhereIf(input.MaxRegDateFilter != null, e => e.RegDate <= input.MaxRegDateFilter)
                        .WhereIf(input.MinPurchaseDateFilter != null, e => e.PurchaseDate >= input.MinPurchaseDateFilter)
                        .WhereIf(input.MaxPurchaseDateFilter != null, e => e.PurchaseDate <= input.MaxPurchaseDateFilter)
                        .WhereIf(input.MinExpiryDateFilter != null, e => e.ExpiryDate >= input.MinExpiryDateFilter)
                        .WhereIf(input.MaxExpiryDateFilter != null, e => e.ExpiryDate <= input.MaxExpiryDateFilter)
                        .WhereIf(input.WarrantyFilter > -1, e => (input.WarrantyFilter == 1 && e.Warranty) || (input.WarrantyFilter == 0 && !e.Warranty))
                        .WhereIf(input.AssetTypeFilter != null, e => e.AssetType == input.AssetTypeFilter)
                        .WhereIf(input.MinDepRateFilter != null, e => e.DepRate >= input.MinDepRateFilter)
                        .WhereIf(input.MaxDepRateFilter != null, e => e.DepRate <= input.MaxDepRateFilter)
                        .WhereIf(input.DepMethodFilter != null, e => e.DepMethod == input.DepMethodFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SerialNumberFilter), e => e.SerialNumber == input.SerialNumberFilter)
                        .WhereIf(input.MinPurchasePriceFilter != null, e => e.PurchasePrice >= input.MinPurchasePriceFilter)
                        .WhereIf(input.MaxPurchasePriceFilter != null, e => e.PurchasePrice <= input.MaxPurchasePriceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NarrationFilter), e => e.Narration == input.NarrationFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AccAssetFilter), e => e.AccAsset == input.AccAssetFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AccDeprFilter), e => e.AccDepr == input.AccDeprFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AccExpFilter), e => e.AccExp == input.AccExpFilter)
                        .WhereIf(input.MinDepStartDateFilter != null, e => e.DepStartDate >= input.MinDepStartDateFilter)
                        .WhereIf(input.MaxDepStartDateFilter != null, e => e.DepStartDate <= input.MaxDepStartDateFilter)
                        .WhereIf(input.MinAssetLifeFilter != null, e => e.AssetLife >= input.MinAssetLifeFilter)
                        .WhereIf(input.MaxAssetLifeFilter != null, e => e.AssetLife <= input.MaxAssetLifeFilter)
                        .WhereIf(input.MinBookValueFilter != null, e => e.BookValue >= input.MinBookValueFilter)
                        .WhereIf(input.MaxBookValueFilter != null, e => e.BookValue <= input.MaxBookValueFilter)
                        .WhereIf(input.MinLastDepAmountFilter != null, e => e.LastDepAmount >= input.MinLastDepAmountFilter)
                        .WhereIf(input.MaxLastDepAmountFilter != null, e => e.LastDepAmount <= input.MaxLastDepAmountFilter)
                        .WhereIf(input.MinLastDepDateFilter != null, e => e.LastDepDate >= input.MinLastDepDateFilter)
                        .WhereIf(input.MaxLastDepDateFilter != null, e => e.LastDepDate <= input.MaxLastDepDateFilter)
                        .WhereIf(input.DisolvedFilter > -1, e => (input.DisolvedFilter == 1 && e.Disolved) || (input.DisolvedFilter == 0 && !e.Disolved))
                        .WhereIf(input.MinActiveFilter != null, e => e.Active >= input.MinActiveFilter)
                        .WhereIf(input.MaxActiveFilter != null, e => e.Active <= input.MaxActiveFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var pagedAndFilteredAssetRegistration = filteredAssetRegistration
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var assetRegistration = from o in pagedAndFilteredAssetRegistration
                                    select new GetAssetRegistrationForViewDto()
                                    {
                                        AssetRegistration = new AssetRegistrationDto
                                        {
                                            AssetID = o.AssetID,
                                            FmtAssetID = o.FmtAssetID,
                                            AssetName = o.AssetName,
                                            ItemID = o.ItemID,
                                            LocID = o.LocID,
                                            RegDate = o.RegDate,
                                            PurchaseDate = o.PurchaseDate,
                                            ExpiryDate = o.ExpiryDate,
                                            Warranty = o.Warranty,
                                            AssetType = o.AssetType,
                                            DepRate = o.DepRate,
                                            DepMethod = o.DepMethod,
                                            SerialNumber = o.SerialNumber,
                                            PurchasePrice = o.PurchasePrice,
                                            Narration = o.Narration,
                                            AccAsset = o.AccAsset,
                                            AccDepr = o.AccDepr,
                                            AccExp = o.AccExp,
                                            DepStartDate = o.DepStartDate,
                                            AssetLife = o.AssetLife,
                                            BookValue = o.BookValue,
                                            LastDepAmount = o.LastDepAmount,
                                            LastDepDate = o.LastDepDate,
                                            Disolved = o.Disolved,
                                            Active = o.Active,
                                            AudtUser = o.AudtUser,
                                            AudtDate = o.AudtDate,
                                            CreatedBy = o.CreatedBy,
                                            CreateDate = o.CreateDate,
                                            AccumulatedDepAmt = o.AccumulatedDepAmt,
                                            Id = o.Id
                                        }
                                    };

            var totalCount = await filteredAssetRegistration.CountAsync();

            return new PagedResultDto<GetAssetRegistrationForViewDto>(
                totalCount,
                await assetRegistration.ToListAsync()
            );
        }

        public async Task<GetAssetRegistrationForViewDto> GetAssetRegistrationForView(int id)
        {
            var assetRegistration = await _assetRegistrationRepository.GetAsync(id);

            var output = new GetAssetRegistrationForViewDto { AssetRegistration = ObjectMapper.Map<AssetRegistrationDto>(assetRegistration) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_AssetRegistration_Edit)]
        public async Task<GetAssetRegistrationForEditOutput> GetAssetRegistrationForEdit(EntityDto input)
        {
            var assetRegistration = await _assetRegistrationRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetAssetRegistrationForEditOutput { AssetRegistration = ObjectMapper.Map<CreateOrEditAssetRegistrationDto>(assetRegistration) };


            if (output.AssetRegistration.ItemID != null)
            {
                output.ItemName = _ICItemAppService.GetName(output.AssetRegistration.ItemID);
            }
            if (output.AssetRegistration.LocID != null)
            {
                output.LocationName = _iCLocationsAppService.GetName((int)output.AssetRegistration.LocID);
            }
            if (output.AssetRegistration.AccAsset != null)
            {
                output.AssetAccName = _chartofControlsAppService.GetName(output.AssetRegistration.AccAsset);
            }
            if (output.AssetRegistration.AccDepr != null)
            {
                output.AccDeprName = _chartofControlsAppService.GetName(output.AssetRegistration.AccDepr);
            }
            if (output.AssetRegistration.AccExp != null)
            {
                output.AccExpName = _chartofControlsAppService.GetName(output.AssetRegistration.AccExp);
            }
            if (output.AssetRegistration.RefID != null)
            {
                output.RefName = _salesReferenceRepository.FirstOrDefault(x => x.RefID == output.AssetRegistration.RefID && x.TenantId == AbpSession.TenantId).RefName;
            }
            return output;
        }

        public async Task CreateOrEdit(CreateOrEditAssetRegistrationDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
                if (input.createAssetRegistrationDetail != null)
                {
                    await _assetRegistrationDetailsAppService.CreateOrEdit(input.createAssetRegistrationDetail);
                }

            }
        }

        [AbpAuthorize(AppPermissions.Pages_AssetRegistration_Create)]
        protected virtual async Task Create(CreateOrEditAssetRegistrationDto input)
        {
            var assetRegistration = ObjectMapper.Map<AssetRegistration>(input);


            if (AbpSession.TenantId != null)
            {
                assetRegistration.TenantId = (int)AbpSession.TenantId;
            }


            var usersss = GetCurrentUserAsync();
            assetRegistration.CreatedBy = usersss.Result.UserName;
            assetRegistration.CreateDate = DateTime.Now;


            await _assetRegistrationRepository.InsertAsync(assetRegistration);
        }

        [AbpAuthorize(AppPermissions.Pages_AssetRegistration_Edit)]
        protected virtual async Task Update(CreateOrEditAssetRegistrationDto input)
        {
            var assetRegistration = await _assetRegistrationRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, assetRegistration);
        }

        [AbpAuthorize(AppPermissions.Pages_AssetRegistration_Delete)]
        public async Task Delete(EntityDto input)
        {

            int assetID = (int)_assetRegistrationRepository.FirstOrDefaultAsync(input.Id).Result.AssetID;

            await _assetRegistrationRepository.DeleteAsync(input.Id);

            await _assetRegistrationDetailsAppService.Delete(assetID);
        }

        public async Task<FileDto> GetAssetRegistrationToExcel(GetAllAssetRegistrationForExcelInput input)
        {

            var filteredAssetRegistration = _assetRegistrationRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.FmtAssetID.Contains(input.Filter) || e.AssetName.Contains(input.Filter) || e.ItemID.Contains(input.Filter) || e.SerialNumber.Contains(input.Filter) || e.Narration.Contains(input.Filter) || e.AccAsset.Contains(input.Filter) || e.AccDepr.Contains(input.Filter) || e.AccExp.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinAssetIDFilter != null, e => e.AssetID >= input.MinAssetIDFilter)
                        .WhereIf(input.MaxAssetIDFilter != null, e => e.AssetID <= input.MaxAssetIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FmtAssetIDFilter), e => e.FmtAssetID == input.FmtAssetIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AssetNameFilter), e => e.AssetName == input.AssetNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ItemIDFilter), e => e.ItemID == input.ItemIDFilter)
                        .WhereIf(input.MinLocIDFilter != null, e => e.LocID >= input.MinLocIDFilter)
                        .WhereIf(input.MaxLocIDFilter != null, e => e.LocID <= input.MaxLocIDFilter)
                        .WhereIf(input.MinRegDateFilter != null, e => e.RegDate >= input.MinRegDateFilter)
                        .WhereIf(input.MaxRegDateFilter != null, e => e.RegDate <= input.MaxRegDateFilter)
                        .WhereIf(input.MinPurchaseDateFilter != null, e => e.PurchaseDate >= input.MinPurchaseDateFilter)
                        .WhereIf(input.MaxPurchaseDateFilter != null, e => e.PurchaseDate <= input.MaxPurchaseDateFilter)
                        .WhereIf(input.MinExpiryDateFilter != null, e => e.ExpiryDate >= input.MinExpiryDateFilter)
                        .WhereIf(input.MaxExpiryDateFilter != null, e => e.ExpiryDate <= input.MaxExpiryDateFilter)
                        .WhereIf(input.WarrantyFilter > -1, e => (input.WarrantyFilter == 1 && e.Warranty) || (input.WarrantyFilter == 0 && !e.Warranty))
                        .WhereIf(input.AssetTypeFilter != null, e => e.AssetType <= input.AssetTypeFilter)
                        .WhereIf(input.MinDepRateFilter != null, e => e.DepRate >= input.MinDepRateFilter)
                        .WhereIf(input.MaxDepRateFilter != null, e => e.DepRate <= input.MaxDepRateFilter)
                        .WhereIf(input.DepMethodFilter != null, e => e.DepMethod <= input.DepMethodFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SerialNumberFilter), e => e.SerialNumber == input.SerialNumberFilter)
                        .WhereIf(input.MinPurchasePriceFilter != null, e => e.PurchasePrice >= input.MinPurchasePriceFilter)
                        .WhereIf(input.MaxPurchasePriceFilter != null, e => e.PurchasePrice <= input.MaxPurchasePriceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NarrationFilter), e => e.Narration == input.NarrationFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AccAssetFilter), e => e.AccAsset == input.AccAssetFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AccDeprFilter), e => e.AccDepr == input.AccDeprFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AccExpFilter), e => e.AccExp == input.AccExpFilter)
                        .WhereIf(input.MinDepStartDateFilter != null, e => e.DepStartDate >= input.MinDepStartDateFilter)
                        .WhereIf(input.MaxDepStartDateFilter != null, e => e.DepStartDate <= input.MaxDepStartDateFilter)
                        .WhereIf(input.MinAssetLifeFilter != null, e => e.AssetLife >= input.MinAssetLifeFilter)
                        .WhereIf(input.MaxAssetLifeFilter != null, e => e.AssetLife <= input.MaxAssetLifeFilter)
                        .WhereIf(input.MinBookValueFilter != null, e => e.BookValue >= input.MinBookValueFilter)
                        .WhereIf(input.MaxBookValueFilter != null, e => e.BookValue <= input.MaxBookValueFilter)
                        .WhereIf(input.MinLastDepAmountFilter != null, e => e.LastDepAmount >= input.MinLastDepAmountFilter)
                        .WhereIf(input.MaxLastDepAmountFilter != null, e => e.LastDepAmount <= input.MaxLastDepAmountFilter)
                        .WhereIf(input.MinLastDepDateFilter != null, e => e.LastDepDate >= input.MinLastDepDateFilter)
                        .WhereIf(input.MaxLastDepDateFilter != null, e => e.LastDepDate <= input.MaxLastDepDateFilter)
                        .WhereIf(input.DisolvedFilter > -1, e => (input.DisolvedFilter == 1 && e.Disolved) || (input.DisolvedFilter == 0 && !e.Disolved))
                        .WhereIf(input.MinActiveFilter != null, e => e.Active >= input.MinActiveFilter)
                        .WhereIf(input.MaxActiveFilter != null, e => e.Active <= input.MaxActiveFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var query = (from o in filteredAssetRegistration
                         select new GetAssetRegistrationForViewDto()
                         {
                             AssetRegistration = new AssetRegistrationDto
                             {
                                 AssetID = o.AssetID,
                                 FmtAssetID = o.FmtAssetID,
                                 AssetName = o.AssetName,
                                 ItemID = o.ItemID,
                                 LocID = o.LocID,
                                 RegDate = o.RegDate,
                                 PurchaseDate = o.PurchaseDate,
                                 ExpiryDate = o.ExpiryDate,
                                 Warranty = o.Warranty,
                                 AssetType = o.AssetType,
                                 DepRate = o.DepRate,
                                 DepMethod = o.DepMethod,
                                 SerialNumber = o.SerialNumber,
                                 PurchasePrice = o.PurchasePrice,
                                 Narration = o.Narration,
                                 AccAsset = o.AccAsset,
                                 AccDepr = o.AccDepr,
                                 AccExp = o.AccExp,
                                 DepStartDate = o.DepStartDate,
                                 AssetLife = o.AssetLife,
                                 BookValue = o.BookValue,
                                 LastDepAmount = o.LastDepAmount,
                                 LastDepDate = o.LastDepDate,
                                 Disolved = o.Disolved,
                                 Active = o.Active,
                                 AudtUser = o.AudtUser,
                                 AudtDate = o.AudtDate,
                                 CreatedBy = o.CreatedBy,
                                 CreateDate = o.CreateDate,
                                 Id = o.Id
                             }
                         });


            var assetRegistrationListDtos = await query.ToListAsync();

            return _assetRegistrationExcelExporter.ExportToFile(assetRegistrationListDtos);
        }

        public List<string> GetAssetRegID()
        {

            var filteredAsset = _assetRegistrationRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId);
            //string x = "" +  id + "%";
            string[] xstring;
            string xformat = "";
            string nString = "";
            string finalSting = "";
            var getMaxID = filteredAsset.Where(c => c.TenantId == AbpSession.TenantId).OrderByDescending(o => o.FmtAssetID).Select(x => x.AssetID).FirstOrDefault();

            List<string> idss = new List<string>();


            if (getMaxID == null)
            {
                getMaxID = 1;
                xformat = string.Format("{0:000}", 1);
                finalSting = "FA-" + xformat; ; //id + "-" + xformat;
            }
            else
            {
                //xstring = getMaxID.Split('-');
                //nString = xstring[1];
                if (Convert.ToInt32(getMaxID) + 1 > 999)
                {
                    xformat = string.Format("{0:000}", 999);
                    finalSting = "FA-" + xformat; //id + "-" + xformat;
                }
                else
                {
                    getMaxID = getMaxID + 1;
                    xformat = string.Format("{0:000}", Convert.ToInt32(getMaxID));
                    finalSting = "FA-" + xformat; ; //id + "-" + xformat;
                }
            }

            idss.Add(getMaxID.ToString());
            idss.Add(finalSting);

            return idss;
        }


        //public decimal GetBankBalance(string Account)
        //{
        //    var balance = from h in _gltrHeaderRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId)
        //                 .WhereIf(FromDate != null, e => Convert.ToDateTime(e.DocDate.ToShortDateString()) >= Convert.ToDateTime(Convert.ToDateTime(FromDate.ToString()).ToShortDateString()))
        //                  .WhereIf(ToDate != null, e => Convert.ToDateTime(e.DocDate.ToShortDateString()) <= Convert.ToDateTime(Convert.ToDateTime(ToDate.ToString()).ToShortDateString()))
        //                  join d in _gltrDetailRepository.GetAll()
        //                  .WhereIf(!string.IsNullOrWhiteSpace(FromAccount), e => false || e.AccountID.CompareTo(FromAccount) >= 0)
        //                  .WhereIf(!string.IsNullOrWhiteSpace(ToAccount), e => false || e.AccountID.CompareTo(ToAccount) <= 0)
        //                  on new { DetID = h.Id, h.TenantId } equals new { d.DetID, d.TenantId }
        //                  join c in _glConfigRepository.GetAll()
        //                    on new { d.AccountID, d.TenantId } equals new { c.AccountID, c.TenantId }
        //                  where new[] { "BP", "BR" }.Contains(c.BookID)
        //                  select new { d.DetID, d.AccountID, d.Narration, Debit = d.Amount > 0 ? d.Amount : 0, Credit = d.Amount < 0 ? d.Amount : 0, d.TenantId }).Distinct()
        //}

    }
}