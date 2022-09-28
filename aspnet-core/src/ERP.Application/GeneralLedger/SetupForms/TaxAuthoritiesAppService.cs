using ERP.CommonServices;


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
using System.Configuration;
using System.Data.SqlClient;

namespace ERP.GeneralLedger.SetupForms
{
	[AbpAuthorize(AppPermissions.Pages_TaxAuthorities)]
    public class TaxAuthoritiesAppService : ERPAppServiceBase, ITaxAuthoritiesAppService
    {
		 private readonly IRepository<TaxAuthority, string> _taxAuthorityRepository;
		 private readonly ITaxAuthoritiesExcelExporter _taxAuthoritiesExcelExporter;
		 private readonly IRepository<CompanyProfile,string> _lookup_companyProfileRepository;
		 

		  public TaxAuthoritiesAppService(IRepository<TaxAuthority, string> taxAuthorityRepository, ITaxAuthoritiesExcelExporter taxAuthoritiesExcelExporter , IRepository<CompanyProfile, string> lookup_companyProfileRepository) 
		  {
			_taxAuthorityRepository = taxAuthorityRepository;
			_taxAuthoritiesExcelExporter = taxAuthoritiesExcelExporter;
			_lookup_companyProfileRepository = lookup_companyProfileRepository;
		
		  }

		 public async Task<PagedResultDto<GetTaxAuthorityForViewDto>> GetAll(GetAllTaxAuthoritiesInput input)
         {

            var filteredTaxAuthorities = _taxAuthorityRepository.GetAll()
                        //.Include( e => e.CompanyFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Id.Contains(input.Filter)  || e.TAXAUTHDESC.Contains(input.Filter) || e.ACCLIABILITY.Contains(input.Filter) || e.ACCRECOVERABLE.Contains(input.Filter) || e.ACCEXPENSE.Contains(input.Filter) || e.AUDTUSER.Contains(input.Filter))

                   
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TAXAUTHDESCFilter), e => e.TAXAUTHDESC.ToLower() == input.TAXAUTHDESCFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TAXAUTHFilter), e => e.Id.ToLower() == input.TAXAUTHFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ACCLIABILITYFilter), e => e.ACCLIABILITY.ToLower() == input.ACCLIABILITYFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ACCRECOVERABLEFilter), e => e.ACCRECOVERABLE.ToLower() == input.ACCRECOVERABLEFilter.ToLower().Trim())
                        .WhereIf(input.MinRECOVERRATEFilter != null, e => e.RECOVERRATE >= input.MinRECOVERRATEFilter)
                        .WhereIf(input.MaxRECOVERRATEFilter != null, e => e.RECOVERRATE <= input.MaxRECOVERRATEFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ACCEXPENSEFilter), e => e.ACCEXPENSE.ToLower() == input.ACCEXPENSEFilter.ToLower().Trim())
                        .WhereIf(input.MinAUDTDATEFilter != null, e => e.AUDTDATE >= input.MinAUDTDATEFilter)
                        .WhereIf(input.MaxAUDTDATEFilter != null, e => e.AUDTDATE <= input.MaxAUDTDATEFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AUDTUSERFilter), e => e.AUDTUSER.ToLower() == input.AUDTUSERFilter.ToLower().Trim()).Where(o => o.TenantId == AbpSession.TenantId);
						//.WhereIf(!string.IsNullOrWhiteSpace(input.CompanyProfileIdFilter), e => e.CompanyFk != null && e.CompanyFk.Id.ToLower() == input.CompanyProfileIdFilter.ToLower().Trim());


					

			var pagedAndFilteredTaxAuthorities = filteredTaxAuthorities
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var taxAuthorities = from o in pagedAndFilteredTaxAuthorities
                         //join o1 in _lookup_companyProfileRepository.GetAll() on o.CompanyId equals o1.Id into j1
                        
                         
                         select new GetTaxAuthorityForViewDto() {
							TaxAuthority = new TaxAuthorityDto
							{

                              
                                TAXAUTHDESC = o.TAXAUTHDESC,
                                ACCLIABILITY = o.ACCLIABILITY,
                                ACCRECOVERABLE = o.ACCRECOVERABLE,
                                RECOVERRATE = o.RECOVERRATE,
                                ACCEXPENSE = o.ACCEXPENSE,
                                AUDTDATE = o.AUDTDATE,
                                AUDTUSER = o.AUDTUSER,
                                Id = o.Id
							},
                         	
						};

            var totalCount = await filteredTaxAuthorities.CountAsync();

            return new PagedResultDto<GetTaxAuthorityForViewDto>(
                totalCount,
                await taxAuthorities.ToListAsync()
            );
         }
		 
		 public async Task<GetTaxAuthorityForViewDto> GetTaxAuthorityForView(string id)
         {
            var taxAuthority = await _taxAuthorityRepository.GetAsync(id);

            var output = new GetTaxAuthorityForViewDto { TaxAuthority = ObjectMapper.Map<TaxAuthorityDto>(taxAuthority) };
            return output;
         }

        public bool CheckTaxAuthExist(string id)
        {
            var check = _taxAuthorityRepository.GetAll().Where(o => o.Id == id && o.TenantId == AbpSession.TenantId).SingleOrDefault();
                return check == null ? false : true;
        }

        [AbpAuthorize(AppPermissions.Pages_TaxAuthorities_Edit)]
		 public async Task<GetTaxAuthorityForEditOutput> GetTaxAuthorityForEdit(EntityDto<string> input)
         {
            var taxAuthority = await _taxAuthorityRepository.FirstOrDefaultAsync(input.Id);

            
           
		    var output = new GetTaxAuthorityForEditOutput {TaxAuthority = ObjectMapper.Map<CreateOrEditTaxAuthorityDto>(taxAuthority)};

		    
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditTaxAuthorityDto input)
         {
            if (input.flag)
            {
                    await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_TaxAuthorities_Create)]
		 protected virtual async Task Create(CreateOrEditTaxAuthorityDto input)
         {
            var taxAuthority = ObjectMapper.Map<TaxAuthority>(input);

			
			if (AbpSession.TenantId != null)
			{
				taxAuthority.TenantId = (int?) AbpSession.TenantId;
			}

           
            taxAuthority.AUDTUSER = "admin";
            taxAuthority.AUDTDATE = DateTime.Now;
            // taxAuthority.CompanyId = input.CMPID;


            await _taxAuthorityRepository.InsertAsync(taxAuthority);
         }

		 [AbpAuthorize(AppPermissions.Pages_TaxAuthorities_Edit)]
		 protected virtual async Task Update(CreateOrEditTaxAuthorityDto input)
         {
            string str = ConfigurationManager.AppSettings["ConnectionString"];
            using (SqlConnection cn = new SqlConnection(str))
            {
                using (SqlCommand command = cn.CreateCommand())
                {
                    command.CommandText = "Update CSTAXAUTH SET TAXAUTH = @TAXAUTH, TAXAUTHDESC = @TAXAUTHDESC WHERE TAXAUTH = @ID AND TenantId = @TENANTID";
                    command.Parameters.AddWithValue("@TAXAUTH", input.Id);
                    command.Parameters.AddWithValue("@TAXAUTHDESC", input.TAXAUTHDESC);
                    command.Parameters.AddWithValue("@ID", input.Id);
                    command.Parameters.AddWithValue("@TENANTID", AbpSession.TenantId);
                    cn.Open();
                    command.ExecuteNonQuery();
                    cn.Close();
                }
            }
            //try
            //{
            //    input.TenantId = AbpSession.TenantId;
            //    var taxAuthority = await _taxAuthorityRepository.FirstOrDefaultAsync((string)input.Id);
            //    ObjectMapper.Map(input, taxAuthority);
            //}
            //catch (Exception ex)
            //{

            //}
            
         }

		 [AbpAuthorize(AppPermissions.Pages_TaxAuthorities_Delete)]
         public async Task Delete(EntityDto<string> input)
         {
            await _taxAuthorityRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetTaxAuthoritiesToExcel(GetAllTaxAuthoritiesForExcelInput input)
         {

            var filteredTaxAuthorities = _taxAuthorityRepository.GetAll()
                        //.Include( e => e.CompanyFk)
                  
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CMPIDFilter), e => e.Id.ToLower() == input.CMPIDFilter.ToLower().Trim())
                      
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TAXAUTHDESCFilter), e => e.TAXAUTHDESC.ToLower() == input.TAXAUTHDESCFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ACCLIABILITYFilter), e => e.ACCLIABILITY.ToLower() == input.ACCLIABILITYFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ACCRECOVERABLEFilter), e => e.ACCRECOVERABLE.ToLower() == input.ACCRECOVERABLEFilter.ToLower().Trim())
                        .WhereIf(input.MinRECOVERRATEFilter != null, e => e.RECOVERRATE >= input.MinRECOVERRATEFilter)
                        .WhereIf(input.MaxRECOVERRATEFilter != null, e => e.RECOVERRATE <= input.MaxRECOVERRATEFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ACCEXPENSEFilter), e => e.ACCEXPENSE.ToLower() == input.ACCEXPENSEFilter.ToLower().Trim())
                        .WhereIf(input.MinAUDTDATEFilter != null, e => e.AUDTDATE >= input.MinAUDTDATEFilter)
                        .WhereIf(input.MaxAUDTDATEFilter != null, e => e.AUDTDATE <= input.MaxAUDTDATEFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AUDTUSERFilter), e => e.AUDTUSER.ToLower() == input.AUDTUSERFilter.ToLower().Trim()).Where(o => o.TenantId == AbpSession.TenantId);
					//	.WhereIf(!string.IsNullOrWhiteSpace(input.CompanyProfileIdFilter), e => e.CompanyFk != null && e.CompanyFk.Id.ToLower() == input.CompanyProfileIdFilter.ToLower().Trim());
						

			var query = (from o in filteredTaxAuthorities
                      
                       
                         
                         
                         select new GetTaxAuthorityForViewDto() { 
							TaxAuthority = new TaxAuthorityDto
							{

                              
                                TAXAUTHDESC = o.TAXAUTHDESC,
                                ACCLIABILITY = o.ACCLIABILITY,
                                ACCRECOVERABLE = o.ACCRECOVERABLE,
                                RECOVERRATE = o.RECOVERRATE,
                                ACCEXPENSE = o.ACCEXPENSE,
                                AUDTDATE = o.AUDTDATE,
                                AUDTUSER = o.AUDTUSER,
                                Id = o.Id
							},
                        
						 });


            var taxAuthorityListDtos = await query.ToListAsync();

            return _taxAuthoritiesExcelExporter.ExportToFile(taxAuthorityListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_TaxAuthorities)]
         public async Task<PagedResultDto<TaxAuthorityCompanyProfileLookupTableDto>> GetAllCompanyProfileForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_companyProfileRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Id.ToString().Contains(input.Filter)
                ).Where(o => o.TenantId == AbpSession.TenantId);

            var totalCount = await query.CountAsync();

            var companyProfileList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<TaxAuthorityCompanyProfileLookupTableDto>();
			foreach(var companyProfile in companyProfileList){
				lookupTableDtoList.Add(new TaxAuthorityCompanyProfileLookupTableDto
				{
					Id = companyProfile.Id,
					DisplayName = companyProfile.Id?.ToString()
				});
			}

            return new PagedResultDto<TaxAuthorityCompanyProfileLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}