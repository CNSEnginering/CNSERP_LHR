

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.SupplyChain.Purchase.Requisition.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ERP.SupplyChain.Inventory;
using ERP.Authorization.Users;
using ERP.SupplyChain.Inventory.IC_Item;
using ERP.SupplyChain.Purchase.Exporting;
using ERP.SupplyChain.Inventory.ICOPT5;
using ERP.SupplyChain.Sales.SaleEntry;
using ERP.GeneralLedger.SetupForms;
using ERP.CommonServices.UserLoc.CSUserLocD;

namespace ERP.SupplyChain.Purchase.Requisition
{
    [AbpAuthorize(AppPermissions.Purchase_Requisitions)]
    public class RequisitionsAppService : ERPAppServiceBase, IRequisitionsAppService
    {
        private readonly IRepository<Requisitions> _requisitionRepository;
        private readonly IRepository<CostCenter> _costCenterRepository;
        private readonly IRepository<RequisitionDetail> _requisitionDetailRepository;
        private readonly IRepository<ICLocation> _locationRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<ICLEDG> _icledgRepository;
        private readonly IRepository<ICItem> _itemRepository;
        private readonly IRepository<ICOPT5> _icopT5Repository;
        private readonly IRepository<OESALEHeader> _oesaleHeaderRepository;
        private readonly IRepository<AccountSubLedger> _accountSubLedgerRepository;
        private readonly IRepository<OESALEDetail> _oesaleDetailRepository;
        private readonly IRequisitionsExcelExporter _requisitionsExcelExporter;
        private readonly CommonAppService _commonappRepository;
        private readonly IRepository<CSUserLocD> _csUserLocDRepository;

        public RequisitionsAppService(IRepository<Requisitions> requisitionRepository,
            IRepository<ICLocation> locationRepository,
            IRepository<ICOPT5> icopT5Repository,
            IRepository<RequisitionDetail> requisitionDetailRepository,
            IRepository<User, long> userRepository,
            IRepository<AccountSubLedger> accountSubLedgerRepository,
            IRepository<ICItem> itemRepository,
            IRepository<ICLEDG> icledgRepository,
            IRepository<OESALEHeader> oesaleHeaderRepository,
            IRepository<OESALEDetail> oesaleDetailRepository, IRepository<CSUserLocD> csUserLocDRepository,
             IRequisitionsExcelExporter requisitionsExcelExporter,
             CommonAppService commonappRepository,
            IRepository<CostCenter> costCenterRepository)
        {
            _requisitionRepository = requisitionRepository;
            _requisitionDetailRepository = requisitionDetailRepository;
            _locationRepository = locationRepository;
            _userRepository = userRepository;
            _oesaleHeaderRepository=oesaleHeaderRepository;
            _oesaleDetailRepository=oesaleDetailRepository;
            _accountSubLedgerRepository = accountSubLedgerRepository;
            _csUserLocDRepository = csUserLocDRepository;
            _itemRepository = itemRepository;
            _icopT5Repository = icopT5Repository;
            _icledgRepository = icledgRepository;
            _requisitionsExcelExporter = requisitionsExcelExporter;
            _costCenterRepository = costCenterRepository;
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
        public async Task<PagedResultDto<GetRequisitionForViewDto>> GetAll(GetAllRequisitionsInput input)
        {
            User currUser = await GetCurrentUserName();

            var userName = currUser.UserName;
            var usrPermissions = await GetUserPermissions(currUser);

            IQueryable<RequisitionDto> filteredRequisitions;
            filteredRequisitions = from a in _requisitionRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId).WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Narration.Contains(input.Filter) || e.DocNo.ToString() == input.Filter)
                        .WhereIf(input.MinLocIDFilter > 0, e => e.LocID >= input.MinLocIDFilter)
                        .WhereIf(input.MaxLocIDFilter > 0, e => e.LocID <= input.MaxLocIDFilter)
                        .WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
                        .WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
                        .WhereIf(input.ActiveFilter > -1, e => (input.ActiveFilter == 1 && e.Approved) || (input.ActiveFilter == 0 && !e.Approved))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.ApprovedBy == input.AudtUserFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                                   join
                                   b in _locationRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                                   on new { A = a.LocID, B = a.TenantId } equals new { A = b.LocID, B = b.TenantId }
                                   select new RequisitionDto()
                                   {
                                       LocID = b.LocID,
                                       LocName = b.LocName,
                                       DocDate = a.DocDate,
                                       ExpArrivalDate = a.ExpArrivalDate,
                                       OrdNo = a.OrdNo,
                                       CCID = a.CCID,
                                       Id = a.Id,
                                       DocNo = a.DocNo,
                                       Approved = a.Approved,
                                       Hold = a.Hold,
                                       Posted = a.Posted
                                   };
            //if (input.Filter == null)
            //{
            //    filteredRequisitions = from a in _requisitionRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
            //                           join
            //                           b in _locationRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
            //                           on new { A = a.LocID, B = a.TenantId } equals new { A = b.LocID, B = b.TenantId }
            //                           select new RequisitionDto()
            //                           {
            //                               LocID = b.LocID,
            //                               LocName = b.LocName,
            //                               DocDate = a.DocDate,
            //                               ExpArrivalDate = a.ExpArrivalDate,
            //                               OrdNo = a.OrdNo,
            //                               CCID = a.CCID,
            //                               Id = a.Id,
            //                               DocNo = a.DocNo,
            //                               Approved = a.Approved,
            //                               Hold = a.Hold
            //                           };
            //}
            //else
            //{
            //    filteredRequisitions = from a in _requisitionRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
            //                           join
            //                           b in _locationRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
            //                           on new { A = a.LocID, B = a.TenantId } equals new { A = b.LocID, B = b.TenantId }
            //                           where (b.LocName.Contains(input.Filter) || b.LocID.ToString() == input.Filter
            //                           || a.DocNo.ToString() == input.Filter
            //                           )
            //                           select new RequisitionDto()
            //                           {
            //                               LocID = b.LocID,
            //                               LocName = b.LocName,
            //                               DocDate = a.DocDate,
            //                               ExpArrivalDate = a.ExpArrivalDate,
            //                               OrdNo = a.OrdNo,
            //                               CCID = a.CCID,
            //                               Id = a.Id,
            //                               DocNo = a.DocNo,
            //                               Approved = a.Approved,
            //                               Hold = a.Hold
            //                           };
            //}
            IQueryable<RequisitionDto> pagedAndFilteredRequisitions = null;
            var UserName= _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            if (UserName=="admin")
            {
                pagedAndFilteredRequisitions = filteredRequisitions
               .OrderBy(input.Sorting ?? "id desc")
               .PageBy(input);
            }
            else
            {
                if (usrPermissions.Any(x => x.Name == AppPermissions.Transaction_Inventory_ShowAll))
                {
                    var Name = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().Name;
                    var Locd = _csUserLocDRepository.GetAll().Where(c => c.TenantId == AbpSession.TenantId && c.TypeID == 2 && c.UserID == Name && c.Status == true).Select(x => x.LocId).ToList();
                   
                    pagedAndFilteredRequisitions = filteredRequisitions.Where(c => Locd.Contains(c.LocID))
             .OrderBy(input.Sorting ?? "id desc")
             .PageBy(input);
                }
                else
                {
                    pagedAndFilteredRequisitions = filteredRequisitions.Where(c => c.CreatedBy == UserName)
               .OrderBy(input.Sorting ?? "id desc")
               .PageBy(input);
                }
              
            }

            

            var requisitions = from o in pagedAndFilteredRequisitions
                               select new GetRequisitionForViewDto()
                               {
                                   Requisition = new RequisitionDto
                                   {
                                       LocID = o.LocID,
                                       DocNo = o.DocNo,
                                       DocDate = o.DocDate,
                                       ExpArrivalDate = o.ExpArrivalDate,
                                       OrdNo = o.OrdNo,
                                       CCID = o.CCID,
                                       LocName = o.LocName,
                                       // Narration = o.Narration,
                                       //TotalQty = o.TotalQty,
                                       // ArrivalDate = o.ArrivalDate,
                                       // ReqNo = o.ReqNo,
                                       //AuditUser = o.AuditUser,
                                       //AuditTime = o.AuditTime,
                                       //SysDate = o.SysDate,
                                       //DbID = o.DbID,
                                       //Completed = o.Completed,
                                       //Active = o.Active,
                                       //Hold = o.Hold,
                                       //AudtUser = o.AudtUser,
                                       //AudtDate = o.AudtDate,
                                       //CreatedBy = o.CreatedBy,
                                       //CreateDate = o.CreateDate,
                                       Id = o.Id,
                                       Approved = o.Approved,
                                       Hold = o.Hold,
                                       Posted = o.Posted
                                   }
                               };

            var totalCount = await pagedAndFilteredRequisitions.CountAsync();

            return new PagedResultDto<GetRequisitionForViewDto>(
                totalCount,
                await requisitions.ToListAsync()
            );
        }

        public async Task<GetRequisitionForViewDto> GetRequisitionForView(int id)
        {
            var requisition = await _requisitionRepository.GetAsync(id);

            var output = new GetRequisitionForViewDto { Requisition = ObjectMapper.Map<RequisitionDto>(requisition) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Purchase_Requisitions_Edit)]
        public async Task<GetRequisitionForEditOutput> GetRequisitionForEdit(EntityDto input)
        {
            var itemList = await _itemRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId).ToListAsync();
            var requisition = await _requisitionRepository.FirstOrDefaultAsync(x => x.DocNo == input.Id && x.TenantId == AbpSession.TenantId);
            var requisitionDetail = await _requisitionDetailRepository.GetAll().Where(e => e.DocNo == input.Id && e.TenantId == AbpSession.TenantId).ToListAsync();
            var output = new GetRequisitionForEditOutput { Requisition = ObjectMapper.Map<CreateOrEditRequisitionDto>(requisition) };
            output.Requisition.requisitionDetailDto = ObjectMapper.Map<List<RequisitionDetailDto>>(requisitionDetail);
            foreach (var data in output.Requisition.requisitionDetailDto)
            {
                var item = itemList.Where(o => o.ItemId == data.ItemID);
                // data.MaxQty = Convert.ToInt32(GetQtyInHand(data.ItemID, data.FromLocId, data.DocNo));
                output.Requisition.requisitionDetailDto.Where(o => o.Id == data.Id).FirstOrDefault().ItemID = item.FirstOrDefault().ItemId + "*" + item.FirstOrDefault().Descp + "*" + data.Unit + "*" + data.Conver;
                if (data.TransId!=null && data.TransId>0)
                {
                    output.Requisition.requisitionDetailDto.Where(o => o.Id == data.Id).FirstOrDefault().TransName = _icopT5Repository.GetAll().Where(x => x.OptID == data.TransId).FirstOrDefault().Descp;
                }
            }
            output.Requisition.LocName = _locationRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId).Count() > 0
               ? _locationRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId).FirstOrDefault().LocName : "";
            if (!string.IsNullOrEmpty(requisition.CCID))
            {
                output.Requisition.CCName = _costCenterRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.CCID==requisition.CCID).Count() > 0
               ? _costCenterRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.CCID == requisition.CCID).FirstOrDefault().CCName : "";
            }
            
            //if (requisition.)
            //{

            //}
            return output;
        }

        public async Task CreateOrEdit(CreateOrEditRequisitionDto input)
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

        [AbpAuthorize(AppPermissions.Purchase_Requisitions_Create)]
        protected virtual async Task Create(CreateOrEditRequisitionDto input)
        {
            var requisition = ObjectMapper.Map<Requisitions>(input);
            requisition.CreatedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            requisition.CreateDate = DateTime.Now;

            requisition.DocNo = GetDocId();
            if (requisition.ExpArrivalDate == null)
                requisition.ExpArrivalDate = DateTime.Now.AddDays(7);


            if (AbpSession.TenantId != null)
            {
                requisition.TenantId = (int)AbpSession.TenantId;
            }

            if (input.OrdNo != "" && input.OrdNo != null && input.OrdNo != "0")
            {
                var SaleDocNo = Convert.ToInt32(input.OrdNo);
                var tenantID = (int)AbpSession.TenantId;
                var SaleData = from a in _oesaleHeaderRepository.GetAll().Where(x => x.TenantId == tenantID)
                               join
                               b in _oesaleDetailRepository.GetAll().Where(x => x.TenantId == tenantID)
                               on new { A = a.TenantId, B = a.DocNo } equals new { A = b.TenantId, B = b.DocNo }
                               join
                               f in _itemRepository.GetAll().Where(a => a.TenantId == tenantID)
                               on new { A = b.ItemID, B = b.TenantId } equals new { A = f.ItemId, B = f.TenantId }

                               where (a.DocNo == SaleDocNo && a.TenantId == tenantID)
                               select new CreateOrEditRequisitionDto() { Narration = a.SalesCtrlAcc, TotalQty = a.TotalQty, DocNo = a.CustID, BasicStyle = a.BasicStyle, ItemName = f.Descp, License = a.License };
                var data1 = SaleData.ToList();

                if (data1 != null && data1.Count>0)
                {

                    data1.FirstOrDefault().PartyName = _accountSubLedgerRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId && x.AccountID == data1.FirstOrDefault().Narration && x.Id == data1.FirstOrDefault().DocNo).WhereIf(!string.IsNullOrWhiteSpace(""),
                      e => e.Id.ToString().ToUpper().Contains("") || e.SubAccName.ToString().ToUpper().Contains("")).FirstOrDefault().SubAccName;

                    requisition.PartyName = data1.FirstOrDefault().PartyName;
                    requisition.ItemName = data1.FirstOrDefault().ItemName;
                    requisition.OrderQty = data1.FirstOrDefault().TotalQty;
                    requisition.BasicStyle = data1.FirstOrDefault().BasicStyle;
                    requisition.License = data1.FirstOrDefault().License;
                }
            
            }


            var getId = await _requisitionRepository.InsertAndGetIdAsync(requisition);

            foreach (var data in input.requisitionDetailDto)
            {
                var reqDetailDetail = ObjectMapper.Map<RequisitionDetail>(data);
                if (AbpSession.TenantId != null)
                {
                    reqDetailDetail.TenantId = (int)AbpSession.TenantId;
                }
                reqDetailDetail.DetID = getId;
                reqDetailDetail.DocNo = requisition.DocNo;
                reqDetailDetail.LocID = requisition.LocID;
                reqDetailDetail.Unit = data.Unit;
                await _requisitionDetailRepository.InsertAsync(reqDetailDetail);
            }
        }

        [AbpAuthorize(AppPermissions.Purchase_Requisitions_Edit)]
        protected virtual async Task Update(CreateOrEditRequisitionDto input)
        {
            try
            {
                var requisition = await _requisitionRepository.FirstOrDefaultAsync(x => x.DocNo == input.DocNo && x.TenantId == AbpSession.TenantId);

                input.AudtDate = DateTime.Now;
                input.AudtUser = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;

                if (input.OrdNo != "" && input.OrdNo != null && input.OrdNo != "0")
                {
                    var SaleDocNo = Convert.ToInt32(input.OrdNo);
                    var tenantID = (int)AbpSession.TenantId;
                    var SaleData = from a in _oesaleHeaderRepository.GetAll().Where(x => x.TenantId == tenantID)
                                   join
                                   b in _oesaleDetailRepository.GetAll().Where(x => x.TenantId == tenantID)
                                   on new { A = a.TenantId, B = a.Id } equals new { A = b.TenantId, B = b.DetID }
                                   join
                                   f in _itemRepository.GetAll().Where(a => a.TenantId == tenantID)
                                   on new { A = b.ItemID, B = b.TenantId } equals new { A = f.ItemId, B = f.TenantId }

                                   where (a.DocNo == SaleDocNo && a.TenantId == tenantID)
                                   select new CreateOrEditRequisitionDto() { Narration = a.SalesCtrlAcc, TotalQty = a.TotalQty, DocNo = a.CustID, BasicStyle = a.BasicStyle, ItemName = f.Descp, License = a.License };
                    var data1 = SaleData.ToList();

                    if (data1 != null)
                    {

                        data1.FirstOrDefault().PartyName = _accountSubLedgerRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId && x.AccountID == data1.FirstOrDefault().Narration && x.Id == data1.FirstOrDefault().DocNo).WhereIf(!string.IsNullOrWhiteSpace(""),
                          e => e.Id.ToString().ToUpper().Contains("") || e.SubAccName.ToString().ToUpper().Contains("")).FirstOrDefault().SubAccName;

                    }
                    input.PartyName = data1.FirstOrDefault().PartyName;
                    input.ItemName = data1.FirstOrDefault().ItemName;
                    input.OrderQty = data1.FirstOrDefault().TotalQty;
                    input.BasicStyle = data1.FirstOrDefault().BasicStyle;
                    input.License = data1.FirstOrDefault().License;
                }

                
                ObjectMapper.Map(input, requisition);
                await _requisitionDetailRepository.DeleteAsync(o => o.TenantId == AbpSession.TenantId && o.DocNo == input.DocNo);
                foreach (var data in input.requisitionDetailDto)
                {
                    var requisitionDetail = ObjectMapper.Map<RequisitionDetail>(data);
                    if (AbpSession.TenantId != null)
                    {
                        requisitionDetail.TenantId = (int)AbpSession.TenantId;
                    }

                    requisitionDetail.DetID = (int)input.Id;
                    requisitionDetail.DocNo = input.DocNo;
                    requisitionDetail.LocID = input.LocID;
                    requisitionDetail.SUBCCID = data.SUBCCID;
                    requisitionDetail.Conver = data.Conver;
                    requisitionDetail.QIH = data.QIH;
                    requisitionDetail.Remarks = data.Remarks;
                    requisitionDetail.Unit = data.Unit;
                    await _requisitionDetailRepository.InsertAsync(requisitionDetail);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
             
        }

        [AbpAuthorize(AppPermissions.Purchase_Requisitions_Delete)]
        public async Task Delete(int id)
        {
            DeleteLog(id);
            await _requisitionRepository.DeleteAsync(x => x.DocNo == id && x.TenantId == AbpSession.TenantId);
            await _requisitionDetailRepository.DeleteAsync(x => x.DocNo == id && x.TenantId == AbpSession.TenantId);
        }
        public void DeleteLog(int detid)
        {
            var data = _requisitionRepository.FirstOrDefault(c => c.DocNo == detid && c.TenantId == AbpSession.TenantId);
            LogModel model = new LogModel()
            {
                Action = "Delete",
                Detid = data.Id,
                DocNo = detid,
                FormName = "Requisitions",
                Status = true,
                ApprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName,
                TenantId = AbpSession.TenantId
            };
            _commonappRepository.ApproveLog(model);
        }
        public int GetDocId()
        {
            var result = _requisitionRepository.GetAll().DefaultIfEmpty().Max(o => o.DocNo);
            return result = result + 1;
        }

        public double? GetQtyInHand(string itemId, int locId, int docId)
        {
            var qty = _icledgRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.ItemID == itemId && o.LocID == locId).Select(x => x.Qty).Sum();
            if (docId > 0)
            {
                qty = _icledgRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.ItemID == itemId && o.DocNo != docId
                && o.LocID == locId).Select(x => x.Qty).Sum();
            }

            return qty;
        }

        public async Task<FileDto> GetDataToExcel(GetAllRequisitionsInput input)
        {
            IQueryable<RequisitionDto> filteredRequisitions;
            if (input.Filter == null)
            {
                filteredRequisitions = from a in _requisitionRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                                       join
                                       b in _locationRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                                       on new { A = a.LocID, B = a.TenantId } equals new { A = b.LocID, B = b.TenantId }
                                       select new RequisitionDto()
                                       {
                                           LocID = b.LocID,
                                           LocName = b.LocName,
                                           DocDate = a.DocDate,
                                           ExpArrivalDate = a.ExpArrivalDate,
                                           OrdNo = a.OrdNo,
                                           CCID = a.CCID,
                                           Id = a.Id,
                                           DocNo = a.DocNo
                                       };
            }
            else
            {
                filteredRequisitions = from a in _requisitionRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                                       join
                                       b in _locationRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                                       on new { A = a.LocID, B = a.TenantId } equals new { A = b.LocID, B = b.TenantId }
                                       where (b.LocName.Contains(input.Filter) || b.LocID.ToString() == input.Filter
                                       || a.DocNo.ToString() == input.Filter
                                       )
                                       select new RequisitionDto()
                                       {
                                           LocID = b.LocID,
                                           LocName = b.LocName,
                                           DocDate = a.DocDate,
                                           ExpArrivalDate = a.ExpArrivalDate,
                                           OrdNo = a.OrdNo,
                                           CCID = a.CCID,
                                           Id = a.Id,
                                           DocNo = a.DocNo
                                       };
            }


            var pagedAndFilteredRequisitions = filteredRequisitions
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var requisitions = from o in pagedAndFilteredRequisitions
                               select new GetRequisitionForViewDto()
                               {
                                   Requisition = new RequisitionDto
                                   {
                                       LocID = o.LocID,
                                       DocNo = o.DocNo,
                                       DocDate = o.DocDate,
                                       ExpArrivalDate = o.ExpArrivalDate,
                                       OrdNo = o.OrdNo,
                                       CCID = o.CCID,
                                       LocName = o.LocName,
                                       // Narration = o.Narration,
                                       //TotalQty = o.TotalQty,
                                       // ArrivalDate = o.ArrivalDate,
                                       // ReqNo = o.ReqNo,
                                       //AuditUser = o.AuditUser,
                                       //AuditTime = o.AuditTime,
                                       //SysDate = o.SysDate,
                                       //DbID = o.DbID,
                                       //Completed = o.Completed,
                                       //Active = o.Active,
                                       //Hold = o.Hold,
                                       //AudtUser = o.AudtUser,
                                       //AudtDate = o.AudtDate,
                                       //CreatedBy = o.CreatedBy,
                                       //CreateDate = o.CreateDate,
                                       Id = o.Id
                                   }
                               };

            var dataDto = await requisitions.ToListAsync();

            return _requisitionsExcelExporter.ExportToFile(dataDto);
        }

        public PagedResultDto<RequisitionDto> GetDetailsForApproval(string fromDate, string toDate, string Mode, int fromDoc, int ToDoc, GetAllRequisitionsInput input)
        {
            IQueryable<Requisitions> data = null;
            if (Mode == "Posting" || Mode == "UnApproval")
            {
                data = _requisitionRepository.GetAll().Where(o => o.DocDate.Value.Date >= Convert.ToDateTime(fromDate).Date
             && o.DocDate.Value.Date <= Convert.ToDateTime(toDate).Date && o.Approved == true
             //&& o.Posted == false 
             && o.TenantId == AbpSession.TenantId && o.DocNo >= fromDoc && o.DocNo <= ToDoc);
            }
            else
            {
                data = _requisitionRepository.GetAll().Where(o => o.DocDate.Value.Date >= Convert.ToDateTime(fromDate).Date
                            && o.DocDate.Value.Date <= Convert.ToDateTime(toDate).Date && o.Approved == false && o.TenantId == AbpSession.TenantId && o.DocNo >= fromDoc && o.DocNo <= ToDoc);
            }

            var pageData = data.PageBy(input);

            var paginatedData = from o in pageData
                                select new RequisitionDto()
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
            return new PagedResultDto<RequisitionDto>(
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
                var DetId = 0;
                if (Mode == "Posting")
                {
                    (from a in _requisitionRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && postedDataIds.Contains(o.DocNo))
                     select a).ToList().ForEach(x =>
                     {
                         x.Posted = true;
                         DetId = x.Id;
                     });

                    //res.Posted = bit;
                    //res.PostedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                    //res.PostedDate = DateTime.Now;
                    //_repository.Update(res);
                }
                else if (Mode == "UnApproval")
                {
                    (from a in _requisitionRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && postedDataIds.Contains(o.DocNo))
                     select a).ToList().ForEach(x =>
                     {
                         x.Approved = false;
                         x.ApprovedDate = DateTime.Now;
                         x.ApprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                         DetId = x.Id;
                     });
                    //res.Approved = false;
                    //res.AprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                    //res.AprovedDate = DateTime.Now;
                    //_icOPNHeaderRepository.Update(res);
                }
                else
                {
                    (from a in _requisitionRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && postedDataIds.Contains(o.DocNo))
                     select a).ToList().ForEach(x =>
                     {
                         x.Approved = true;
                         x.ApprovedDate = DateTime.Now;
                         x.ApprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                         DetId = x.Id;
                     });
                    //res.Approved = bit;
                    //res.AprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                    //res.AprovedDate = DateTime.Now;
                    //_icOPNHeaderRepository.Update(res);
                }
                LogModel Log = new LogModel()
                {
                    Status = bit,
                    ApprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName,
                    Detid = DetId,
                    DocNo = Convert.ToInt32(postedDataIds.FirstOrDefault().ToString()),
                    FormName = "Requisitions",
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