

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
using ERP.GeneralLedger.SetupForms;
using ERP.Authorization.Users;
using ERP.SupplyChain.Inventory.Exporting;
using ERP.SupplyChain.Inventory.IC_Segment1;
using ERP.SupplyChain.Inventory.IC_Segment2;
using ERP.SupplyChain.Inventory.IC_Segment3;

namespace ERP.SupplyChain.Inventory
{
    [AbpAuthorize(AppPermissions.Inventory_InventoryGlLinks)]
    public class InventoryGlLinksAppService : ERPAppServiceBase, IInventoryGlLinksAppService
    {
        private readonly IRepository<InventoryGlLink> _inventoryGlLinkRepository;
        private readonly IRepository<ICSetup> _icsetupRepository;
        private readonly IRepository<ICSegment1> _seg1Repository;
        private readonly IRepository<ICSegment2> _seg2Repository;
        private readonly IRepository<ICSegment3> _seg3Repository;
        private readonly IRepository<ICLocation> _locationRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<ChartofControl, string> _chartofControlRepository;
        private readonly IInventoryGlLinksExcelExporter _IInventoryGlLinksExcelExporter;
        private readonly IRepository<GLLocation> _glLocationRepository;
        public InventoryGlLinksAppService(IRepository<InventoryGlLink> inventoryGlLinkRepository,
            IRepository<ICSegment1> seg1Repository,
            IRepository<ICLocation> locationRepository,
            IRepository<User, long> userRepository,
            IInventoryGlLinksExcelExporter IInventoryGlLinksExcelExporter,
            IRepository<ChartofControl, string> chartofControlRepository,
            IRepository<ICSetup> icsetupRepository,
            IRepository<ICSegment2> seg2Repository,
            IRepository<ICSegment3> seg3Repository,
               IRepository<GLLocation> glLocationRepository)
        {
            _inventoryGlLinkRepository = inventoryGlLinkRepository;
            _seg1Repository = seg1Repository;
            _locationRepository = locationRepository;
            _userRepository = userRepository;
            _IInventoryGlLinksExcelExporter = IInventoryGlLinksExcelExporter;
            _chartofControlRepository = chartofControlRepository;
            _icsetupRepository = icsetupRepository;
            _seg2Repository = seg2Repository;
            _seg3Repository = seg3Repository;
            _glLocationRepository = glLocationRepository;
        }

        public async Task<PagedResultDto<GetInventoryGlLinkForViewDto>> GetAll(GetAllInventoryGlLinksInput input)
        {
            var seg = await _seg1Repository.GetAll().Where(o => o.TenantId == AbpSession.TenantId).ToListAsync();
            IQueryable<InventoryGlLinkDto> filteredInventoryGlLinks;
            if (input.Filter != null)
            {
                filteredInventoryGlLinks = from a in _inventoryGlLinkRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                                           join b in _locationRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                                           on new { A = a.LocID, B = a.TenantId } equals new { A = b.LocID, B = b.TenantId }
                                           //join c in seg
                                           //on new { A = a.SegID, B = a.TenantId } equals new { A = c.Seg1ID, B = c.TenantId }
                                           where ((b.LocName.Contains(input.Filter) && a.TenantId == AbpSession.TenantId)
                                           //||  (c.Seg1Name.Contains(input.Filter) && a.TenantId == AbpSession.TenantId)
                                           || a.SegID == input.Filter || a.LocID.ToString() == input.Filter
                                           )
                                           select new InventoryGlLinkDto()
                                           {
                                               LocID = a.LocID,
                                               SegID = a.SegID,
                                               LocName = b.LocName,
                                               // SegName = c.Seg1Name,
                                               Id = a.Id
                                           };
            }
            else
            {
                filteredInventoryGlLinks = from a in _inventoryGlLinkRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                                           join b in _locationRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                                           on new { A = a.LocID, B = a.TenantId } equals new { A = b.LocID, B = b.TenantId }
                                           //join c in seg
                                           //on new { A = a.SegID, B = a.TenantId } equals new { A = c.Seg1ID, B = c.TenantId }
                                           where (a.TenantId == AbpSession.TenantId)
                                           select new InventoryGlLinkDto()
                                           {
                                               LocID = a.LocID,
                                               SegID = a.SegID,
                                               LocName = b.LocName,
                                               //  SegName = c.Seg1Name,
                                               Id = a.Id
                                           };
            }



            var pagedAndFilteredInventoryGlLinks = filteredInventoryGlLinks
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var inventoryGlLinks = from o in pagedAndFilteredInventoryGlLinks
                                   select new GetInventoryGlLinkForViewDto()
                                   {
                                       InventoryGlLink = new InventoryGlLinkDto
                                       {
                                           LocID = o.LocID,
                                           SegID = o.SegID,
                                           Id = o.Id,
                                           SegName = _seg1Repository.GetAll().Where(p => p.Seg1ID == o.SegID).SingleOrDefault().Seg1Name,
                                           LocName = o.LocName
                                       }
                                   };

            var totalCount = await filteredInventoryGlLinks.CountAsync();

            return new PagedResultDto<GetInventoryGlLinkForViewDto>(
                totalCount,
                 inventoryGlLinks.ToList()
            );
        }

        public async Task<FileDto> GetInventoryGlLinksToExcel(GetAllInventoryGlLinksInput input)
        {
            var SegLink = _icsetupRepository.GetAll().Where(c => c.TenantId == AbpSession.TenantId).FirstOrDefault().GLSegLink;

            IQueryable<InventoryGlLinkDto> filteredInventoryGlLinks = null ;
            if (SegLink == 3)
            {

           
            if (input.Filter != null)
            {
                filteredInventoryGlLinks = from a in _inventoryGlLinkRepository.GetAll()
                                           join b in _locationRepository.GetAll()
                                           on new { A = a.LocID, B = a.TenantId } equals new { A = b.LocID, B = b.TenantId }
                                           join c in _seg3Repository.GetAll()
                                           on new { A = a.SegID, B = a.TenantId } equals new { A = c.Seg3Id, B = c.TenantId }
                                           where ((b.LocName.Contains(input.Filter) && a.TenantId == AbpSession.TenantId) || (c.Seg3Id.Contains(input.Filter) && a.TenantId == AbpSession.TenantId))
                                           select new InventoryGlLinkDto()
                                           {
                                               LocID = a.LocID,
                                               SegID = a.SegID,
                                               LocName = b.LocName,
                                               SegName = c.Seg3Name,
                                               Id = a.Id,
                                               AccAdj = a.AccAdj,
                                               AccCGS = a.AccCGS,
                                               AccRec = a.AccRec,
                                               AccRet = a.AccRet,
                                               AccWIP = a.AccWIP,
                                               AccAdjDesc= _chartofControlRepository.GetAll().Where(o => o.Id == a.AccAdj && o.TenantId == AbpSession.TenantId).Count() > 0 ?
                                                _chartofControlRepository.GetAll().Where(o => o.Id == a.AccAdj && o.TenantId == AbpSession.TenantId).FirstOrDefault().AccountName : "",
                                               AccCGSDesc= _chartofControlRepository.GetAll().Where(o => o.Id == a.AccCGS && o.TenantId == AbpSession.TenantId).Count() > 0 ?
                                                _chartofControlRepository.GetAll().Where(o => o.Id == a.AccCGS && o.TenantId == AbpSession.TenantId).FirstOrDefault().AccountName : "",
                                               AccRecDesc = _chartofControlRepository.GetAll().Where(o => o.Id == a.AccRec && o.TenantId == AbpSession.TenantId).Count() > 0 ?
                                                _chartofControlRepository.GetAll().Where(o => o.Id == a.AccRec && o.TenantId == AbpSession.TenantId).FirstOrDefault().AccountName : "",
                                               AccRetDesc= _chartofControlRepository.GetAll().Where(o => o.Id == a.AccRet && o.TenantId == AbpSession.TenantId).Count() > 0 ?
                                                _chartofControlRepository.GetAll().Where(o => o.Id == a.AccRet && o.TenantId == AbpSession.TenantId).FirstOrDefault().AccountName : ""
                                                ,AccWIPDesc= _chartofControlRepository.GetAll().Where(o => o.Id == a.AccWIP && o.TenantId == AbpSession.TenantId).Count() > 0 ?
                                                         _chartofControlRepository.GetAll().Where(o => o.Id == a.AccWIP && o.TenantId == AbpSession.TenantId).FirstOrDefault().AccountName : ""



            };
            }
            else
            {
                filteredInventoryGlLinks = from a in _inventoryGlLinkRepository.GetAll()
                                           join b in _locationRepository.GetAll()
                                           on new { A = a.LocID, B = a.TenantId } equals new { A = b.LocID, B = b.TenantId }
                                           join c in _seg3Repository.GetAll()
                                           on new { A = a.SegID, B = a.TenantId } equals new { A = c.Seg3Id, B = c.TenantId }
                                           where (a.TenantId == AbpSession.TenantId)
                                           select new InventoryGlLinkDto()
                                           {
                                               LocID = a.LocID,
                                               SegID = a.SegID,
                                               LocName = b.LocName,
                                               SegName = c.Seg3Name,
                                               Id = a.Id,
                                               AccAdj=a.AccAdj,
                                          
                                               AccCGS=a.AccCGS,
                                               AccRec=a.AccRec,
                                               AccRet=a.AccRet,
                                               AccWIP=a.AccWIP,
                                               AccAdjDesc = _chartofControlRepository.GetAll().Where(o => o.Id == a.AccAdj && o.TenantId == AbpSession.TenantId).Count() > 0 ?
                                                _chartofControlRepository.GetAll().Where(o => o.Id == a.AccAdj && o.TenantId == AbpSession.TenantId).FirstOrDefault().AccountName : "",
                                               AccCGSDesc = _chartofControlRepository.GetAll().Where(o => o.Id == a.AccCGS && o.TenantId == AbpSession.TenantId).Count() > 0 ?
                                                _chartofControlRepository.GetAll().Where(o => o.Id == a.AccCGS && o.TenantId == AbpSession.TenantId).FirstOrDefault().AccountName : "",
                                               AccRecDesc = _chartofControlRepository.GetAll().Where(o => o.Id == a.AccRec && o.TenantId == AbpSession.TenantId).Count() > 0 ?
                                                _chartofControlRepository.GetAll().Where(o => o.Id == a.AccRec && o.TenantId == AbpSession.TenantId).FirstOrDefault().AccountName : "",
                                                 AccRetDesc = _chartofControlRepository.GetAll().Where(o => o.Id == a.AccRet && o.TenantId == AbpSession.TenantId).Count() > 0 ?
                                                _chartofControlRepository.GetAll().Where(o => o.Id == a.AccRet && o.TenantId == AbpSession.TenantId).FirstOrDefault().AccountName : ""
                                                 ,
                                               AccWIPDesc = _chartofControlRepository.GetAll().Where(o => o.Id == a.AccWIP && o.TenantId == AbpSession.TenantId).Count() > 0 ?
                                                         _chartofControlRepository.GetAll().Where(o => o.Id == a.AccWIP && o.TenantId == AbpSession.TenantId).FirstOrDefault().AccountName : ""


                                           };
            }
            }else if (SegLink == 2)
            {
                if (input.Filter != null)
                {
                    filteredInventoryGlLinks = from a in _inventoryGlLinkRepository.GetAll()
                                               join b in _locationRepository.GetAll()
                                               on new { A = a.LocID, B = a.TenantId } equals new { A = b.LocID, B = b.TenantId }
                                               join c in _seg2Repository.GetAll()
                                               on new { A = a.SegID, B = a.TenantId } equals new { A = c.Seg2Id, B = c.TenantId }
                                               where ((b.LocName.Contains(input.Filter) && a.TenantId == AbpSession.TenantId) || (c.Seg2Id.Contains(input.Filter) && a.TenantId == AbpSession.TenantId))
                                               select new InventoryGlLinkDto()
                                               {
                                                   LocID = a.LocID,
                                                   SegID = a.SegID,
                                                   LocName = b.LocName,
                                                   SegName = c.Seg2Name,
                                                   Id = a.Id,
                                                   AccAdj = a.AccAdj,
                                                   AccCGS = a.AccCGS,
                                                   AccRec = a.AccRec,
                                                   AccRet = a.AccRet,
                                                   AccWIP = a.AccWIP,
                                                   AccAdjDesc = _chartofControlRepository.GetAll().Where(o => o.Id == a.AccAdj && o.TenantId == AbpSession.TenantId).Count() > 0 ?
                                                    _chartofControlRepository.GetAll().Where(o => o.Id == a.AccAdj && o.TenantId == AbpSession.TenantId).FirstOrDefault().AccountName : "",
                                                   AccCGSDesc = _chartofControlRepository.GetAll().Where(o => o.Id == a.AccCGS && o.TenantId == AbpSession.TenantId).Count() > 0 ?
                                                    _chartofControlRepository.GetAll().Where(o => o.Id == a.AccCGS && o.TenantId == AbpSession.TenantId).FirstOrDefault().AccountName : "",
                                                   AccRecDesc = _chartofControlRepository.GetAll().Where(o => o.Id == a.AccRec && o.TenantId == AbpSession.TenantId).Count() > 0 ?
                                                    _chartofControlRepository.GetAll().Where(o => o.Id == a.AccRec && o.TenantId == AbpSession.TenantId).FirstOrDefault().AccountName : "",
                                                   AccRetDesc = _chartofControlRepository.GetAll().Where(o => o.Id == a.AccRet && o.TenantId == AbpSession.TenantId).Count() > 0 ?
                                                    _chartofControlRepository.GetAll().Where(o => o.Id == a.AccRet && o.TenantId == AbpSession.TenantId).FirstOrDefault().AccountName : ""
                                                    ,
                                                   AccWIPDesc = _chartofControlRepository.GetAll().Where(o => o.Id == a.AccWIP && o.TenantId == AbpSession.TenantId).Count() > 0 ?
                                                             _chartofControlRepository.GetAll().Where(o => o.Id == a.AccWIP && o.TenantId == AbpSession.TenantId).FirstOrDefault().AccountName : ""



                                               };
                }
                else
                {
                    filteredInventoryGlLinks = from a in _inventoryGlLinkRepository.GetAll()
                                               join b in _locationRepository.GetAll()
                                               on new { A = a.LocID, B = a.TenantId } equals new { A = b.LocID, B = b.TenantId }
                                               join c in _seg2Repository.GetAll()
                                               on new { A = a.SegID, B = a.TenantId } equals new { A = c.Seg2Id, B = c.TenantId }
                                               where (a.TenantId == AbpSession.TenantId)
                                               select new InventoryGlLinkDto()
                                               {
                                                   LocID = a.LocID,
                                                   SegID = a.SegID,
                                                   LocName = b.LocName,
                                                   SegName = c.Seg2Name,
                                                   Id = a.Id,
                                                   AccAdj = a.AccAdj,

                                                   AccCGS = a.AccCGS,
                                                   AccRec = a.AccRec,
                                                   AccRet = a.AccRet,
                                                   AccWIP = a.AccWIP,
                                                   AccAdjDesc = _chartofControlRepository.GetAll().Where(o => o.Id == a.AccAdj && o.TenantId == AbpSession.TenantId).Count() > 0 ?
                                                    _chartofControlRepository.GetAll().Where(o => o.Id == a.AccAdj && o.TenantId == AbpSession.TenantId).FirstOrDefault().AccountName : "",
                                                   AccCGSDesc = _chartofControlRepository.GetAll().Where(o => o.Id == a.AccCGS && o.TenantId == AbpSession.TenantId).Count() > 0 ?
                                                    _chartofControlRepository.GetAll().Where(o => o.Id == a.AccCGS && o.TenantId == AbpSession.TenantId).FirstOrDefault().AccountName : "",
                                                   AccRecDesc = _chartofControlRepository.GetAll().Where(o => o.Id == a.AccRec && o.TenantId == AbpSession.TenantId).Count() > 0 ?
                                                    _chartofControlRepository.GetAll().Where(o => o.Id == a.AccRec && o.TenantId == AbpSession.TenantId).FirstOrDefault().AccountName : "",
                                                   AccRetDesc = _chartofControlRepository.GetAll().Where(o => o.Id == a.AccRet && o.TenantId == AbpSession.TenantId).Count() > 0 ?
                                                    _chartofControlRepository.GetAll().Where(o => o.Id == a.AccRet && o.TenantId == AbpSession.TenantId).FirstOrDefault().AccountName : ""
                                                     ,
                                                   AccWIPDesc = _chartofControlRepository.GetAll().Where(o => o.Id == a.AccWIP && o.TenantId == AbpSession.TenantId).Count() > 0 ?
                                                             _chartofControlRepository.GetAll().Where(o => o.Id == a.AccWIP && o.TenantId == AbpSession.TenantId).FirstOrDefault().AccountName : ""


                                               };
                }
            }
            else if (SegLink == 1)
            {
                if (input.Filter != null)
                {
                    filteredInventoryGlLinks = from a in _inventoryGlLinkRepository.GetAll()
                                               join b in _locationRepository.GetAll()
                                               on new { A = a.LocID, B = a.TenantId } equals new { A = b.LocID, B = b.TenantId }
                                               join c in _seg1Repository.GetAll()
                                               on new { A = a.SegID, B = a.TenantId } equals new { A = c.Seg1ID, B = c.TenantId }
                                               where ((b.LocName.Contains(input.Filter) && a.TenantId == AbpSession.TenantId) || (c.Seg1ID.Contains(input.Filter) && a.TenantId == AbpSession.TenantId))
                                               select new InventoryGlLinkDto()
                                               {
                                                   LocID = a.LocID,
                                                   SegID = a.SegID,
                                                   LocName = b.LocName,
                                                   SegName = c.Seg1Name,
                                                   Id = a.Id,
                                                   AccAdj = a.AccAdj,
                                                   AccCGS = a.AccCGS,
                                                   AccRec = a.AccRec,
                                                   AccRet = a.AccRet,
                                                   AccWIP = a.AccWIP,
                                                   AccAdjDesc = _chartofControlRepository.GetAll().Where(o => o.Id == a.AccAdj && o.TenantId == AbpSession.TenantId).Count() > 0 ?
                                                    _chartofControlRepository.GetAll().Where(o => o.Id == a.AccAdj && o.TenantId == AbpSession.TenantId).FirstOrDefault().AccountName : "",
                                                   AccCGSDesc = _chartofControlRepository.GetAll().Where(o => o.Id == a.AccCGS && o.TenantId == AbpSession.TenantId).Count() > 0 ?
                                                    _chartofControlRepository.GetAll().Where(o => o.Id == a.AccCGS && o.TenantId == AbpSession.TenantId).FirstOrDefault().AccountName : "",
                                                   AccRecDesc = _chartofControlRepository.GetAll().Where(o => o.Id == a.AccRec && o.TenantId == AbpSession.TenantId).Count() > 0 ?
                                                    _chartofControlRepository.GetAll().Where(o => o.Id == a.AccRec && o.TenantId == AbpSession.TenantId).FirstOrDefault().AccountName : "",
                                                   AccRetDesc = _chartofControlRepository.GetAll().Where(o => o.Id == a.AccRet && o.TenantId == AbpSession.TenantId).Count() > 0 ?
                                                    _chartofControlRepository.GetAll().Where(o => o.Id == a.AccRet && o.TenantId == AbpSession.TenantId).FirstOrDefault().AccountName : ""
                                                    ,
                                                   AccWIPDesc = _chartofControlRepository.GetAll().Where(o => o.Id == a.AccWIP && o.TenantId == AbpSession.TenantId).Count() > 0 ?
                                                             _chartofControlRepository.GetAll().Where(o => o.Id == a.AccWIP && o.TenantId == AbpSession.TenantId).FirstOrDefault().AccountName : ""



                                               };
                }
                else
                {
                    filteredInventoryGlLinks = from a in _inventoryGlLinkRepository.GetAll()
                                               join b in _locationRepository.GetAll()
                                               on new { A = a.LocID, B = a.TenantId } equals new { A = b.LocID, B = b.TenantId }
                                               join c in _seg1Repository.GetAll()
                                               on new { A = a.SegID, B = a.TenantId } equals new { A = c.Seg1ID, B = c.TenantId }
                                               where (a.TenantId == AbpSession.TenantId)
                                               select new InventoryGlLinkDto()
                                               {
                                                   LocID = a.LocID,
                                                   SegID = a.SegID,
                                                   LocName = b.LocName,
                                                   SegName = c.Seg1Name,
                                                   Id = a.Id,
                                                   AccAdj = a.AccAdj,

                                                   AccCGS = a.AccCGS,
                                                   AccRec = a.AccRec,
                                                   AccRet = a.AccRet,
                                                   AccWIP = a.AccWIP,
                                                   AccAdjDesc = _chartofControlRepository.GetAll().Where(o => o.Id == a.AccAdj && o.TenantId == AbpSession.TenantId).Count() > 0 ?
                                                    _chartofControlRepository.GetAll().Where(o => o.Id == a.AccAdj && o.TenantId == AbpSession.TenantId).FirstOrDefault().AccountName : "",
                                                   AccCGSDesc = _chartofControlRepository.GetAll().Where(o => o.Id == a.AccCGS && o.TenantId == AbpSession.TenantId).Count() > 0 ?
                                                    _chartofControlRepository.GetAll().Where(o => o.Id == a.AccCGS && o.TenantId == AbpSession.TenantId).FirstOrDefault().AccountName : "",
                                                   AccRecDesc = _chartofControlRepository.GetAll().Where(o => o.Id == a.AccRec && o.TenantId == AbpSession.TenantId).Count() > 0 ?
                                                    _chartofControlRepository.GetAll().Where(o => o.Id == a.AccRec && o.TenantId == AbpSession.TenantId).FirstOrDefault().AccountName : "",
                                                   AccRetDesc = _chartofControlRepository.GetAll().Where(o => o.Id == a.AccRet && o.TenantId == AbpSession.TenantId).Count() > 0 ?
                                                    _chartofControlRepository.GetAll().Where(o => o.Id == a.AccRet && o.TenantId == AbpSession.TenantId).FirstOrDefault().AccountName : ""
                                                     ,
                                                   AccWIPDesc = _chartofControlRepository.GetAll().Where(o => o.Id == a.AccWIP && o.TenantId == AbpSession.TenantId).Count() > 0 ?
                                                             _chartofControlRepository.GetAll().Where(o => o.Id == a.AccWIP && o.TenantId == AbpSession.TenantId).FirstOrDefault().AccountName : ""


                                               };
                }
            }

            var query = (from o in filteredInventoryGlLinks
                         select new GetInventoryGlLinkForViewDto()
                         {
                             InventoryGlLink = new InventoryGlLinkDto
                             {
                                 LocID = o.LocID,
                                 SegID = o.SegID,
                                 LocName = o.LocName,
                                 SegName = o.SegName,
                                 Id = o.Id,
                                 AccAdj = o.AccAdj,
                                 AccAdjDesc=o.AccAdjDesc,
                                 AccCGS = o.AccCGS,
                                 AccCGSDesc=o.AccCGSDesc,
                                 AccRec = o.AccRec,
                                 AccRecDesc=o.AccRecDesc,
                                 AccRet = o.AccRet,
                                 AccRetDesc=o.AccRetDesc,
                                 AccWIP = o.AccWIP,
                                 AccWIPDesc=o.AccWIPDesc

                             }
                         });


            var dataDto = await query.ToListAsync();

            return _IInventoryGlLinksExcelExporter.ExportToFile(dataDto);
        }

        public async Task<GetInventoryGlLinkForViewDto> GetInventoryGlLinkForView(int id)
        {
            var inventoryGlLink = await _inventoryGlLinkRepository.GetAsync(id);

            var output = new GetInventoryGlLinkForViewDto { InventoryGlLink = ObjectMapper.Map<InventoryGlLinkDto>(inventoryGlLink) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Inventory_InventoryGlLinks_Edit)]
        public async Task<GetInventoryGlLinkForEditOutput> GetInventoryGlLinkForEdit(EntityDto input)
        {
            var inventoryGlLink = await _inventoryGlLinkRepository.FirstOrDefaultAsync(input.Id);
            var output = new GetInventoryGlLinkForEditOutput { InventoryGlLink = ObjectMapper.Map<CreateOrEditInventoryGlLinkDto>(inventoryGlLink) };

            if (output.InventoryGlLink.SegID.Replace("-", "").Length == 2)
            {
                output.InventoryGlLink.SegDesc = _seg1Repository.GetAll().
                    Where(o => o.Seg1ID == output.InventoryGlLink.SegID && o.TenantId == AbpSession.TenantId).Count() > 0
                    ? _seg1Repository.GetAll().
                    Where(o => o.Seg1ID == output.InventoryGlLink.SegID && o.TenantId == AbpSession.TenantId).FirstOrDefault().Seg1Name
                    : "";
            }

            if (output.InventoryGlLink.SegID.Replace("-", "").Length == 5)
            {
                output.InventoryGlLink.SegDesc = _seg2Repository.GetAll()
                    .Where(o => o.Seg2Id == output.InventoryGlLink.SegID && o.TenantId == AbpSession.TenantId).Count() > 0 ?
                    _seg2Repository.GetAll()
                    .Where(o => o.Seg2Id == output.InventoryGlLink.SegID && o.TenantId == AbpSession.TenantId).FirstOrDefault().Seg2Name : "";
            }

            if (output.InventoryGlLink.SegID.Replace("-", "").Length == 9)
            {
                output.InventoryGlLink.SegDesc = _seg3Repository.GetAll()
                    .Where(o => o.Seg3Id == output.InventoryGlLink.SegID && o.TenantId == AbpSession.TenantId).Count() > 0 ?
                    _seg3Repository.GetAll()
                    .Where(o => o.Seg3Id == output.InventoryGlLink.SegID && o.TenantId == AbpSession.TenantId).FirstOrDefault().Seg3Name : "";
            }

            output.InventoryGlLink.LocDesc = _locationRepository.GetAll().Where(o => o.LocID == output.InventoryGlLink.LocID && o.TenantId == AbpSession.TenantId).FirstOrDefault().LocName;
            output.InventoryGlLink.GLLocDesc = _glLocationRepository.FirstOrDefault(o => o.LocId == output.InventoryGlLink.GLLocID && o.TenantId == AbpSession.TenantId).LocDesc;
            output.InventoryGlLink.AccAdjDesc = _chartofControlRepository.GetAll().Where(o => o.Id == output.InventoryGlLink.AccAdj && o.TenantId == AbpSession.TenantId).Count() > 0 ?
                                                _chartofControlRepository.GetAll().Where(o => o.Id == output.InventoryGlLink.AccAdj && o.TenantId == AbpSession.TenantId).FirstOrDefault().AccountName : "";
            output.InventoryGlLink.AccCGSDesc = _chartofControlRepository.GetAll().Where(o => o.Id == output.InventoryGlLink.AccCGS && o.TenantId == AbpSession.TenantId).Count() > 0 ?
                                                _chartofControlRepository.GetAll().Where(o => o.Id == output.InventoryGlLink.AccCGS && o.TenantId == AbpSession.TenantId).FirstOrDefault().AccountName : "";
            output.InventoryGlLink.AccRecDesc = _chartofControlRepository.GetAll().Where(o => o.Id == output.InventoryGlLink.AccRec && o.TenantId == AbpSession.TenantId).Count() > 0 ?
                                                _chartofControlRepository.GetAll().Where(o => o.Id == output.InventoryGlLink.AccRec && o.TenantId == AbpSession.TenantId).FirstOrDefault().AccountName : "";
            output.InventoryGlLink.AccRetDesc = _chartofControlRepository.GetAll().Where(o => o.Id == output.InventoryGlLink.AccRet && o.TenantId == AbpSession.TenantId).Count() > 0 ?
                                                _chartofControlRepository.GetAll().Where(o => o.Id == output.InventoryGlLink.AccRet && o.TenantId == AbpSession.TenantId).FirstOrDefault().AccountName : "";
            output.InventoryGlLink.AccWIPDesc = _chartofControlRepository.GetAll().Where(o => o.Id == output.InventoryGlLink.AccWIP && o.TenantId == AbpSession.TenantId).Count() > 0 ?
                                                         _chartofControlRepository.GetAll().Where(o => o.Id == output.InventoryGlLink.AccWIP && o.TenantId == AbpSession.TenantId).FirstOrDefault().AccountName : "";
            //output.InventoryGlLink.SegDesc = _seg2Repository.FirstOrDefault(o => o.Seg2Id == output.InventoryGlLink.SegID && o.TenantId == AbpSession.TenantId).Seg2Name;
            return output;
        }

        public async Task CreateOrEdit(CreateOrEditInventoryGlLinkDto input)
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

        [AbpAuthorize(AppPermissions.Inventory_InventoryGlLinks_Create)]
        protected virtual async Task Create(CreateOrEditInventoryGlLinkDto input)
        {
            var inventoryGlLink = ObjectMapper.Map<InventoryGlLink>(input);
            inventoryGlLink.AudtUser = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            inventoryGlLink.CreatedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            inventoryGlLink.AudtDate = DateTime.Now;
            inventoryGlLink.CreateDate = DateTime.Now;

            if (AbpSession.TenantId != null)
            {
                inventoryGlLink.TenantId = (int)AbpSession.TenantId;
            }


            await _inventoryGlLinkRepository.InsertAsync(inventoryGlLink);
        }

        [AbpAuthorize(AppPermissions.Inventory_InventoryGlLinks_Edit)]
        protected virtual async Task Update(CreateOrEditInventoryGlLinkDto input)
        {
            var inventoryGlLink = await _inventoryGlLinkRepository.FirstOrDefaultAsync((int)input.Id);
            input.AudtUser = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            input.AudtDate = DateTime.Now;
            input.CreatedBy = inventoryGlLink.CreatedBy;
            input.CreateDate = inventoryGlLink.CreateDate;
            ObjectMapper.Map(input, inventoryGlLink);
        }

        [AbpAuthorize(AppPermissions.Inventory_InventoryGlLinks_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _inventoryGlLinkRepository.DeleteAsync(input.Id);
        }

        public bool GetSegIdAgainstLoc(int locId, string seg)
        {
            var data = _inventoryGlLinkRepository.GetAll().Where(o => o.LocID == locId && o.SegID == seg && o.TenantId == AbpSession.TenantId);
            return data.Count() > 0 ? true : false;
        }

        public int? GetGLlinkSeg()
        {
            var data = _icsetupRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId).Count() > 0 ?
                _icsetupRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId).FirstOrDefault().GLSegLink : 1;
            return data;
        }
    }
}