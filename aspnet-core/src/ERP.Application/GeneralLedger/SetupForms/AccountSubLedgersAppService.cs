using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.GeneralLedger.SetupForms.Exporting;
using ERP.GeneralLedger.SetupForms.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ERP.CommonServices;
using ERP.EntityFrameworkCore;
using ERP.AccountPayables.Setup.ApSetup;
using ERP.AccountPayables;
using Abp.UI;
using System.IO;
using OfficeOpenXml;
using System.Diagnostics;
using ERP.SupplyChain.Inventory;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace ERP.GeneralLedger.SetupForms
{
    [AbpAuthorize(AppPermissions.SetupForms_AccountSubLedgers)]
    public class AccountSubLedgersAppService : ERPAppServiceBase, IAccountSubLedgersAppService
    {
        private readonly IRepository<AccountSubLedger> _accountSubLedgerRepository;
        private readonly IAccountSubLedgersExcelExporter _accountSubLedgersExcelExporter;
        private readonly IRepository<ChartofControl, string> _lookup_chartofControlRepository;
        private readonly IRepository<TaxAuthority, string> _lookup_taxAuthorityRepository;
        private readonly IRepository<TaxClass, int> _taxClassRepository;
        private readonly IRepository<AROption> _arOptionRepository;
        private readonly IRepository<APOption> _apOptionRepository;
        private readonly IRepository<TaxAuthority, string> _taxAuthorityRepository;
        private readonly IRepository<LedgerType.LedgerType> _ledgerTypeRepository;
        private readonly IRepository<GLSLGroups.GLSLGroups> _glslGroupsRepository;
        private readonly IRepository<PriceLists> _itemPricingRepository;

        public string ItemPriceDes1;

        public AccountSubLedgersAppService(IRepository<AccountSubLedger> accountSubLedgerRepository, IAccountSubLedgersExcelExporter accountSubLedgersExcelExporter,
            IRepository<ChartofControl, string> lookup_chartofControlRepository, IRepository<TaxAuthority,
                string> lookup_taxAuthorityRepository, IRepository<TaxClass> taxClassRepository,
            IRepository<AROption> arOptionRepository,
            IRepository<APOption> apOptionRepository, IRepository<GLSLGroups.GLSLGroups> glslGroupsRepository,
        IRepository<TaxAuthority, string> taxAuthorityRepository,
             IRepository<LedgerType.LedgerType> ledgerTypeRepository)
        {
            _accountSubLedgerRepository = accountSubLedgerRepository;
            _accountSubLedgersExcelExporter = accountSubLedgersExcelExporter;
            _lookup_chartofControlRepository = lookup_chartofControlRepository;
            _lookup_taxAuthorityRepository = lookup_taxAuthorityRepository;
            _taxClassRepository = taxClassRepository;
            _apOptionRepository = apOptionRepository;
            _arOptionRepository = arOptionRepository;
            _taxAuthorityRepository = taxAuthorityRepository;
            _ledgerTypeRepository = ledgerTypeRepository;
            _glslGroupsRepository = glslGroupsRepository;
        }

        public async Task<PagedResultDto<GetAccountSubLedgerForViewDto>> GetAll(GetAllAccountSubLedgersInput input)
        {
            int? modeId = null;
            var slTypes = _ledgerTypeRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);
            if (!string.IsNullOrEmpty(input.SlType))
            {
                //Customer and Vendor should have explicit values
                if (input.SlType.ToLower().Contains("customer"))
                {
                    modeId = 2;
                }

                else if (input.SlType.ToLower().Contains("vendor"))
                {
                    modeId = 1;
                }
            }

            var filteredAccountSubLedgers = _accountSubLedgerRepository.GetAll().Join(_lookup_chartofControlRepository.GetAll(), x => x.AccountID, y => y.Id, (x, y) => new { x.AccountID, x.TenantId, x.SubAccName, x.City, x.Phone, x.Contact, x.RegNo, x.TAXAUTH, x.ClassID, x.STTAXAUTH, x.STClassID, x.OldSL, x.LedgerType, x.Agreement1, x.Agreement2, x.PayTerm, x.OtherCondition, x.AUDTDATE, x.AUDTUSER, x.Address1, x.Address2, x.ItemPriceID, x.Reference, x.Id, y.AccountName, x.SLType }).Where(x => x.TenantId == AbpSession.TenantId)
                    //.Include(e=> e.ChartofControlFk)
                    .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.AccountID.Contains(input.Filter) || e.Id.ToString().Contains(input.Filter) || e.AccountID.Contains(input.Filter) || e.AccountName.Contains(input.Filter) || e.SubAccName.Contains(input.Filter) || e.City.Contains(input.Filter) || e.Phone.Contains(input.Filter) || e.Contact.Contains(input.Filter) || e.RegNo.Contains(input.Filter) || e.TAXAUTH.Contains(input.Filter))
                    .WhereIf(!string.IsNullOrWhiteSpace(input.AccountIDFilter), e => e.AccountID.ToLower() == input.AccountIDFilter.ToLower().Trim())
                    .WhereIf(!string.IsNullOrWhiteSpace(input.SubAccNameFilter), e => e.SubAccName.ToLower() == input.SubAccNameFilter.ToLower().Trim())
                    .WhereIf(!string.IsNullOrWhiteSpace(input.ChartofControlAccountNameFilter), e => e.AccountName.ToLower() == input.ChartofControlAccountNameFilter.ToLower().Trim())
                    .WhereIf(!string.IsNullOrWhiteSpace(input.CityFilter), e => e.City.ToLower() == input.CityFilter.ToLower().Trim())
                    .WhereIf(!string.IsNullOrWhiteSpace(input.PhoneFilter), e => e.Phone.ToLower() == input.PhoneFilter.ToLower().Trim())
                    .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFilter), e => e.Contact.ToLower() == input.ContactFilter.ToLower().Trim())
                    .WhereIf(!string.IsNullOrWhiteSpace(input.RegNoFilter), e => e.RegNo.ToLower() == input.RegNoFilter.ToLower().Trim())
                    .WhereIf(!string.IsNullOrWhiteSpace(input.TAXAUTHFilter), e => e.TAXAUTH.ToLower() == input.TAXAUTHFilter.ToLower().Trim())
                    .WhereIf(input.MaxSubAccIDFilter != null && input.MinSubAccIDFilter != null, e => e.Id <= input.MaxSubAccIDFilter && e.Id >= input.MinSubAccIDFilter)
                    // .WhereIf(input.MinSubAccIDFilter != null, )
                    .WhereIf(modeId != null, e => e.SLType == modeId);

            var pagedAndFilteredAccountSubLedgers = filteredAccountSubLedgers
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var accountSubLedgers = from o in pagedAndFilteredAccountSubLedgers
                                    join type in slTypes on o.SLType equals type.LedgerID //into type1
                                    //from types in type1.DefaultIfEmpty()
                                    join o1 in _lookup_chartofControlRepository.GetAll() on o.AccountID equals o1.Id into j1
                                    from s1 in j1.DefaultIfEmpty().Where(x => x.TenantId == AbpSession.TenantId)
                                        // where s1.AccountName.ToString().Contains(input.Filter.ToUpper())
                                    select new GetAccountSubLedgerForViewDto()
                                    {
                                        AccountSubLedger = new AccountSubLedgerDto
                                        {
                                            AccountID = o.AccountID,
                                            SubAccName = o.SubAccName,
                                            Address1 = o.Address1,

                                            Address2 = o.Address2,
                                            City = o.City,
                                            Phone = o.Phone,
                                            Contact = o.Contact,
                                            RegNo = o.RegNo,
                                            TAXAUTH = o.TAXAUTH,
                                            ClassID = o.ClassID,
                                            STTAXAUTH = o.STTAXAUTH,
                                            STClassID = o.STClassID,
                                            OldSL = o.OldSL,
                                            LedgerType = o.LedgerType,
                                            Agreement1 = o.Agreement1,
                                            Agreement2 = o.Agreement2,
                                            PayTerm = o.PayTerm,
                                            OtherCondition = o.OtherCondition,
                                            Reference = o.Reference,
                                            AUDTDATE = o.AUDTDATE,
                                            AUDTUSER = o.AUDTUSER,
                                            Id = o.Id,
                                            SLType = o.SLType,
                                            SLDesc = type.LedgerDesc,
                                            ItemPriceID = o.ItemPriceID,

                                        },
                                        ChartofControlAccountName = s1 == null ? "" : s1.AccountName.ToString(),
                                        //TaxAuthorityTAXAUTHDESC = s2 == null ? "" : s2.TAXAUTHDESC.ToString()
                                    };

            var totalCount = await filteredAccountSubLedgers.CountAsync();

            return new PagedResultDto<GetAccountSubLedgerForViewDto>(
                totalCount,
                await accountSubLedgers.ToListAsync()
            );
        }


        public async Task<GetAccountSubLedgerForViewDto> GetAccountSubLedgerForView(int id)
        {
            var accountSubLedger = await _accountSubLedgerRepository.FirstOrDefaultAsync(x => x.Id == id && x.TenantId == AbpSession.TenantId);

            var output = new GetAccountSubLedgerForViewDto { AccountSubLedger = ObjectMapper.Map<AccountSubLedgerDto>(accountSubLedger) };

            if (output.AccountSubLedger.AccountID != null)
            {
                var _lookupChartofControl = await _lookup_chartofControlRepository.FirstOrDefaultAsync(x => x.Id == (string)output.AccountSubLedger.AccountID && x.TenantId == AbpSession.TenantId);
                output.ChartofControlAccountName = _lookupChartofControl.AccountName.ToString();
            }

            if (output.AccountSubLedger.TAXAUTH != null)
            {
                var _lookupTaxAuthority = await _lookup_taxAuthorityRepository.FirstOrDefaultAsync(x => x.Id == (string)output.AccountSubLedger.TAXAUTH && x.TenantId == AbpSession.TenantId);
                output.TaxAuthorityTAXAUTHDESC = _lookupTaxAuthority.TAXAUTHDESC.ToString();
            }

            if (output.AccountSubLedger.Linked)
            {
                if (output.AccountSubLedger.ParentID != null)
                {
                    var _lookupChartofControl = await _lookup_chartofControlRepository.FirstOrDefaultAsync(x => x.Id == (string)output.AccountSubLedger.ParentID && x.TenantId == AbpSession.TenantId);
                    output.ParentAccountName = _lookupChartofControl.AccountName.ToString();
                }

            }

            return output;
        }

        [AbpAuthorize(AppPermissions.SetupForms_AccountSubLedgers_Edit)]
        public async Task<GetAccountSubLedgerForEditOutput> GetAccountSubLedgerForEdit(EntityDto input, string AccountID, string ItemPriceDes)
        {
            var accountSubLedger = await _accountSubLedgerRepository.FirstOrDefaultAsync(x => x.Id == input.Id && x.AccountID == AccountID && x.TenantId == AbpSession.TenantId);

            var output = new GetAccountSubLedgerForEditOutput { AccountSubLedger = ObjectMapper.Map<CreateOrEditAccountSubLedgerDto>(accountSubLedger) };

            if (output.AccountSubLedger.AccountID != null)
            {
                var _lookupChartofControl = await _lookup_chartofControlRepository.FirstOrDefaultAsync(x => x.Id == (string)output.AccountSubLedger.AccountID && x.TenantId == AbpSession.TenantId);
                output.ChartofControlAccountName = _lookupChartofControl.AccountName.ToString();
            }

            if (output.AccountSubLedger.TAXAUTH != null)
            {
                var _lookupTaxAuthority = await _lookup_taxAuthorityRepository.FirstOrDefaultAsync(x => x.Id == (string)output.AccountSubLedger.TAXAUTH && x.TenantId == AbpSession.TenantId);
                if (_lookupTaxAuthority != null)
                    output.TaxAuthorityTAXAUTHDESC = _lookupTaxAuthority.TAXAUTHDESC.ToString();
            }


            if (output.AccountSubLedger.SLGrpId != null)
            {
                var _glslGroups = await _glslGroupsRepository.FirstOrDefaultAsync(x => x.SLGrpID == output.AccountSubLedger.SLGrpId && x.TenantId == AbpSession.TenantId);
                if (_glslGroups != null)
                    output.AccountSubLedger.SLGrpName = _glslGroups.SLGRPDESC.ToString();
            }

            if (output.AccountSubLedger.Linked)
            {
                if (output.AccountSubLedger.ParentID != null)
                {
                    var _lookupChartofControl = await _lookup_chartofControlRepository.FirstOrDefaultAsync(x => x.Id == (string)output.AccountSubLedger.ParentID && x.TenantId == AbpSession.TenantId);
                    output.ParentAccountName = _lookupChartofControl.AccountName;

                    if (output.AccountSubLedger.ParentSubID != null)
                    {
                        var _lookupsubledger = await _accountSubLedgerRepository.FirstOrDefaultAsync(x => x.AccountID == output.AccountSubLedger.ParentID && x.Id == output.AccountSubLedger.ParentSubID && x.TenantId == AbpSession.TenantId);
                        output.ParentSubAccountName = _lookupsubledger.SubAccName;
                    }
                }
            }

            if (output.AccountSubLedger.ItemPriceID == null)
            {
                output.AccountSubLedger.ItemPriceID = "";
                output.ItemPriceLIst = ItemPriceDes = "";
            }
            else
            {


                this.GetPriceLiST(output.AccountSubLedger.ItemPriceID, ItemPriceDes);
                //if (output.AccountSubLedger.ItemPriceID != null)
                //{
                //    var _ItemPriceDes = await _itemPricingRepository.FirstOrDefaultAsync(x => x.PriceList == output.AccountSubLedger.ItemPriceID && x.TenantId == AbpSession.TenantId);
                //    if (_ItemPriceDes != null)
                //        output.AccountSubLedger.ItemPriceID = _ItemPriceDes.PriceListName.ToString();
                //}
                if (ItemPriceDes1 != "")
                {
                    ItemPriceDes = ItemPriceDes1;
                    //var _ItemPriceDescriptions= ItemPriceDes;
                    output.ItemPriceLIst = ItemPriceDes;

                }
            }
            return output;
        }

        public string GetPriceLiST(string PriceList, string ItemPriceDes)
        {
            SqlConnection CN = null;
            SqlCommand SqlCom;
            string S;

            DataTable DT = new DataTable();
            var output = "";
            SqlDataAdapter DA = new SqlDataAdapter();

            try
            {

                // Connection Open
                CN = new SqlConnection();
                string str = ConfigurationManager.AppSettings["ConnectionString"];

                CN.ConnectionString = str;
                CN.Open();

                var tenantId = AbpSession.TenantId;



                S = "SP_select_ITEM_PRICE_List";

                SqlCom = new SqlCommand();

                //SqlCom.Parameters.AddWithValue("@TENANTID", tenantId);
                //SqlCom.Parameters.AddWithValue("@PRICElIST", PriceList);
                //SqlCom.Parameters.AddWithValue("@ITEMID", ItemID);

                //SqlCom.Parameters.Add(new SqlParameter("@TENANTID", tenantId));


                SqlCom.Parameters.Add(new SqlParameter("@TENANTID", tenantId));
                SqlCom.Parameters.Add(new SqlParameter("@PriceList", PriceList.Trim()));



                try
                {
                    // Execute Query

                    SqlCom.CommandText = S;
                    SqlCom.CommandType = CommandType.StoredProcedure;
                    SqlCom.Connection = CN;
                    DA.SelectCommand = SqlCom;
                    DA.Fill(DT);
                }


                catch (Exception ex)
                {
                    // RetString = "ERROR: " & ex.Message.ToString
                    throw ex;
                }
                finally
                {
                    // Close Connection/Transactions
                    CN.Close();
                    DA = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            // Error
            // RetString = "ERROR: " & ex.Message.ToString
            finally
            {
                // Close/Remove Connection
                CN = null;
                DA = null;
            }

            ItemPriceDes1 = DT.Rows[0]["PriceListName"].ToString();

            return ItemPriceDes1;

        }


        public async Task<string> TransferSubledgers(string fromAccountID, string toAccountID)
        {
            try
            {
                var accountSubLedgers = _accountSubLedgerRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId && x.AccountID == fromAccountID);



                //int subAccountId = int.Parse(Maxid(toAccountID));
                //int i = 0;


                foreach (var item in accountSubLedgers)
                {

                    var transferToAccountSL = _accountSubLedgerRepository.GetAll().
                        Where(x => x.TenantId == AbpSession.TenantId && x.AccountID == toAccountID && x.Id == item.Id);


                    if (transferToAccountSL.Count() == 0)
                    {
                        item.AccountID = toAccountID;

                        //item.Id = subAccountId + i;
                        //i++;

                        var subLedgerToInsert = ObjectMapper.Map<AccountSubLedger>(item);
                        await _accountSubLedgerRepository.InsertAsync(subLedgerToInsert);
                    }
                }


            }
            catch (Exception)
            {
                return "failed";
            }

            return "done";
        }

        public GetAccountSubLedgerForCreateOutputDto GetAccountSubLedgerForCreate(string Mode)
        {
            var tenandId = (int?)AbpSession.TenantId;
            var getAccountSubLedgerForCreateOutputDto = new GetAccountSubLedgerForCreateOutputDto();
            var dataAp = new APOption();
            var dataAr = new AROption();
            int subAcc;
            if (tenandId != null)
            {

                if (Mode == "customerMaster")
                {
                    dataAr = _arOptionRepository.GetAll().Where(o => o.TenantId == tenandId).SingleOrDefault();

                    if (dataAr != null)
                    {
                        subAcc = Convert.ToInt32(Maxid(dataAr.DEFCUSCTRLACC));
                        var custDesc = _lookup_chartofControlRepository.GetAll().Where(o => o.Id == dataAr.DEFCUSCTRLACC && o.TenantId == AbpSession.TenantId);
                        if (custDesc.Count() > 0)
                        {
                            getAccountSubLedgerForCreateOutputDto.AccountId = dataAr.DEFCUSCTRLACC;
                            getAccountSubLedgerForCreateOutputDto.SubAccountId = subAcc;
                            getAccountSubLedgerForCreateOutputDto.AccountDesc = custDesc.SingleOrDefault().AccountName;
                        }
                    }

                }
                else
                {
                    dataAp = _apOptionRepository.GetAll().Where(o => o.TenantId == tenandId).SingleOrDefault();
                    if (dataAp != null)
                    {
                        subAcc = Convert.ToInt32(Maxid(dataAp.DEFVENCTRLACC));
                        var vendDesc = _lookup_chartofControlRepository.GetAll().Where(o => o.Id == dataAp.DEFVENCTRLACC && o.TenantId == AbpSession.TenantId);
                        if (vendDesc.Count() > 0)
                        {
                            getAccountSubLedgerForCreateOutputDto.AccountId = dataAp.DEFVENCTRLACC;
                            getAccountSubLedgerForCreateOutputDto.SubAccountId = subAcc;
                            getAccountSubLedgerForCreateOutputDto.AccountDesc = vendDesc.SingleOrDefault().AccountName;
                        }

                    }
                }

            }

            return getAccountSubLedgerForCreateOutputDto;
        }

        public async Task CreateOrEdit(CreateOrEditAccountSubLedgerDto input)
        {
            if (input.flag == false)
            {

                var segmentlevel3 = await _accountSubLedgerRepository.FirstOrDefaultAsync(x => x.Id == input.Id && x.AccountID == input.AccountID && x.TenantId == AbpSession.TenantId);

                if (segmentlevel3 != null)
                {
                    throw new UserFriendlyException("Ooppps! There is a problem!", "Sub Ledger ID " + input.Id + " already taken....");
                }
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.SetupForms_AccountSubLedgers_Create)]
        protected virtual async Task Create(CreateOrEditAccountSubLedgerDto input)
        {
            var accountSubLedger = ObjectMapper.Map<AccountSubLedger>(input);


            if (AbpSession.TenantId != null)
            {
                accountSubLedger.TenantId = (int)AbpSession.TenantId;
            }


            await _accountSubLedgerRepository.InsertAsync(accountSubLedger);
        }

        [AbpAuthorize(AppPermissions.SetupForms_AccountSubLedgers_Edit)]
        protected virtual async Task Update(CreateOrEditAccountSubLedgerDto input)
        {
            var accountSubLedger = await _accountSubLedgerRepository.FirstOrDefaultAsync(x => x.Id == input.Id && x.AccountID == input.AccountID && x.TenantId == AbpSession.TenantId);
            ObjectMapper.Map(input, accountSubLedger);
        }

        [AbpAuthorize(AppPermissions.SetupForms_AccountSubLedgers_Delete)]
        public async Task Delete(EntityDto input, string AccountID)
        {
            await _accountSubLedgerRepository.DeleteAsync(x => x.Id == input.Id && x.AccountID == AccountID && x.TenantId == AbpSession.TenantId);
        }

        public async Task<FileDto> GetAccountSubLedgersToExcel(GetAllAccountSubLedgersForExcelInput input)
        {

            var filteredAccountSubLedgers = _accountSubLedgerRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.AccountID.Contains(input.Filter) || e.SubAccName.Contains(input.Filter) || e.Address1.Contains(input.Filter) || e.Address2.Contains(input.Filter) || e.City.Contains(input.Filter) || e.Phone.Contains(input.Filter) || e.Contact.Contains(input.Filter) || e.RegNo.Contains(input.Filter) || e.TAXAUTH.Contains(input.Filter) || e.OldSL.Contains(input.Filter) || e.Agreement1.Contains(input.Filter) || e.Agreement2.Contains(input.Filter) || e.OtherCondition.Contains(input.Filter) || e.Reference.Contains(input.Filter) || e.AUDTUSER.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AccountIDFilter), e => e.AccountID.ToLower() == input.AccountIDFilter.ToLower().Trim())

                        .WhereIf(!string.IsNullOrWhiteSpace(input.SubAccNameFilter), e => e.SubAccName.ToLower() == input.SubAccNameFilter.ToLower().Trim())

                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityFilter), e => e.City.ToLower() == input.CityFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PhoneFilter), e => e.Phone.ToLower() == input.PhoneFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactFilter), e => e.Contact.ToLower() == input.ContactFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RegNoFilter), e => e.RegNo.ToLower() == input.RegNoFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TAXAUTHFilter), e => e.TAXAUTH.ToLower() == input.TAXAUTHFilter.ToLower().Trim());

            var query = from o in filteredAccountSubLedgers
                        join o1 in _lookup_chartofControlRepository.GetAll() on o.AccountID equals o1.Id into j1
                        from s1 in j1.DefaultIfEmpty()
                        join o2 in _lookup_taxAuthorityRepository.GetAll() on o.TAXAUTH equals o2.Id into j2
                        from s2 in j2.DefaultIfEmpty()

                        select new GetAccountSubLedgerForViewDto()
                        {
                            AccountSubLedger = new AccountSubLedgerDto
                            {
                                AccountID = o.AccountID,
                                SubAccName = o.SubAccName,
                                Address1 = o.Address1,
                                Address2 = o.Address2,
                                City = o.City,
                                Phone = o.Phone,
                                Contact = o.Contact,
                                RegNo = o.RegNo,
                                TAXAUTH = o.TAXAUTH,
                                ClassID = o.ClassID,
                                STTAXAUTH = o.STTAXAUTH,
                                STClassID = o.STClassID,
                                OldSL = o.OldSL,
                                LedgerType = o.LedgerType,
                                Agreement1 = o.Agreement1,
                                Agreement2 = o.Agreement2,
                                PayTerm = o.PayTerm,
                                OtherCondition = o.OtherCondition,
                                Reference = o.Reference,
                                AUDTDATE = o.AUDTDATE,
                                AUDTUSER = o.AUDTUSER,
                                SubAccID = o.Id
                            },
                            ChartofControlAccountName = s1 == null ? "" : s1.AccountName.ToString(),
                            TaxAuthorityTAXAUTHDESC = s2 == null ? "" : s2.TAXAUTHDESC.ToString()
                        };


            var accountSubLedgerListDtos = await query.ToListAsync();

            return _accountSubLedgersExcelExporter.ExportToFile(accountSubLedgerListDtos);
        }

        [AbpAuthorize(AppPermissions.SetupForms_AccountSubLedgers)]
        public async Task<PagedResultDto<AccountSubLedgerChartofControlLookupTableDto>> GetAllChartofControlForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_chartofControlRepository.GetAll().Where(o => o.SubLedger == true && o.TenantId == AbpSession.TenantId).WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.AccountName.ToString().Contains(input.Filter)

               );

            var totalCount = await query.CountAsync();

            var chartofControlList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<AccountSubLedgerChartofControlLookupTableDto>();
            foreach (var chartofControl in chartofControlList)
            {
                lookupTableDtoList.Add(new AccountSubLedgerChartofControlLookupTableDto
                {
                    Id = chartofControl.Id,
                    DisplayName = chartofControl.AccountName?.ToString()
                });
            }

            return new PagedResultDto<AccountSubLedgerChartofControlLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }


        public async Task<PagedResultDto<AccountSubLedgerTaxAuthorityLookupTableDto>> GetAllTaxAuthorityForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_taxAuthorityRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter), e => false || e.TAXAUTHDESC.ToString().ToLower().Contains(input.Filter.ToLower())
                   || e.Id.ToString().ToLower().Contains(input.Filter.ToLower())
               );

            var totalCount = await query.CountAsync();

            var taxAuthorityList = await query
                .PageBy(input)
                .OrderBy(input.Sorting ?? "id desc")
                .ToListAsync();

            var lookupTableDtoList = new List<AccountSubLedgerTaxAuthorityLookupTableDto>();
            foreach (var taxAuthority in taxAuthorityList)
            {
                lookupTableDtoList.Add(new AccountSubLedgerTaxAuthorityLookupTableDto
                {
                    Id = taxAuthority.Id,
                    DisplayName = taxAuthority.TAXAUTHDESC?.ToString()
                });
            }

            return new PagedResultDto<AccountSubLedgerTaxAuthorityLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        public async Task<ListResultDto<GetAllTaxClassesForCombobox>> GetAllTaxClassesForCombobox(string input)
        {
            var query = _taxClassRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId)
                .WhereIf(!string.IsNullOrWhiteSpace(input), x => x.TAXAUTH == input);
            //.WhereIf(!string.IsNullOrWhiteSpace(input.Trim()), e => e.TAXAUTH.ToLower().Trim() == input.ToLower().Trim());


            //Where(o => o.TAXAUTH.Trim() == input.Trim());

            var totalCount = await query.CountAsync();

            var taxClassList = await query
                .ToListAsync();

            var lookupTableDtoList = new List<GetAllTaxClassesForCombobox>();
            foreach (var taxClass in taxClassList)
            {
                lookupTableDtoList.Add(new GetAllTaxClassesForCombobox
                {
                    Value = taxClass.CLASSID.ToString(),
                    DisplayText = taxClass.CLASSDESC?.ToString()
                });
            }

            return new ListResultDto<GetAllTaxClassesForCombobox>(
                lookupTableDtoList
            );
        }

        public string Maxid(string AccountID)
        {
            // var xquery = _accountSubLedgerRepository.GetAll().FirstOrDefault();

            string xformat = "";
            string nString = "";
            string finalSting = "";

            var maxid = ((from tab1 in _accountSubLedgerRepository.GetAll().Where(o => o.AccountID == AccountID && o.TenantId == AbpSession.TenantId) select (int?)tab1.Id).Max() ?? 0);
            if (maxid == 0)
                maxid = 10000;
            else
                maxid = maxid + 1;

            xformat = string.Format("{0:00000}", maxid);
            finalSting = xformat;

            if (maxid == 9999)
            {
                xformat = "9999";
            }
            return xformat;
        }

        [AbpAuthorize(AppPermissions.SetupForms_AccountSubLedgers)]
        public async Task<PagedResultDto<AccountSubLedgerChartofControlLookupTableDto>> GetAllAccountSubledger_lookup(GetAllForLookupTableInput input)
        {
            var query = _accountSubLedgerRepository.GetAll().Where(x => x.AccountID == input.Filter && x.TenantId == AbpSession.TenantId);
            var slTypes = _ledgerTypeRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);
            var totalCount = await query.CountAsync();

            var taxAuthorityList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<AccountSubLedgerChartofControlLookupTableDto>();
            foreach (var taxAuthority in taxAuthorityList)
            {
                lookupTableDtoList.Add(new AccountSubLedgerChartofControlLookupTableDto
                {
                    Id = taxAuthority.Id.ToString(),
                    DisplayName = taxAuthority.SubAccName.ToString(),
                    SlType = taxAuthority.SLType.ToString(),
                    SlDesc = slTypes.Where(c => c.LedgerID == taxAuthority.SLType).Select(c => c.LedgerDesc).FirstOrDefault().ToString()

                });
            }

            return new PagedResultDto<AccountSubLedgerChartofControlLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );

        }

        public string GetSalesTaxDesc(string taxAuth)
        {
            string taxDesc;
            taxDesc = _taxAuthorityRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == taxAuth).Count() > 0 ?
                                       _taxAuthorityRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == taxAuth).SingleOrDefault().TAXAUTHDESC : "";
            return taxDesc;
        }

    }
}