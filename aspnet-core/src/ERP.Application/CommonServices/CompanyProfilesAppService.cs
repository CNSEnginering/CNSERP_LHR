

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
using ERP.MultiTenancy;
using ERP.MultiTenancy.Dto;

namespace ERP.CommonServices
{
    [AbpAuthorize(AppPermissions.SetupForms_CompanyProfiles)]
    public class CompanyProfilesAppService : ERPAppServiceBase, ICompanyProfilesAppService
    {
        private readonly IRepository<CompanyProfile, string> _companyProfileRepository;
        private readonly ICompanyProfilesExcelExporter _companyProfilesExcelExporter;
        private readonly TenantManager _tenantManager;

        public CompanyProfilesAppService(IRepository<CompanyProfile, string> companyProfileRepository, ICompanyProfilesExcelExporter companyProfilesExcelExporter, TenantManager tenantManager)
        {
            _companyProfileRepository = companyProfileRepository;
            _companyProfilesExcelExporter = companyProfilesExcelExporter;
            _tenantManager = tenantManager;
        }

        public async Task<PagedResultDto<GetCompanyProfileForViewDto>> GetAll(GetAllCompanyProfilesInput input)
        {

            var filteredCompanyProfiles = _companyProfileRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CompanyName.Contains(input.Filter) || e.Address1.Contains(input.Filter) || e.Address2.Contains(input.Filter) || e.Phone.Contains(input.Filter) || e.Fax.Contains(input.Filter) || e.City.Contains(input.Filter) || e.State.Contains(input.Filter) || e.ZipCode.Contains(input.Filter) || e.Email.Contains(input.Filter) || e.SLRegNo.Contains(input.Filter) || e.CONTPERSON.Contains(input.Filter) || e.CONTPHONE.Contains(input.Filter) || e.CONTEMAIL.Contains(input.Filter) || e.url.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CompanyNameFilter), e => e.CompanyName.ToLower() == input.CompanyNameFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PhoneFilter), e => e.Phone.ToLower() == input.PhoneFilter.ToLower().Trim());

            var pagedAndFilteredCompanyProfiles = filteredCompanyProfiles
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var companyProfiles = from o in pagedAndFilteredCompanyProfiles
                                  select new GetCompanyProfileForViewDto()
                                  {
                                      CompanyProfile = new CompanyProfileDto
                                      {
                                          CompanyName = o.CompanyName,
                                          Phone = o.Phone,
                                          Id = o.Id,
                                          ReportPath = o.ReportPath
                                      }
                                  };

            var totalCount = await filteredCompanyProfiles.CountAsync();

            return new PagedResultDto<GetCompanyProfileForViewDto>(
                totalCount,
                await companyProfiles.ToListAsync()
            );
        }

        public async Task<GetCompanyProfileForViewDto> GetCompanyProfileForView(string id)
        {

            //    var tennan = _tenantManager.GetByIdAsync((int)AbpSession.TenantId);
            var companyProfile = await _companyProfileRepository.GetAsync(id);

            var output = new GetCompanyProfileForViewDto { CompanyProfile = ObjectMapper.Map<CompanyProfileDto>(companyProfile) };

            return output;
        }

        [AbpAuthorize(AppPermissions.SetupForms_CompanyProfiles_Edit)]
        public async Task<GetCompanyProfileForEditOutput> GetCompanyProfileForEdit(EntityDto<string> input)
        {
            var companyProfile = await _companyProfileRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCompanyProfileForEditOutput { CompanyProfile = ObjectMapper.Map<CreateOrEditCompanyProfileDto>(companyProfile) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCompanyProfileDto input)
        {
            var companyProfile = await _companyProfileRepository.FirstOrDefaultAsync(input.Id);

            if (companyProfile == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.SetupForms_CompanyProfiles_Create)]
        private async Task Create(CreateOrEditCompanyProfileDto input)
        {
            var companyProfile = ObjectMapper.Map<CompanyProfile>(input);


            if (AbpSession.TenantId != null)
            {
                companyProfile.TenantId = (int)AbpSession.TenantId;
            }


            if (companyProfile.Id.IsNullOrWhiteSpace())
            {
                companyProfile.Id = Guid.NewGuid().ToString();
            }

            await _companyProfileRepository.InsertAsync(companyProfile);
        }

        [AbpAuthorize(AppPermissions.SetupForms_CompanyProfiles_Edit)]
        private async Task Update(CreateOrEditCompanyProfileDto input)
        {
            var companyProfile = await _companyProfileRepository.FirstOrDefaultAsync((string)input.Id);
            ObjectMapper.Map(input, companyProfile);
        }

        [AbpAuthorize(AppPermissions.SetupForms_CompanyProfiles_Delete)]
        public async Task Delete(EntityDto<string> input)
        {
            await _companyProfileRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetCompanyProfilesToExcel(GetAllCompanyProfilesForExcelInput input)
        {

            var filteredCompanyProfiles = _companyProfileRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CompanyName.Contains(input.Filter) || e.Address1.Contains(input.Filter) || e.Address2.Contains(input.Filter) || e.Phone.Contains(input.Filter) || e.Fax.Contains(input.Filter) || e.City.Contains(input.Filter) || e.State.Contains(input.Filter) || e.ZipCode.Contains(input.Filter) || e.Email.Contains(input.Filter) || e.SLRegNo.Contains(input.Filter) || e.CONTPERSON.Contains(input.Filter) || e.CONTPHONE.Contains(input.Filter) || e.CONTEMAIL.Contains(input.Filter) || e.url.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CompanyNameFilter), e => e.CompanyName.ToLower() == input.CompanyNameFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PhoneFilter), e => e.Phone.ToLower() == input.PhoneFilter.ToLower().Trim());

            var query = (from o in filteredCompanyProfiles
                         select new GetCompanyProfileForViewDto()
                         {
                             CompanyProfile = new CompanyProfileDto
                             {

                                 CompanyName = o.CompanyName,
                                 Phone = o.Phone,
                                 Id = o.Id
                             }
                         });



            var companyProfileListDtos = await query.ToListAsync();

            return _companyProfilesExcelExporter.ExportToFile(companyProfileListDtos);
        }

        public List<ReportDataForParams> GetReportDataForParams()
        {
            var currentuser = GetCurrentUser();
            
            var reportParamsData = new List<ReportDataForParams>();
            var companyData = _companyProfileRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);
            if (companyData.Count() > 0)
            {
                reportParamsData.Add(new ReportDataForParams()
                {
                    CompanyName = companyData.FirstOrDefault().CompanyName,
                    ReportPath = companyData.FirstOrDefault().ReportPath,
                    Address = companyData.FirstOrDefault().Address1,
                    Address2 = companyData.FirstOrDefault().Address2,
                    Phone = companyData.FirstOrDefault().Phone,
                    SalesTaxRegNo = companyData.FirstOrDefault().SLRegNo,
                    TenantId = AbpSession.TenantId.ToString(),
                    UserName = currentuser.UserName
                });
            }
            else
            {
                reportParamsData.Add(new ReportDataForParams()
                {
                    CompanyName = "",
                    ReportPath = "",
                    Address = "",
                    Address2 = "",
                    Phone = "",
                    SalesTaxRegNo = "",
                    TenantId = ""
                });
            }

            return reportParamsData;
        }
    }

}