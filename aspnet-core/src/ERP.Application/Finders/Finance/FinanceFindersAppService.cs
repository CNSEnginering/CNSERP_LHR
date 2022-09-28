using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using ERP.GeneralLedger.SetupForms;
using Abp.Linq.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ERP.Finders.Dtos;
using Abp.Collections.Extensions;
using System.Linq;
using ERP.AccountPayables;
using ERP.SupplyChain.Sales.SaleAccounts;
using ERP.GeneralLedger.Transaction.BankReconcile;
using ERP.CommonServices.ChequeBooks;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH;
using ERP.GeneralLedger.SetupForms.LedgerType;
using ERP.AccountReceivables;
using ERP.GeneralLedger.DirectInvoice;
using ERP.SupplyChain.Purchase.ReceiptEntry;
using ERP.SupplyChain.Purchase.APINVH;
using ERP.SupplyChain.Purchase.Requisition;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using ERP.PayRoll.Cader;
using ERP.GeneralLedger.SetupForms.GLSLGroups;
using ERP.Authorization.Users;
using ERP.CommonServices.UserLoc.CSUserLocD;
using ERP.GeneralLedger.SetupForms.GLDocRev;
using ERP.SupplyChain.Sales.OERoutes;
using ERP.SupplyChain.Sales.OEDrivers;
using ERP.CommonServices;

namespace ERP.Finders.Finance
{
    public class FinanceFindersAppService : ERPAppServiceBase
    {
        private readonly IRepository<ControlDetail> _controlDetailRepository;
        private readonly IRepository<ChartofControl, string> _chartofControlRepository;
        private readonly IRepository<GLSecChartofControl, string> _glSecChartofControlRepository;
        private readonly IRepository<AccountSubLedger> _accountSubLedgerRepository;
        private readonly IRepository<SubControlDetail> _subControlDetailRepository;
        private readonly IRepository<Segmentlevel3> _segmentlevel3Repository;
        private readonly IRepository<GLBOOKS> _glbooksRepository;
        private readonly IRepository<GLCONFIG> _glconfigRepository;
        private readonly IRepository<GLLocation> _glLocationRepository;
        private readonly IRepository<APTerm> _apTermRepository;
        private readonly IRepository<OECOLL> _oecollRepository;
        private readonly IRepository<BankReconcile> _bankReconcileRepository;
        private readonly IRepository<ChequeBookDetail> _chequeBookDetailrepository;
        private readonly IRepository<GLTRHeader> _gLTRHeaderRepository;
        private readonly IRepository<LedgerType> _ledgerTypeRepository;
        private readonly IRepository<ARTerm> _arTermRepository;
        private readonly IRepository<GLINVHeader> _glinvHeaderRepository;
        private readonly IRepository<GLINVDetail> _glinvDetailRepository;
        private readonly IRepository<PORECHeader> _porecHeaderRepository;
        private readonly IRepository<APINVH> _apinvhRepository;
        private readonly IRepository<Cader> _caderRepository;
        private readonly IRepository<GLSLGroups> _glslGroupsRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<CSUserLocD> _csUserLocDRepository;
        private readonly IRepository<GLDocRev> _glDocRev;
        private readonly IRepository<OERoutes> _oeRoutesRepository;
        private readonly IRepository<OEDrivers> _oeDriversRepository;
        private readonly IRepository<Bank> _bankRepository;

        public FinanceFindersAppService(
            IRepository<ARTerm> arTermRepository,
            IRepository<GLTRHeader> gLTRHeaderRepository,
            IRepository<ControlDetail> controlDetailRepository, IRepository<Cader> caderRepository,
            IRepository<ChartofControl, string> chartofControlRepository, IRepository<GLSLGroups> glslGroupsRepository,
            IRepository<GLSecChartofControl, string> glSecChartofControlRepository,
            IRepository<AccountSubLedger> accountSubLedgerRepository,
            IRepository<SubControlDetail> subControlDetailRepository,
            IRepository<Segmentlevel3> segmentlevel3Repository,
            IRepository<GLBOOKS> glbooksRepository, IRepository<User, long> userRepository,
            IRepository<GLCONFIG> glconfigRepository,
            IRepository<GLLocation> glLocationRepository,
            IRepository<APTerm> apTermRepository,
            IRepository<OECOLL> oecollRepository,
            IRepository<BankReconcile> bankReconcileRepository,
            IRepository<ChequeBookDetail> chequeBookDetailrepository,
            IRepository<LedgerType> ledgerTypeRepository,
            IRepository<GLINVHeader> glinvHeaderRepository,
            IRepository<GLINVDetail> glinvDetailRepository,
            IRepository<PORECHeader> porecHeaderRepository, IRepository<CSUserLocD> csUserLocDRepository,
            IRepository<APINVH> apinvhRepository,
             IRepository<GLDocRev> glDocRev,
             IRepository<OERoutes> oeRoutesRepository,
             IRepository<OEDrivers> oeDriversRepository,
             IRepository<Bank> bankRepository
            )
        {
            _porecHeaderRepository = porecHeaderRepository;
            _controlDetailRepository = controlDetailRepository; _csUserLocDRepository = csUserLocDRepository;
            _chartofControlRepository = chartofControlRepository;
            _glSecChartofControlRepository = glSecChartofControlRepository;
            _accountSubLedgerRepository = accountSubLedgerRepository; _userRepository = userRepository;
            _subControlDetailRepository = subControlDetailRepository;
            _segmentlevel3Repository = segmentlevel3Repository;
            _glbooksRepository = glbooksRepository;
            _glconfigRepository = glconfigRepository;
            _glLocationRepository = glLocationRepository;
            _apTermRepository = apTermRepository;
            _oecollRepository = oecollRepository;
            _bankReconcileRepository = bankReconcileRepository;
            _chequeBookDetailrepository = chequeBookDetailrepository;
            _gLTRHeaderRepository = gLTRHeaderRepository;
            _ledgerTypeRepository = ledgerTypeRepository;
            _arTermRepository = arTermRepository;
            _glinvHeaderRepository = glinvHeaderRepository;
            _glinvDetailRepository = glinvDetailRepository;
            _apinvhRepository = apinvhRepository;
            _caderRepository = caderRepository;
            _glslGroupsRepository = glslGroupsRepository;
            _glDocRev = glDocRev;
            _oeRoutesRepository = oeRoutesRepository;
            _oeDriversRepository = oeDriversRepository;
            _bankRepository = bankRepository;

        }

        public async Task<PagedResultDto<FinanceFindersDto>> GetFinanceLookupTable(GetAllForLookupTableInput input)
        {
            //  var resultDtos = new List<FinanceFindersDto>();
            var resultDtos = new LookupDto<FinanceFindersDto>();
            switch (input.Target)
            {
                case "Level1":
                    resultDtos = await GetAllControlDetailForLookupTable(input);
                    break;
                case "ChartOfAccount":
                    resultDtos = await GetAllChartofControlForLookupTable(input);
                    break;
                case "SubLedger":
                    resultDtos = await GetAllAccountSubledgerlookupTable(input);
                    break;
                case "Customer":
                    resultDtos = await GetAllAccountCustomerlookupTable(input);
                    break;
                case "Level2":
                    resultDtos = await GetAllSubControlDetailForLookupTable(input);
                    break;
                case "Level3":
                    resultDtos = await GetAllSegmentlevel3ForLookupTable(input);
                    break;
                case "GLBooks":
                    resultDtos = await GetAllGLBOOKSForLookupTable(input);
                    break;
                case "GLConfig":
                    resultDtos = await GetAllGLCONFIGForLookupTable(input);
                    break;
                case "GLLocation":
                    resultDtos = await GetAllICUserLocationForLookupTable(input);
                    break;
                case "WHTerm":
                    resultDtos = await GetAllWHTermForLookupTable(input);
                    break;
                case "BankReconcileBankID":
                    resultDtos = await GetBankReconcileBankForLookupTable(input);
                    break;
                case "ReconcilationDocument":
                    resultDtos = await GetBankReconcileDocForLookupTable(input);
                    break;
                case "ChequeBookDetail":
                    resultDtos = await GetAllChequeBookDetailLookupTable(input);
                    break;
                case "VoucherType":
                    resultDtos = await GetVoucherTypeLookupTable(input);
                    break;
                case "VoucherNo":
                    resultDtos = await GetVoucherNoLookupTable(input);
                    break;
                case "ArTerm":
                    resultDtos = await GetARTermLookupTable(input);
                    break;
                case "InvoiceNo":
                    resultDtos = await GetInvoiceNoLookupTable(input);
                    break;
                case "InvoiceNoPV":
                    resultDtos = await GetInvoiceNoPVLookupTable(input);
                    break;
                case "Cader":
                    resultDtos = await GetAllCader(input);
                    break;
                case "SLGrp":
                    resultDtos = await GetAllSLGrp(input);
                    break;
                case "Debtors":
                    resultDtos = await GetAllDebtorForLookupTable(input);
                    break;
                case "CustomerByDebtor":
                    resultDtos = await GetAllCustomerByDebtorlookupTable(input);
                    break;
                case "Voucher":
                    resultDtos = await GetVoucherLookupTable(input);
                    break;
                case "Routes":
                    resultDtos = await GetAllRoutes(input);
                    break;
                case "Drivers":
                    resultDtos = await GetAllDrivers(input);
                    break;
                case "Bank":
                    resultDtos = await GetAllBanks(input);
                    break;
                default:
                    break;



            }
            return new PagedResultDto<FinanceFindersDto>(
                    resultDtos.Count,
                    resultDtos.Items
                );
        }

        private async Task<LookupDto<FinanceFindersDto>> GetAllRoutes(GetAllForLookupTableInput input)
        {
            var query = _oeRoutesRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);

            var lookupTableDto = from o in query
                                 select new FinanceFindersDto
                                 {
                                     Id = o.RoutID.ToString(),
                                     DisplayName = o.RoutDesc
                                 };
            lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<FinanceFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }

        private async Task<LookupDto<FinanceFindersDto>> GetAllBanks(GetAllForLookupTableInput input)
        {
            var query = _bankRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);

            var lookupTableDto = from o in query
                                 select new FinanceFindersDto
                                 {
                                     Id = o.BANKID,
                                     DisplayName = o.BANKNAME
                                 };
            lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<FinanceFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }

        //Function For Drivers
        private async Task<LookupDto<FinanceFindersDto>> GetAllDrivers(GetAllForLookupTableInput input)
        {
            var query = _oeDriversRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);

            var lookupTableDto = from o in query
                                 select new FinanceFindersDto
                                 {
                                     Id = o.DriverID.ToString(),
                                     DisplayName = o.DriverName,
                                     DriverCtrlAcc=o.DriverCtrlAcc,
                                     DriverSubAcc = o.DriverSubAccID,

                                 };
            lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<FinanceFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }
        //------------------------------------------------------//
        private async Task<LookupDto<FinanceFindersDto>> GetVoucherLookupTable(GetAllForLookupTableInput input)
        {
            var detList = _glDocRev.GetAll().Where(e => e.TenantId == AbpSession.TenantId).Select(c => c.DetId).ToList();
            var query = _gLTRHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Reversed == false && !detList.Contains(o.Id))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.DocNo.ToString().Contains(input.Filter) || e.BookID.Contains(input.Filter) || e.DocMonth.ToString().Contains(input.Filter));


            var lookupTableDto = from o in query
                                 select new FinanceFindersDto
                                 {
                                     Id = o.Id.ToString(),
                                     BookId = o.BookID,
                                     DisplayName = o.NARRATION,
                                     FmtDocNo = o.FmtDocNo,
                                     DocDate = o.DocDate,
                                     DocMonth = o.DocMonth,
                                     DocNo = o.DocNo
                                 };
            lookupTableDto = (input.Sorting == "id DESC") ? lookupTableDto.OrderByDescending(o => int.Parse(o.Id))
                : (input.Sorting == "id ASC") ? lookupTableDto.OrderBy(o => int.Parse(o.Id))
                : (input.Sorting == "voucherDate DESC") ? lookupTableDto.OrderByDescending(o => o.DocDate)
                : (input.Sorting == "voucherDate ASC") ? lookupTableDto.OrderBy(o => o.DocDate)
                : lookupTableDto; var pageData = lookupTableDto.PageBy(input);

            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<FinanceFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }
        private async Task<LookupDto<FinanceFindersDto>> GetAllSLGrp(GetAllForLookupTableInput input)
        {
            var query = _glslGroupsRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);

            var lookupTableDto = from o in query
                                 select new FinanceFindersDto
                                 {
                                     Id = o.SLGrpID.ToString(),
                                     DisplayName = o.SLGRPDESC
                                 };
            lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();  
            var getData = new LookupDto<FinanceFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }

        private async Task<LookupDto<FinanceFindersDto>> GetARTermLookupTable(GetAllForLookupTableInput input)
        {
            var query = _arTermRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Active == true);

            var lookupTableDto = from o in query
                                 select new FinanceFindersDto
                                 {
                                     Id = o.TermID.ToString(),
                                     DisplayName = o.TERMDESC,
                                     AccountID = o.TERMACCID,
                                     TermRate = Convert.ToDouble(o.TERMRATE)
                                 };
            lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<FinanceFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }
        private async Task<LookupDto<FinanceFindersDto>> GetAllDebtorForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _chartofControlRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.SLType == 1 && o.Inactive == false)
                .WhereIf(input.Filter != null, e => e.Id == input.Filter || e.AccountName.Contains(input.Filter));

            var lookupTableDto = from o in query
                                 select new FinanceFindersDto
                                 {
                                     Id = o.Id,
                                     DisplayName = o.AccountName
                                 };

            lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<FinanceFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }
        private async Task<LookupDto<FinanceFindersDto>> GetAllCustomerByDebtorlookupTable(GetAllForLookupTableInput input)
        {
            var query = _accountSubLedgerRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId && x.Active == true && x.SLType == 1)
                .WhereIf(!string.IsNullOrWhiteSpace(input.ParamFilter),
                    e => e.AccountID == input.ParamFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => e.Id.ToString() == input.Filter || e.SubAccName.Contains(input.Filter));

            var lookupTableDto = from o in query
                                 select new FinanceFindersDto
                                 {
                                     Id = o.Id.ToString(),
                                     DisplayName = o.SubAccName
                                 };
            lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<FinanceFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }
        private async Task<LookupDto<FinanceFindersDto>> GetAllCader(GetAllForLookupTableInput input)
        {
            var query = _caderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);

            var lookupTableDto = from o in query
                                 select new FinanceFindersDto
                                 {
                                     Id = o.Id.ToString(),
                                     DisplayName = o.CADER_NAME,
                                 };
            lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<FinanceFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }

        private async Task<LookupDto<FinanceFindersDto>> GetVoucherNoLookupTable(GetAllForLookupTableInput input)
        {

            var query = _gLTRHeaderRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                    e => e.DocNo.ToString().Equals(input.Filter) || e.NARRATION.ToUpper().Contains(input.Filter.ToUpper())
                ).Where(o => o.TenantId == AbpSession.TenantId && o.BookID == input.ParamFilter);

            var lookupTableDto = from o in query
                                 select new FinanceFindersDto
                                 {
                                     Id = o.DocNo.ToString(),
                                     DisplayName = o.NARRATION,
                                     DocDate = o.DocDate,
                                     DocMonth = o.DocMonth,
                                     ConfigID = o.ConfigID
                                 };

            lookupTableDto = (input.Sorting == "id DESC") ? lookupTableDto.OrderByDescending(o => int.Parse(o.Id))
                : (input.Sorting == "id ASC") ? lookupTableDto.OrderBy(o => int.Parse(o.Id))
                : (input.Sorting == "voucherDate DESC") ? lookupTableDto.OrderByDescending(o => o.DocDate)
                : (input.Sorting == "voucherDate ASC") ? lookupTableDto.OrderBy(o => o.DocDate)
                : lookupTableDto;

            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<FinanceFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }

        private async Task<LookupDto<FinanceFindersDto>> GetVoucherTypeLookupTable(GetAllForLookupTableInput input)
        {
            var query = _glbooksRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.BookName.ToUpper().ToString().Contains(input.Filter.ToUpper()) || e.BookID.ToUpper().ToString().Contains(input.Filter.ToUpper())
               ).Where(o => o.TenantId == AbpSession.TenantId && o.INACTIVE == false && o.Integrated == false);

            var lookupTableDto = from o in query
                                 select new FinanceFindersDto
                                 {
                                     Id = o.BookID,
                                     DisplayName = o.BookName
                                 };
            lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<FinanceFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }

        private async Task<LookupDto<FinanceFindersDto>> GetAllChequeBookDetailLookupTable(GetAllForLookupTableInput input)
        {
            var query = _chequeBookDetailrepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.ParamFilter),
                   e => e.BankAccNo == input.ParamFilter
                ).Where(o => o.TenantId == AbpSession.TenantId);


            var data = from s in _gLTRHeaderRepository.GetAll().Where(c => c.TenantId == AbpSession.TenantId) select s.ChNumber ?? "0";

            var lookupTableDto = from o in query
                                 where !data.Contains(o.FromChNo)
                                 select new FinanceFindersDto
                                 {
                                     Id = o.FromChNo,
                                     DisplayName = o.BANKID
                                 };

            lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<FinanceFindersDto>(
                query.Count(),
                lookupTableDtoList
            );
            return getData;
        }
        private async Task<LookupDto<FinanceFindersDto>> GetAllControlDetailForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _controlDetailRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => false || e.Seg1ID.Trim().ToUpper().Contains(input.Filter.ToUpper()) || e.SegmentName.ToString().ToLower().ToUpper().Contains(input.Filter.ToUpper())
                ).Where(o => o.TenantId == AbpSession.TenantId);

            var lookupTableDto = from o in query
                                 select new FinanceFindersDto
                                 {
                                     Id = o.Seg1ID,
                                     DisplayName = o.SegmentName
                                 };

            lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<FinanceFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }

        private async Task<LookupDto<FinanceFindersDto>> GetAllChartofControlForLookupTable(GetAllForLookupTableInput input)
        {
            //IQueryable<GLSecChartofControl> query;
            //if (!string.IsNullOrWhiteSpace(input.Param2Filter))
            //{
            //    query = from o in _glSecChartofControlRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
            //         .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => e.AccountName.ToString().ToUpper().Contains(input.Filter.ToUpper()) || e.Id.ToString().ToUpper().Contains(input.Filter.ToUpper()))
            //         .WhereIf(!string.IsNullOrWhiteSpace(input.ParamFilter), o => o.SubLedger == true)
            //            join o1 in _ledgerTypeRepository.GetAll()

            //            on new { SLType = (int)o.SLType, o.TenantId } equals new { SLType = o1.LedgerID, o1.TenantId }
            //            into f
            //            from o1 in f.DefaultIfEmpty()

            //            where o1.LedgerDesc == input.Param2Filter
            //            select o;




            //}
            //else
            //{



            var query = from o in _glSecChartofControlRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                        join o1 in _ledgerTypeRepository.GetAll()
                        on new { SLType = (int)o.SLType, o.TenantId } equals new { SLType = o1.LedgerID, o1.TenantId }
                        into f
                        from o1 in f.DefaultIfEmpty()
                        select new { o.Id, o.AccountName, o.SubLedger, o.SLType, LedgerDesc = o1 != null ? o1.LedgerDesc : "" };
            //}

            var lookupTableDto = from o in query
                                  .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => e.AccountName.ToString().ToUpper().Contains(input.Filter.ToUpper()) || e.Id.ToString().ToUpper().Contains(input.Filter.ToUpper()))
                                  .WhereIf(!string.IsNullOrWhiteSpace(input.ParamFilter), o => o.SubLedger == Convert.ToBoolean(input.ParamFilter))
                                  .WhereIf(!string.IsNullOrWhiteSpace(input.Param2Filter), o => o.LedgerDesc == input.Param2Filter)
                                 select new FinanceFindersDto
                                 {
                                     Id = o.Id,
                                     DisplayName = o.AccountName,
                                     Subledger = o.SubLedger,
                                     SLType = o.SLType
                                 };
            lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<FinanceFindersDto>(
                lookupTableDto.Count(),
                lookupTableDtoList
            );

            return getData;
        }

        private async Task<LookupDto<FinanceFindersDto>> GetAllAccountSubledgerlookupTable(GetAllForLookupTableInput input)
        {
            var query = _accountSubLedgerRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId && x.Active == true).WhereIf(!string.IsNullOrWhiteSpace(input.ParamFilter),
                    e => e.AccountID.ToString().ToUpper().Contains(input.ParamFilter.ToUpper())).WhereIf(input.Param2Filter != null,
                    e => e.SLType.Equals(Convert.ToInt32(input.Param2Filter))).WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => e.Id.ToString().ToUpper().Contains(input.Filter.ToUpper()) || e.SubAccName.ToString().ToUpper().Contains(input.Filter.ToUpper()));

            var lookupTableDto = from o in query
                                 select new FinanceFindersDto
                                 {
                                     Id = o.Id.ToString(),
                                     DisplayName = o.SubAccName
                                 };
            lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<FinanceFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }
        // changing old code
        //private async Task<LookupDto<FinanceFindersDto>> GetAllAccountCustomerlookupTable(GetAllForLookupTableInput input)
        //{
        //    var accountID = _oecollRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.TypeID.ToUpper().Trim() == input.ParamFilter.ToUpper().Trim()).Count() > 0 ?
        //            _oecollRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.TypeID.ToUpper().Trim() == input.ParamFilter.ToUpper().Trim()).SingleOrDefault().ChAccountID : "";
        //    var query = _accountSubLedgerRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId && x.AccountID == accountID && x.Active == true).WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
        //            e => e.Id.ToString().ToUpper().Contains(input.Filter.ToUpper()) || e.SubAccName.ToString().ToUpper().Contains(input.Filter.ToUpper()));

        //    var lookupTableDto = from o in query
        //                         select new FinanceFindersDto
        //                         {
        //                             Id = o.Id.ToString(),
        //                             DisplayName = o.SubAccName
        //                         };
        //    lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
        //    var pageData = lookupTableDto.PageBy(input);
        //    var lookupTableDtoList = await pageData.ToListAsync();
        //    var getData = new LookupDto<FinanceFindersDto>(
        //        query.Count(),
        //        lookupTableDtoList
        //    );

        //    return getData;
        //}
        //changing new

        private async Task<LookupDto<FinanceFindersDto>> GetAllAccountCustomerlookupTable(GetAllForLookupTableInput input)
        {
            var accountID = _oecollRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.TypeID.ToUpper().Trim() == input.ParamFilter.ToUpper().Trim()).Count() > 0 ?
                    _oecollRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.TypeID.ToUpper().Trim() == input.ParamFilter.ToUpper().Trim()).SingleOrDefault().ChAccountID : "";
            var query = _accountSubLedgerRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId && x.AccountID == accountID && x.Active == true).WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => e.Id.ToString().ToUpper().Contains(input.Filter.ToUpper()) || e.SubAccName.ToString().ToUpper().Contains(input.Filter.ToUpper()));

            var lookupTableDto = from o in query
                                 select new FinanceFindersDto
                                 {
                                     Id = o.Id.ToString(),
                                     DisplayName = o.SubAccName,
                                     ItemPriceID = o.ItemPriceID,

                                 };
            lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<FinanceFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }




        private async Task<LookupDto<FinanceFindersDto>> GetAllSubControlDetailForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _subControlDetailRepository.GetAll().Where(c => EF.Functions.Like(c.Seg2ID, $"{input.ParamFilter}%") && c.TenantId == AbpSession.TenantId).WhereIf(
                  !string.IsNullOrWhiteSpace(input.Filter),
                 e => false || e.Seg2ID.Trim().ToUpper().Contains(input.Filter.ToUpper()) || e.SegmentName.ToString().ToUpper().Contains(input.Filter.ToUpper())
               ).Where(o => o.TenantId == AbpSession.TenantId);

            var lookupTableDto = from o in query
                                 select new FinanceFindersDto
                                 {
                                     Id = o.Seg2ID,
                                     DisplayName = o.SegmentName
                                 };
            lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<FinanceFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }
        private async Task<LookupDto<FinanceFindersDto>> GetAllSegmentlevel3ForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _segmentlevel3Repository.GetAll().Where(c => EF.Functions.Like(c.Seg3ID, $"{input.ParamFilter}%") && c.TenantId == AbpSession.TenantId).WhereIf(
                  !string.IsNullOrWhiteSpace(input.Filter),
                 e => false || e.Seg3ID.Trim().ToUpper().Contains(input.Filter.ToUpper()) || e.SegmentName.ToString().ToUpper().Contains(input.Filter.ToUpper())
               ).Where(o => o.TenantId == AbpSession.TenantId);

            var lookupTableDto = from o in query
                                 select new FinanceFindersDto
                                 {
                                     Id = o.Seg3ID,
                                     DisplayName = o.SegmentName
                                 };
            lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<FinanceFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }
        private async Task<LookupDto<FinanceFindersDto>> GetAllGLBOOKSForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _glbooksRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.BookName.ToUpper().ToString().Contains(input.Filter.ToUpper()) || e.BookID.ToUpper().ToString().Contains(input.Filter.ToUpper())
               ).Where(o => o.TenantId == AbpSession.TenantId && o.INACTIVE == false).WhereIf(!string.IsNullOrWhiteSpace(input.ParamFilter), o => o.Integrated == true);

            var lookupTableDto = from o in query
                                 select new FinanceFindersDto
                                 {
                                     Id = o.BookID,
                                     DisplayName = o.BookName
                                 };
            lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<FinanceFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }
        private async Task<LookupDto<FinanceFindersDto>> GetAllGLCONFIGForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _glconfigRepository.GetAll().Join(_chartofControlRepository.GetAll(), x => x.AccountID, y => y.Id, (x, y) => new { x.ConfigID, x.AccountID, x.TenantId, x.BookID, y.AccountName })
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ConfigID.ToString().ToUpper().Contains(input.Filter.ToUpper().Trim()) || e.AccountID.ToUpper().Trim().ToString().Contains(input.Filter.ToUpper().Trim()) || e.AccountName.ToUpper().Trim().ToString().Contains(input.Filter.ToUpper().Trim()))
                .Where(o => o.TenantId == AbpSession.TenantId && o.BookID == input.ParamFilter);
            //.GroupBy(x => x.AccountID).Select(x => x.First());

            var lookupTableDto = from o in query
                                 select new FinanceFindersDto
                                 {
                                     Id = o.ConfigID.ToString(),
                                     AccountID = o.AccountID,
                                     DisplayName = o.AccountName
                                 };
            lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<FinanceFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }
        public string userInfo()
        {
            var data = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).FirstOrDefault();
            return data.Name;
        }
        private async Task<LookupDto<FinanceFindersDto>> GetAllICUserLocationForLookupTable(GetAllForLookupTableInput input)
        {
            var userid = userInfo();
            LookupDto<FinanceFindersDto> getData;
            if (userid.ToLower() != "admin")
            {
                var query = _csUserLocDRepository.GetAll().Where(c => c.TenantId == AbpSession.TenantId && c.TypeID == 2 && c.UserID == userid && c.Status == true);
                // .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                // e => e.LocId.ToString().ToUpper().Contains(input.Filter.ToUpper()) || e..ToUpper().Contains(input.Filter.ToUpper())).Where(o => o.TenantId == AbpSession.TenantId);
                var locQuery = _glLocationRepository.GetAll().Where(c => c.TenantId == AbpSession.TenantId);

                var lookupTableDto = from o in query
                                     join p in locQuery on o.LocId equals p.LocId
                                     select new FinanceFindersDto
                                     {
                                         Id = o.LocId.ToString(),
                                         DisplayName = p.LocDesc
                                     };
                lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
                var pageData = lookupTableDto.PageBy(input);
                var lookupTableDtoList = await pageData.ToListAsync();
                 getData = new LookupDto<FinanceFindersDto>(
                    query.Count(),
                    lookupTableDtoList
                );
            }
            else
            {
                var query = _glLocationRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.LocId.ToString().ToUpper().Contains(input.Filter.ToUpper()) || e.LocDesc.ToString().ToUpper().Contains(input.Filter.ToUpper())
               ).Where(o => o.TenantId == AbpSession.TenantId);

                var lookupTableDto = from o in query
                                     select new FinanceFindersDto
                                     {
                                         Id = o.LocId.ToString(),
                                         DisplayName = o.LocDesc
                                     };
                lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
                var pageData = lookupTableDto.PageBy(input);
                var lookupTableDtoList = await pageData.ToListAsync();
                 getData = new LookupDto<FinanceFindersDto>(
                    query.Count(),
                    lookupTableDtoList
                );
            }
         

            return getData;
        }
        private async Task<LookupDto<FinanceFindersDto>> GetAllGLLocationForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _glLocationRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e => e.LocId.ToString().ToUpper().Contains(input.Filter.ToUpper()) || e.LocDesc.ToString().ToUpper().Contains(input.Filter.ToUpper())
                ).Where(o => o.TenantId == AbpSession.TenantId);

            var lookupTableDto = from o in query
                                 select new FinanceFindersDto
                                 {
                                     Id = o.LocId.ToString(),
                                     DisplayName = o.LocDesc
                                 };
            lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<FinanceFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }

        private async Task<LookupDto<FinanceFindersDto>> GetInvoiceNoLookupTable(GetAllForLookupTableInput input)
        {

            var header = _glinvHeaderRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e => e.DocNo.ToString().ToUpper().Contains(input.Filter.ToUpper()) || e.PartyInvNo.ToString().ToUpper().Contains(input.Filter.ToUpper()) || e.InvAmount.ToString().ToUpper().Contains(input.Filter.ToUpper())
                ).Where(o => o.TenantId == AbpSession.TenantId && o.TypeID == input.ParamFilter);

            var detail = _glinvDetailRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.AccountID == input.Param2Filter && o.SubAccID == input.Param3Filter
            && o.SubAccID > 0
            );

            var lookupTableDto = from h in header
                                 join d in detail on h.Id equals d.DetID
                                 select new FinanceFindersDto
                                 {
                                     Id = h.DocNo.ToString(),
                                     DisplayName = h.PartyInvNo,
                                     DocDate = h.DocDate,
                                     Amount = d.Amount,
                                 };
            lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<FinanceFindersDto>(
                lookupTableDto.Count(),
                lookupTableDtoList
            );

            return getData;
        }

        private async Task<LookupDto<FinanceFindersDto>> GetInvoiceNoPVLookupTable(GetAllForLookupTableInput input)
        {
            try
            {
                var header = _porecHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Posted == true && o.AccountID == input.Param2Filter && o.SubAccID == input.Param3Filter);

                var apinvh = _apinvhRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId);
                List<FinanceFindersDto> FinalList = new List<FinanceFindersDto>();
                var lookupTableDto = from h in header
                                     select new FinanceFindersDto
                                     {
                                         Id = h.DocNo.ToString(),
                                         DisplayName = h.Narration,
                                         DocDate = h.DocDate,
                                         Amount = h.TotalAmt,
                                        
                                     };
                lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
                var pageData = lookupTableDto.PageBy(input);
                var lookupTableDtoList = await pageData.ToListAsync();
                var list = lookupTableDto.ToList();
                //foreach (var api in apinvh)
                //{
                //    foreach (var item in lookupTableDtoList)
                //    {
                //        if (api.PartyInvNo == item.Id)
                //        {
                //            var s = list.Where(x => x.Id == item.Id).ToList();
                //            list.Remove(s.FirstOrDefault());
                //        }
                //    }
                //}

                var getData = new LookupDto<FinanceFindersDto>(
                    list.Count(),
                    list
                );

                return getData;
            }
            catch (Exception ex)
            {

                throw;
            }
          
        }

      
        private async Task<LookupDto<FinanceFindersDto>> GetAllWHTermForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _apTermRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e => e.Id.ToString().ToUpper().Contains(input.Filter.ToUpper()) || e.TERMDESC.ToString().ToUpper().Contains(input.Filter.ToUpper()) || e.TERMRATE.ToString().ToUpper().Contains(input.Filter.ToUpper())
                ).Where(o => o.TenantId == AbpSession.TenantId && o.INACTIVE == false);

            var lookupTableDto = from o in query
                                 select new FinanceFindersDto
                                 {
                                     Id = o.Id.ToString(),
                                     DisplayName = o.TERMDESC,
                                     TermRate = Convert.ToDouble(o.TERMRATE)
                                 };
            lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<FinanceFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }
        private async Task<LookupDto<FinanceFindersDto>> GetBankReconcileDocForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _bankReconcileRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e => e.DocID.ToString().ToUpper().Contains(input.Filter.ToUpper())
                ).WhereIf(!string.IsNullOrWhiteSpace(input.ParamFilter), o => SplitMethod(o.DocID) == (input.ParamFilter)).Where(o => o.TenantId == AbpSession.TenantId);

            var lookupTableDto = from o in query
                                 select new FinanceFindersDto
                                 {
                                     Id = o.DocID.ToString()
                                 };
            lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<FinanceFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }
        private string SplitMethod(string str)
        {
            return str.Split("-")[0];
        }
        private async Task<LookupDto<FinanceFindersDto>> GetBankReconcileBankForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _bankReconcileRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e => e.BankID.ToString().ToUpper().Contains(input.Filter.ToUpper()) || e.BankName.ToString().ToUpper().Contains(input.Filter.ToUpper())
                ).Where(o => o.TenantId == AbpSession.TenantId).GroupBy(o => o.BankID).Select(x => x.FirstOrDefault());
            var lookupTableDto = from o in query
                                 select new FinanceFindersDto
                                 {
                                     Id = o.BankID.ToString(),
                                     DisplayName = o.BankName
                                 };
            lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<FinanceFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }

    }
}
