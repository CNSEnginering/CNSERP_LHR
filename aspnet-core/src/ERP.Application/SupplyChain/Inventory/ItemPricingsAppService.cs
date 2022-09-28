

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
using ERP.SupplyChain.Inventory.IC_Item;
using ERP.Authorization.Users;
using ERP.SupplyChain.Inventory.Exporting;

namespace ERP.SupplyChain.Inventory
{
    [AbpAuthorize(AppPermissions.Inventory_ItemPricings)]
    public class ItemPricingsAppService : ERPAppServiceBase
    //, IItemPricingsAppService
    {
        private readonly IRepository<PriceLists> _priceListRepository;
        private readonly IRepository<ItemPricing> _itemPricingRepository;
        private readonly IRepository<PriceListsM> _itemPricingMRepository; private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<ICItem> _itemRepository;
        private readonly IItemPricingExcelExporter _IItemPricingExcelExporter;
        public ItemPricingsAppService(IRepository<ItemPricing> itemPricingRepository,
            IRepository<User, long> userRepository,
            IRepository<ICItem> itemRepository,
            IItemPricingExcelExporter IItemPricingExcelExporter,
            IRepository<PriceLists> priceListRepository,
            IRepository<PriceListsM> itemPricingMRepository
            )
        {
            _itemPricingRepository = itemPricingRepository;
            _userRepository = userRepository;
            _itemRepository = itemRepository;
            _IItemPricingExcelExporter = IItemPricingExcelExporter;
            _priceListRepository = priceListRepository;
            _itemPricingMRepository = itemPricingMRepository;
        }

        public async Task<PagedResultDto<GetItemPricingForViewDto>> GetAll(GetAllItemPricingsInput input)
        {
            var filteredItemPricings = _itemPricingMRepository.GetAll().WhereIf(!string.IsNullOrEmpty(input.Filter), o => o.PriceList.ToLower().Contains(input.Filter)
            && o.TenantId == AbpSession.TenantId
            );
            //var itemData = await _itemRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId).ToListAsync();

            //var filteredItemPricings = _itemPricingRepository.GetAll()
            //            .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.PriceList.Contains(input.Filter) || e.ItemID.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
            //            //.WhereIf(!string.IsNullOrWhiteSpace(input.PriceListFilter),  e => e.PriceList == input.PriceListFilter)
            //            //.WhereIf(!string.IsNullOrWhiteSpace(input.ItemIDFilter),  e => e.ItemID == input.ItemIDFilter)
            //            //.WhereIf(!string.IsNullOrWhiteSpace(input.priceTypeFilter),  e => e.priceType == input.priceTypeFilter)
            //            .WhereIf(input.MinPriceFilter != null, e => e.Price >= input.MinPriceFilter)
            //            .WhereIf(input.MaxPriceFilter != null, e => e.Price <= input.MaxPriceFilter)
            //            .WhereIf(input.MinDiscValueFilter != null, e => e.DiscValue >= input.MinDiscValueFilter)
            //            .WhereIf(input.MaxDiscValueFilter != null, e => e.DiscValue <= input.MaxDiscValueFilter)
            //            .WhereIf(input.MinNetPriceFilter != null, e => e.NetPrice >= input.MinNetPriceFilter)
            //            .WhereIf(input.MaxNetPriceFilter != null, e => e.NetPrice <= input.MaxNetPriceFilter)
            //            .WhereIf(input.MinActiveFilter != null, e => e.Active >= input.MinActiveFilter)
            //            .WhereIf(input.MaxActiveFilter != null, e => e.Active <= input.MaxActiveFilter)
            //            .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
            //            .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
            //            .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
            //            .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
            //            .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
            //            .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter)
            //            .Where(o => o.TenantId == AbpSession.TenantId);

            var pagedAndFilteredItemPricings = filteredItemPricings
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var itemPricings = from o in pagedAndFilteredItemPricings
                               select new GetItemPricingForViewDto()
                               {
                                   ItemPricing = new ItemPricingDto
                                   {
                                       PriceList = o.PriceList,
                                       // ItemID = o.ItemID,
                                       // ItemName = _itemRepository.GetAll().Where(p => p.ItemId == o.ItemID).Count() > 0 ?
                                       //_itemRepository.GetAll().Where(p => p.ItemId == o.ItemID).FirstOrDefault().Descp : "",
                                       //priceType = o.priceType,
                                       //Price = o.Price,
                                       //DiscValue = o.DiscValue,
                                       //NetPrice = o.NetPrice,
                                       DocDate = o.DocDate.Date.Year.ToString() + "-" + o.DocDate.Month.ToString() + "-" + o.DocDate.Day.ToString(),
                                       Active = o.Active,
                                       AudtUser = o.AudtUser,
                                       AudtDate = o.AudtDate,
                                       CreatedBy = o.CreatedBy,
                                       CreateDate = o.CreateDate,
                                       Id = o.Id
                                   }
                               };

            var totalCount = await filteredItemPricings.CountAsync();

            return new PagedResultDto<GetItemPricingForViewDto>(
                totalCount,
                await itemPricings.ToListAsync()
            );
        }

        public async Task<GetItemPricingForViewDto> GetItemPricingForView(int id)
        {
            var itemPricing = await _itemPricingRepository.GetAsync(id);

            var output = new GetItemPricingForViewDto { ItemPricing = ObjectMapper.Map<ItemPricingDto>(itemPricing) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Inventory_ItemPricings_Edit)]
        public async Task<List<GetItemPricingForEditOutput>> GetItemPricingForEdit(EntityDto<string> input)
        {
            var itemPricing = await _itemPricingRepository.GetAll().Where(o => o.PriceList == input.Id).ToListAsync();
            var outputList = new List<GetItemPricingForEditOutput>();
            foreach (var data in itemPricing)
            {
                var item = await _itemRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId).ToListAsync();
                var output = new GetItemPricingForEditOutput { ItemPricing = ObjectMapper.Map<CreateOrEditItemPricingDto>(data) };
                output.ItemPricing.ItemID = _itemRepository.GetAll().Where(o => o.ItemId == output.ItemPricing.ItemID).Count() > 0 ?
                    _itemRepository.GetAll().Where(o => o.ItemId == output.ItemPricing.ItemID).FirstOrDefault().ItemId
                    + "*" + _itemRepository.GetAll().Where(o => o.ItemId == output.ItemPricing.ItemID).FirstOrDefault().Descp + "*" +
                    _itemRepository.GetAll().Where(o => o.ItemId == output.ItemPricing.ItemID).FirstOrDefault().StockUnit + "*"
                    + _itemRepository.GetAll().Where(o => o.ItemId == output.ItemPricing.ItemID).FirstOrDefault().Conver
                    : "";
                //output.ItemPricing.PriceListDesc = _priceListRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.PriceList == itemPricing.PriceList).Count() > 0 ?
                //    _priceListRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.PriceList == itemPricing.PriceList).FirstOrDefault().PriceListName : "";
                output.ItemPricing.DocDate = _itemPricingMRepository.GetAll().Where(o => o.PriceList == input.Id).FirstOrDefault().DocDate.ToShortDateString();
                output.ItemPricing.Active = _itemPricingMRepository.GetAll().Where(o => o.PriceList == input.Id).FirstOrDefault().Active == 1 ? true : false;


                //output.ItemPricing.ItemDate = output.ItemPricing.ItemDate.ToString("dd/mm/yyyy");

                outputList.Add(output);
            }

            return outputList;
        }

        public async Task CreateOrEdit(List<CreateOrEditItemPricingDto> input)
        {
            if (input.FirstOrDefault().Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Inventory_ItemPricings_Create)]
        //protected virtual async Task Create(List<CreateOrEditItemPricingDto> input)
        //{
        //    foreach (var data in input)
        //    {
        //        PriceListsM priceListsM = new PriceListsM();
        //        if (data.Active)
        //        {
        //            priceListsM.Active = 1;
        //        }
        //        else
        //        {
        //            priceListsM.Active = 0;
        //        }
        //        priceListsM.DocDate = Convert.ToDateTime(data.DocDate);
        //        priceListsM.PriceList = data.PriceList;
        //        if (AbpSession.TenantId != null)
        //        {
        //            priceListsM.TenantId = (int)AbpSession.TenantId;
        //        }
        //        priceListsM.AudtUser = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
        //        priceListsM.CreatedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
        //        priceListsM.AudtDate = DateTime.Now;
        //        priceListsM.CreateDate = DateTime.Now;
        //        await _itemPricingMRepository.InsertAsync(priceListsM);
        //        break;
        //    }
        //    foreach (var data in input)
        //    {

        //        //data.ItemDate = Convert.ToDateTime(data.ItemDate.ToString('dd-MMM-yyyy'));


        //        //data.ItemDate = Convert.ToDateTime(DateTime.ParseExact(data.ItemDate, "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentCulture));

        //        var itemPricing = ObjectMapper.Map<ItemPricing>(data);

        //        itemPricing.AudtUser = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
        //        itemPricing.CreatedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
        //        itemPricing.AudtDate = DateTime.Now;
        //        itemPricing.CreateDate = DateTime.Now;
        //        //string s = data.ItemDate;
        //        //var date = DateTime.ParseExact(s, "YYYY-MM-DD hh:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture);
        //        //itemPricing.ItemDate = date;

        //        //itemPricing.ItemDate = Convert.ToDateTime(DateTime.ParseExact(data.ItemDate, "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentCulture));


        //        if (AbpSession.TenantId != null)
        //        {
        //            itemPricing.TenantId = (int)AbpSession.TenantId;
        //        }

        //        await _itemPricingRepository.InsertAsync(itemPricing);
        //    }
        //}

        protected virtual async Task Create(List<CreateOrEditItemPricingDto> input)
        {
            foreach (var data in input)
            {
                PriceListsM priceListsM = new PriceListsM();
                if (data.Active)
                {
                    priceListsM.Active = 1;
                }
                else
                {
                    priceListsM.Active = 0;
                }
                priceListsM.DocDate = Convert.ToDateTime(data.DocDate);
                priceListsM.PriceList = data.PriceList;
                if (AbpSession.TenantId != null)
                {
                    priceListsM.TenantId = (int)AbpSession.TenantId;
                }
                priceListsM.AudtUser = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                priceListsM.CreatedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                priceListsM.AudtDate = DateTime.Now;
                priceListsM.CreateDate = DateTime.Now;
                await _itemPricingMRepository.InsertAsync(priceListsM);
                break;
            }
            foreach (var data in input)
            {

                //data.ItemDate = Convert.ToDateTime(data.ItemDate.ToString('dd-MMM-yyyy'));


                //data.ItemDate = Convert.ToDateTime(DateTime.ParseExact(data.ItemDate, "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentCulture));

                var itemPricing = ObjectMapper.Map<ItemPricing>(data);

                itemPricing.AudtUser = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                itemPricing.CreatedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                itemPricing.AudtDate = DateTime.Now;
                itemPricing.CreateDate = DateTime.Now;
                //string s = data.ItemDate;
                //var date = DateTime.ParseExact(s, "YYYY-MM-DD hh:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture);
                //itemPricing.ItemDate = date;

                //itemPricing.ItemDate = Convert.ToDateTime(DateTime.ParseExact(data.ItemDate, "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentCulture));


                if (AbpSession.TenantId != null)
                {
                    itemPricing.TenantId = (int)AbpSession.TenantId;
                }

                await _itemPricingRepository.InsertAsync(itemPricing);
            }
        }
        [AbpAuthorize(AppPermissions.Inventory_ItemPricings_Edit)]
        protected virtual async Task Update(List<CreateOrEditItemPricingDto> input)
        {
            await _itemPricingRepository.DeleteAsync(o => o.PriceList == input[0].PriceList);

            var itemPricingM = await _itemPricingMRepository.FirstOrDefaultAsync(o => o.PriceList == input[0].PriceList);
            //PriceListsM priceListsM = new PriceListsM();
            foreach (var data in input)
            {

                if (data.Active)
                {
                    itemPricingM.Active = 1;
                }
                else
                {
                    itemPricingM.Active = 0;
                }
                itemPricingM.DocDate = Convert.ToDateTime(data.DocDate);
                itemPricingM.PriceList = data.PriceList;
                if (AbpSession.TenantId != null)
                {
                    itemPricingM.TenantId = (int)AbpSession.TenantId;
                }
                // await _itemPricingMRepository.InsertAsync(priceListsM);
                break;
            }
            // ObjectMapper.Map(priceListsM, itemPricingM);
            itemPricingM.AudtUser = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            //priceListsM.CreatedBy = itemPricingM.CreatedBy;
            itemPricingM.AudtDate = DateTime.Now;
            // priceListsM.CreateDate = itemPricingM.CreateDate;
            itemPricingM.Id = itemPricingM.Id;
            await _itemPricingMRepository.UpdateAsync(itemPricingM);
            foreach (var data in input)
            {
                var itemPricing = ObjectMapper.Map<ItemPricing>(data);

                itemPricing.AudtUser = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                itemPricing.CreatedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                itemPricing.AudtDate = DateTime.Now;
                itemPricing.CreateDate = DateTime.Now;

                if (AbpSession.TenantId != null)
                {
                    itemPricing.TenantId = (int)AbpSession.TenantId;
                }

                await _itemPricingRepository.InsertAsync(itemPricing);
            }

        }

        [AbpAuthorize(AppPermissions.Inventory_ItemPricings_Delete)]
        public async Task Delete(EntityDto<string> input)
        {
            await _itemPricingMRepository.DeleteAsync(o => o.PriceList == input.Id.Trim());
            await _itemPricingRepository.DeleteAsync(o => o.PriceList == input.Id.Trim());
        }

        public bool GetItemIdCheckAgainstPriceList(string PriceList, string itemId)
        {
            var data = _itemPricingRepository.GetAll().Where(o => o.ItemID == itemId && o.PriceList == PriceList && o.TenantId == AbpSession.TenantId);
            return data.Count() > 0 ? true : false;
        }

        public async Task<FileDto> GetItemPricingToExcel(GetAllItemPricingsInput input)
        {
            // var itemData = await _itemRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId).ToListAsync();
            var filteredItemPricings = _itemPricingRepository.GetAll()
               .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false ||
               e.PriceList.Contains(input.Filter) || e.ItemID.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
               .WhereIf(input.MinPriceFilter != null, e => e.Price >= input.MinPriceFilter)
               .WhereIf(input.MaxPriceFilter != null, e => e.Price <= input.MaxPriceFilter)
               .WhereIf(input.MinDiscValueFilter != null, e => e.DiscValue >= input.MinDiscValueFilter)
               .WhereIf(input.MaxDiscValueFilter != null, e => e.DiscValue <= input.MaxDiscValueFilter)
               .WhereIf(input.MinNetPriceFilter != null, e => e.NetPrice >= input.MinNetPriceFilter)
               .WhereIf(input.MaxNetPriceFilter != null, e => e.NetPrice <= input.MaxNetPriceFilter)
               .WhereIf(input.MinActiveFilter != null, e => e.Active >= input.MinActiveFilter)
               .WhereIf(input.MaxActiveFilter != null, e => e.Active <= input.MaxActiveFilter)
               .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
               .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
               .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
               .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
               .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
               .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter)
               .Where(o => o.TenantId == AbpSession.TenantId);

            var query = (from o in filteredItemPricings
                         select new GetItemPricingForViewDto()
                         {
                             ItemPricing = new ItemPricingDto
                             {
                                 PriceList = o.PriceList,
                                 ItemID = o.ItemID,
                                 ItemName = _itemRepository.GetAll().Where(p => p.ItemId == o.ItemID).Count() > 0 ?
                                      _itemRepository.GetAll().Where(p => p.ItemId == o.ItemID).FirstOrDefault().Descp : "",
                                 priceType = o.priceType,
                                 Price = o.Price,
                                 DiscValue = o.DiscValue,
                                 NetPrice = o.NetPrice,
                                 PurchasePrice = o.PurchasePrice,
                                 SalePrice = o.SalePrice,
                                 ItemDate = o.ItemDate,
                                 Active = o.Active,
                                 AudtUser = o.AudtUser,
                                 AudtDate = o.AudtDate,
                                 CreatedBy = o.CreatedBy,
                                 CreateDate = o.CreateDate,
                                 Id = o.Id
                             }
                         });


            var dataDto = await query.ToListAsync();

            return _IItemPricingExcelExporter.ExportToFile(dataDto);
        }

        public bool GetCheckPriceListExists(string priceList)
        {
            return _itemPricingMRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.PriceList == priceList).Count() > 0 ? true : false;
        }

        public async Task<List<ItemPricingDto>> GetAllItemsPricing()
        {
            var filteredItemPricings = _itemPricingRepository.GetAll();

            var query = (from o in filteredItemPricings
                         select new ItemPricingDto()
                         {
                                PriceList = o.PriceList,
                                ItemID = o.ItemID,
                                ItemName = _itemRepository.GetAll().Where(p => p.ItemId == o.ItemID).Count() > 0 ?
                                _itemRepository.GetAll().Where(p => p.ItemId == o.ItemID).FirstOrDefault().Descp : "",
                                priceType = o.priceType,
                                Price = o.Price,
                                DiscValue = o.DiscValue,
                                NetPrice = o.NetPrice,
                                PurchasePrice=o.PurchasePrice,
                                SalePrice=o.SalePrice,
                                ItemDate=o.ItemDate,
                                Active = o.Active,
                                AudtUser = o.AudtUser,
                                AudtDate = o.AudtDate,
                                CreatedBy = o.CreatedBy,
                                CreateDate = o.CreateDate,
                                Id = o.Id
                             
                         });

            return await query.ToListAsync();
        }
    }
}