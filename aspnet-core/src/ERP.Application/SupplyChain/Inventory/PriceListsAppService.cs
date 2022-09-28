

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.SupplyChain.Inventory.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ERP.Authorization.Users;
using ERP.SupplyChain.Inventory.Exporting;

namespace ERP.SupplyChain.Inventory
{
    [AbpAuthorize(AppPermissions.Inventory_PriceLists)]
    public class PriceListsAppService : ERPAppServiceBase, IPriceListsAppService
    {
        private readonly IRepository<PriceLists> _priceListRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IPriceListExcelExporter _IPriceListExcelExporter;
        public PriceListsAppService(
            IRepository<PriceLists> priceListRepository,
            IRepository<User, long> userRepository,
            IPriceListExcelExporter IPriceListExcelExporter
            )
        {
            _priceListRepository = priceListRepository;
            _userRepository = userRepository;
            _IPriceListExcelExporter = IPriceListExcelExporter;
        }

        public async Task<PagedResultDto<GetPriceListForViewDto>> GetAll(GetAllPriceListsInput input)
        {

            var filteredPriceLists = _priceListRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.PriceList.Contains(input.Filter) || e.PriceListName.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PriceListFilter), e => e.PriceList.ToLower() == input.PriceListFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PriceListNameFilter), e => e.PriceListName.ToLower() == input.PriceListNameFilter.ToLower().Trim())
                        .WhereIf(input.MinActiveFilter != null, e => e.Active >= input.MinActiveFilter)
                        .WhereIf(input.MaxActiveFilter != null, e => e.Active <= input.MaxActiveFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser.ToLower() == input.AudtUserFilter.ToLower().Trim())
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy.ToLower() == input.CreatedByFilter.ToLower().Trim())
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter)
                        .Where(o => o.TenantId == AbpSession.TenantId); 

            var pagedAndFilteredPriceLists = filteredPriceLists
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var priceLists = from o in pagedAndFilteredPriceLists
                             select new GetPriceListForViewDto()
                             {
                                 PriceList = new PriceListDto
                                 {
                                     PriceList = o.PriceList,
                                     PriceListName = o.PriceListName,
                                     Active = o.Active,
                                     AudtUser = o.AudtUser,
                                     AudtDate = o.AudtDate,
                                     CreatedBy = o.CreatedBy,
                                     CreateDate = o.CreateDate,
                                     Id = o.Id
                                 }
                             };

            var totalCount = await filteredPriceLists.CountAsync();

            return new PagedResultDto<GetPriceListForViewDto>(
                totalCount,
                await priceLists.ToListAsync()
            );
        }



        public async Task<PagedResultDto<GetPriceListForViewDto>> GetActiveListOfPriceList(GetAllPriceListsInput input)
        {

            var filteredPriceLists = _priceListRepository.GetAll()
                        .Where(o => o.TenantId == AbpSession.TenantId && o.Active == 1);

            var pagedAndFilteredPriceLists = filteredPriceLists
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var priceLists = from o in pagedAndFilteredPriceLists
                             select new GetPriceListForViewDto()
                             {
                                 PriceList = new PriceListDto
                                 {
                                     PriceList = o.PriceList,
                                     PriceListName = o.PriceListName,
                                     Active = o.Active,
                                     AudtUser = o.AudtUser,
                                     AudtDate = o.AudtDate,
                                     CreatedBy = o.CreatedBy,
                                     CreateDate = o.CreateDate,
                                     Id = o.Id
                                 }
                             };

            var totalCount = await filteredPriceLists.CountAsync();

            return new PagedResultDto<GetPriceListForViewDto>(
                totalCount,
                await priceLists.ToListAsync()
            );
        }

        public async Task<GetPriceListForViewDto> GetPriceListForView(int id)
        {
            var priceList = await _priceListRepository.GetAsync(id);

            var output = new GetPriceListForViewDto { PriceList = ObjectMapper.Map<PriceListDto>(priceList) };

            return output;
        }

        public bool GetPriceListStatusIfExists(PriceListChkDto priceList)
        {
            var priceListItem = _priceListRepository.GetAll().Where(o => o.PriceList == priceList.PriceListChk && o.TenantId == AbpSession.TenantId).SingleOrDefault();

            return priceListItem == null ? false : true;
        }

        [AbpAuthorize(AppPermissions.Inventory_PriceLists_Edit)]
        public async Task<GetPriceListForEditOutput> GetPriceListForEdit(EntityDto input)
        {
            var priceList = await _priceListRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetPriceListForEditOutput { PriceList = ObjectMapper.Map<CreateOrEditPriceListDto>(priceList) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditPriceListDto input)
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

        [AbpAuthorize(AppPermissions.Inventory_PriceLists_Create)]
        protected virtual async Task Create(CreateOrEditPriceListDto input)
        {
            var priceList = ObjectMapper.Map<PriceLists>(input);


            if (AbpSession.TenantId != null)
            {
                priceList.TenantId = (int?)AbpSession.TenantId;
            }

            priceList.AudtUser = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            priceList.CreatedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            priceList.AudtDate = DateTime.Now;
            priceList.CreateDate = DateTime.Now;
            priceList.Active = 1;
            await _priceListRepository.InsertAsync(priceList);
        }

        [AbpAuthorize(AppPermissions.Inventory_PriceLists_Edit)]
        protected virtual async Task Update(CreateOrEditPriceListDto input)
        {
            var priceList = await _priceListRepository.FirstOrDefaultAsync((int)input.Id);
            input.AudtUser = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            input.AudtDate = DateTime.Now;
            input.CreatedBy = priceList.CreatedBy;
            input.CreateDate = priceList.CreateDate;
            ObjectMapper.Map(input, priceList);
        }

        [AbpAuthorize(AppPermissions.Inventory_PriceLists_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _priceListRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetDataToExcel(GetAllPriceListsInput input)
        {

            var filteredPriceLists = _priceListRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.PriceList.Contains(input.Filter) || e.PriceListName.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PriceListFilter), e => e.PriceList.ToLower() == input.PriceListFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PriceListNameFilter), e => e.PriceListName.ToLower() == input.PriceListNameFilter.ToLower().Trim())
                        .WhereIf(input.MinActiveFilter != null, e => e.Active >= input.MinActiveFilter)
                        .WhereIf(input.MaxActiveFilter != null, e => e.Active <= input.MaxActiveFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser.ToLower() == input.AudtUserFilter.ToLower().Trim())
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy.ToLower() == input.CreatedByFilter.ToLower().Trim())
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);


            var query = from o in filteredPriceLists
                        select new GetPriceListForViewDto()
                        {
                            PriceList = new PriceListDto
                            {
                                PriceList = o.PriceList,
                                PriceListName = o.PriceListName,
                                Active = o.Active,
                                AudtUser = o.AudtUser,
                                AudtDate = o.AudtDate,
                                CreatedBy = o.CreatedBy,
                                CreateDate = o.CreateDate,
                                Id = o.Id
                            }
                        };


            var dataDto = await query.ToListAsync();

            return _IPriceListExcelExporter.ExportToFile(dataDto);
        }
    }
}