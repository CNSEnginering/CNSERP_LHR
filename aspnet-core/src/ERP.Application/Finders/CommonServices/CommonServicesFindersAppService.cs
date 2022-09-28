

using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using ERP.CommonServices;
using ERP.CommonServices.RecurringVoucher;
using ERP.Finders.Dtos;
using ERP.GeneralLedger.SetupForms;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRD;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH;
using ERP.Reports.SupplyChain.Inventory.Dtos;
using ERP.SupplyChain.Purchase.Requisition;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Finders.CommonServices
{
    public class CommonServicesFindersAppService : ERPAppServiceBase
    {
        private readonly IRepository<Bank, int> _bankRepository;
        private readonly IRepository<RecurringVoucher> _recurringVoucherRepository;
        private readonly IRepository<GLTRHeader> _gltrHeaderRepository;
        private readonly IRepository<GLTRDetail> _gltrDetailRepository;
        private readonly IRepository<TaxAuthority, string> _taxAuthorityRepository;
        private readonly IRepository<TaxClass> _taxClassRepository;
        private readonly IRepository<CPR> _cprRepository;
        private readonly IRepository<CurrencyRate, string> _currencyRateRepository;
        private readonly IRepository<Requisitions> _requisitionRepository;
        public CommonServicesFindersAppService(
            IRepository<Bank, int> bankRepository,
            IRepository<GLTRHeader> gltrHeaderRepository,
            IRepository<GLTRDetail> gltrDetailRepository,
            IRepository<TaxAuthority, string> taxAuthorityRepository,
            IRepository<TaxClass> taxClassRepository,
            IRepository<CurrencyRate, string> currencyRateRepository,
            IRepository<CPR> cprRepository,
            IRepository<RecurringVoucher> recurringVoucherRepository,
             IRepository<Requisitions> requisitionRepository)
        {
            _bankRepository = bankRepository;
            _gltrHeaderRepository = gltrHeaderRepository;
            _gltrDetailRepository = gltrDetailRepository;
            _taxAuthorityRepository = taxAuthorityRepository;
            _taxClassRepository = taxClassRepository;
            _currencyRateRepository = currencyRateRepository;
            _cprRepository = cprRepository;
            _recurringVoucherRepository = recurringVoucherRepository;
            _requisitionRepository = requisitionRepository;
        }

        public async Task<PagedResultDto<CommonServiceFindersDto>> GetCommonServiceLookupTable(GetAllForLookupTableInput input)
        {
            //var resultDtos = new List<CommonServiceFindersDto>();
            var resultDtos = new LookupDto<CommonServiceFindersDto>();
            switch (input.Target)
            {
                case "Bank":
                    resultDtos = await GetAllBankForLookupTable(input);
                    break;
                case "CPR":
                    resultDtos = await GetAllCPRForLookupTable(input);
                    break;
                case "BankTransfer":
                    resultDtos = await GetAllBankTransferForLookupTable(input);
                    break;
                case "TaxAuthority":
                    resultDtos = await GetAllTaxAuthorityForLookupTable(input);
                    break;
                case "TaxClass":
                    resultDtos = await GetAllTaxClassForLookupTable(input);
                    break;
                case "Currency":
                    resultDtos = await GetAllCurrencyRateForLookupTable(input);
                    break;
                case "RecurringVouchers":
                    resultDtos = await GetRecurringVouchers(input);
                    break;
                case "RequsitionNo":
                    resultDtos = await GetRequistionLookupTable(input);
                    break;
                default:
                    break;
            }
            return new PagedResultDto<CommonServiceFindersDto>(
                    resultDtos.Count,
                    resultDtos.Items
                );
        }
        private async Task<LookupDto<CommonServiceFindersDto>> GetRequistionLookupTable(GetAllForLookupTableInput input)
        {
            var query = _requisitionRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Approved == true && o.Active == true);

            var lookupTableDto = from o in query
                                 select new CommonServiceFindersDto
                                 {
                                     Id = o.DocNo.ToString(),
                                     DisplayName = o.DocDate.ToString(),
                                     AccountID = o.TotalQty.ToString(),
                                     //TermRate = Convert.ToDouble(o.OrdNo)
                                 };
            lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<CommonServiceFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }

        private async Task<LookupDto<CommonServiceFindersDto>> GetRecurringVouchers(GetAllForLookupTableInput input)
        {
            var query = _recurringVoucherRepository.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => e.BookID == input.Filter
                    ||
                    e.Reference.Contains(input.Filter)
                     ||
                    e.VoucherNo.ToString() == input.Filter
                    )
                .Where(o => o.TenantId == AbpSession.TenantId && o.Active == true);

            var lookupTableDto = from o in query
                                 select new CommonServiceFindersDto
                                 {
                                     Id = o.VoucherNo.ToString(),
                                     DisplayName = o.BookID.ToString(),
                                     DetId = _gltrHeaderRepository.GetAll().Where(p => p.TenantId == AbpSession.TenantId && p.BookID == o.BookID.ToString()
                                     && p.DocNo == o.VoucherNo
                                     ).Count() > 0 ? _gltrHeaderRepository.GetAll().Where(p => p.TenantId == AbpSession.TenantId && p.BookID == o.BookID.ToString()
                                     && p.DocNo == o.VoucherNo
                                     ).First().Id : 0,
                                     Narration = o.Reference
                                 };
            lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);

            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList =  pageData.ToList();
            var getData = new LookupDto<CommonServiceFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }

        private async Task<LookupDto<CommonServiceFindersDto>> GetAllBankForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _bankRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e => e.BANKID.ToString().ToUpper().Contains(input.Filter.ToUpper()) || e.BANKNAME.ToString().ToUpper().Contains(input.Filter.ToUpper()) || e.IDACCTBANK.ToString().ToUpper().Contains(input.Filter.ToUpper())
                ).WhereIf(!string.IsNullOrWhiteSpace(input.ParamFilter),
                    e => e.DocType.Equals(Convert.ToInt32(input.ParamFilter.Trim()))).Where(o => o.TenantId == AbpSession.TenantId);

            var lookupTableDto = from o in query
                                 select new CommonServiceFindersDto
                                 {
                                     Id = o.BANKID,
                                     DisplayName = o.BANKNAME.ToString(),
                                    BKAccountID = o.BKACCTNUMBER,
                                     AccountID = o.IDACCTBANK,
                                     DocType = o.DocType
                                 };
            lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<CommonServiceFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }
        private async Task<LookupDto<CommonServiceFindersDto>> GetAllBankTransferForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _bankRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e => e.BANKID.ToString().ToUpper().Contains(input.Filter.ToUpper()) || e.BANKNAME.ToString().ToUpper().Contains(input.Filter.ToUpper()) || e.BKACCTNUMBER.ToString().ToUpper().Contains(input.Filter.ToUpper()) || e.IDACCTBANK.ToString().ToUpper().Contains(input.Filter.ToUpper())
                ).Where(o => o.TenantId == AbpSession.TenantId);

            var lookupTableDto = from e in query
                                 select new CommonServiceFindersDto
                                 {
                                     Id = e.BANKID,
                                     DisplayName = e.BANKNAME.ToString(),
                                     AccountID = e.BKACCTNUMBER,
                                     AvailableLimit = Math.Abs(Convert.ToDouble((from a in _gltrHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                                                                                 join
                                                  b in _gltrDetailRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                                                  on new { A = a.Id, B = a.TenantId } equals new { A = b.DetID, B = b.TenantId }
                                                                                 where (b.AccountID == e.IDACCTBANK && b.TenantId == AbpSession.TenantId && a.Approved == true)
                                                                                 select b.Amount).Sum()))

                                 };
            lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<CommonServiceFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }
        private async Task<LookupDto<CommonServiceFindersDto>> GetAllTaxAuthorityForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _taxAuthorityRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e => e.Id.ToString().ToUpper().Contains(input.Filter.ToUpper()) || e.TAXAUTHDESC.ToUpper().Contains(input.Filter.ToUpper())
                ).Where(o => o.TenantId == AbpSession.TenantId);

            var lookupTableDto = from o in query
                                 select new CommonServiceFindersDto
                                 {
                                     Id = o.Id,
                                     DisplayName = o.TAXAUTHDESC
                                 };
            lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<CommonServiceFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }
        private async Task<LookupDto<CommonServiceFindersDto>> GetAllCPRForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _cprRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e => e.CprId.ToString().ToUpper().Contains(input.Filter.ToUpper()) || e.CprNo.ToUpper().Contains(input.Filter.ToUpper())
                ).Where(o => o.TenantId == AbpSession.TenantId);

            var lookupTableDto = from o in query
                                 select new CommonServiceFindersDto
                                 {
                                     Id = o.CprId.ToString(),
                                     DisplayName = o.CprNo
                                 };
            lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<CommonServiceFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }
        private async Task<LookupDto<CommonServiceFindersDto>> GetAllTaxClassForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _taxClassRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e => e.Id.ToString().ToUpper().Contains(input.Filter.ToUpper()) || e.CLASSDESC.ToString().ToUpper().Contains(input.Filter.ToUpper())
                ).WhereIf(!string.IsNullOrWhiteSpace(input.ParamFilter), e => e.TAXAUTH.ToUpper().Contains(input.ParamFilter.ToUpper())).Where(o => o.TenantId == AbpSession.TenantId
                && o.IsActive == true
                );

            var lookupTableDto = from o in query
                                 select new CommonServiceFindersDto
                                 {
                                     Id = o.CLASSID.ToString(),
                                     DisplayName = o.CLASSDESC,
                                     TaxRate = o.CLASSRATE.Value,
                                     AccountID = o.TAXACCID
                                 };
            lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<CommonServiceFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }
        private async Task<LookupDto<CommonServiceFindersDto>> GetAllCurrencyRateForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _currencyRateRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e => e.Id.ToString().ToUpper().Contains(input.Filter.ToUpper()) || e.CURRATE.ToString().ToUpper().Contains(input.Filter.ToUpper()) || e.CURNAME.ToString().ToUpper().Contains(input.Filter.ToUpper())
                ).Where(o => o.TenantId == AbpSession.TenantId);

            var lookupTableDto = from o in query
                                 select new CommonServiceFindersDto
                                 {
                                     Id = o.Id,
                                     DisplayName = o.CURNAME,
                                     CurrRate = o.CURRATE
                                 };
            lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<CommonServiceFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }
        public async Task<List<FormDto>> GetFormIDList()
        {
            SqlCommand cmd;
            string str = ConfigurationManager.AppSettings["ConnectionString"];
            List<FormDto> FormList = new List<FormDto>();
            using (SqlConnection cn = new SqlConnection(str))
            {
                cmd = new SqlCommand("sp_GetAllForm", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@TenantId", AbpSession.TenantId);
                // cmd.Parameters.AddWithValue("@FormName", FormName);
                cn.Open();
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        FormDto form = new FormDto()
                        {
                            FormId = rdr["ID"] is DBNull ? 0 : Convert.ToInt32(rdr["ID"]),
                            FormName = rdr["FormName"] is DBNull ? "" : rdr["FormName"].ToString(),

                        };
                        FormList.Add(form);
                    }
                }

            }
            return FormList;
        }
    }
}