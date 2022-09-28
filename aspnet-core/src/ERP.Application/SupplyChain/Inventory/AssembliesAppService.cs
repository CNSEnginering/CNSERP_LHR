

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
using ERP.Costing;
using ERP.Authorization.Users;
using ERP.SupplyChain.Inventory.IC_Item;
using ERP.SupplyChain.Inventory.Exporting;

namespace ERP.SupplyChain.Inventory
{
    [AbpAuthorize(AppPermissions.Inventory_Assemblies)]
    public class AssembliesAppService : ERPAppServiceBase, IAssembliesAppService
    {
        private readonly IRepository<Assembly> _assemblyRepository;
        private readonly IRepository<AssemblyDetails> _assemblyDetailRepository;
        private readonly IRepository<ICLocation> _locRepository;
        private readonly CostingAppService _costingService;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<ICItem> _itemRepository;
        private readonly IAssemblyExcelExporter _assemblyExcelExporter;
        private readonly CommonAppService _commonappRepository;
        public AssembliesAppService(IRepository<Assembly> assemblyRepository,
            IRepository<ICLocation> locRepository,
            IRepository<AssemblyDetails> assemblyDetailRepository,
            CostingAppService costingService,
            IRepository<User, long> userRepository,
            IRepository<ICItem> itemRepository,
            CommonAppService commonappRepository,
            IAssemblyExcelExporter assemblyExcelExporter)
        {
            _assemblyRepository = assemblyRepository;
            _locRepository = locRepository;
            _assemblyDetailRepository = assemblyDetailRepository;
            _costingService = costingService;
            _userRepository = userRepository;
            _itemRepository = itemRepository;
            _assemblyExcelExporter = assemblyExcelExporter;
            _commonappRepository = commonappRepository;
        }

        public async Task<PagedResultDto<GetAssemblyForViewDto>> GetAll(GetAllAssembliesInput input)
        {
            var fitlerLoc = _locRepository.GetAll().Where(o => o.LocName.Contains(input.Filter)).Select(x => x.LocID).ToList();
            var filteredAssemblies = _assemblyRepository.GetAll()
                                     .WhereIf(input.Filter != null, e => e.DocNo.ToString() == input.Filter || fitlerLoc.Contains(e.LocID) ||
                                     e.DocDate.Value.Date.ToString() == input.Filter);
            //.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Narration.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
            //.WhereIf(input.MinLocIDFilter != null, e => e.LocID >= input.MinLocIDFilter)
            //.WhereIf(input.MaxLocIDFilter != null, e => e.LocID <= input.MaxLocIDFilter)
            //.WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
            //.WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
            //.WhereIf(input.MinDocDateFilter != null, e => e.DocDate >= input.MinDocDateFilter)
            //.WhereIf(input.MaxDocDateFilter != null, e => e.DocDate <= input.MaxDocDateFilter)
            //.WhereIf(!string.IsNullOrWhiteSpace(input.NarrationFilter), e => e.Narration == input.NarrationFilter)
            //.WhereIf(input.PostedFilter > -1, e => (input.PostedFilter == 1 && e.Posted) || (input.PostedFilter == 0 && !e.Posted))
            //.WhereIf(input.MinLinkDetIDFilter != null, e => e.LinkDetID >= input.MinLinkDetIDFilter)
            //.WhereIf(input.MaxLinkDetIDFilter != null, e => e.LinkDetID <= input.MaxLinkDetIDFilter)
            //.WhereIf(input.MinActiveFilter != null, e => e.Active >= input.MinActiveFilter)
            //.WhereIf(input.MaxActiveFilter != null, e => e.Active <= input.MaxActiveFilter)
            //.WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
            //.WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
            //.WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
            //.WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
            //.WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
            //.WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var pagedAndFilteredAssemblies = filteredAssemblies
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var assemblies = from o in pagedAndFilteredAssemblies
                             select new GetAssemblyForViewDto()
                             {
                                 Assembly = new AssemblyDto
                                 {
                                     LocID = o.LocID,
                                     DocNo = o.DocNo,
                                     DocDate = o.DocDate,
                                     Narration = o.Narration,
                                     Posted = o.Posted,
                                     LinkDetID = o.LinkDetID,
                                     OrdNo = o.OrdNo,
                                     Active = o.Active,
                                     AudtUser = o.AudtUser,
                                     AudtDate = o.AudtDate,
                                     CreatedBy = o.CreatedBy,
                                     CreateDate = o.CreateDate,
                                     Id = o.Id,
                                     locName = _locRepository.GetAll().Where(p => p.LocID == o.LocID).FirstOrDefault().LocName
                                 }
                             };

            var totalCount = await filteredAssemblies.CountAsync();

            return new PagedResultDto<GetAssemblyForViewDto>(
                totalCount,
                await assemblies.ToListAsync()
            );
        }

        public async Task<GetAssemblyForViewDto> GetAssemblyForView(int id)
        {
            var assembly = await _assemblyRepository.GetAsync(id);

            var output = new GetAssemblyForViewDto { Assembly = ObjectMapper.Map<AssemblyDto>(assembly) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Inventory_Assemblies_Edit)]
        public async Task<GetAssemblyForEditOutput> GetAssemblyForEdit(EntityDto input)
        {
            var itemList = await _itemRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId).ToListAsync();
            var assembly = await _assemblyRepository.FirstOrDefaultAsync(x => x.DocNo == input.Id && x.TenantId == AbpSession.TenantId);
            var assemblyDetail = await _assemblyDetailRepository.GetAll().Where(e => e.DocNo == input.Id && e.TenantId == AbpSession.TenantId).ToListAsync();
            var output = new GetAssemblyForEditOutput { Assembly = ObjectMapper.Map<CreateOrEditAssemblyDto>(assembly) };
            output.Assembly.AssemblyDetailDto = ObjectMapper.Map<List<AssemblyDetailDto>>(assemblyDetail);
            output.Assembly.LocDesc = _locRepository.GetAll().Where(o => o.LocID == assembly.LocID).Count() > 0
                ?
                _locRepository.GetAll().Where(o => o.LocID == assembly.LocID).FirstOrDefault().LocName
                : "";
            foreach (var data in output.Assembly.AssemblyDetailDto)
            {
                var item = itemList.Where(o => o.ItemId == data.ItemId);
                output.Assembly.AssemblyDetailDto.Where(o => o.Id == data.Id).FirstOrDefault().ItemId = item.FirstOrDefault().ItemId + "*" + item.FirstOrDefault().Descp + "*" + data.Unit + "*" + data.Conver;
            }
            //var output = new GetAssemblyForEditOutput { Assembly = ObjectMapper.Map<CreateOrEditAssemblyDto>(assembly) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditAssemblyDto input)
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

        [AbpAuthorize(AppPermissions.Inventory_Assemblies_Create)]
        protected virtual async Task Create(CreateOrEditAssemblyDto input)
        {
            var assembly = ObjectMapper.Map<Assembly>(input);
            assembly.CreatedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            assembly.CreateDate = DateTime.Now;
            assembly.DocNo = GetDocId();
            if (AbpSession.TenantId != null)
            {
                assembly.TenantId = (int)AbpSession.TenantId;
            }

            var getId = await _assemblyRepository.InsertAndGetIdAsync(assembly);
            decimal rawMaterialRateTotal = 0;
            foreach (var data in input.AssemblyDetailDto)
            {
                if (data.TransType == 9)
                {
                    var assemblyDetail = ObjectMapper.Map<AssemblyDetails>(data);
                    if (AbpSession.TenantId != null)
                    {
                        assemblyDetail.TenantId = (int)AbpSession.TenantId;
                    }
                    assemblyDetail.DetID = getId;
                    assemblyDetail.DocNo = input.DocNo;
                    assemblyDetail.LocID = assembly.LocID;
                    assemblyDetail.Rate = Convert.ToDecimal(_costingService.getCosting(input.DocDate.Value.Date, assemblyDetail.ItemID, assemblyDetail.LocID, assemblyDetail.TransType, input.DocNo));
                    assemblyDetail.Amount = assemblyDetail.Qty * assemblyDetail.Rate;
                    rawMaterialRateTotal = rawMaterialRateTotal + assemblyDetail.Amount;
                    await _assemblyDetailRepository.InsertAsync(assemblyDetail);
                }

            }
            foreach (var data in input.AssemblyDetailDto)
            {
                if (data.TransType == 7 || data.TransType == 8)
                {
                    var assemblyDetail = ObjectMapper.Map<AssemblyDetails>(data);
                    if (AbpSession.TenantId != null)
                    {
                        assemblyDetail.TenantId = (int)AbpSession.TenantId;
                    }
                    assemblyDetail.DetID = getId;
                    assemblyDetail.DocNo = input.DocNo;
                    assemblyDetail.LocID = assembly.LocID;
                    assemblyDetail.Amount = rawMaterialRateTotal + input.OverHead;
                    assemblyDetail.Rate = rawMaterialRateTotal / assemblyDetail.Qty;
                    await _assemblyDetailRepository.InsertAsync(assemblyDetail);
                }
            }
        }

        [AbpAuthorize(AppPermissions.Inventory_Assemblies_Edit)]
        protected virtual async Task Update(CreateOrEditAssemblyDto input)
        {
            var assembly = await _assemblyRepository.FirstOrDefaultAsync(x => x.DocNo == input.DocNo && x.TenantId == AbpSession.TenantId);
            input.AudtUser = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            input.AudtDate = DateTime.Now;
            ObjectMapper.Map(input, assembly);
            await _assemblyDetailRepository.DeleteAsync(o => o.TenantId == AbpSession.TenantId && o.DocNo == input.DocNo);
            decimal rawMaterialRateTotal = 0;
            foreach (var data in input.AssemblyDetailDto)
            {
                if (data.TransType == 9)
                {
                    var assemblyDetail = ObjectMapper.Map<AssemblyDetails>(data);
                    if (AbpSession.TenantId != null)
                    {
                        assemblyDetail.TenantId = (int)AbpSession.TenantId;
                    }
                    assemblyDetail.DetID = assembly.Id;
                    assemblyDetail.DocNo = input.DocNo;
                    assemblyDetail.LocID = assembly.LocID;
                    assemblyDetail.Rate = Convert.ToDecimal(_costingService.getCosting(input.DocDate.Value.Date, assemblyDetail.ItemID, assemblyDetail.LocID, assemblyDetail.TransType, input.DocNo));
                    assemblyDetail.Amount = assemblyDetail.Qty * assemblyDetail.Rate;
                    rawMaterialRateTotal = rawMaterialRateTotal + assemblyDetail.Amount;
                    await _assemblyDetailRepository.InsertAsync(assemblyDetail);
                }

            }
            foreach (var data in input.AssemblyDetailDto)
            {
                if (data.TransType == 7 || data.TransType == 8)
                {
                    var assemblyDetail = ObjectMapper.Map<AssemblyDetails>(data);
                    if (AbpSession.TenantId != null)
                    {
                        assemblyDetail.TenantId = (int)AbpSession.TenantId;
                    }
                    assemblyDetail.DetID = assembly.Id;
                    assemblyDetail.DocNo = input.DocNo;
                    assemblyDetail.LocID = assembly.LocID;
                    assemblyDetail.Amount = rawMaterialRateTotal + input.OverHead;
                    assemblyDetail.Rate = rawMaterialRateTotal / assemblyDetail.Qty;
                    await _assemblyDetailRepository.InsertAsync(assemblyDetail);
                }
            }
        }

        [AbpAuthorize(AppPermissions.Inventory_Assemblies_Delete)]
        public async Task Delete(EntityDto input)
        {
            DeleteLog(input.Id);
            await _assemblyRepository.DeleteAsync(x => x.DocNo == input.Id && x.TenantId == AbpSession.TenantId);
            await _assemblyDetailRepository.DeleteAsync(o => o.TenantId == AbpSession.TenantId && o.DocNo == input.Id);
        }
        public void DeleteLog(int detid)
        {
            var data = _assemblyRepository.FirstOrDefault(c => c.DocNo == detid && c.TenantId == AbpSession.TenantId);
            LogModel model = new LogModel()
            {
                Action = "Delete",
                Detid = data.Id,
                DocNo = detid,
                FormName = "Assemblies",
                Status = true,
                ApprovedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName,
                TenantId = AbpSession.TenantId
            };
            _commonappRepository.ApproveLog(model);
        }
        public int GetDocId()
        {
            var result = _assemblyRepository.GetAll().DefaultIfEmpty().Max(o => o.DocNo);
            return result = result + 1;
        }

        public async Task<FileDto> GetDataToExcel(GetAllAssembliesInput input)
        {
            var filteredAssemblies = _assemblyRepository.GetAll()
                           .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Narration.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter))
                           .WhereIf(input.MinLocIDFilter != null, e => e.LocID >= input.MinLocIDFilter)
                           .WhereIf(input.MaxLocIDFilter != null, e => e.LocID <= input.MaxLocIDFilter)
                           .WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
                           .WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
                           .WhereIf(input.MinDocDateFilter != null, e => e.DocDate >= input.MinDocDateFilter)
                           .WhereIf(input.MaxDocDateFilter != null, e => e.DocDate <= input.MaxDocDateFilter)
                           .WhereIf(!string.IsNullOrWhiteSpace(input.NarrationFilter), e => e.Narration == input.NarrationFilter)
                           .WhereIf(input.PostedFilter > -1, e => (input.PostedFilter == 1 && e.Posted) || (input.PostedFilter == 0 && !e.Posted))
                           .WhereIf(input.MinLinkDetIDFilter != null, e => e.LinkDetID >= input.MinLinkDetIDFilter)
                           .WhereIf(input.MaxLinkDetIDFilter != null, e => e.LinkDetID <= input.MaxLinkDetIDFilter)
                           .WhereIf(input.MinActiveFilter != null, e => e.Active >= input.MinActiveFilter)
                           .WhereIf(input.MaxActiveFilter != null, e => e.Active <= input.MaxActiveFilter)
                           .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                           .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                           .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                           .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                           .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                           .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter);

            var query = from o in filteredAssemblies
                        select new GetAssemblyForViewDto()
                        {
                            Assembly = new AssemblyDto
                            {
                                LocID = o.LocID,
                                DocNo = o.DocNo,
                                DocDate = o.DocDate,
                                Narration = o.Narration,
                                Posted = o.Posted,
                                LinkDetID = o.LinkDetID,
                                OrdNo = o.OrdNo,
                                Active = o.Active,
                                AudtUser = o.AudtUser,
                                AudtDate = o.AudtDate,
                                CreatedBy = o.CreatedBy,
                                CreateDate = o.CreateDate,
                                Id = o.Id,
                                locName = _locRepository.GetAll().Where(p => p.LocID == o.LocID).FirstOrDefault().LocName
                            }
                        };


            var dataDto = await query.ToListAsync();

            return _assemblyExcelExporter.ExportToFile(dataDto);
        }

        public PagedResultDto<AssemblyDto> GetDetailsForApproval(string fromDate, string toDate, string Mode, int fromDoc, int ToDoc, GetAllAssembliesInput input)
        {
            IQueryable<Assembly> data = null;
            if (Mode == "Posting" || Mode == "UnApproval")
            {
                data = _assemblyRepository.GetAll().Where(o => o.DocDate.Value.Date >= Convert.ToDateTime(fromDate).Date
             && o.DocDate.Value.Date <= Convert.ToDateTime(toDate).Date && o.Approved == true
             //&& o.Posted == false 
             && o.TenantId == AbpSession.TenantId && o.DocNo >= fromDoc && o.DocNo <= ToDoc);
            }
            else
            {
                data = _assemblyRepository.GetAll().Where(o => o.DocDate.Value.Date >= Convert.ToDateTime(fromDate).Date
                            && o.DocDate.Value.Date <= Convert.ToDateTime(toDate).Date && o.Approved == false && o.TenantId == AbpSession.TenantId && o.DocNo >= fromDoc && o.DocNo <= ToDoc);
            }

            var pageData = data.PageBy(input);

            var paginatedData = from o in pageData
                                select new AssemblyDto()
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
            return new PagedResultDto<AssemblyDto>(
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
                    (from a in _assemblyRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && postedDataIds.Contains(o.Id))
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
                    (from a in _assemblyRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && postedDataIds.Contains(o.Id))
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