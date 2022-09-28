using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using ERP.Dto;
using ERP.SupplyChain.Inventory.IC_Item.Dto;
using Abp.Linq.Extensions;
using Abp.UI;
using ERP.SupplyChain.Inventory.IC_UNIT;
using ERP.SupplyChain.Inventory.IC_Segment3;
using ERP.SupplyChain.Inventory.IC_Segment1;
using ERP.SupplyChain.Inventory.IC_Segment2;
using ERP.Storage;
using System.Drawing;
using System.IO;
using ERP.GeneralLedger.SetupForms;
using ERP.SupplyChain.Inventory.IC_Item.Exporting;
using ERP.Authorization.Users;

namespace ERP.SupplyChain.Inventory.IC_Item
{
    public class ICItemAppService : ERPAppServiceBase, IICItemAppService
    {

        private readonly IRepository<ICLEDG> _icledgRepository;

        private const int MaxProfilPictureBytes = 5242880; //5MB
        public readonly IRepository<ICItem> _ICItemRepository;
        private readonly IBinaryObjectManager _binaryObjectManager;
        public readonly IIC_UNITsAppService _IC_UNITsAppService;
        private readonly IRepository<ICSegment3> _ICSegment3Repository;
        private readonly IRepository<ICSegment1> _ICSegment1Repository;
        private readonly IRepository<ICSegment2> _ICSegment2Repository;
        private readonly IRepository<IC_UNIT.IC_UNIT> _unitRepository;
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IRepository<ERP.SupplyChain.Inventory.ICOPT4.ICOPT4> _icopT4Repository;
        private readonly IRepository<ERP.SupplyChain.Inventory.ICOPT5.ICOPT5> _icopT5Repository;
        private readonly IRepository<TaxAuthority, string> _taxAuthorityRepository;
        private readonly IRepository<ChartofControl, string> _ChartofAccountRepository;

        private readonly IICITEMExcelExporter _ICITEMExcelExporter;



        public ICItemAppService(IRepository<ICItem> ICItemRepository, IIC_UNITsAppService IC_UNITsAppService, IRepository<ICSegment3> ICSegment3Repository, IRepository<ICSegment1> ICSegment1Repository, IRepository<ICSegment2> ICSegment2Repository, ITempFileCacheManager tempFileCacheManager, IBinaryObjectManager binaryObjectManager, IRepository<ICLEDG> icledgRepository, IRepository<ERP.SupplyChain.Inventory.ICOPT4.ICOPT4> icopT4Repository, IRepository<ERP.SupplyChain.Inventory.ICOPT5.ICOPT5> icopT5Repository,
            IRepository<TaxAuthority, string> taxAuthorityRepository, IRepository<ChartofControl, string> ChartofAccountRepository,
            IICITEMExcelExporter ICITEMExcelExporter, IRepository<IC_UNIT.IC_UNIT> unitRepository
            )
        {
            _ICItemRepository = ICItemRepository;
            _IC_UNITsAppService = IC_UNITsAppService;
            _unitRepository = unitRepository;
            _ICSegment3Repository = ICSegment3Repository;
            _ICSegment2Repository = ICSegment2Repository;
            _ICSegment1Repository = ICSegment1Repository;
            _tempFileCacheManager = tempFileCacheManager;
            _binaryObjectManager = binaryObjectManager;
            _icledgRepository = icledgRepository;
            _icopT4Repository = icopT4Repository;
            _icopT5Repository = icopT5Repository;
            _taxAuthorityRepository = taxAuthorityRepository;
            _ChartofAccountRepository = ChartofAccountRepository;
            _ICITEMExcelExporter = ICITEMExcelExporter;
        }

        public async Task<PagedResultDto<GetIcItemForViewDto>> GetAll(GetAllIcItemInput input)
        {
            var filteredItems = _ICItemRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ItemId.Contains(input.Filter) || e.Descp.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ItemIdFilter), e => e.ItemId == input.ItemIdFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.DescpFilter), e => e.Descp == input.DescpFilter);


            var pagedAndFilteredItems = filteredItems
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var Items = from o in pagedAndFilteredItems
                        select new GetIcItemForViewDto()
                        {
                            IcItem = new IcItemDto
                            {
                                Id = o.Id,
                                Seg1Id = o.Seg1Id,
                                Seg2Id = o.Seg2Id,
                                Seg3Id = o.Seg3Id,
                                ItemId = o.ItemId,
                                Descp = o.Descp,
                                CreationDate = o.CreationDate,
                                ItemCtg = o.ItemCtg,
                                ItemType = o.ItemType,
                                ItemStatus = o.ItemStatus,
                                StockUnit = o.StockUnit,
                                Packing = o.Packing,
                                Weight = o.Weight,
                                Taxable = o.Taxable,
                                Saleable = o.Saleable,
                                Active = o.Active,
                                Barcode = o.Barcode,
                                AltItemID = o.AltItemID,
                                AltDescp = o.AltDescp,
                                Opt1 = o.Opt1,
                                Opt2 = o.Opt2,
                                Opt3 = o.Opt3,
                                Opt4 = o.Opt4,
                                Opt5 = o.Opt5,
                                DefPriceList = o.DefPriceList,
                                DefVendorAC = o.DefVendorAC,
                                DefVendorID = o.DefVendorID,
                                DefCustAC = o.DefCustAC,
                                DefCustID = o.DefCustID,
                                DefTaxAuth = o.DefTaxAuth,
                                DefTaxClassID = o.DefTaxClassID,
                                Picture = o.Picture,
                                AudtUser = o.AudtUser,
                                AudtDate = o.AudtDate,
                                Conver = o.Conver
                            }
                        };
            var totalCount = await filteredItems.CountAsync();

            return new PagedResultDto<GetIcItemForViewDto>(
                totalCount,
                await Items.ToListAsync()
            );
        }

        public async Task<GetIcItemForEditOutput> GetIcItemForEdit(EntityDto input)
        {
            var items = await _ICItemRepository.FirstOrDefaultAsync(input.Id);
            var output = new GetIcItemForEditOutput { IcItem = ObjectMapper.Map<CreateOrEditIcItemDto>(items) };


            var segment3 = await _ICSegment3Repository.FirstOrDefaultAsync(x=>x.Seg3Id == output.IcItem.Seg3Id && x.TenantId == AbpSession.TenantId);
            output.Seg3Name = segment3.Seg3Name;

            var segment2 = await _ICSegment2Repository.FirstOrDefaultAsync(x => x.Seg2Id == output.IcItem.Seg2Id && x.TenantId == AbpSession.TenantId);
            output.Seg2Name = segment2.Seg2Name;

            var segment1 = await _ICSegment1Repository.FirstOrDefaultAsync(x => x.Seg1ID == output.IcItem.Seg1Id && x.TenantId == AbpSession.TenantId);
            output.Seg1Name = segment1.Seg1Name;

            if (output.IcItem.Opt4 != null)
            {
                var opt4 = _icopT4Repository.FirstOrDefault(x => x.OptID == output.IcItem.Opt4 && x.TenantId == AbpSession.TenantId);
                output.Option4Desc = opt4.Descp;
            }

            if (output.IcItem.Opt5 != null)
            {
                var opt5 = _icopT5Repository.FirstOrDefault(x => x.OptID == output.IcItem.Opt5 && x.TenantId == AbpSession.TenantId);
                output.Option5Desc = opt5.Descp;
            }


            if (output.IcItem.DefTaxAuth != null)
            {
                var taxAuth = _taxAuthorityRepository.FirstOrDefault(x => x.Id == output.IcItem.DefTaxAuth && x.TenantId == AbpSession.TenantId);
                output.DefTaxAuthDesc = taxAuth.TAXAUTHDESC;
            }

            if (output.IcItem.DefCustAC != null)
            {
                var custAccount = _ChartofAccountRepository.FirstOrDefault(x => x.Id == output.IcItem.DefCustAC && x.TenantId == AbpSession.TenantId);
                output.defCustomerAccDesc = custAccount.AccountName;
            }

            if (output.IcItem.DefVendorAC != null)
            {
                var venAccount = _ChartofAccountRepository.FirstOrDefault(x => x.Id == output.IcItem.DefVendorAC && x.TenantId == AbpSession.TenantId);
                output.defVendorAccDesc = venAccount.AccountName;
            }

            var UnitList = await _IC_UNITsAppService.GetIC_UNITForEdit(output.IcItem.ItemId);
            output.IcItem.IC_Units = UnitList.IC_UNIT;
            return output;
        }

        public async Task<GetIcItemForViewDto> GetIcItemForView(int id)
        {
            var items = await _ICItemRepository.FirstOrDefaultAsync(id);

            var output = new GetIcItemForViewDto { IcItem = ObjectMapper.Map<IcItemDto>(items) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditIcItemDto input)
        {
            
            if (input.flag == false)
            {
                var icItem = await _ICItemRepository.FirstOrDefaultAsync(x => x.ItemId == input.Seg3Id + '-' + input.ItemId && x.TenantId == AbpSession.TenantId);

                if (icItem != null)
                {
                    throw new UserFriendlyException("Ooppps! There is a problem!", "Items ID " + input.ItemId + " already taken....");
                }

                await Create(input);
                foreach (var item in input.IC_Units)
                {
                    item.ItemId = input.Seg3Id + "-" + input.ItemId;
                }
                await _IC_UNITsAppService.CreateOrEdit(input.IC_Units);
            }
            else
            {
                await Update(input);
                if (input.IC_Units!=null)
                {
                    var itemunit = _unitRepository.GetAll().Where(c => c.TenantId == AbpSession.TenantId && c.ItemId == input.ItemId).ToList();
                    if (itemunit!=null)
                    {
                        foreach (var unit in itemunit)
                        {
                        await _unitRepository.DeleteAsync(unit);

                        }
                    }
                    
                }
                
               
                await _IC_UNITsAppService.CreateOrEdit(input.IC_Units);
            }
        }

        public  void DeleteUnit()
        {

        }
        protected async Task Update(CreateOrEditIcItemDto input)
        {
            var items = await _ICItemRepository.FirstOrDefaultAsync(input.Id);
            items.AudtDate = DateTime.Now;
            items.AudtUser = GetCurrentUserName().Result.UserName;
            ObjectMapper.Map(input, items);
        }

        protected async Task Create(CreateOrEditIcItemDto input)
        {
            var items = ObjectMapper.Map<ICItem>(input);
            items.ItemId = items.Seg3Id + "-" + items.ItemId;
            items.CreationDate = DateTime.Now;            
            if (AbpSession.TenantId != null)
            {
                items.TenantId = (int)AbpSession.TenantId;
            }


            await _ICItemRepository.InsertAsync(items);
        }

        public async Task Delete(EntityDto input)
        {
            var itemid = _ICItemRepository.GetAll().Where(x=>x.Id == input.Id).Select(x=>x.ItemId).FirstOrDefault();
            var itemidList = _icledgRepository.GetAll().Select(x => x.ItemID).ToList();

            if (itemidList.Contains(itemid))
            {
                throw new UserFriendlyException(001, itemid + " Already In use");
            }

            await _ICItemRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetIcItemToExcel(GetAllIcItemForExcelInput input)
        {
            var filteredItems = _ICItemRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
               .WhereIf(!string.IsNullOrWhiteSpace(input.ItemIdFilter), e => e.ItemId == input.ItemIdFilter)
               .WhereIf(!string.IsNullOrWhiteSpace(input.DescpFilter), e => e.Descp == input.DescpFilter);
            
            var Items = from o in filteredItems
                        join seg1 in _ICSegment1Repository.GetAll() on new { o.Seg1Id, o.TenantId } equals new { Seg1Id = seg1.Seg1ID, seg1.TenantId }
                        join seg2 in _ICSegment2Repository.GetAll() on new { o.Seg2Id, o.TenantId } equals new { seg2.Seg2Id, seg2.TenantId}
                        join seg3 in _ICSegment3Repository.GetAll() on new { o.Seg3Id, o.TenantId } equals new { seg3.Seg3Id, seg3.TenantId }
                        select new GetIcItemForViewDto()
                        {
                            IcItem = new IcItemDto
                            {
                                Id = o.Id,
                                Seg1Id = o.Seg1Id,
                                Seg2Id = o.Seg2Id,
                                Seg3Id = o.Seg3Id,
                                ItemId = o.ItemId,
                                Descp = o.Descp,
                                CreationDate = o.CreationDate,
                                ItemCtg = o.ItemCtg,
                                ItemType = o.ItemType,
                                ItemStatus = o.ItemStatus,
                                StockUnit = o.StockUnit,
                                Packing = o.Packing,
                                Weight = o.Weight,
                                Taxable = o.Taxable,
                                Saleable = o.Saleable,
                                Active = o.Active,
                                Barcode = o.Barcode,
                                AltItemID = o.AltItemID,
                                AltDescp = o.AltDescp,
                                Opt1 = o.Opt1,
                                Opt2 = o.Opt2,
                                Opt3 = o.Opt3,
                                Opt4 = o.Opt4,
                                Opt5 = o.Opt5,
                                DefPriceList = o.DefPriceList,
                                DefVendorAC = o.DefVendorAC,
                                DefVendorID = o.DefVendorID,
                                DefCustAC = o.DefCustAC,
                                DefCustID = o.DefCustID,
                                DefTaxAuth = o.DefTaxAuth,
                                DefTaxClassID = o.DefTaxClassID,
                                Picture = o.Picture,
                                AudtUser = o.AudtUser,
                                AudtDate = o.AudtDate,
                                Conver = o.Conver,
                                ItemSrNo = o.ItemSrNo,
                                Expirydate =  o.Expirydate,
                                ManufectureDate = o.ManufectureDate,
                                Venitemcode = o.Venitemcode,
                                VenLotNo = o.VenLotNo,
                                 VenSrNo = o.VenSrNo,
                                 warrantyinfo = o.warrantyinfo
                            },
                          Seg1Name = seg1.Seg1Name,
                            Seg2Name =  seg2.Seg2Name,
                            Seg3Name = seg3.Seg3Name
                        };

            var icItemListDtos = await Items.ToListAsync();
            return _ICITEMExcelExporter.ExportToFile(icItemListDtos);
        }

        public string GetIcItemMaxId(string Seg3ID)
        {
            var filteredItems = _ICItemRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId);
            //string x = "" +  id + "%";
            string[] xstring;
            string xformat = "";
            string nString = "";
            string finalSting = "";
            var getMaxID = filteredItems.Where(c => EF.Functions.Like(c.ItemId, $"{Seg3ID}%") && c.TenantId == AbpSession.TenantId).OrderByDescending(o => o.Id).Select(x => x.ItemId).FirstOrDefault();

            if (getMaxID == null)
            {

                xformat = string.Format("{0:0000}", 1);
                finalSting = xformat;
            }
            else
            {
                xstring = getMaxID.Split('-');
                nString = xstring[3];

                if (Convert.ToInt32(nString) + 1 > 9999)
                {
                    xformat = string.Format("{0:0000}", 9999);
                    finalSting = xformat;
                }
                else
                {
                    xformat = string.Format("{0:0000}", Convert.ToInt32(nString) + 1);
                    finalSting = xformat;
                }
            }
            return finalSting;
        }

        public string GetName(string Id)
        {
            var itemName = _ICItemRepository.GetAll().Where(x => x.ItemId == Id).Select(x => x.Descp).FirstOrDefault();

            return itemName;


        }

        public async Task UpdateItemPicture(UpdateItemPicture input)
        {
            byte[] byteArray;

            var imageBytes = _tempFileCacheManager.GetFile(input.FileToken);

            if (imageBytes == null)
            {
                throw new UserFriendlyException("There is no such image file with the token: " + input.FileToken);
            }

            using (var bmpImage = new Bitmap(new MemoryStream(imageBytes)))
            {
                var width = (input.Width == 0 || input.Width > bmpImage.Width) ? bmpImage.Width : input.Width;
                var height = (input.Height == 0 || input.Height > bmpImage.Height) ? bmpImage.Height : input.Height;
                var bmCrop = bmpImage.Clone(new Rectangle(input.X, input.Y, width, height), bmpImage.PixelFormat);

                using (var stream = new MemoryStream())
                {
                    bmCrop.Save(stream, bmpImage.RawFormat);
                    byteArray = stream.ToArray();
                }
            }

            if (byteArray.Length > MaxProfilPictureBytes)
            {
                throw new UserFriendlyException(L("ResizedProfilePicture_Warn_SizeLimit", AppConsts.ResizedMaxProfilPictureBytesUserFriendlyValue));
            }

            var item = await _ICItemRepository.FirstOrDefaultAsync(x => x.ItemId == input.ItemId);

            if (item.Picture.HasValue)
            {
                await _binaryObjectManager.DeleteAsync(item.Picture.Value);
            }

            var storedFile = new BinaryObject(AbpSession.TenantId, byteArray);
            await _binaryObjectManager.SaveAsync(storedFile);

            item.Picture = storedFile.Id;
        }

        public async Task<GetItemPictureOutput> GetItemPicture(string ItemID)
        {
            var user = await _ICItemRepository.FirstOrDefaultAsync(x=> x.ItemId == ItemID);
            if (user.Picture == null)
            {
                return new GetItemPictureOutput(string.Empty);
            }

            return await GetItemPictureById(user.Picture.Value);
        }

        public async Task<GetItemPictureOutput> GetItemPictureById(Guid itemPictureId)
        {
            return await GetItemPictureByIdInternal(itemPictureId);
        }
        private async Task<GetItemPictureOutput> GetItemPictureByIdInternal(Guid itemPictureId)
        {
            var bytes = await GeItemPictureByIdOrNull(itemPictureId);
            if (bytes == null)
            {
                return new GetItemPictureOutput(string.Empty);
            }

            return new GetItemPictureOutput(Convert.ToBase64String(bytes));
        }

        private async Task<byte[]> GeItemPictureByIdOrNull(Guid itemPictureId)
        {
            var file = await _binaryObjectManager.GetOrNullAsync(itemPictureId);
            if (file == null)
            {
                return null;
            }

            return file.Bytes;
        }

        public async Task<User> GetCurrentUserName()
        {
            var user = await UserManager.FindByIdAsync(AbpSession.UserId.ToString());
            return user;
        }
    }
}
