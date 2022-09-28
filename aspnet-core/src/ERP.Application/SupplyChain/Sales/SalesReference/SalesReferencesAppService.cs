

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.SupplyChain.Sales.SalesReference.Exporting;
using ERP.SupplyChain.Sales.SalesReference.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ERP.SupplyChain.Sales.SalesReference
{
    [AbpAuthorize(AppPermissions.Sales_SalesReferences)]
    public class SalesReferencesAppService : ERPAppServiceBase, ISalesReferencesAppService
    {
        private readonly IRepository<SalesReference> _salesReferenceRepository;
        private readonly ISalesReferencesExcelExporter _salesReferencesExcelExporter;


        public SalesReferencesAppService(IRepository<SalesReference> salesReferenceRepository, ISalesReferencesExcelExporter salesReferencesExcelExporter)
        {
            _salesReferenceRepository = salesReferenceRepository;
            _salesReferencesExcelExporter = salesReferencesExcelExporter;

        }

        public async Task<PagedResultDto<GetSalesReferenceForViewDto>> GetAll(GetAllSalesReferencesInput input)
        {

            var filteredSalesReferences = _salesReferenceRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.RefName.Contains(input.Filter) || e.AUDTUSER.Contains(input.Filter) || e.CreatedUSER.Contains(input.Filter))
                        .WhereIf(input.MinRefIDFilter != null, e => e.RefID >= input.MinRefIDFilter)
                        .WhereIf(input.MaxRefIDFilter != null, e => e.RefID <= input.MaxRefIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RefNameFilter), e => e.RefName == input.RefNameFilter)
                        .WhereIf(input.ACTIVEFilter > -1, e => (input.ACTIVEFilter == 1 && e.ACTIVE) || (input.ACTIVEFilter == 0 && !e.ACTIVE))
                        .WhereIf(input.MinAUDTDATEFilter != null, e => e.AUDTDATE >= input.MinAUDTDATEFilter)
                        .WhereIf(input.MaxAUDTDATEFilter != null, e => e.AUDTDATE <= input.MaxAUDTDATEFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AUDTUSERFilter), e => e.AUDTUSER == input.AUDTUSERFilter)
                        .WhereIf(input.MinCreatedDATEFilter != null, e => e.CreatedDATE >= input.MinCreatedDATEFilter)
                        .WhereIf(input.MaxCreatedDATEFilter != null, e => e.CreatedDATE <= input.MaxCreatedDATEFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedUSERFilter), e => e.CreatedUSER == input.CreatedUSERFilter);
                        //.WhereIf(!string.IsNullOrWhiteSpace(input.RefType), e => e.RefType == input.RefType);

            var pagedAndFilteredSalesReferences = filteredSalesReferences
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var salesReferences = from o in pagedAndFilteredSalesReferences
                                  select new GetSalesReferenceForViewDto()
                                  {
                                      SalesReference = new SalesReferenceDto
                                      {
                                         // RefType = o.RefType,
                                          RefID = o.RefID,
                                          RefName = o.RefName,
                                          ACTIVE = o.ACTIVE,
                                          AUDTDATE = o.AUDTDATE,
                                          AUDTUSER = o.AUDTUSER,
                                          CreatedDATE = o.CreatedDATE,
                                          CreatedUSER = o.CreatedUSER,
                                          Id = o.Id
                                      }
                                  };

            var totalCount = await filteredSalesReferences.CountAsync();

            return new PagedResultDto<GetSalesReferenceForViewDto>(
                totalCount,
                await salesReferences.ToListAsync()
            );
        }

        public async Task<GetSalesReferenceForViewDto> GetSalesReferenceForView(int id)
        {
            var salesReference = await _salesReferenceRepository.GetAsync(id);

            var output = new GetSalesReferenceForViewDto { SalesReference = ObjectMapper.Map<SalesReferenceDto>(salesReference) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Sales_SalesReferences_Edit)]
        public async Task<GetSalesReferenceForEditOutput> GetSalesReferenceForEdit(EntityDto input)
        {
            var salesReference = await _salesReferenceRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetSalesReferenceForEditOutput { SalesReference = ObjectMapper.Map<CreateOrEditSalesReferenceDto>(salesReference) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditSalesReferenceDto input)
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

        [AbpAuthorize(AppPermissions.Sales_SalesReferences_Create)]
        protected virtual async Task Create(CreateOrEditSalesReferenceDto input)
        {
            var salesReference = ObjectMapper.Map<SalesReference>(input);


            if (AbpSession.TenantId != null)
            {
                salesReference.TenantId = (int)AbpSession.TenantId;
            }

            salesReference.RefID = GetMaxReferenceID();
            await _salesReferenceRepository.InsertAsync(salesReference);
        }

        [AbpAuthorize(AppPermissions.Sales_SalesReferences_Edit)]
        protected virtual async Task Update(CreateOrEditSalesReferenceDto input)
        {
            var salesReference = await _salesReferenceRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, salesReference);
        }

        [AbpAuthorize(AppPermissions.Sales_SalesReferences_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _salesReferenceRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetSalesReferencesToExcel(GetAllSalesReferencesForExcelInput input)
        {

            var filteredSalesReferences = _salesReferenceRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.RefName.Contains(input.Filter) || e.AUDTUSER.Contains(input.Filter) || e.CreatedUSER.Contains(input.Filter))
                        .WhereIf(input.MinRefIDFilter != null, e => e.RefID >= input.MinRefIDFilter)
                        .WhereIf(input.MaxRefIDFilter != null, e => e.RefID <= input.MaxRefIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RefNameFilter), e => e.RefName == input.RefNameFilter)
                        .WhereIf(input.ACTIVEFilter > -1, e => (input.ACTIVEFilter == 1 && e.ACTIVE) || (input.ACTIVEFilter == 0 && !e.ACTIVE))
                        .WhereIf(input.MinAUDTDATEFilter != null, e => e.AUDTDATE >= input.MinAUDTDATEFilter)
                        .WhereIf(input.MaxAUDTDATEFilter != null, e => e.AUDTDATE <= input.MaxAUDTDATEFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AUDTUSERFilter), e => e.AUDTUSER == input.AUDTUSERFilter)
                        .WhereIf(input.MinCreatedDATEFilter != null, e => e.CreatedDATE >= input.MinCreatedDATEFilter)
                        .WhereIf(input.MaxCreatedDATEFilter != null, e => e.CreatedDATE <= input.MaxCreatedDATEFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedUSERFilter), e => e.CreatedUSER == input.CreatedUSERFilter);
                           // .WhereIf(!string.IsNullOrWhiteSpace(input.RefType), e => e.RefType == input.RefType);

            var query = (from o in filteredSalesReferences
                         select new GetSalesReferenceForViewDto()
                         {
                             SalesReference = new SalesReferenceDto
                             {
                                 RefID = o.RefID,
                                 RefName = o.RefName,
                                 ACTIVE = o.ACTIVE,
                                 AUDTDATE = o.AUDTDATE,
                                 AUDTUSER = o.AUDTUSER,
                                 CreatedDATE = o.CreatedDATE,
                                 CreatedUSER = o.CreatedUSER,
                                 Id = o.Id
                             }
                         });


            var salesReferenceListDtos = await query.ToListAsync();

            return _salesReferencesExcelExporter.ExportToFile(salesReferenceListDtos);
        }

        public int GetMaxReferenceID()
        {
            var maxid = ((from tab1 in _salesReferenceRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId) select (int?)tab1.RefID).Max() ?? 0) + 1;
            return maxid;
        }


    }
}