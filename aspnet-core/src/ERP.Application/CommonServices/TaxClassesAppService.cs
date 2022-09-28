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
    [AbpAuthorize(AppPermissions.Pages_TaxClasses)]
    public class TaxClassesAppService : ERPAppServiceBase, ITaxClassesAppService
    {
        private readonly IRepository<TaxClass> _taxClassRepository;
        private readonly ITaxClassesExcelExporter _taxClassesExcelExporter;
        private readonly IRepository<TaxAuthority, string> _lookup_taxAuthorityRepository;
        private readonly IRepository<ChartofControl, string> _chartofControlRepository;


        public TaxClassesAppService(IRepository<TaxClass> taxClassRepository, ITaxClassesExcelExporter taxClassesExcelExporter, IRepository<TaxAuthority, string> lookup_taxAuthorityRepository, IRepository<ChartofControl, string> chartofControlRepository)
        {
            _taxClassRepository = taxClassRepository;
            _taxClassesExcelExporter = taxClassesExcelExporter;
            _lookup_taxAuthorityRepository = lookup_taxAuthorityRepository;
            _chartofControlRepository = chartofControlRepository;

        }

        public async Task<PagedResultDto<GetTaxClassForViewDto>> GetAll(GetAllTaxClassesInput input)
        {

            var filteredTaxClasses = _taxClassRepository.GetAll()
                        //.Include( e => e.TaxAuthorityFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.TAXAUTH.Contains(input.Filter) || e.CLASSDESC.Contains(input.Filter) || e.CLASSTYPE.Contains(input.Filter) || e.AUDTUSER.Contains(input.Filter) || e.TAXACCID.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TAXAUTHFilter), e => e.TAXAUTH.ToLower() == input.TAXAUTHFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CLASSDESCFilter), e => e.CLASSDESC.ToLower() == input.CLASSDESCFilter.ToLower().Trim())
                        .WhereIf(input.MinCLASSRATEFilter != null, e => e.CLASSRATE >= input.MinCLASSRATEFilter)
                        .WhereIf(input.MaxCLASSRATEFilter != null, e => e.CLASSRATE <= input.MaxCLASSRATEFilter)
                        .WhereIf(input.MinTRANSTYPEFilter != null, e => e.TRANSTYPE >= input.MinTRANSTYPEFilter)
                        .WhereIf(input.MaxTRANSTYPEFilter != null, e => e.TRANSTYPE <= input.MaxTRANSTYPEFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CLASSTYPEFilter), e => e.CLASSTYPE.ToLower() == input.CLASSTYPEFilter.ToLower().Trim())
                        .WhereIf(input.MinAUDTDATEFilter != null, e => e.AUDTDATE >= input.MinAUDTDATEFilter)
                        .WhereIf(input.MaxAUDTDATEFilter != null, e => e.AUDTDATE <= input.MaxAUDTDATEFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AUDTUSERFilter), e => e.AUDTUSER.ToLower() == input.AUDTUSERFilter.ToLower().Trim()).Where(o => o.TenantId == AbpSession.TenantId);
            //.WhereIf(!string.IsNullOrWhiteSpace(input.TaxAuthorityTAXAUTHFilter), e => e.TaxAuthorityFk != null && e.TaxAuthorityFk.TAXAUTH.ToLower() == input.TaxAuthorityTAXAUTHFilter.ToLower().Trim());

            var pagedAndFilteredTaxClasses = filteredTaxClasses
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var taxClasses = from o in pagedAndFilteredTaxClasses
                                 //  join o1 in _lookup_taxAuthorityRepository.GetAll() on o.TAXAUTH equals o1.TAXAUTH into j1
                                 //  from s1 in j1.DefaultIfEmpty()

                             select new GetTaxClassForViewDto()
                             {
                                 TaxClass = new TaxClassDto
                                 {
                                     TAXAUTH = o.TAXAUTH,
                                     CLASSDESC = o.CLASSDESC,
                                     CLASSRATE = o.CLASSRATE,
                                     TRANSTYPE = o.TRANSTYPE,
                                     CLASSTYPE = o.CLASSTYPE,
                                     TAXACCID = o.TAXACCID,
                                     TAXACCDESC = _chartofControlRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.Id == o.TAXACCID).Count() > 0 ? _chartofControlRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.Id == o.TAXACCID).SingleOrDefault().AccountName : "",
                                     IsActive = o.IsActive,
                                     AUDTDATE = o.AUDTDATE,
                                     AUDTUSER = o.AUDTUSER,
                                     Id = o.Id
                                 },
                                 //	TaxAuthorityTAXAUTH = s1 == null ? "" : s1.TAXAUTH.ToString()
                             };

            var totalCount = await filteredTaxClasses.CountAsync();

            return new PagedResultDto<GetTaxClassForViewDto>(
                totalCount,
                await taxClasses.ToListAsync()
            );
        }

        public async Task<GetTaxClassForViewDto> GetTaxClassForView(int id)
        {
            var taxClass = await _taxClassRepository.GetAsync(id);

            var output = new GetTaxClassForViewDto { TaxClass = ObjectMapper.Map<TaxClassDto>(taxClass) };

            if (output.TaxClass.TAXAUTH != null)
            {
                var _lookupTaxAuthority = await _lookup_taxAuthorityRepository.FirstOrDefaultAsync((string)output.TaxClass.TAXAUTH);
                output.TaxAuthorityTAXAUTH = _lookupTaxAuthority.Id.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_TaxClasses_Edit)]
        public async Task<GetTaxClassForEditOutput> GetTaxClassForEdit(EntityDto input)
        {
            var taxClass = await _taxClassRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetTaxClassForEditOutput { TaxClass = ObjectMapper.Map<CreateOrEditTaxClassDto>(taxClass) };

            if (output.TaxClass.TAXAUTH != null)
            {
                var _lookupTaxAuthority = await _lookup_taxAuthorityRepository.FirstOrDefaultAsync((string)output.TaxClass.TAXAUTH);
                output.TaxAuthorityTAXAUTH = _lookupTaxAuthority.Id.ToString();
            }
            output.TaxAccDesc = _chartofControlRepository.GetAll().Where(o => o.Id == output.TaxClass.TAXACCID && o.TenantId == AbpSession.TenantId).Count() > 0
                ? _chartofControlRepository.GetAll().Where(o => o.Id == output.TaxClass.TAXACCID && o.TenantId == AbpSession.TenantId).FirstOrDefault().AccountName : "";
            return output;
        }

        public async Task CreateOrEdit(CreateOrEditTaxClassDto input)
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

        [AbpAuthorize(AppPermissions.Pages_TaxClasses_Create)]
        protected virtual async Task Create(CreateOrEditTaxClassDto input)
        {
            var taxClass = ObjectMapper.Map<TaxClass>(input);


            if (AbpSession.TenantId != null)
            {
                taxClass.TenantId = (int)AbpSession.TenantId;
            }


            await _taxClassRepository.InsertAsync(taxClass);
        }

        [AbpAuthorize(AppPermissions.Pages_TaxClasses_Edit)]
        protected virtual async Task Update(CreateOrEditTaxClassDto input)
        {
            var taxClass = await _taxClassRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, taxClass);
        }

        [AbpAuthorize(AppPermissions.Pages_TaxClasses_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _taxClassRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetTaxClassesToExcel(GetAllTaxClassesForExcelInput input)
        {

            var filteredTaxClasses = _taxClassRepository.GetAll()
                        //	.Include( e => e.TaxAuthorityFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.TAXAUTH.Contains(input.Filter) || e.CLASSDESC.Contains(input.Filter) || e.CLASSTYPE.Contains(input.Filter) || e.AUDTUSER.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TAXAUTHFilter), e => e.TAXAUTH.ToLower() == input.TAXAUTHFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CLASSDESCFilter), e => e.CLASSDESC.ToLower() == input.CLASSDESCFilter.ToLower().Trim())
                        .WhereIf(input.MinCLASSRATEFilter != null, e => e.CLASSRATE >= input.MinCLASSRATEFilter)
                        .WhereIf(input.MaxCLASSRATEFilter != null, e => e.CLASSRATE <= input.MaxCLASSRATEFilter)
                        .WhereIf(input.MinTRANSTYPEFilter != null, e => e.TRANSTYPE >= input.MinTRANSTYPEFilter)
                        .WhereIf(input.MaxTRANSTYPEFilter != null, e => e.TRANSTYPE <= input.MaxTRANSTYPEFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CLASSTYPEFilter), e => e.CLASSTYPE.ToLower() == input.CLASSTYPEFilter.ToLower().Trim())
                        .WhereIf(input.MinAUDTDATEFilter != null, e => e.AUDTDATE >= input.MinAUDTDATEFilter)
                        .WhereIf(input.MaxAUDTDATEFilter != null, e => e.AUDTDATE <= input.MaxAUDTDATEFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AUDTUSERFilter), e => e.AUDTUSER.ToLower() == input.AUDTUSERFilter.ToLower().Trim()).Where(o => o.TenantId == AbpSession.TenantId);
            //	.WhereIf(!string.IsNullOrWhiteSpace(input.TaxAuthorityTAXAUTHFilter), e => e.TaxAuthorityFk != null && e.TaxAuthorityFk.TAXAUTH.ToLower() == input.TaxAuthorityTAXAUTHFilter.ToLower().Trim());

            var query = (from o in filteredTaxClasses
                             // join o1 in _lookup_taxAuthorityRepository.GetAll() on o.TAXAUTH equals o1.TAXAUTH into j1
                             //  from s1 in j1.DefaultIfEmpty()

                         select new GetTaxClassForViewDto()
                         {
                             TaxClass = new TaxClassDto
                             {
                                 TAXAUTH = o.TAXAUTH,
                                 CLASSDESC = o.CLASSDESC,
                                 CLASSRATE = o.CLASSRATE,
                                 TRANSTYPE = o.TRANSTYPE,
                                 CLASSTYPE = o.CLASSTYPE,
                                 IsActive = o.IsActive,
                                 AUDTDATE = o.AUDTDATE,
                                 AUDTUSER = o.AUDTUSER,
                                 Id = o.Id
                             },
                             //	TaxAuthorityTAXAUTH = s1 == null ? "" : s1.TAXAUTH.ToString()
                         });


            var taxClassListDtos = await query.ToListAsync();

            return _taxClassesExcelExporter.ExportToFile(taxClassListDtos);
        }



        [AbpAuthorize(AppPermissions.Pages_TaxClasses)]
        public async Task<PagedResultDto<TaxClassTaxAuthorityLookupTableDto>> GetAllTaxAuthorityForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_taxAuthorityRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Id.ToString().Contains(input.Filter)
               ).Where(o => o.TenantId == AbpSession.TenantId);

            var totalCount = await query.CountAsync();

            var taxAuthorityList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<TaxClassTaxAuthorityLookupTableDto>();
            foreach (var taxAuthority in taxAuthorityList)
            {
                lookupTableDtoList.Add(new TaxClassTaxAuthorityLookupTableDto
                {
                    Id = taxAuthority.Id,
                    DisplayName = taxAuthority.TAXAUTHDESC?.ToString()
                });
            }

            return new PagedResultDto<TaxClassTaxAuthorityLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        public async Task<int> GetMaxTaxClassId(string authId)
        {
            var filteredTaxClasses = _taxClassRepository.GetAll().WhereIf(!string.IsNullOrWhiteSpace(authId), e => e.TAXAUTH.ToLower() == authId.ToLower().Trim()).Where(o => o.TenantId == AbpSession.TenantId);
            int maxClassID=0;
            if (filteredTaxClasses.Count()>0)
            {
                maxClassID = await filteredTaxClasses.MaxAsync(x => x.CLASSID);
            }
            return maxClassID;
        }

        public bool GetTaxClassExists(string accId, string taxAuth)
        {
            var flag = _taxClassRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.TAXAUTH == taxAuth && o.TAXACCID == accId).Count();

            return flag > 0 ? true : false;
        }
    }
}