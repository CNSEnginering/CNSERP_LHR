

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
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ERP.Authorization.Users;
using ERP.Costing;
using ERP.SupplyChain.Inventory.Exporting;
using ERP.SupplyChain.Inventory.IC_Item;
using ERP.GeneralLedger.Transaction.VoucherEntry;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRD.Dtos;
using ERP.GeneralLedger.Transaction.VoucherEntry.Dtos;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH.Dtos;
using ERP.SupplyChain.Inventory.Consumption;

namespace ERP.SupplyChain.Inventory
{
    [AbpAuthorize(AppPermissions.Inventory_Transfers)]
    public class TransfersAppService : ERPAppServiceBase, ITransfersAppService
    {
        private readonly IRepository<Transfer> _transferRepository;
        private readonly IRepository<TransferDetail> _transferDetailRepository;
        private readonly IRepository<ICLocation> _locationRepository;
        private readonly IRepository<ICLEDG> _icledgRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly CostingAppService _costingService;
        private readonly ITransfersExcelExporter _ItransferExcelExpoter;
        private readonly IRepository<ICSetup> _icSetupRepository;
        private readonly IRepository<ICItem> _itemRepository;
        private readonly IRepository<InventoryGlLink> _inventoryGlLinkRepository;
        private readonly IRepository<CostCenter> _ccRepository;
        private VoucherEntryAppService _voucherEntryAppService;
        private readonly CommonAppService _commonappRepository;
        public TransfersAppService(
            IRepository<Transfer> transferRepository,
            IRepository<ICLocation> locationRepository,
            IRepository<ICLEDG> icledgRepository,
            IRepository<User, long> userRepository,
            IRepository<TransferDetail> transferDetailRepository,
            CostingAppService costingService,
            ITransfersExcelExporter ItransferExcelExpoter,
            IRepository<ICItem> itemRepository,
            IRepository<ICSetup> icSetupRepository,
            IRepository<InventoryGlLink> inventoryGlLinkRepository,
            VoucherEntryAppService voucherEntryAppService,
            IRepository<CostCenter> ccRepository,
            CommonAppService commonappRepository
            )
        {
            _transferRepository = transferRepository;
            _locationRepository = locationRepository;
            _icledgRepository = icledgRepository;
            _userRepository = userRepository;
            _transferDetailRepository = transferDetailRepository;
            _costingService = costingService;
            _ItransferExcelExpoter = ItransferExcelExpoter;
            _itemRepository = itemRepository;
            _icSetupRepository = icSetupRepository;
            _inventoryGlLinkRepository = inventoryGlLinkRepository;
            _voucherEntryAppService = voucherEntryAppService;
            _ccRepository = ccRepository;
            _commonappRepository = commonappRepository;
        }
        public async Task<User> GetCurrentUserName()
        {
            var user = await UserManager.FindByIdAsync(AbpSession.UserId.ToString());
            return user;
        }
        public async Task<IQueryable<Permission>> GetUserPermissions(User user)
        {
            var permissions = await UserManager.GetGrantedPermissionsAsync(user);

            return permissions.AsQueryable();
        }

        public async Task<PagedResultDto<GetTransfersForViewDto>> GetAll(GetAllTransfersInput input)
        {
            User currUser = await GetCurrentUserName();

            var userName = currUser.UserName;
            var usrPermissions = await GetUserPermissions(currUser);

            //var fitlerLoc = _locationRepository.GetAll().Where(o => o.LocName.Contains(input.Filter)).Select(x => x.LocID).ToList();
            var filteredTransfers = _transferRepository.GetAll()
                .Where(o => o.TenantId == AbpSession.TenantId).WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Narration.Contains(input.Filter) || e.DocNo.ToString() == input.Filter)
                        .WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
                        .WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
                        .WhereIf(input.ActiveFilter > -1, e => (input.ActiveFilter == 1 && e.Approved) || (input.ActiveFilter == 0 && !e.Approved))
                        .WhereIf(input.PostedFilter > -1, e => (input.PostedFilter == 1 && e.Posted) || (input.PostedFilter == 0 && !e.Posted))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.ApprovedBy == input.AudtUserFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter);

            //.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Narration.Contains(input.Filter) || e.OrdNo.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
            //.WhereIf(input.MinFromLocIDFilter != null, e => e.FromLocID >= input.MinFromLocIDFilter)
            //.WhereIf(input.MaxFromLocIDFilter != null, e => e.FromLocID <= input.MaxFromLocIDFilter)
            //.WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
            //.WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
            //.WhereIf(input.MinDocDateFilter != null, e => e.DocDate >= input.MinDocDateFilter)
            //.WhereIf(input.MaxDocDateFilter != null, e => e.DocDate <= input.MaxDocDateFilter)
            //.WhereIf(!string.IsNullOrWhiteSpace(input.NarrationFilter), e => e.Narration == input.NarrationFilter)
            //.WhereIf(input.MinToLocIDFilter != null, e => e.ToLocID >= input.MinToLocIDFilter)
            //.WhereIf(input.MaxToLocIDFilter != null, e => e.ToLocID <= input.MaxToLocIDFilter)
            //.WhereIf(input.MinTotalQtyFilter != null, e => e.TotalQty >= input.MinTotalQtyFilter)
            //.WhereIf(input.MaxTotalQtyFilter != null, e => e.TotalQty <= input.MaxTotalQtyFilter)
            //.WhereIf(input.MinTotalAmtFilter != null, e => e.TotalAmt >= input.MinTotalAmtFilter)
            //.WhereIf(input.MaxTotalAmtFilter != null, e => e.TotalAmt <= input.MaxTotalAmtFilter)
            //.WhereIf(input.PostedFilter > -1, e => (input.PostedFilter == 1 && e.Posted) || (input.PostedFilter == 0 && !e.Posted))
            //.WhereIf(input.MinLinkDetIDFilter != null, e => e.LinkDetID >= input.MinLinkDetIDFilter)
            //.WhereIf(input.MaxLinkDetIDFilter != null, e => e.LinkDetID <= input.MaxLinkDetIDFilter)
            //.WhereIf(!string.IsNullOrWhiteSpace(input.OrdNoFilter), e => e.OrdNo == input.OrdNoFilter)
            //.WhereIf(input.HOLDFilter > -1, e => (input.HOLDFilter == 1 && e.HOLD) || (input.HOLDFilter == 0 && !e.HOLD))
            //.WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
            //.WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
            //.WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
            //.WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
            //.WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
            //.WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            //var pagedAndFilteredTransfers = filteredTransfers
            //    .OrderBy(input.Sorting ?? "id desc")
            //    .PageBy(input);

            IQueryable<Transfer> pagedAndFilteredTransfers = null;
            pagedAndFilteredTransfers = filteredTransfers
               .OrderBy(input.Sorting ?? "id desc")
               .PageBy(input);
            //var UserName = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            //if (UserName == "admin" || usrPermissions.Any(x => x.Name == AppPermissions.Transaction_Inventory_ShowAll))
            //{
            //    pagedAndFilteredTransfers = filteredTransfers
            //    .OrderBy(input.Sorting ?? "id desc")
            //    .PageBy(input);
            //}
            //else
            //{
            //    pagedAndFilteredTransfers = filteredTransfers.Where(c => c.CreatedBy == UserName)
            //  .OrderBy(input.Sorting ?? "id desc")
            //  .PageBy(input);
            //}

            var transfers = from o in pagedAndFilteredTransfers
                            select new GetTransfersForViewDto()
                            {
                                Transfer = new TransferDto
                                {
                                    FromLocID = o.FromLocID,
                                    DocNo = o.DocNo,
                                    DocDate = o.DocDate,
                                    Narration = o.Narration,
                                    ToLocID = o.ToLocID,
                                    TotalQty = o.TotalQty,
                                    TotalAmt = o.TotalAmt,
                                    Posted = o.Posted,
                                    Approved = o.Approved,
                                    PostedBy = o.PostedBy,
                                    PostedDate = o.PostedDate,
                                    LinkDetID = o.LinkDetID,
                                    OrdNo = o.OrdNo,
                                    HOLD = o.HOLD,
                                    AudtUser = o.AudtUser,
                                    AudtDate = o.AudtDate,
                                    CreatedBy = o.CreatedBy,
                                    CreateDate = o.CreateDate,
                                    FromLocName = _locationRepository.GetAll().Where(a => a.TenantId == AbpSession.TenantId && a.LocID == o.FromLocID).FirstOrDefault().LocName,
                                    ToLocName = _locationRepository.GetAll().Where(a => a.TenantId == AbpSession.TenantId && a.LocID == o.ToLocID).FirstOrDefault().LocName,
                                    Id = o.Id
                                }
                            };

            var totalCount = await pagedAndFilteredTransfers.CountAsync();

            return new PagedResultDto<GetTransfersForViewDto>(
                totalCount,
                await transfers.ToListAsync()
            );
        }

        public async Task<GetTransfersForViewDto> GetTransferForView(int id)
        {
            var transfer = await _transferRepository.GetAsync(id);

            var output = new GetTransfersForViewDto { Transfer = ObjectMapper.Map<TransferDto>(transfer) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Inventory_Transfers_Edit)]
        public async Task<GetTransferForEditOutput> GetTransferForEdit(EntityDto input)
        {
            try
            {
                //var itemList = await _itemRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId).ToListAsync();
                var transfer = await _transferRepository.FirstOrDefaultAsync(x => x.DocNo == input.Id && x.TenantId == AbpSession.TenantId);
                transfer.AudtUser= _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                var transferDetail = await _transferDetailRepository.GetAll().Where(o => o.DocNo == transfer.DocNo && o.TenantId == AbpSession.TenantId).ToListAsync();
                var output = new GetTransferForEditOutput { Transfer = ObjectMapper.Map<CreateOrEditTransferDto>(transfer) };
                output.Transfer.transferDetailDto = ObjectMapper.Map<List<TransferDetailDto>>(transferDetail);
                output.Transfer.FromLocName = _locationRepository.GetAll().Where(o => o.LocID == output.Transfer.FromLocID).Count() > 0
                    ? _locationRepository.GetAll().Where(o => o.LocID == output.Transfer.FromLocID).FirstOrDefault().LocName :
                    "";
                output.Transfer.ToLocName = _locationRepository.GetAll().Where(o => o.LocID == output.Transfer.ToLocID).Count() > 0
                   ? _locationRepository.GetAll().Where(o => o.LocID == output.Transfer.ToLocID).FirstOrDefault().LocName :
                   "";
                foreach (var data in output.Transfer.transferDetailDto)
                {
                    var item = _itemRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId
                    && o.ItemId == data.ItemID
                    );
                    data.MaxQty = Convert.ToInt32(GetQtyInHand(data.ItemID, data.FromLocId, data.DocNo));
                    if (item.Count() > 0)
                    {
                        output.Transfer.transferDetailDto.Where(o => o.DocNo == data.DocNo && o.ItemID==data.ItemID).FirstOrDefault().ItemID =
                            item.FirstOrDefault().ItemId + "*" + item.FirstOrDefault().Descp + "*" + data.Unit + "*" + data.Conver;

                    }
                    else
                    {
                        output.Transfer.transferDetailDto.Where(o => o.DocNo == data.DocNo && o.ItemID == data.ItemID).FirstOrDefault().ItemID =
                            data.ItemID + "*" + " " + "*" + " " + "*" + " ";
                    }
                }
                output.Transfer.ccdesc = _ccRepository.FirstOrDefault(x => x.TenantId == AbpSession.TenantId && x.CCID == output.Transfer.CCID)?.CCName ?? "";
                return output;
            }
            catch (Exception ex)
            {

                throw;
            }
          
        }

        public async Task CreateOrEdit(CreateOrEditTransferDto input)
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

        [AbpAuthorize(AppPermissions.Inventory_Transfers_Create)]
        protected virtual async Task Create(CreateOrEditTransferDto input)
        {
            var transfer = ObjectMapper.Map<Transfer>(input);
            var user = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).Count() > 0 ?
                _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName :
                "";
            transfer.CreatedBy = user;
            transfer.CreateDate = DateTime.Now;
            transfer.DocNo = GetDocId();
            if (AbpSession.TenantId != null)
            {
                transfer.TenantId = (int)AbpSession.TenantId;
            }


            var getId = await _transferRepository.InsertAndGetIdAsync(transfer);

            foreach (var data in input.transferDetailDto)
            {
                var transferDetail = ObjectMapper.Map<TransferDetail>(data);
                if (AbpSession.TenantId != null)
                {
                    transferDetail.TenantId = (int)AbpSession.TenantId;
                }
                transferDetail.DetID = getId;
                transferDetail.DocNo = input.DocNo;
                transferDetail.FromLocId = input.FromLocID;
                transferDetail.Cost = Convert.ToDecimal(_costingService.getCosting(input.DocDate.Date, transferDetail.ItemId, transferDetail.FromLocId, 3, input.DocNo));
                transferDetail.Amount = transferDetail.Qty * transferDetail.Cost;
                await _transferDetailRepository.InsertAsync(transferDetail);
            }

        }

        [AbpAuthorize(AppPermissions.Inventory_Transfers_Edit)]
        protected virtual async Task Update(CreateOrEditTransferDto input)
        {
            var transfer = await _transferRepository.FirstOrDefaultAsync(x => x.DocNo == input.DocNo && x.TenantId == AbpSession.TenantId);
            input.AudtUser = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            input.AudtDate = DateTime.Now;
            ObjectMapper.Map(input, transfer);
            await _transferDetailRepository.DeleteAsync(o => o.TenantId == AbpSession.TenantId && o.DocNo == input.DocNo);
            foreach (var data in input.transferDetailDto)
            {
                var transferDetail = ObjectMapper.Map<TransferDetail>(data);
                if (AbpSession.TenantId != null)
                {
                    transferDetail.TenantId = (int)AbpSession.TenantId;
                }
                transferDetail.DetID = (int)input.Id;
                transferDetail.DocNo = input.DocNo;
                transferDetail.FromLocId = input.FromLocID;
                transferDetail.Cost = Convert.ToDecimal(_costingService.getCosting(input.DocDate.Date, transferDetail.ItemId, transferDetail.FromLocId, 3, input.DocNo));
                transferDetail.Amount = transferDetail.Qty * transferDetail.Cost;
                await _transferDetailRepository.InsertAsync(transferDetail);
            }

        }

        [AbpAuthorize(AppPermissions.Inventory_Transfers_Delete)]
        public async Task Delete(EntityDto input)
        {
            DeleteLog(input.Id);
            await _transferRepository.DeleteAsync(x => x.DocNo == input.Id && x.TenantId == AbpSession.TenantId);
            await _transferDetailRepository.DeleteAsync(o => o.TenantId == AbpSession.TenantId && o.DocNo == input.Id);
        }

        public void DeleteLog(int detid)
        {
            var data = _transferRepository.FirstOrDefault(c => c.DocNo == detid && c.TenantId == AbpSession.TenantId);
            LogModel model = new LogModel()
            {
                Action = "Delete",
                Detid = data.Id,
                DocNo = detid,
                FormName = "Tranfer",
                Status = true,
                ApprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName,
                TenantId = AbpSession.TenantId
            };
            _commonappRepository.ApproveLog(model);
        }

        public double? GetQtyInHand(string itemId, int locId, int docId)
        {
            var qty = _icledgRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.ItemID == itemId && o.LocID == locId).Select(x => x.Qty).Sum();
            if (docId > 0)
            {
                //qty = _icledgRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.ItemID == itemId && o.DocNo != docId
                //&& o.LocID == locId).Select(x => x.Qty).Sum();
                qty = qty+ Convert.ToDouble(_transferDetailRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.ItemId == itemId
              && o.DocNo == docId).Select(x => x.Qty).Sum());

            }

            return qty;
        }

        public int GetDocId()
        {
            var result = _transferRepository.GetAll().DefaultIfEmpty().Max(o => o.DocNo);
            return result = result + 1;
        }

        public async Task<FileDto> GetDataToExcel(GetAllTransfersInput input)
        {
            var filteredTransfers = _transferRepository.GetAll()
                                 .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Narration.Contains(input.Filter) || e.OrdNo.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                                 .WhereIf(input.MinFromLocIDFilter != null, e => e.FromLocID >= input.MinFromLocIDFilter)
                                 .WhereIf(input.MaxFromLocIDFilter != null, e => e.FromLocID <= input.MaxFromLocIDFilter)
                                 .WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
                                 .WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
                                 .WhereIf(input.MinDocDateFilter != null, e => e.DocDate >= input.MinDocDateFilter)
                                 .WhereIf(input.MaxDocDateFilter != null, e => e.DocDate <= input.MaxDocDateFilter)
                                 .WhereIf(!string.IsNullOrWhiteSpace(input.NarrationFilter), e => e.Narration == input.NarrationFilter)
                                 .WhereIf(input.MinToLocIDFilter != null, e => e.ToLocID >= input.MinToLocIDFilter)
                                 .WhereIf(input.MaxToLocIDFilter != null, e => e.ToLocID <= input.MaxToLocIDFilter)
                                 .WhereIf(input.MinTotalQtyFilter != null, e => e.TotalQty >= input.MinTotalQtyFilter)
                                 .WhereIf(input.MaxTotalQtyFilter != null, e => e.TotalQty <= input.MaxTotalQtyFilter)
                                 .WhereIf(input.MinTotalAmtFilter != null, e => e.TotalAmt >= input.MinTotalAmtFilter)
                                 .WhereIf(input.MaxTotalAmtFilter != null, e => e.TotalAmt <= input.MaxTotalAmtFilter)
                                 .WhereIf(input.PostedFilter > -1, e => (input.PostedFilter == 1 && e.Posted) || (input.PostedFilter == 0 && !e.Posted))
                                 .WhereIf(input.MinLinkDetIDFilter != null, e => e.LinkDetID >= input.MinLinkDetIDFilter)
                                 .WhereIf(input.MaxLinkDetIDFilter != null, e => e.LinkDetID <= input.MaxLinkDetIDFilter)
                                 .WhereIf(!string.IsNullOrWhiteSpace(input.OrdNoFilter), e => e.OrdNo == input.OrdNoFilter)
                                 .WhereIf(input.HOLDFilter > -1, e => (input.HOLDFilter == 1 && e.HOLD) || (input.HOLDFilter == 0 && !e.HOLD))
                                 .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                                 .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                                 .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                                 .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                                 .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                                 .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var query = (from o in filteredTransfers
                         select new GetTransfersForViewDto()
                         {
                             Transfer = new TransferDto
                             {
                                 FromLocID = o.FromLocID,
                                 DocNo = o.DocNo,
                                 DocDate = o.DocDate,
                                 Narration = o.Narration,
                                 ToLocID = o.ToLocID,
                                 TotalQty = o.TotalQty,
                                 TotalAmt = o.TotalAmt,
                                 Posted = o.Posted,
                                 PostedBy = o.PostedBy,
                                 PostedDate = o.PostedDate,
                                 LinkDetID = o.LinkDetID,
                                 OrdNo = o.OrdNo,
                                 HOLD = o.HOLD,
                                 AudtUser = o.AudtUser,
                                 AudtDate = o.AudtDate,
                                 CreatedBy = o.CreatedBy,
                                 CreateDate = o.CreateDate,
                                 FromLocName = _locationRepository.GetAll().Where(a => a.TenantId == AbpSession.TenantId && a.LocID == o.FromLocID).FirstOrDefault().LocName,
                                 ToLocName = _locationRepository.GetAll().Where(a => a.TenantId == AbpSession.TenantId && a.LocID == o.ToLocID).FirstOrDefault().LocName,
                                 Id = o.Id
                             }
                         });


            var dataDto = await query.ToListAsync();

            return _ItransferExcelExpoter.ExportToFile(dataDto);
        }


        public string ProcessTransfer(CreateOrEditTransferDto input)
        {
            var alertMsg = "";
            var currency = _voucherEntryAppService.GetBaseCurrency();
            var user = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            var transBook = _icSetupRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId).SingleOrDefault().TRBookID;
            var transHeader = _transferRepository.FirstOrDefault(o => o.TenantId == AbpSession.TenantId && o.DocNo == input.DocNo);
            var transDetail = _transferDetailRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.DocNo == input.DocNo);
            var icItem = _itemRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);
            string narration = "Doc No: " + transHeader.DocNo + " " + transHeader.Narration;

            transHeader.FromLocID = _inventoryGlLinkRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LocID == transHeader.FromLocID).Count() > 0 ?
           _inventoryGlLinkRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LocID == transHeader.FromLocID).FirstOrDefault().GLLocID : 0;



            List<CreateOrEditGLTRDetailDto> gltrdetailsList = new List<CreateOrEditGLTRDetailDto>();

            var transferDetailList = from o in transDetail
                                     join i in icItem on new { A = o.ItemId, B = o.TenantId } equals new { A = i.ItemId, B = i.TenantId }
                                     group o by new { i.Seg1Id } into gd
                                     select new TransferDetailDto
                                     {
                                         Amount = gd.Sum(x => x.Amount),
                                         ItemID = gd.Key.Seg1Id,
                                     };

            foreach (var item in transferDetailList)
            {
                var caID = _inventoryGlLinkRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LocID == transHeader.FromLocID && o.SegID == item.ItemID).FirstOrDefault().AccRec;
                var daID = _inventoryGlLinkRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LocID == transHeader.FromLocID && o.SegID == item.ItemID).FirstOrDefault().AccRec;
                //Credit Amount
                gltrdetailsList.Add(new CreateOrEditGLTRDetailDto
                {
                    Amount = -Convert.ToDouble(item.Amount),
                    AccountID = caID,
                    Narration = narration,
                    SubAccID = 0,
                    LocId = transHeader.FromLocID,
                    IsAuto = true
                });

                //Debit Amount
                gltrdetailsList.Add(new CreateOrEditGLTRDetailDto
                {
                    Amount = Convert.ToDouble(item.Amount),
                    AccountID = daID,
                    Narration = narration,
                    SubAccID = 0,
                    LocId = Convert.ToInt32(transHeader.ToLocID),
                    IsAuto = false
                });
            }

            VoucherEntryDto autoEntry = new VoucherEntryDto()
            {
                GLTRHeader = new CreateOrEditGLTRHeaderDto
                {
                    BookID = transBook,
                    NARRATION = narration,
                    DocDate = Convert.ToDateTime(input.DocDate),
                    DocMonth = Convert.ToDateTime(input.DocDate).Month,
                    Approved = true,
                    AprovedBy = user,
                    AprovedDate = DateTime.Now,
                    Posted = false,
                    LocId = transHeader.FromLocID,
                    CreatedBy = user,
                    CreatedOn = DateTime.Now,
                    AuditUser = user,
                    AuditTime = DateTime.Now,
                    CURID = currency.Id,
                    CURRATE = currency.CurrRate,
                    ConfigID = 0
                },
                GLTRDetail = gltrdetailsList
            };

            if (autoEntry.GLTRDetail.Count() > 0 && autoEntry.GLTRHeader != null)
            {
                var voucher = _voucherEntryAppService.ProcessVoucherEntry(autoEntry);
                transHeader.Posted = true;
                transHeader.PostedBy = user;
                transHeader.PostedDate = DateTime.Now;
                transHeader.LinkDetID = voucher[0].Id;
                var transh = _transferRepository.FirstOrDefault((int)transHeader.Id);
                ObjectMapper.Map(transHeader, transh);

                alertMsg = "Save";
            }
            else
            {
                alertMsg = "NoRecord";
            }
            return alertMsg;
        }
        public PagedResultDto<TransferDto> GetDetailsForApproval(string fromDate, string toDate, string Mode, int fromDoc, int ToDoc, GetAllGatePassesInput input)
        {
            IQueryable<Transfer> data = null;
            if (Mode == "Posting" || Mode == "UnApproval")
            {
                data = _transferRepository.GetAll().Where(o => o.DocDate.Date >= Convert.ToDateTime(fromDate).Date
             && o.DocDate.Date <= Convert.ToDateTime(toDate).Date && o.Approved == true
             //&& o.Posted == false 
             && o.TenantId == AbpSession.TenantId && o.DocNo >= fromDoc && o.DocNo <= ToDoc);
            }
            else
            {
                data = _transferRepository.GetAll().Where(o => o.DocDate.Date >= Convert.ToDateTime(fromDate).Date
                            && o.DocDate.Date <= Convert.ToDateTime(toDate).Date && o.Approved == false && o.TenantId == AbpSession.TenantId && o.DocNo >= fromDoc && o.DocNo <= ToDoc);
            }

            var pageData = data.PageBy(input);

            var paginatedData = from o in pageData
                                select new TransferDto()
                                {
                                    //LocID = o.LocID,
                                    DocNo = o.DocNo,
                                    DocDate = o.DocDate,
                                    AudtUser = o.AudtUser,
                                    Narration = o.Narration,
                                    Id = o.Id
                                };

            var count = data.Count();
            // return ICOPNHeaderDtoList;
            return new PagedResultDto<TransferDto>(
              count,
              paginatedData.ToList()
          );
        }

        public void ApprovalData(int[] postedData, string Mode, bool bit)
        {
            try
            {
                var postedDataIds = postedData.Distinct();
                // foreach (var data in postedDataIds)
                //  {
                //   var result = _icOPNHeaderRepository.GetAll().Where(o => o.Id == data).ToList();
                // var gltrHeader = await _icOPNHeaderRepository.FirstOrDefaultAsync((int)data);

                // foreach (var res in result)
                // {
                var DocNo = 0;
                if (Mode == "Posting")
                {
                    (from a in _transferRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && postedDataIds.Contains(o.DocNo))
                     select a).ToList().ForEach(x =>
                     {
                         x.Posted = true;
                         x.PostedDate = DateTime.Now;
                         x.PostedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                         DocNo = x.DocNo;

                     });
                    //res.Posted = bit;
                    //res.PostedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                    //res.PostedDate = DateTime.Now;
                    //_repository.Update(res);
                }
                else if (Mode == "UnApproval")
                {
                    (from a in _transferRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && postedDataIds.Contains(o.DocNo))
                     select a).ToList().ForEach(x =>
                     {
                         x.Approved = false;
                         x.ApprovedDate = DateTime.Now;
                         x.ApprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                         DocNo = x.DocNo;
                     });
                    //res.Approved = false;
                    //res.AprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                    //res.AprovedDate = DateTime.Now;
                    //_icOPNHeaderRepository.Update(res);
                    //_transferRepository.Update();
                }
                else
                {
                    (from a in _transferRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && postedDataIds.Contains(o.DocNo))
                     select a).ToList().ForEach(x =>
                     {
                         x.Approved = true;
                         x.ApprovedDate = DateTime.Now;
                         x.ApprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                         DocNo = x.DocNo;
                     });
                    //res.approved = bit;
                    //res.aprovedby = _userrepository.getall().where(o => o.id == abpsession.userid).singleordefault().username;
                    //res.aproveddate = datetime.now;
                    //_icopnheaderrepository.update(res);
                }
                LogModel Log = new LogModel()
                {
                    Status = bit,
                    ApprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName,
                    Detid = Convert.ToInt32(postedDataIds.FirstOrDefault().ToString()),
                    DocNo = DocNo,
                    FormName = "Tranfer",
                    Action = Mode,
                    TenantId = AbpSession.TenantId
                };
                _commonappRepository.ApproveLog(Log);
                //  await _repository.SaveChangesAsync();
                //  }
                // }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}