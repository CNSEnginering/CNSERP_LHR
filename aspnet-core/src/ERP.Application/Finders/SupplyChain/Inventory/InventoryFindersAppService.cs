using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using ERP.Finders.Dtos;
using ERP.SupplyChain.Inventory;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.SupplyChain.Inventory.IC_Segment1;
using ERP.SupplyChain.Inventory.IC_Segment2;
using ERP.SupplyChain.Inventory.IC_Segment3;
using ERP.SupplyChain.Inventory.IC_Item;
using ERP.SupplyChain.Inventory.ICOPT5;
using ERP.SupplyChain.Inventory.Consumption;
using ERP.Manufacturing.SetupForms;
using ERP.SupplyChain.Inventory.ICOPT4;
//using ERP.SupplyChain.Sales.OEQH;
//using ERP.SupplyChain.Sales.OECompReg;
//using ERP.SupplyChain.Sales.OECompApr;
//using ERP.SupplyChain.Sales.OECompAss;
//using ERP.SupplyChain.Inventory.ICTerms;
using ERP.SupplyChain.Sales.SaleEntry;
using ERP.SupplyChain.Purchase.ReceiptEntry;
using PayPal.v1.BillingPlans;
using ERP.SupplyChain.Inventory.WorkOrder;
using ERP.CommonServices.UserLoc.CSUserLocD;
using ERP.Authorization.Users;

namespace ERP.Finders.SupplyChain.Inventory
{
    public class InventoryFindersAppService: ERPAppServiceBase
    {
        private readonly IRepository<ICLocation> _icLocationRepository;
        private readonly IRepository<ICUOM> _icuomRepository;
        private readonly IRepository<SubCostCenter> _subCostCenterRepository;
        private readonly IRepository<CostCenter> _costCenterRepository;
        private readonly IRepository<ICItem> _itemRepository;
        private readonly IRepository<PriceLists> _priceListRepository;
        private readonly IRepository<ItemPricing> _itemPricingRepository;
        private readonly IRepository<TransactionType> _transactionTypeRepository;
        private readonly IRepository<ICSegment1> _icSegment1Repository;
        private readonly IRepository<ICSegment2> _icSegment2Repository;
        private readonly IRepository<ICSegment3> _icSegment3Repository;
        private readonly IRepository<ICOPT5> _icopT5Repository;
        private readonly IRepository<ICLEDG> _icledgRepository;
        private readonly IRepository<ICCNSHeader> _icCNSHeaderRepository;
        private readonly IRepository<ICCNSDetail> _icCNSDetailRepository;
        private readonly IRepository<ICOPT4> _icopT4Repository;
        private readonly IRepository<CSUserLocD> _csUserLocDRepository;
        //private readonly IRepository<OEQH> _oeqhRepository;
        //private readonly IRepository<OECompReg> _oeCompRegRepository;
        //private readonly IRepository<OECompApr> _oeCompAprRepository;
        private readonly IRepository<OESALEHeader> _oesaleHeaderRepository;
        private readonly IRepository<PORECHeader> _porecHeaderRepository;
        private readonly IRepository<ICWOHeader> _icwoHeaderRepository;
        private readonly IRepository<User, long> _userRepository;
        //private readonly IRepository<OECompAss> _oeCompAssRepository;
        //private readonly IRepository<Terms> _icTermsRepository;

        public InventoryFindersAppService(
           
            IRepository<ICLocation> icLocationRepository,
            IRepository<ICUOM> icuomRepository,
            IRepository<SubCostCenter> subCostCenterRepository, IRepository<CSUserLocD> csUserLocDRepository,
            IRepository<CostCenter> costCenterRepository,
            IRepository<OESALEHeader> oesaleHeaderRepository,
            IRepository<PORECHeader> porecHeaderRepository,
             IRepository<ICItem> itemRepository,
            IRepository<PriceLists> priceListRepository,
            IRepository<ItemPricing> itemPricingRepository,
            IRepository<TransactionType> transactionTypeRepository,
            IRepository<ICSegment1> icSegment1Repository,
            IRepository<ICSegment2> icSegment2Repository,
            IRepository<ICSegment3> icSegment3Repository,
            IRepository<ICLEDG> icledgRepository,
            IRepository<ICOPT5> icopT5Repository,
            IRepository<ICCNSHeader> icCNSHeaderRepository,
            //IRepository<OECompReg> oeCompRegRepository,
            IRepository<User, long> userRepository,
            //IRepository<OECompAss> oeCompAssRepository,
            //IRepository<OECompApr> oeCompAprRepository,
            IRepository<ICCNSDetail> icCNSDetailRepository, 
            IRepository<ICOPT4> icopT4Repository,
             IRepository<ICWOHeader> icwoHeaderRepository
            // ,
            //IRepository<OEQH> oeqhRepository,
            //IRepository<ICTerms> icTermsRepository
            )
        {
            _icLocationRepository = icLocationRepository;
            _oesaleHeaderRepository = oesaleHeaderRepository;
            _icuomRepository = icuomRepository;
            _subCostCenterRepository= subCostCenterRepository;
            _porecHeaderRepository = porecHeaderRepository;
            _costCenterRepository = costCenterRepository;
            _itemRepository = itemRepository;
            _priceListRepository= priceListRepository;
            _userRepository = userRepository;
            //_oeCompAssRepository = oeCompAssRepository;
            //_oeCompRegRepository = oeCompRegRepository;
            //_oeCompAprRepository = oeCompAprRepository;
            _itemPricingRepository = itemPricingRepository;
            _transactionTypeRepository = transactionTypeRepository;
            _icSegment1Repository= icSegment1Repository;
            _icSegment2Repository= icSegment2Repository;
            _icSegment3Repository= icSegment3Repository;
            _icopT5Repository = icopT5Repository;
            _csUserLocDRepository = csUserLocDRepository;
            _icledgRepository = icledgRepository;
            _icCNSHeaderRepository = icCNSHeaderRepository;
            _icCNSDetailRepository = icCNSDetailRepository;
            _icopT4Repository = icopT4Repository;
            _icwoHeaderRepository = icwoHeaderRepository;
            //_oeqhRepository = oeqhRepository;
            //_icTermsRepository = icTermsRepository;
        }


        public async Task<PagedResultDto<InventoryFindersDto>> GetInventoryLookupTable(GetAllForLookupTableInput input)
        {
            if (!string.IsNullOrEmpty(input.ParamFilter))
                input.ParamFilter = input.ParamFilter.Trim();


            var resultDtos = new LookupDto<InventoryFindersDto>();
            switch (input.Target)
            {
                case "Location":
                    resultDtos = await GetAllICUserLocationForLookupTable(input);
                    break;
                case "ToLocation":
                    resultDtos = await GetAllICLocationForLookupTable(input);
                    break;
                case "UOM":
                    resultDtos = await GetAllICUOMForLookupTable(input);
                    break;
                case "SubCostCenter":
                    resultDtos = await GetAllSubCostCenterForLookupTable(input);
                    break;
                case "CostCenter":
                    resultDtos = await GetAllCostCenterForLookupTable(input);
                    break;
                case "Items":
                    resultDtos = await GetAllItemsForLookupTable(input);
                    break;
                case "ItemsQ":
                    resultDtos = await GetAllItemsQForLookupTable(input);
                    break;
                case "PriceList":
                    resultDtos = await GetAllPriceListForLookupTable(input);
                    break;
                case "ItemPricing":
                    resultDtos = await GetAllItemPricingForLookupTable(input);
                    break;
                case "TransactionType":
                    resultDtos = await GetAllTransactionTypeForLookupTable(input);
                    break;
                case "Segment1":
                    resultDtos = await GetAllSegment1ForLookupTable(input);
                    break;
                case "Segment2":
                    resultDtos = await GetAllSegment2ForLookupTable(input);
                    break;
                case "Segment3":
                    resultDtos = await GetAllSegment3ForLookupTable(input);
                    break;
                case "Consumption":
                    resultDtos = await GetConsumptionLookupTable(input);
                    break;
                case "TransType":
                    resultDtos = await GetAllOpt4ForLookupTable(input);
                    break;
                case "workOrder":
                    resultDtos = await GetAllworkorderForLookupTable(input);
                    break;

                case "InvDocNo":
                    resultDtos = await GetAllinvDocLookupTable(input);
                    break;
                //case "Note Text":
                //case "Pay Terms":
                //case "Delievery Terms":
                //case "Validity Terms":
                //    resultDtos = await GetAllICTerms(input);
                //    break;
                    

            }
            return new PagedResultDto<InventoryFindersDto>(
                    resultDtos.Count,
                    resultDtos.Items
                );
        }

        private async Task<LookupDto<InventoryFindersDto>> GetAllinvDocLookupTable(GetAllForLookupTableInput input)
        {
            IQueryable<InventoryFindersDto> lookupTableDto;
            IQueryable<OESALEHeader> saleDoc;
            IQueryable<PORECHeader> PorDoc;
            if (input.ParamFilter=="BR" || input.ParamFilter=="CR")
            {
                
                saleDoc = _oesaleHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);
                lookupTableDto = from o in saleDoc
                                 select new InventoryFindersDto
                                 {
                                     Id = o.DocNo.ToString(),

                                 };
                if (!string.IsNullOrEmpty(input.Filter))
                {
                    lookupTableDto = lookupTableDto.Where(c => c.Id == input.Filter);
                }
                lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
                var pageData = lookupTableDto.PageBy(input);
                var lookupTableDtoList = await pageData.ToListAsync();
                var getData = new LookupDto<InventoryFindersDto>(
                    lookupTableDto.Count(),
                    lookupTableDtoList
                );
                return getData;
            }
            else
            {
                PorDoc = _porecHeaderRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);
                lookupTableDto = from o in PorDoc
                                 select new InventoryFindersDto
                                 {
                                     Id = o.DocNo.ToString(),

                                 };
                if (!string.IsNullOrEmpty(input.Filter))
                {
                    lookupTableDto = lookupTableDto.Where(c => c.Id == input.Filter);
                }
                lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
                var pageData = lookupTableDto.PageBy(input);
                var lookupTableDtoList = await pageData.ToListAsync();
                var getData = new LookupDto<InventoryFindersDto>(
                    lookupTableDto.Count(),
                    lookupTableDtoList
                );
                return getData;
            }
        }

        private async Task<LookupDto<InventoryFindersDto>> GetAllworkorderForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _icwoHeaderRepository.GetAll().Where(c => c.TenantId == AbpSession.TenantId && c.Approved == true).WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Narration.Contains(input.Filter));//.WhereIf(input.Filter != null, e => e.DocNo >=Convert.ToInt32(input.Filter));

            var lookupTableDto = from o in query
                                 select new InventoryFindersDto
                                 {
                                     Id = o.DocNo.ToString(),
                                     DisplayName = o.Narration,
                                     Unit = o.CCID,
                                     Option5 = o.LocID.ToString(),
                                     ManfDate = _costCenterRepository.GetAll().Where(v => v.CCID == o.CCID && v.TenantId == AbpSession.TenantId).FirstOrDefault().CCName
                                 };
            lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<InventoryFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }

        private async Task<LookupDto<InventoryFindersDto>> GetAllOpt4ForLookupTable(GetAllForLookupTableInput input)
        {
            var opt4 = _icopT4Repository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);
           
            IQueryable<InventoryFindersDto> lookupTableDto;
            lookupTableDto = from o in opt4
                             select new InventoryFindersDto
                             {
                                 Id = o.OptID.ToString(),
                                 DisplayName = o.Descp,
                             };


            lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<InventoryFindersDto>(
                lookupTableDto.Count(),
                lookupTableDtoList
            );

            return getData;
        }
      
        private async Task<LookupDto<InventoryFindersDto>> GetAllItemsForLookupTable(GetAllForLookupTableInput input)
        {
            var items = _itemRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.ItemStatus == 0);
            var option5 = _icopT5Repository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);

            //var query = _itemRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.ItemStatus == 0).Join(_icopT5Repository.GetAll().Where(o => o.TenantId == AbpSession.TenantId).DefaultIfEmpty(), x=>x.Opt5,y=>y.OptID,(x,y)=>new {x.TenantId,x.ItemStatus, x.ItemId,x.Descp,x.StockUnit,x.Conver,x.Opt5,OptDesc=y.Descp!=null?y.Descp:""})
            //    .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
            //    e => e.ItemId.ToString().ToUpper().Contains(input.Filter.ToUpper()) || e.Descp.ToUpper().Contains(input.Filter.ToUpper()) || (e.OptDesc != null ? e.OptDesc : "").Trim().ToUpper().Contains(input.Filter.Trim().ToUpper()));

            IQueryable<InventoryFindersDto> lookupTableDto;
            if(input.Filter != null)
            {
                lookupTableDto = from o in items
                                 join op in option5 on o.Opt5 equals op.OptID into opp
                                 from opt in opp.DefaultIfEmpty()
                                 where (o.ItemId.ToString().Contains(input.Filter.ToUpper())
                                 || (o.Descp).ToUpper().Contains(input.Filter.ToUpper())
                                 || (opt.Descp != null ? opt.Descp : "").ToUpper().Contains(input.Filter.ToUpper())
                                 )
                                 select new InventoryFindersDto
                                 {
                                     Id = o.ItemId.ToString(),
                                     DisplayName = o.Descp,
                                     Unit = o.StockUnit,
                                     Conver = o.Conver,
                                     Rate = _icledgRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.ItemID == o.ItemId).Count() > 0 ? Convert.ToDecimal(_icledgRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.ItemID == o.ItemId).Sum(a => a.Rate)) : 0,
                                     Option5 = opt.Descp != null ? opt.Descp : "",
                                     Qty= _icledgRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.ItemID == o.ItemId).Count() > 0 ? Convert.ToDecimal(_icledgRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.ItemID == o.ItemId).Sum(a => a.Qty)) : 0,

                                 };
            }
            else
            {
                lookupTableDto = from o in items
                  join op in option5 on o.Opt5 equals op.OptID into opp
                  from opt in opp.DefaultIfEmpty()
                  select new InventoryFindersDto
                  {
                      Id = o.ItemId.ToString(),
                      DisplayName = o.Descp,
                      Unit = o.StockUnit,
                      Conver = o.Conver,
                      Rate = _icledgRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.ItemID == o.ItemId).Count() > 0 ? Convert.ToDecimal(_icledgRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.ItemID == o.ItemId).Sum(a=>a.Rate)) : 0,
                      Option5 = opt.Descp != null ? opt.Descp : "",
                      Qty = _icledgRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.ItemID == o.ItemId).Count() > 0 ? Convert.ToDecimal(_icledgRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.ItemID == o.ItemId).Sum(a => a.Qty)) : 0,
                  };
            
            }

            //var lookupTableDto = from o in query
            //                     select new InventoryFindersDto
            //                     {
            //                         Id = o.ItemId.ToString(),
            //                         DisplayName = o.Descp,
            //                         Unit = o.StockUnit,
            //                         Conver = o.Conver,
            //                         Option5 =o.OptDesc!=null? o.OptDesc:""
            //                     };
            lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<InventoryFindersDto>(
                lookupTableDto.Count(),
                lookupTableDtoList
            );

            return getData;
        }

        private async Task<LookupDto<InventoryFindersDto>> GetAllItemsQForLookupTable(GetAllForLookupTableInput input)
        {
            var opt4Id = Convert.ToInt32(input.Filter);
            var items = _itemRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.ItemStatus == 0 && o.Opt4== opt4Id);
            var option5 = _icopT5Repository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);

            //var query = _itemRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.ItemStatus == 0).Join(_icopT5Repository.GetAll().Where(o => o.TenantId == AbpSession.TenantId).DefaultIfEmpty(), x=>x.Opt5,y=>y.OptID,(x,y)=>new {x.TenantId,x.ItemStatus, x.ItemId,x.Descp,x.StockUnit,x.Conver,x.Opt5,OptDesc=y.Descp!=null?y.Descp:""})
            //    .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
            //    e => e.ItemId.ToString().ToUpper().Contains(input.Filter.ToUpper()) || e.Descp.ToUpper().Contains(input.Filter.ToUpper()) || (e.OptDesc != null ? e.OptDesc : "").Trim().ToUpper().Contains(input.Filter.Trim().ToUpper()));

            IQueryable<InventoryFindersDto> lookupTableDto;
            if (input.Filter != null)
            {
                lookupTableDto = from o in items
                                 join op in option5 on o.Opt5 equals op.OptID into opp
                                 from opt in opp.DefaultIfEmpty()
                                 where (o.ItemId.ToString().Contains(input.Filter.ToUpper())
                                 || (o.Descp).ToUpper().Contains(input.Filter.ToUpper())
                                 || (opt.Descp != null ? opt.Descp : "").ToUpper().Contains(input.Filter.ToUpper())
                                 )
                                 select new InventoryFindersDto
                                 {
                                     Id = o.ItemId.ToString(),
                                     DisplayName = o.Descp,
                                     Unit = o.StockUnit,
                                     Conver = o.Conver,
                                     Rate = _icledgRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.ItemID == o.ItemId).Count() > 0 ? Convert.ToDecimal(_icledgRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.ItemID == o.ItemId).Sum(a => a.Rate)) : 0,
                                     Option5 = opt.Descp != null ? opt.Descp : ""
                                 };
            }
            else
            {
                lookupTableDto = from o in items
                                 join op in option5 on o.Opt5 equals op.OptID into opp
                                 from opt in opp.DefaultIfEmpty()
                                 select new InventoryFindersDto
                                 {
                                     Id = o.ItemId.ToString(),
                                     DisplayName = o.Descp,
                                     Unit = o.StockUnit,
                                     Conver = o.Conver,
                                     Rate = _icledgRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.ItemID == o.ItemId).Count() > 0 ? Convert.ToDecimal(_icledgRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId && e.ItemID == o.ItemId).Sum(a => a.Rate)) : 0,
                                     Option5 = opt.Descp != null ? opt.Descp : ""
                                 };

            }

            //var lookupTableDto = from o in query
            //                     select new InventoryFindersDto
            //                     {
            //                         Id = o.ItemId.ToString(),
            //                         DisplayName = o.Descp,
            //                         Unit = o.StockUnit,
            //                         Conver = o.Conver,
            //                         Option5 =o.OptDesc!=null? o.OptDesc:""
            //                     };
            lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<InventoryFindersDto>(
                lookupTableDto.Count(),
                lookupTableDtoList
            );

            return getData;
        }

        private async Task<LookupDto<InventoryFindersDto>> GetAllICLocationForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _icLocationRepository.GetAll().WhereIf(!string.IsNullOrWhiteSpace(input.Filter), 
                e => e.LocID.ToString().ToUpper().Contains(input.Filter.ToUpper()) || e.LocName.ToUpper().Contains(input.Filter.ToUpper())).Where(o => o.TenantId == AbpSession.TenantId);

            var lookupTableDto = from o in query
                                 select new InventoryFindersDto
                                 {
                                     Id = o.LocID.ToString(),
                                     DisplayName = o.LocName
                                 };
            lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<InventoryFindersDto>(
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
        private async Task<LookupDto<InventoryFindersDto>> GetAllICUserLocationForLookupTable(GetAllForLookupTableInput input)
        {
            LookupDto<InventoryFindersDto> getData;
            var userid = userInfo();
            if (userid.ToLower() != "admin")
            {
                var query = _csUserLocDRepository.GetAll().Where(c => c.TenantId == AbpSession.TenantId && c.TypeID == 2 && c.UserID == userid && c.Status == true);
                // .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                // e => e.LocId.ToString().ToUpper().Contains(input.Filter.ToUpper()) || e..ToUpper().Contains(input.Filter.ToUpper())).Where(o => o.TenantId == AbpSession.TenantId);
                var locQuery = _icLocationRepository.GetAll().Where(c => c.TenantId == AbpSession.TenantId);

                var lookupTableDto = from o in query
                                     join p in locQuery on o.LocId equals p.LocID
                                     select new InventoryFindersDto
                                     {
                                         Id = o.LocId.ToString(),
                                         DisplayName = p.LocName
                                     };
                lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
                var pageData = lookupTableDto.PageBy(input);
                var lookupTableDtoList = await pageData.ToListAsync();
                getData = new LookupDto<InventoryFindersDto>(
                   query.Count(),
                   lookupTableDtoList
               );
            }
            else
            {
                var query = _icLocationRepository.GetAll().WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
               e => e.LocID.ToString().ToUpper().Contains(input.Filter.ToUpper()) || e.LocName.ToUpper().Contains(input.Filter.ToUpper())).Where(o => o.TenantId == AbpSession.TenantId);

                var lookupTableDto = from o in query
                                     select new InventoryFindersDto
                                     {
                                         Id = o.LocID.ToString(),
                                         DisplayName = o.LocName
                                     };
                lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
                var pageData = lookupTableDto.PageBy(input);
                var lookupTableDtoList = await pageData.ToListAsync();
                 getData = new LookupDto<InventoryFindersDto>(
                    query.Count(),
                    lookupTableDtoList
                );

               // return getData;
            }
           

            return getData;
        }
        private async Task<LookupDto<InventoryFindersDto>> GetAllICUOMForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _icuomRepository.GetAll().WhereIf(!string.IsNullOrWhiteSpace(input.Filter), 
                e => e.UNITDESC.ToString().ToUpper().Contains(input.Filter.ToUpper()) || e.Id.ToString().ToUpper().Contains(input.Filter.ToUpper())).Where(o => o.TenantId == AbpSession.TenantId);

            var lookupTableDto = from o in query
                                 select new InventoryFindersDto
                                 {
                                     Id = o.Id.ToString(),
                                     DisplayName = o.UNITDESC,
                                     Unit=o.Unit,
                                     Conver=Convert.ToDecimal(o.Conver)
                                 };
            lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<InventoryFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }

        private async Task<LookupDto<InventoryFindersDto>> GetConsumptionLookupTable(GetAllForLookupTableInput input)
        {
            var query = _icCNSHeaderRepository.GetAll().WhereIf(!string.IsNullOrWhiteSpace(input.Filter),e=>false|| e.DocNo.ToString().Trim().ToUpper().Contains(input.Filter)).Where(o => o.TenantId == AbpSession.TenantId);

            var lookupTableDto = from o in query
                                 select new InventoryFindersDto
                                 {
                                     Id = o.DocNo.ToString(),
                                     DisplayName = o.Narration,
                                     Option5=o.LocID.ToString()
                                 };
            lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<InventoryFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }
        private async Task<LookupDto<InventoryFindersDto>> GetAllSubCostCenterForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _subCostCenterRepository.GetAll().Where(x=>x.CCID==input.ParamFilter).WhereIf(!string.IsNullOrWhiteSpace(input.Filter),x=>x.SUBCCID.ToString().Contains(input.Filter))
                .Where(o => o.TenantId == AbpSession.TenantId && o.Active == true);
            //var query = _subCostCenterRepository.GetAll().WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CCID.ToString().Trim().ToUpper().Contains(input.Filter.ToUpper())/* || e.SubCCName.ToUpper().Trim().Contains(input.Filter.ToUpper())*/)
            //  .WhereIf(!string.IsNullOrEmpty(input.ParamFilter),o => o.CCID == input.ParamFilter)
            //  .Where(o => o.TenantId == AbpSession.TenantId && o.Active==true);

            var lookupTableDto = from o in query
                                 select new InventoryFindersDto
                                 {
                                     Id = o.SUBCCID.ToString(),
                                     DisplayName = o.SubCCName
                                 };
            lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<InventoryFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }
        private async Task<LookupDto<InventoryFindersDto>> GetAllCostCenterForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _costCenterRepository.GetAll().WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.CCID.Trim().ToUpper().Contains(input.Filter.ToUpper()) || e.CCName.ToUpper().Trim().Contains(input.Filter.ToUpper()))
              .Where(o => o.TenantId == AbpSession.TenantId && o.Active==1);

            var lookupTableDto = from o in query
                                 select new InventoryFindersDto
                                 {
                                     Id = o.CCID,
                                     DisplayName = o.CCName
                                 };
            lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<InventoryFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }
        private async Task<LookupDto<InventoryFindersDto>> GetAllPriceListForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _priceListRepository.GetAll().WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                e => e.PriceList.ToString().ToUpper().Contains(input.Filter.ToUpper()) || e.PriceListName.ToUpper().Contains(input.Filter.ToUpper())).Where(o => o.TenantId == AbpSession.TenantId);

            var lookupTableDto = from o in query
                                 select new InventoryFindersDto
                                 {
                                     Id = o.PriceList,
                                     DisplayName = o.PriceListName
                                 };
            lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<InventoryFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }
        private async Task<LookupDto<InventoryFindersDto>> GetAllItemPricingForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _itemPricingRepository.GetAll().WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                e => e.PriceList.ToString().ToUpper().Contains(input.Filter.ToUpper()) || e.ItemID.ToUpper().Contains(input.Filter.ToUpper())).Where(o => o.TenantId == AbpSession.TenantId);

            var lookupTableDto = from o in query
                                 select new InventoryFindersDto
                                 {
                                     Id = o.ItemID,
                                     DisplayName = o.PriceList,
                                     Rate=o.NetPrice
                                 };
            lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<InventoryFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }
        private async Task<LookupDto<InventoryFindersDto>> GetAllTransactionTypeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _transactionTypeRepository.GetAll().WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                e => e.TypeId.ToString().ToUpper().Contains(input.Filter) || e.Description.ToUpper().Contains(input.Filter.ToUpper()))
                .Where(o => o.TenantId == AbpSession.TenantId);

            var lookupTableDto = from o in query
                                 select new InventoryFindersDto
                                 {
                                     Id = o.TypeId,
                                     DisplayName = o.Description
                                 };
            lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<InventoryFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }
        private async Task<LookupDto<InventoryFindersDto>> GetAllSegment1ForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _icSegment1Repository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Seg1ID.ToUpper().Contains(input.Filter.ToUpper()) || e.Seg1Name.ToUpper().Contains(input.Filter.ToUpper()));

            var lookupTableDto = from o in query
                                 select new InventoryFindersDto
                                 {
                                     Id = o.Seg1ID,
                                     DisplayName = o.Seg1Name
                                 };
            lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<InventoryFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        } private async Task<LookupDto<InventoryFindersDto>> GetAllSegment2ForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _icSegment2Repository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Seg2Id.ToUpper().Contains(input.Filter) || e.Seg2Name.ToUpper().Contains(input.Filter));

            var lookupTableDto = from o in query
                                 select new InventoryFindersDto
                                 {
                                     Id = o.Seg2Id,
                                     DisplayName = o.Seg2Name
                                 };
            lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<InventoryFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }

        private async Task<LookupDto<InventoryFindersDto>> GetAllSegment3ForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _icSegment3Repository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Seg3Id.ToUpper().Contains(input.Filter.ToUpper()) || e.Seg3Name.ToUpper().Contains(input.Filter.ToUpper()));

            var lookupTableDto = from o in query
                                 select new InventoryFindersDto
                                 {
                                     Id = o.Seg3Id,
                                     DisplayName = o.Seg3Name
                                 };
            lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<InventoryFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }
        //private async Task<LookupDto<InventoryFindersDto>> GetAllICTerms(GetAllForLookupTableInput input)
        //{
        //    var query = _icTermsRepository.GetAll().WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
        //        e => e.Descr.ToString().ToUpper().Contains(input.Filter) || e.Descr.ToUpper().Contains(input.Filter.ToUpper()))
        //        .Where(o => o.TenantId == AbpSession.TenantId && o.Type == input.Target);

        //    var lookupTableDto = from o in query
        //                         select new InventoryFindersDto
        //                         {
        //                             Id = o.Id.ToString(),
        //                             DisplayName = o.Descr
        //                         };
        //    lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
        //    var pageData = lookupTableDto.PageBy(input);
        //    var lookupTableDtoList = await pageData.ToListAsync();
        //    var getData = new LookupDto<InventoryFindersDto>(
        //        query.Count(),
        //        lookupTableDtoList
        //    );

        //    return getData;
        //}
    }
}
