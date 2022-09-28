using ERP.GeneralLedger.SetupForms;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.CommonServices.Exporting;
using ERP.CommonServices.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ERP.CommonServices
{
	[AbpAuthorize(AppPermissions.Pages_Banks)]
    public class BanksAppService : ERPAppServiceBase, IBanksAppService
    {
		 private readonly IRepository<Bank> _bankRepository;
		 private readonly IBanksExcelExporter _banksExcelExporter;
		 private readonly IRepository<ChartofControl,string> _lookup_chartofControlRepository;
		 

		  public BanksAppService(IRepository<Bank> bankRepository, IBanksExcelExporter banksExcelExporter , IRepository<ChartofControl, string> lookup_chartofControlRepository) 
		  {
			_bankRepository = bankRepository;
			_banksExcelExporter = banksExcelExporter;
			_lookup_chartofControlRepository = lookup_chartofControlRepository;
		
		  }

		 public async Task<PagedResultDto<GetBankForViewDto>> GetAll(GetAllBanksInput input)
         {

            var filteredBanks = _bankRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CMPID.Contains(input.Filter) || e.BANKID.Contains(input.Filter) || e.BANKNAME.Contains(input.Filter) || e.ADDR1.Contains(input.Filter) || e.ADDR2.Contains(input.Filter) || e.ADDR3.Contains(input.Filter) || e.ADDR4.Contains(input.Filter) || e.CITY.Contains(input.Filter) || e.STATE.Contains(input.Filter) || e.COUNTRY.Contains(input.Filter) || e.POSTAL.Contains(input.Filter) || e.CONTACT.Contains(input.Filter) || e.PHONE.Contains(input.Filter) || e.FAX.Contains(input.Filter) || e.BKACCTNUMBER.Contains(input.Filter) || e.IDACCTBANK.Contains(input.Filter) || e.IDACCTWOFF.Contains(input.Filter) || e.IDACCTCRCARD.Contains(input.Filter) || e.AUDTUSER.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CMPIDFilter), e => e.CMPID.ToLower() == input.CMPIDFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BANKIDFilter), e => e.BANKID.ToLower() == input.BANKIDFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BANKNAMEFilter), e => e.BANKNAME.ToLower() == input.BANKNAMEFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ADDR1Filter), e => e.ADDR1.ToLower() == input.ADDR1Filter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ADDR2Filter), e => e.ADDR2.ToLower() == input.ADDR2Filter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ADDR3Filter), e => e.ADDR3.ToLower() == input.ADDR3Filter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ADDR4Filter), e => e.ADDR4.ToLower() == input.ADDR4Filter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CITYFilter), e => e.CITY.ToLower() == input.CITYFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.STATEFilter), e => e.STATE.ToLower() == input.STATEFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.COUNTRYFilter), e => e.COUNTRY.ToLower() == input.COUNTRYFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.POSTALFilter), e => e.POSTAL.ToLower() == input.POSTALFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CONTACTFilter), e => e.CONTACT.ToLower() == input.CONTACTFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PHONEFilter), e => e.PHONE.ToLower() == input.PHONEFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FAXFilter), e => e.FAX.ToLower() == input.FAXFilter.ToLower().Trim())
                        .WhereIf(input.INACTIVEFilter > -1, e => Convert.ToInt32(e.INACTIVE) == input.INACTIVEFilter)
                        .WhereIf(input.MinINACTDATEFilter != null, e => e.INACTDATE >= input.MinINACTDATEFilter)
                        .WhereIf(input.MaxINACTDATEFilter != null, e => e.INACTDATE <= input.MaxINACTDATEFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BKACCTNUMBERFilter), e => e.BKACCTNUMBER.ToLower() == input.BKACCTNUMBERFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IDACCTBANKFilter), e => e.IDACCTBANK.ToLower() == input.IDACCTBANKFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IDACCTWOFFFilter), e => e.IDACCTWOFF.ToLower() == input.IDACCTWOFFFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IDACCTCRCARDFilter), e => e.IDACCTCRCARD.ToLower() == input.IDACCTCRCARDFilter.ToLower().Trim())
                        .WhereIf(input.MinAUDTDATEFilter != null, e => e.AUDTDATE >= input.MinAUDTDATEFilter)
                        .WhereIf(input.MaxAUDTDATEFilter != null, e => e.AUDTDATE <= input.MaxAUDTDATEFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AUDTUSERFilter), e => e.AUDTUSER.ToLower() == input.AUDTUSERFilter.ToLower().Trim()).Where(o => o.TenantId == AbpSession.TenantId);

			var pagedAndFilteredBanks = filteredBanks
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);


            var banks = from o in pagedAndFilteredBanks
                                                  

                            //var banks = from o in pagedAndFilteredBanks
                            //                      join o1 in _lookup_chartofControlRepository.GetAll() on o.AccountID equals o1.Id into j1
                            //                      from s1 in j1.DefaultIfEmpty()

                        select new GetBankForViewDto() {
							Bank = new BankDto
							{
                                CMPID = o.CMPID,
                                DocType=o.DocType,
                                BANKID = o.BANKID,
                                BANKNAME = o.BANKNAME,
                                BranchName=o.BranchName,
                                ADDR1 = o.ADDR1,
                                ADDR2 = o.ADDR2,
                                ADDR3 = o.ADDR3,
                                ADDR4 = o.ADDR4,
                                CITY = o.CITY,
                                STATE = o.STATE,
                                COUNTRY = o.COUNTRY,
                                POSTAL = o.POSTAL,
                                CONTACT = o.CONTACT,
                                PHONE = o.PHONE,
                                FAX = o.FAX,
                                ODLIMIT=o.ODLIMIT,
                                INACTIVE = o.INACTIVE,
                                INACTDATE = o.INACTDATE,
                                BKACCTNUMBER = o.BKACCTNUMBER,
                                IDACCTBANK = o.IDACCTBANK,
                                IDACCTWOFF = o.IDACCTWOFF,
                                IDACCTCRCARD = o.IDACCTCRCARD,
                                AUDTDATE = o.AUDTDATE,
                                AUDTUSER = o.AUDTUSER,
                                Id = o.Id
							},
                         
						};

            var totalCount = await filteredBanks.CountAsync();

            return new PagedResultDto<GetBankForViewDto>(
                totalCount,
                await banks.ToListAsync()
            );
         }
		 
		 public async Task<GetBankForViewDto> GetBankForView(int id)
         {
            var bank = await _bankRepository.GetAsync(id);

            var output = new GetBankForViewDto { Bank = ObjectMapper.Map<BankDto>(bank) };

		    //if (output.Bank.AccountID != null)
      //      {
      //          var _lookupChartofControl = await _lookup_chartofControlRepository.FirstOrDefaultAsync((string)output.Bank.AccountID);
      //          output.ChartofControlId = _lookupChartofControl.Id.ToString();
      //      }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Banks_Edit)]
		 public async Task<GetBankForEditOutput> GetBankForEdit(EntityDto input)
         {
            var bank = await _bankRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetBankForEditOutput {Bank = ObjectMapper.Map<CreateOrEditBankDto>(bank)};

		    //if (output.Bank.AccountID != null)
      //      {
      //          var _lookupChartofControl = await _lookup_chartofControlRepository.FirstOrDefaultAsync((string)output.Bank.AccountID);
      //          output.ChartofControlId = _lookupChartofControl.Id.ToString();
      //      }
			
            return output;
         }

		 public async Task<string> CreateOrEdit(CreateOrEditBankDto input)
         {
            if(input.Id == null){
                if(_bankRepository.GetAll().WhereIf(!string.IsNullOrWhiteSpace(input.BANKID), e => e.BANKID.ToLower() == input.BANKID.ToLower().Trim()).Where(x=>x.TenantId == AbpSession.TenantId).Count()>0)
                {
                    return "Present";
                }
				await Create(input);
			}
			else{
				await Update(input);
			}
            return "Save";
        }

		 [AbpAuthorize(AppPermissions.Pages_Banks_Create)]
		 protected virtual async Task Create(CreateOrEditBankDto input)
         {
            var bank = ObjectMapper.Map<Bank>(input);

			
			if (AbpSession.TenantId != null)
			{
				bank.TenantId = (int) AbpSession.TenantId;
			}
		

            await _bankRepository.InsertAsync(bank);
         }

		 [AbpAuthorize(AppPermissions.Pages_Banks_Edit)]
		 protected virtual async Task Update(CreateOrEditBankDto input)
         {
            var bank = await _bankRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, bank);
         }

		 [AbpAuthorize(AppPermissions.Pages_Banks_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _bankRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetBanksToExcel(GetAllBanksForExcelInput input)
         {

            var filteredBanks = _bankRepository.GetAll()

                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CMPID.Contains(input.Filter) || e.BANKID.Contains(input.Filter) || e.BANKNAME.Contains(input.Filter) || e.ADDR1.Contains(input.Filter) || e.ADDR2.Contains(input.Filter) || e.ADDR3.Contains(input.Filter) || e.ADDR4.Contains(input.Filter) || e.CITY.Contains(input.Filter) || e.STATE.Contains(input.Filter) || e.COUNTRY.Contains(input.Filter) || e.POSTAL.Contains(input.Filter) || e.CONTACT.Contains(input.Filter) || e.PHONE.Contains(input.Filter) || e.FAX.Contains(input.Filter) || e.BKACCTNUMBER.Contains(input.Filter) || e.IDACCTBANK.Contains(input.Filter) || e.IDACCTWOFF.Contains(input.Filter) || e.IDACCTCRCARD.Contains(input.Filter) || e.AUDTUSER.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CMPIDFilter), e => e.CMPID.ToLower() == input.CMPIDFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BANKIDFilter), e => e.BANKID.ToLower() == input.BANKIDFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BANKNAMEFilter), e => e.BANKNAME.ToLower() == input.BANKNAMEFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ADDR1Filter), e => e.ADDR1.ToLower() == input.ADDR1Filter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ADDR2Filter), e => e.ADDR2.ToLower() == input.ADDR2Filter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ADDR3Filter), e => e.ADDR3.ToLower() == input.ADDR3Filter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ADDR4Filter), e => e.ADDR4.ToLower() == input.ADDR4Filter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CITYFilter), e => e.CITY.ToLower() == input.CITYFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.STATEFilter), e => e.STATE.ToLower() == input.STATEFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.COUNTRYFilter), e => e.COUNTRY.ToLower() == input.COUNTRYFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.POSTALFilter), e => e.POSTAL.ToLower() == input.POSTALFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CONTACTFilter), e => e.CONTACT.ToLower() == input.CONTACTFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PHONEFilter), e => e.PHONE.ToLower() == input.PHONEFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FAXFilter), e => e.FAX.ToLower() == input.FAXFilter.ToLower().Trim())
                        .WhereIf(input.INACTIVEFilter > -1, e => Convert.ToInt32(e.INACTIVE) == input.INACTIVEFilter)
                        .WhereIf(input.MinINACTDATEFilter != null, e => e.INACTDATE >= input.MinINACTDATEFilter)
                        .WhereIf(input.MaxINACTDATEFilter != null, e => e.INACTDATE <= input.MaxINACTDATEFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BKACCTNUMBERFilter), e => e.BKACCTNUMBER.ToLower() == input.BKACCTNUMBERFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IDACCTBANKFilter), e => e.IDACCTBANK.ToLower() == input.IDACCTBANKFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IDACCTWOFFFilter), e => e.IDACCTWOFF.ToLower() == input.IDACCTWOFFFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IDACCTCRCARDFilter), e => e.IDACCTCRCARD.ToLower() == input.IDACCTCRCARDFilter.ToLower().Trim())
                        .WhereIf(input.MinAUDTDATEFilter != null, e => e.AUDTDATE >= input.MinAUDTDATEFilter)
                        .WhereIf(input.MaxAUDTDATEFilter != null, e => e.AUDTDATE <= input.MaxAUDTDATEFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AUDTUSERFilter), e => e.AUDTUSER.ToLower() == input.AUDTUSERFilter.ToLower().Trim()).Where(o => o.TenantId == AbpSession.TenantId);


                        var query = (from o in filteredBanks


                                         //var query = (from o in filteredBanks
                                         //                      join o1 in _lookup_chartofControlRepository.GetAll() on o.AccountID equals o1.Id into j1
                                         //                      from s1 in j1.DefaultIfEmpty()

                                     select new GetBankForViewDto() { 
							Bank = new BankDto
							{
                                CMPID = o.CMPID,
                                DocType = o.DocType,
                                BANKID = o.BANKID,
                                BANKNAME = o.BANKNAME,
                                BranchName = o.BranchName,
                                ADDR1 = o.ADDR1,
                                ADDR2 = o.ADDR2,
                                ADDR3 = o.ADDR3,
                                ADDR4 = o.ADDR4,
                                CITY = o.CITY,
                                STATE = o.STATE,
                                COUNTRY = o.COUNTRY,
                                POSTAL = o.POSTAL,
                                CONTACT = o.CONTACT,
                                PHONE = o.PHONE,
                                FAX = o.FAX,
                                ODLIMIT=o.ODLIMIT,
                                INACTIVE = o.INACTIVE,
                                INACTDATE = o.INACTDATE,
                                BKACCTNUMBER = o.BKACCTNUMBER,
                                IDACCTBANK = o.IDACCTBANK,
                                IDACCTWOFF = o.IDACCTWOFF,
                                IDACCTCRCARD = o.IDACCTCRCARD,
                                AUDTDATE = o.AUDTDATE,
                                AUDTUSER = o.AUDTUSER,
                                Id = o.Id
							},
                         	//ChartofControlId = s1 == null ? "" : s1.Id.ToString()
						 });


            var bankListDtos = await query.ToListAsync();

            return _banksExcelExporter.ExportToFile(bankListDtos);
         }

		[AbpAuthorize(AppPermissions.Pages_Banks)]
         public async Task<PagedResultDto<BankChartofControlLookupTableDto>> GetAllChartofControlForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_chartofControlRepository.GetAll().WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Id.ToLower().Trim().ToString().Contains(input.Filter.ToLower().Trim()) || e.AccountName.ToLower().Trim().ToString().Contains(input.Filter.ToLower().Trim()))
                   .Where(o => o.TenantId == AbpSession.TenantId);

            var totalCount = await query.CountAsync();

            var chartofControlList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<BankChartofControlLookupTableDto>();
			foreach(var chartofControl in chartofControlList){
				lookupTableDtoList.Add(new BankChartofControlLookupTableDto
				{
					Id = chartofControl.Id,
					DisplayName = chartofControl.AccountName.ToString(),
                    Subledger= chartofControl.SubLedger
                });
			}

            return new PagedResultDto<BankChartofControlLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }



    }
}