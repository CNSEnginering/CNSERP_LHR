

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.GeneralLedger.SetupForms.GLPLCategory.Exporting;
using ERP.GeneralLedger.SetupForms.GLPLCategory.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ERP.GeneralLedger.SetupForms.GLPLCategory
{
    [AbpAuthorize(AppPermissions.Pages_PLCategories)]
    public class PLCategoriesAppService : ERPAppServiceBase, IPLCategoriesAppService
    {
        private readonly IRepository<PLCategory> _plCategoryRepository;
        private readonly IPLCategoriesExcelExporter _plCategoriesExcelExporter;


        public PLCategoriesAppService(IRepository<PLCategory> plCategoryRepository, IPLCategoriesExcelExporter plCategoriesExcelExporter)
        {
            _plCategoryRepository = plCategoryRepository;
            _plCategoriesExcelExporter = plCategoriesExcelExporter;

        }

        public async Task<PagedResultDto<GetPLCategoryForViewDto>> GetAll(GetAllPLCategoriesInput input)
        {

            var filteredPLCategories = _plCategoryRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.TypeID.Contains(input.Filter) || e.HeadingText.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TypeIDFilter), e => e.TypeID == input.TypeIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HeadingTextFilter), e => e.HeadingText == input.HeadingTextFilter)
                        .WhereIf(input.MinSortOrderFilter != null, e => e.SortOrder >= input.MinSortOrderFilter)
                        .WhereIf(input.MaxSortOrderFilter != null, e => e.SortOrder <= input.MaxSortOrderFilter);

            var pagedAndFilteredPLCategories = filteredPLCategories
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var plCategories = from o in pagedAndFilteredPLCategories
                               select new GetPLCategoryForViewDto()
                               {
                                   PLCategory = new PLCategoryDto
                                   {
                                       TypeID = o.TypeID,
                                       HeadingText = o.HeadingText,
                                       SortOrder = o.SortOrder,
                                       Id = o.Id
                                   }
                               };

            var totalCount = await filteredPLCategories.CountAsync();

            return new PagedResultDto<GetPLCategoryForViewDto>(
                totalCount,
                await plCategories.ToListAsync()
            );
        }

        public async Task<GetPLCategoryForViewDto> GetPLCategoryForView(int id)
        {
            var plCategory = await _plCategoryRepository.GetAsync(id);

            var output = new GetPLCategoryForViewDto { PLCategory = ObjectMapper.Map<PLCategoryDto>(plCategory) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_PLCategories_Edit)]
        public async Task<GetPLCategoryForEditOutput> GetPLCategoryForEdit(EntityDto input)
        {
            var plCategory = await _plCategoryRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetPLCategoryForEditOutput { PLCategory = ObjectMapper.Map<CreateOrEditPLCategoryDto>(plCategory) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditPLCategoryDto input)
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

        [AbpAuthorize(AppPermissions.Pages_PLCategories_Create)]
        protected virtual async Task Create(CreateOrEditPLCategoryDto input)
        {
            var plCategory = ObjectMapper.Map<PLCategory>(input);


            if (AbpSession.TenantId != null)
            {
                plCategory.TenantId = (int)AbpSession.TenantId;
            }


            await _plCategoryRepository.InsertAsync(plCategory);
        }

        [AbpAuthorize(AppPermissions.Pages_PLCategories_Edit)]
        protected virtual async Task Update(CreateOrEditPLCategoryDto input)
        {
            var plCategory = await _plCategoryRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, plCategory);
        }

        [AbpAuthorize(AppPermissions.Pages_PLCategories_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _plCategoryRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetPLCategoriesToExcel(GetAllPLCategoriesForExcelInput input)
        {

            var filteredPLCategories = _plCategoryRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.TypeID.Contains(input.Filter) || e.HeadingText.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TypeIDFilter), e => e.TypeID == input.TypeIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HeadingTextFilter), e => e.HeadingText == input.HeadingTextFilter)
                        .WhereIf(input.MinSortOrderFilter != null, e => e.SortOrder >= input.MinSortOrderFilter)
                        .WhereIf(input.MaxSortOrderFilter != null, e => e.SortOrder <= input.MaxSortOrderFilter);

            var query = (from o in filteredPLCategories
                         select new GetPLCategoryForViewDto()
                         {
                             PLCategory = new PLCategoryDto
                             {

                                 TypeID = o.TypeID,
                                 HeadingText = o.HeadingText,
                                 SortOrder = o.SortOrder,
                                 Id = o.Id
                             }
                         });


            var plCategoryListDtos = await query.ToListAsync();

            return _plCategoriesExcelExporter.ExportToFile(plCategoryListDtos);
        }

        public async Task<ListResultDto<PLCategoryComboboxItemDto>> getCategoryList(string input)
        {

            var filteredPLCategories = _plCategoryRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId)
                .WhereIf(!string.IsNullOrWhiteSpace(input), e => e.TypeID.ToLower() == input.ToLower());

            var query = (from o in filteredPLCategories
                         select new PLCategoryComboboxItemDto
                         {
                             SortOrder = (int?)o.SortOrder,
                             DisplayText = o.HeadingText,
                             Id = o.Id
                         });


            var plCategoryListDtos = await query.ToListAsync();
            plCategoryListDtos.Add(new PLCategoryComboboxItemDto
            {
                SortOrder = 0,
                DisplayText = "Please Select",
                Id = -1,
                IsSelected = true
            });
            plCategoryListDtos.Reverse();
            return new ListResultDto<PLCategoryComboboxItemDto>(
                plCategoryListDtos
                );
        }


    }
}