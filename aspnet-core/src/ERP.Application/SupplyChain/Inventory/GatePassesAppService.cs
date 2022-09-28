

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
using ERP.SupplyChain.Inventory.IC_Item;
using ERP.SupplyChain.Inventory.Exporting;
using ERP.GeneralLedger.SetupForms;

namespace ERP.SupplyChain.Inventory
{
    [AbpAuthorize(AppPermissions.Inventory_GatePasses)]
    public class GatePassesAppService : ERPAppServiceBase, IGatePassesAppService
    {
        private readonly IRepository<GatePass> _gatePassRepository;
        private readonly IRepository<GatePassDetail> _gatePassDetailRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<ICItem> _itemRepository;
        private readonly IRepository<ChartofControl, string> _accountRepository;
        private readonly IRepository<AccountSubLedger> _accountSubLedgerRepository;
        private readonly IGatePassExcelExporter _IgatePassExcelExpoter;
        public GatePassesAppService(IRepository<GatePass> gatePassRepository,
            IRepository<User, long> userRepository,
            IRepository<GatePassDetail> gatePassDetailRepository,
            IRepository<ICItem> itemRepository,
            IGatePassExcelExporter IgatePassExcelExpoter,
            IRepository<ChartofControl, string> accountRepository,
            IRepository<AccountSubLedger> accountSubLedgerRepository
            )
        {
            _gatePassRepository = gatePassRepository;
            _userRepository = userRepository;
            _gatePassDetailRepository = gatePassDetailRepository;
            _itemRepository = itemRepository;
            _IgatePassExcelExpoter = IgatePassExcelExpoter;
            _accountRepository = accountRepository;
            _accountSubLedgerRepository = accountSubLedgerRepository;
        }

        public async Task<PagedResultDto<GetGatePassForViewDto>> GetAll(GetAllGatePassesInput input)
        {
            IQueryable<GatePass> filteredGatePasses;
            if ((input.Filter != "inward" && input.Filter != "outward") && input.Filter != null)
            {
                filteredGatePasses = _gatePassRepository.GetAll()
                       .Where(o => o.TenantId == AbpSession.TenantId && o.DocNo.ToString() == input.Filter
                       );
            }
            else if (input.Filter == "inward" || input.Filter == "outward")
            {
                filteredGatePasses = _gatePassRepository.GetAll()
                       .WhereIf(input.Filter == "inward" && input.Filter != null, e => e.TypeID == 1)
                       .WhereIf(input.Filter == "outward" && input.Filter != null, e => e.TypeID == 2)
                       .Where(o => o.TenantId == AbpSession.TenantId);
            }
            else
            {
                filteredGatePasses = _gatePassRepository.GetAll()
                        .Where(o => o.TenantId == AbpSession.TenantId);
            }


            var pagedAndFilteredGatePasses = filteredGatePasses
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var gatePasses = from o in pagedAndFilteredGatePasses
                             select new GetGatePassForViewDto()
                             {
                                 GatePass = new GatePassDto
                                 {
                                     TypeID = o.TypeID,
                                     DocNo = o.DocNo,
                                     DocDate = o.DocDate,
                                     AccountID = o.AccountID,
                                     PartyID = o.PartyID,
                                     Narration = o.Narration,
                                     GPType = o.GPType,
                                     DriverName = o.DriverName,
                                     VehicleNo = o.VehicleNo,
                                     GPDocNo = o.GPDocNo,
                                     AudtUser = o.AudtUser,
                                     AudtDate = o.AudtDate,
                                     CreatedBy = o.CreatedBy,
                                     CreateDate = o.CreateDate,
                                     Id = o.Id
                                 }
                             };

            var totalCount = await filteredGatePasses.CountAsync();

            return new PagedResultDto<GetGatePassForViewDto>(
                totalCount,
                await gatePasses.ToListAsync()
            );
        }
        public async Task<PagedResultDto<GetGatePassForViewDto>> GetAllInward(GetAllGatePassesInput input)
        {

            var filteredGatePasses = _gatePassRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.AccountID.Contains(input.Filter) || e.Narration.Contains(input.Filter) || e.DriverName.Contains(input.Filter) || e.VehicleNo.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinTypeIDFilter != null, e => e.TypeID >= input.MinTypeIDFilter)
                        .WhereIf(input.MaxTypeIDFilter != null, e => e.TypeID <= input.MaxTypeIDFilter)
                        .WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
                        .WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
                        .WhereIf(input.MinDocDateFilter != null, e => e.DocDate >= input.MinDocDateFilter)
                        .WhereIf(input.MaxDocDateFilter != null, e => e.DocDate <= input.MaxDocDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AccountIDFilter), e => e.AccountID == input.AccountIDFilter)
                        .WhereIf(input.MinPartyIDFilter != null, e => e.PartyID >= input.MinPartyIDFilter)
                        .WhereIf(input.MaxPartyIDFilter != null, e => e.PartyID <= input.MaxPartyIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NarrationFilter), e => e.Narration == input.NarrationFilter)
                        .WhereIf(input.MinGPTypeFilter != null, e => e.GPType >= input.MinGPTypeFilter)
                        .WhereIf(input.MaxGPTypeFilter != null, e => e.GPType <= input.MaxGPTypeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DriverNameFilter), e => e.DriverName == input.DriverNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VehicleNoFilter), e => e.VehicleNo == input.VehicleNoFilter)
                        .WhereIf(input.MinGPDocNoFilter != null, e => e.GPDocNo >= input.MinGPDocNoFilter)
                        .WhereIf(input.MaxGPDocNoFilter != null, e => e.GPDocNo <= input.MaxGPDocNoFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter)
                        .Where(o => o.TypeID == 1)
                        .Where(o => o.TenantId == AbpSession.TenantId);
            var pagedAndFilteredGatePasses = filteredGatePasses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var gatePasses = from o in pagedAndFilteredGatePasses
                             select new GetGatePassForViewDto()
                             {
                                 GatePass = new GatePassDto
                                 {
                                     TypeID = o.TypeID,
                                     DocNo = o.DocNo,
                                     DocDate = o.DocDate,
                                     AccountID = o.AccountID,
                                     PartyID = o.PartyID,
                                     Narration = o.Narration,
                                     GPType = o.GPType,
                                     DriverName = o.DriverName,
                                     VehicleNo = o.VehicleNo,
                                     GPDocNo = o.GPDocNo,
                                     AudtUser = o.AudtUser,
                                     AudtDate = o.AudtDate,
                                     CreatedBy = o.CreatedBy,
                                     CreateDate = o.CreateDate,
                                     Id = o.Id
                                 }
                             };

            var totalCount = await filteredGatePasses.CountAsync();

            return new PagedResultDto<GetGatePassForViewDto>(
                totalCount,
                await gatePasses.ToListAsync()
            );
        }

        public async Task<PagedResultDto<GetGatePassForViewDto>> GetAllOutward(GetAllGatePassesInput input)
        {

            var filteredGatePasses = _gatePassRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.AccountID.Contains(input.Filter) || e.Narration.Contains(input.Filter) || e.DriverName.Contains(input.Filter) || e.VehicleNo.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinTypeIDFilter != null, e => e.TypeID >= input.MinTypeIDFilter)
                        .WhereIf(input.MaxTypeIDFilter != null, e => e.TypeID <= input.MaxTypeIDFilter)
                        .WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
                        .WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
                        .WhereIf(input.MinDocDateFilter != null, e => e.DocDate >= input.MinDocDateFilter)
                        .WhereIf(input.MaxDocDateFilter != null, e => e.DocDate <= input.MaxDocDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AccountIDFilter), e => e.AccountID == input.AccountIDFilter)
                        .WhereIf(input.MinPartyIDFilter != null, e => e.PartyID >= input.MinPartyIDFilter)
                        .WhereIf(input.MaxPartyIDFilter != null, e => e.PartyID <= input.MaxPartyIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NarrationFilter), e => e.Narration == input.NarrationFilter)
                        .WhereIf(input.MinGPTypeFilter != null, e => e.GPType >= input.MinGPTypeFilter)
                        .WhereIf(input.MaxGPTypeFilter != null, e => e.GPType <= input.MaxGPTypeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DriverNameFilter), e => e.DriverName == input.DriverNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VehicleNoFilter), e => e.VehicleNo == input.VehicleNoFilter)
                        .WhereIf(input.MinGPDocNoFilter != null, e => e.GPDocNo >= input.MinGPDocNoFilter)
                        .WhereIf(input.MaxGPDocNoFilter != null, e => e.GPDocNo <= input.MaxGPDocNoFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter)
                        .Where(o => o.TypeID == 2)
                        .Where(o => o.TenantId == AbpSession.TenantId);

            var pagedAndFilteredGatePasses = filteredGatePasses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var gatePasses = from o in pagedAndFilteredGatePasses
                             select new GetGatePassForViewDto()
                             {
                                 GatePass = new GatePassDto
                                 {
                                     TypeID = o.TypeID,
                                     DocNo = o.DocNo,
                                     DocDate = o.DocDate,
                                     AccountID = o.AccountID,
                                     PartyID = o.PartyID,
                                     Narration = o.Narration,
                                     GPType = o.GPType,
                                     DriverName = o.DriverName,
                                     VehicleNo = o.VehicleNo,
                                     GPDocNo = o.GPDocNo,
                                     AudtUser = o.AudtUser,
                                     AudtDate = o.AudtDate,
                                     CreatedBy = o.CreatedBy,
                                     CreateDate = o.CreateDate,
                                     Id = o.Id
                                 }
                             };

            var totalCount = await filteredGatePasses.CountAsync();

            return new PagedResultDto<GetGatePassForViewDto>(
                totalCount,
                await gatePasses.ToListAsync()
            );
        }
        public async Task<GetGatePassForViewDto> GetGatePassForView(int id)
        {
            var gatePass = await _gatePassRepository.GetAsync(id);

            var output = new GetGatePassForViewDto { GatePass = ObjectMapper.Map<GatePassDto>(gatePass) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Inventory_GatePasses_Edit)]
        public async Task<GetGatePassForEditOutput> GetGatePassForEdit(EntityDto input, string type)
        {
            var itemList = await _itemRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId).ToListAsync();
            var gatePass = await _gatePassRepository.FirstOrDefaultAsync(input.Id);
            var gatePassDetail = await _gatePassDetailRepository.GetAll().Where(o => o.DetID == input.Id).ToListAsync();
            var output = new GetGatePassForEditOutput { GatePass = ObjectMapper.Map<CreateOrEditGatePassDto>(gatePass) };
            output.GatePass.AccountName = _accountRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == gatePass.AccountID).Count() > 0 ?
                _accountRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == gatePass.AccountID).FirstOrDefault().AccountName : "";
            output.GatePass.PartyName = _accountSubLedgerRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.AccountID == output.GatePass.AccountID && o.Id == Convert.ToInt32(output.GatePass.PartyID)).Count() > 0 ?
                _accountSubLedgerRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.AccountID == output.GatePass.AccountID && o.Id == Convert.ToInt32(output.GatePass.PartyID)).FirstOrDefault().SubAccName : "";
            output.GatePass.gatePassDetailDto = ObjectMapper.Map<List<GatePassDetailDto>>(gatePassDetail);
            foreach (var data in output.GatePass.gatePassDetailDto)
            {
                var item = itemList.Where(o => o.ItemId == data.ItemID);
                var itemId = itemList.Where(o => o.ItemId == data.ItemID).FirstOrDefault().ItemId;
                output.GatePass.gatePassDetailDto.Where(o => o.Id == data.Id).FirstOrDefault().ItemID = item.FirstOrDefault().ItemId + "*" + item.FirstOrDefault().Descp + "*" + data.Unit + "*" + data.Conver;
                if (type == "OGP")
                {
                    var inwardItemQty = from a in _gatePassRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                                        join
                                        b in _gatePassDetailRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                                        on new { A = a.Id, B = a.TenantId } equals new { A = b.DetID, B = b.TenantId }
                                        where (b.ItemID == itemId && a.TypeID == 1 && a.GPDocNo == gatePass.DocNo && a.TenantId == AbpSession.TenantId)
                                        group b by b.ItemID into g
                                        select new
                                        {
                                            qty = g.Select(x => x.Qty).Sum(),

                                        }.qty;
                    //var outwardItemQty = from a in _gatePassRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                    //                     join
                    //                     b in _gatePassDetailRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                    //                     on new { A = a.Id, B = a.TenantId } equals new { A = b.DetID, B = b.TenantId }
                    //                     where (b.ItemID == itemId && a.Id == data.DetID && a.TenantId == AbpSession.TenantId)
                    //                     group b by b.ItemID into g
                    //                     select new
                    //                     {
                    //                         qty = g.Select(x => x.Qty).Sum(),

                    //                     }.qty;
                    output.GatePass.gatePassDetailDto.Where(o => o.Id == data.Id).FirstOrDefault().Qty = data.Qty - inwardItemQty.FirstOrDefault();
                }
                else if (type == "IGP")
                {
                    //var inwardItemQty = from a in _gatePassRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                    //                    join
                    //                    b in _gatePassDetailRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                    //                    on new { A = a.Id, B = a.TenantId } equals new { A = b.DetID, B = b.TenantId }
                    //                    where (b.ItemID == itemId && a.TypeID == 1 && a.DocNo == gatePass.DocNo && a.TenantId == AbpSession.TenantId)
                    //                    group b by b.ItemID into g
                    //                    select new
                    //                    {
                    //                        qty = g.Select(x => x.Qty).Sum(),

                    //                    }.qty;
                    var outwardItemQty = from a in _gatePassRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                                         join
                                         b in _gatePassDetailRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                                         on new { A = a.Id, B = a.TenantId } equals new { A = b.DetID, B = b.TenantId }
                                         where (b.ItemID == itemId && a.TypeID == 2 && a.GPDocNo == gatePass.DocNo && a.TenantId == AbpSession.TenantId)
                                         group b by b.ItemID into g
                                         select new
                                         {
                                             qty = g.Select(x => x.Qty).Sum(),

                                         }.qty;
                    output.GatePass.gatePassDetailDto.Where(o => o.Id == data.Id).FirstOrDefault().Qty = data.Qty - outwardItemQty.FirstOrDefault();

                }
            }
            return output;
        }


        public async Task CreateOrEdit(CreateOrEditGatePassDto input)
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

        [AbpAuthorize(AppPermissions.Inventory_GatePasses_Create)]
        protected virtual async Task Create(CreateOrEditGatePassDto input)
        {
            var gatePass = ObjectMapper.Map<GatePass>(input);

            gatePass.AudtUser = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            gatePass.CreatedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            gatePass.AudtDate = DateTime.Now;
            gatePass.CreateDate = DateTime.Now;
            gatePass.DocNo = GetDocIdAgainstGatePassType(gatePass.TypeID);
            if (AbpSession.TenantId != null)
            {
                gatePass.TenantId = (int)AbpSession.TenantId;
                //gatePassDetail.TenantId = (int)AbpSession.TenantId;
            }
            var getId = await _gatePassRepository.InsertAndGetIdAsync(gatePass);

            foreach (var data in input.gatePassDetailDto)
            {
                var gatePassDetail = ObjectMapper.Map<GatePassDetail>(data);
                if (AbpSession.TenantId != null)
                {
                    gatePassDetail.TenantId = (int)AbpSession.TenantId;
                }
                gatePassDetail.DetID = getId;
                if (gatePassDetail.ItemID != null)
                {
                    await _gatePassDetailRepository.InsertAsync(gatePassDetail);
                }

            }

        }

        [AbpAuthorize(AppPermissions.Inventory_GatePasses_Edit)]
        protected virtual async Task Update(CreateOrEditGatePassDto input)
        {
            var gatePass = await _gatePassRepository.FirstOrDefaultAsync((int)input.Id);
            input.AudtUser = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            input.AudtDate = DateTime.Now;
            input.CreatedBy = gatePass.CreatedBy;
            input.CreateDate = gatePass.CreateDate;
            ObjectMapper.Map(input, gatePass);
            await _gatePassDetailRepository.DeleteAsync(o => o.TenantId == AbpSession.TenantId && o.DetID == input.Id);
            foreach (var data in input.gatePassDetailDto)
            {
                var gatePassDetail = ObjectMapper.Map<GatePassDetail>(data);
                if (AbpSession.TenantId != null)
                {
                    gatePassDetail.TenantId = (int)AbpSession.TenantId;
                }
                gatePassDetail.DetID = (int)input.Id;
                if (gatePassDetail.ItemID != null)
                {
                    await _gatePassDetailRepository.InsertAsync(gatePassDetail);
                }
            }
        }


        public double GetQtyAgainstItem(string itemId, int gpDocNo, int typeId, int id)
        {
            var maxQty = from a in _gatePassRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                         join b in _gatePassDetailRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                         on new { A = a.TenantId, B = a.Id } equals new { A = b.TenantId, B = b.DetID }
                         where (b.ItemID == itemId && a.DocNo == gpDocNo && a.TypeID != typeId && a.Id != id)
                         select new
                         {
                             b.Qty
                         };

            var qty = from a in _gatePassRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                      join b in _gatePassDetailRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                      on new { A = a.TenantId, B = a.Id } equals new { A = b.TenantId, B = b.DetID }
                      where (b.ItemID == itemId && a.GPDocNo == gpDocNo && a.TypeID == typeId && a.Id != id)
                      group b by b.ItemID into g
                      select new
                      {
                          qty = g.Select(x => x.Qty).Sum()
                      };
            return maxQty.Count() > 0 ? (maxQty.FirstOrDefault().Qty - (qty.Count() > 0 ?
                qty.FirstOrDefault().qty : 0)) : 0;

        }

        [AbpAuthorize(AppPermissions.Inventory_GatePasses_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _gatePassRepository.DeleteAsync(input.Id);
            await _gatePassDetailRepository.DeleteAsync(o => o.TenantId == AbpSession.TenantId && o.DetID == input.Id);
        }

        public int GetDocIdAgainstGatePassType(int typeId)
        {
            var result = _gatePassRepository.GetAllIncluding().Where(o => o.TypeID == typeId && o.TenantId == AbpSession.TenantId);
            if (result.Count() > 0)
                return _gatePassRepository.GetAllIncluding().Where(o => o.TypeID == typeId && o.TenantId == AbpSession.TenantId).Max(o => o.DocNo) + 1;
            else
                return 1;
        }

        public async Task<PagedResultDto<GetGatePassForViewDto>> GetOGPNo(GetAllGatePassesInput input)
        {
            List<int> ogpWithLeftQty = new List<int>();
            var gatePassesData = _gatePassRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.TypeID == 2 && o.GPType == 1);
            foreach (var data in gatePassesData)
            {
                var itemDetails = _gatePassDetailRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.DetID == data.Id);
                foreach (var item in itemDetails)
                {
                    var inwardItemQty = from a in _gatePassRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                                        join
                                        b in _gatePassDetailRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                                        on new { A = a.Id, B = a.TenantId } equals new { A = b.DetID, B = b.TenantId }
                                        where (a.TypeID == 1 && b.ItemID == item.ItemID && a.GPDocNo == data.DocNo)
                                        group b by b.ItemID into g
                                        select new
                                        {
                                            qty = g.Select(x => x.Qty).Sum(),

                                        };
                    //var outwardItemQty = from a in _gatePassRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                    //                     join
                    //                     b in _gatePassDetailRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                    //                     on new { A = a.Id, B = a.TenantId } equals new { A = b.DetID, B = b.TenantId }
                    //                     where (a.TypeID == 2 && b.ItemID == item.ItemID && a.Id != data.Id && a.GPDocNo == data.DocNo)
                    //                     group b by b.ItemID into g
                    //                     select new
                    //                     {
                    //                         qty = g.Select(x => x.Qty).Sum(),

                    //                     };
                    var qty = inwardItemQty.Count() == 0 ? 0 : inwardItemQty.FirstOrDefault().qty;
                    if (Convert.ToInt32(qty) != Convert.ToInt32(item.Qty))
                        ogpWithLeftQty.Add(item.DetID);
                }
            }

            var filteredGatePasses = _gatePassRepository.GetAll()
                       .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.AccountID.Contains(input.Filter) || e.Narration.Contains(input.Filter) || e.DriverName.Contains(input.Filter) || e.VehicleNo.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                       .WhereIf(input.MinTypeIDFilter != null, e => e.TypeID >= input.MinTypeIDFilter)
                       .WhereIf(input.MaxTypeIDFilter != null, e => e.TypeID <= input.MaxTypeIDFilter)
                       .WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
                       .WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
                       .WhereIf(input.MinDocDateFilter != null, e => e.DocDate >= input.MinDocDateFilter)
                       .WhereIf(input.MaxDocDateFilter != null, e => e.DocDate <= input.MaxDocDateFilter)
                       .WhereIf(!string.IsNullOrWhiteSpace(input.AccountIDFilter), e => e.AccountID == input.AccountIDFilter)
                       .WhereIf(input.MinPartyIDFilter != null, e => e.PartyID >= input.MinPartyIDFilter)
                       .WhereIf(input.MaxPartyIDFilter != null, e => e.PartyID <= input.MaxPartyIDFilter)
                       .WhereIf(!string.IsNullOrWhiteSpace(input.NarrationFilter), e => e.Narration == input.NarrationFilter)
                       .WhereIf(input.MinGPTypeFilter != null, e => e.GPType >= input.MinGPTypeFilter)
                       .WhereIf(input.MaxGPTypeFilter != null, e => e.GPType <= input.MaxGPTypeFilter)
                       .WhereIf(!string.IsNullOrWhiteSpace(input.DriverNameFilter), e => e.DriverName == input.DriverNameFilter)
                       .WhereIf(!string.IsNullOrWhiteSpace(input.VehicleNoFilter), e => e.VehicleNo == input.VehicleNoFilter)
                       .WhereIf(input.MinGPDocNoFilter != null, e => e.GPDocNo >= input.MinGPDocNoFilter)
                       .WhereIf(input.MaxGPDocNoFilter != null, e => e.GPDocNo <= input.MaxGPDocNoFilter)
                       .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                       .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                       .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                       .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                       .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                       .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter).Where(e => ogpWithLeftQty.Contains(e.Id));


            var pagedAndFilteredGatePasses = filteredGatePasses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var gatePasses = from o in pagedAndFilteredGatePasses
                             select new GetGatePassForViewDto()
                             {
                                 GatePass = new GatePassDto
                                 {
                                     TypeID = o.TypeID,
                                     DocNo = o.DocNo,
                                     DocDate = o.DocDate,
                                     AccountID = o.AccountID,
                                     AccountName = _accountRepository.GetAll().Where(p => p.TenantId == AbpSession.TenantId
                                     && p.Id == o.AccountID
                                     ).Count() > 0 ? _accountRepository.GetAll().Where(p => p.TenantId == AbpSession.TenantId
                                      && p.Id == o.AccountID
                                     ).FirstOrDefault().AccountName : ""
                                     ,
                                     PartyID = o.PartyID,
                                     PartyName = _accountSubLedgerRepository.GetAll().Where(p => p.TenantId == AbpSession.TenantId
                                     && p.Id == o.PartyID
                                     ).Count() > 0 ? _accountSubLedgerRepository.GetAll().Where(p => p.TenantId == AbpSession.TenantId
                                     && p.Id == o.PartyID
                                     ).FirstOrDefault().SubAccName : ""
                                     ,
                                     Narration = o.Narration,
                                     GPType = o.GPType,
                                     DriverName = o.DriverName,
                                     VehicleNo = o.VehicleNo,
                                     GPDocNo = o.GPDocNo,
                                     AudtUser = o.AudtUser,
                                     AudtDate = o.AudtDate,
                                     CreatedBy = o.CreatedBy,
                                     CreateDate = o.CreateDate,
                                     Id = o.Id
                                 }
                             };

            var totalCount = await filteredGatePasses.CountAsync();

            return new PagedResultDto<GetGatePassForViewDto>(
                totalCount,
                await gatePasses.ToListAsync()
            );
        }
        public async Task<PagedResultDto<GetGatePassForViewDto>> GetIGPNo(GetAllGatePassesInput input)
        {
            List<int> igpWithLeftQty = new List<int>();
            var gatePassesData = _gatePassRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.TypeID == 1 && o.GPType == 1);
            foreach (var data in gatePassesData)
            {
                var itemDetails = _gatePassDetailRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.DetID == data.Id);
                foreach (var item in itemDetails)
                {
                    //var inwardItemQty = from a in _gatePassRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                    //                    join
                    //                    b in _gatePassDetailRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                    //                    on new { A = a.Id, B = a.TenantId } equals new { A = b.DetID, B = b.TenantId }
                    //                    where (a.TypeID == 1 && b.ItemID == item.ItemID && a.Id != data.Id && a.GPDocNo == data.DocNo)
                    //                    group b by b.ItemID into g
                    //                    select new
                    //                    {
                    //                        qty = g.Select(x => x.Qty).Sum(),

                    //                    };
                    var outwardItemQty = from a in _gatePassRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                                         join
                                         b in _gatePassDetailRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                                         on new { A = a.Id, B = a.TenantId } equals new { A = b.DetID, B = b.TenantId }
                                         where (a.TypeID == 2 && b.ItemID == item.ItemID && a.GPDocNo == data.DocNo)
                                         group b by b.ItemID into g
                                         select new
                                         {
                                             qty = g.Select(x => x.Qty).Sum(),

                                         };
                    var qty = outwardItemQty.Count() == 0 ? 0 : outwardItemQty.FirstOrDefault().qty;
                    if (Convert.ToInt32(qty) != Convert.ToInt32(item.Qty))
                        igpWithLeftQty.Add(item.DetID);
                }
            }

            var filteredGatePasses = _gatePassRepository.GetAll()
                       .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.AccountID.Contains(input.Filter) || e.Narration.Contains(input.Filter) || e.DriverName.Contains(input.Filter) || e.VehicleNo.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                       .WhereIf(input.MinTypeIDFilter != null, e => e.TypeID >= input.MinTypeIDFilter)
                       .WhereIf(input.MaxTypeIDFilter != null, e => e.TypeID <= input.MaxTypeIDFilter)
                       .WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
                       .WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
                       .WhereIf(input.MinDocDateFilter != null, e => e.DocDate >= input.MinDocDateFilter)
                       .WhereIf(input.MaxDocDateFilter != null, e => e.DocDate <= input.MaxDocDateFilter)
                       .WhereIf(!string.IsNullOrWhiteSpace(input.AccountIDFilter), e => e.AccountID == input.AccountIDFilter)
                       .WhereIf(input.MinPartyIDFilter != null, e => e.PartyID >= input.MinPartyIDFilter)
                       .WhereIf(input.MaxPartyIDFilter != null, e => e.PartyID <= input.MaxPartyIDFilter)
                       .WhereIf(!string.IsNullOrWhiteSpace(input.NarrationFilter), e => e.Narration == input.NarrationFilter)
                       .WhereIf(input.MinGPTypeFilter != null, e => e.GPType >= input.MinGPTypeFilter)
                       .WhereIf(input.MaxGPTypeFilter != null, e => e.GPType <= input.MaxGPTypeFilter)
                       .WhereIf(!string.IsNullOrWhiteSpace(input.DriverNameFilter), e => e.DriverName == input.DriverNameFilter)
                       .WhereIf(!string.IsNullOrWhiteSpace(input.VehicleNoFilter), e => e.VehicleNo == input.VehicleNoFilter)
                       .WhereIf(input.MinGPDocNoFilter != null, e => e.GPDocNo >= input.MinGPDocNoFilter)
                       .WhereIf(input.MaxGPDocNoFilter != null, e => e.GPDocNo <= input.MaxGPDocNoFilter)
                       .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                       .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                       .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                       .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                       .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                       .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter).Where(e => igpWithLeftQty.Contains(e.Id));


            var pagedAndFilteredGatePasses = filteredGatePasses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var gatePasses = from o in pagedAndFilteredGatePasses
                             select new GetGatePassForViewDto()
                             {
                                 GatePass = new GatePassDto
                                 {
                                     TypeID = o.TypeID,
                                     DocNo = o.DocNo,
                                     DocDate = o.DocDate,
                                     AccountID = o.AccountID,
                                     AccountName = _accountRepository.GetAll().Where(p => p.TenantId == AbpSession.TenantId
                                     && p.Id == o.AccountID
                                     ).Count() > 0 ? _accountRepository.GetAll().Where(p => p.TenantId == AbpSession.TenantId
                                      && p.Id == o.AccountID
                                     ).FirstOrDefault().AccountName : ""
                                     ,
                                     PartyID = o.PartyID,
                                     PartyName = _accountSubLedgerRepository.GetAll().Where(p => p.TenantId == AbpSession.TenantId
                                     && p.Id == o.PartyID
                                     ).Count() > 0 ? _accountSubLedgerRepository.GetAll().Where(p => p.TenantId == AbpSession.TenantId
                                     && p.Id == o.PartyID
                                     ).FirstOrDefault().SubAccName : ""
                                     ,
                                     Narration = o.Narration,
                                     GPType = o.GPType,
                                     DriverName = o.DriverName,
                                     VehicleNo = o.VehicleNo,
                                     GPDocNo = o.GPDocNo,
                                     AudtUser = o.AudtUser,
                                     AudtDate = o.AudtDate,
                                     CreatedBy = o.CreatedBy,
                                     CreateDate = o.CreateDate,
                                     Id = o.Id
                                 }
                             };

            var totalCount = await filteredGatePasses.CountAsync();

            return new PagedResultDto<GetGatePassForViewDto>(
                totalCount,
                await gatePasses.ToListAsync()
            );
        }
        public async Task<FileDto> GetDataToExcel(GetAllGatePassesInput input)
        {
            var filteredGatePasses = _gatePassRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.AccountID.Contains(input.Filter) || e.Narration.Contains(input.Filter) || e.DriverName.Contains(input.Filter) || e.VehicleNo.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                        .WhereIf(input.MinTypeIDFilter != null, e => e.TypeID >= input.MinTypeIDFilter)
                        .WhereIf(input.MaxTypeIDFilter != null, e => e.TypeID <= input.MaxTypeIDFilter)
                        .WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
                        .WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
                        .WhereIf(input.MinDocDateFilter != null, e => e.DocDate >= input.MinDocDateFilter)
                        .WhereIf(input.MaxDocDateFilter != null, e => e.DocDate <= input.MaxDocDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AccountIDFilter), e => e.AccountID == input.AccountIDFilter)
                        .WhereIf(input.MinPartyIDFilter != null, e => e.PartyID >= input.MinPartyIDFilter)
                        .WhereIf(input.MaxPartyIDFilter != null, e => e.PartyID <= input.MaxPartyIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NarrationFilter), e => e.Narration == input.NarrationFilter)
                        .WhereIf(input.MinGPTypeFilter != null, e => e.GPType >= input.MinGPTypeFilter)
                        .WhereIf(input.MaxGPTypeFilter != null, e => e.GPType <= input.MaxGPTypeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DriverNameFilter), e => e.DriverName == input.DriverNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.VehicleNoFilter), e => e.VehicleNo == input.VehicleNoFilter)
                        .WhereIf(input.MinGPDocNoFilter != null, e => e.GPDocNo >= input.MinGPDocNoFilter)
                        .WhereIf(input.MaxGPDocNoFilter != null, e => e.GPDocNo <= input.MaxGPDocNoFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var query = from o in filteredGatePasses
                        select new GetGatePassForViewDto()
                        {
                            GatePass = new GatePassDto
                            {
                                TypeID = o.TypeID,
                                DocNo = o.DocNo,
                                DocDate = o.DocDate,
                                AccountID = o.AccountID,
                                PartyID = o.PartyID,
                                Narration = o.Narration,
                                GPType = o.GPType,
                                DriverName = o.DriverName,
                                VehicleNo = o.VehicleNo,
                                GPDocNo = o.GPDocNo,
                                AudtUser = o.AudtUser,
                                AudtDate = o.AudtDate,
                                CreatedBy = o.CreatedBy,
                                CreateDate = o.CreateDate,
                                Id = o.Id
                            }
                        };


            var dataDto = await query.ToListAsync();

            return _IgatePassExcelExpoter.ExportToFile(dataDto);
        }


        public PagedResultDto<GatePassDto> GetDetailsForApproval(string fromDate, string toDate, string Mode, int fromDoc, int ToDoc, GetAllGatePassesInput input)
        {
            IQueryable<GatePass> data = null;
            if (Mode == "Posting" || Mode == "UnApproval")
            {
                data = _gatePassRepository.GetAll().Where(o => o.DocDate.Value.Date >= Convert.ToDateTime(fromDate).Date
             && o.DocDate.Value.Date <= Convert.ToDateTime(toDate).Date && o.Approved == true
             //&& o.Posted == false 
             && o.TenantId == AbpSession.TenantId && o.DocNo >= fromDoc && o.DocNo <= ToDoc);
            }
            else
            {
                data = _gatePassRepository.GetAll().Where(o => o.DocDate.Value.Date >= Convert.ToDateTime(fromDate).Date
                            && o.DocDate.Value.Date <= Convert.ToDateTime(toDate).Date && o.Approved == false && o.TenantId == AbpSession.TenantId && o.DocNo >= fromDoc && o.DocNo <= ToDoc);
            }

            var pageData = data.PageBy(input);

            var paginatedData = from o in pageData
                                select new GatePassDto()
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
            return new PagedResultDto<GatePassDto>(
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
                if (Mode == "Posting")
                {
                    //res.Posted = bit;
                    //res.PostedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                    //res.PostedDate = DateTime.Now;
                    //_repository.Update(res);
                }
                else if (Mode == "UnApproval")
                {
                    (from a in _gatePassRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && postedDataIds.Contains(o.Id))
                     select a).ToList().ForEach(x =>
                     {
                         x.Approved = false;
                         x.ApprovedDate = DateTime.Now;
                         x.ApprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                     });
                    //res.Approved = false;
                    //res.AprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                    //res.AprovedDate = DateTime.Now;
                    //_icOPNHeaderRepository.Update(res);
                }
                else
                {
                    (from a in _gatePassRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && postedDataIds.Contains(o.Id))
                     select a).ToList().ForEach(x =>
                     {
                         x.Approved = true;
                         x.ApprovedDate = DateTime.Now;
                         x.ApprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                     });
                    //res.Approved = bit;
                    //res.AprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                    //res.AprovedDate = DateTime.Now;
                    //_icOPNHeaderRepository.Update(res);
                }
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