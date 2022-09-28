using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.Payroll.SlabSetup.Exporting;
using ERP.Payroll.SlabSetup.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using ERP.Tenants.DbPerTenant;

namespace ERP.Payroll.SlabSetup
{
    [AbpAuthorize(AppPermissions.PayRoll_SlabSetup)]
    public class SlabSetupAppService : ERPAppServiceBase, ISlabSetupAppService
    {
        private readonly IRepository<SlabSetup> _slabSetupRepository;
        private readonly ISlabSetupExcelExporter _slabSetupExcelExporter;
        private readonly IDBPerTenantAppService _IDBPerTenantAppService;
        public SlabSetupAppService(IRepository<SlabSetup> slabSetupRepository, IDBPerTenantAppService iDBPerTenantAppService, ISlabSetupExcelExporter slabSetupExcelExporter)
        {
            _slabSetupRepository = slabSetupRepository;
            _slabSetupExcelExporter = slabSetupExcelExporter;
            _IDBPerTenantAppService = iDBPerTenantAppService;
        }

        public async Task<PagedResultDto<GetSlabSetupForViewDto>> GetAll(GetAllSlabSetupInput input)
        {

            var filteredSlabSetup = _slabSetupRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter) || e.ModifiedBy.Contains(input.Filter))
                        .WhereIf(input.MinTypeIDFilter != null, e => e.TypeID >= input.MinTypeIDFilter)
                        .WhereIf(input.MaxTypeIDFilter != null, e => e.TypeID <= input.MaxTypeIDFilter)
                        .WhereIf(input.MinSlabFromFilter != null, e => e.SlabFrom >= input.MinSlabFromFilter)
                        .WhereIf(input.MaxSlabFromFilter != null, e => e.SlabFrom <= input.MaxSlabFromFilter)
                        .WhereIf(input.MinSlabToFilter != null, e => e.SlabTo >= input.MinSlabToFilter)
                        .WhereIf(input.MaxSlabToFilter != null, e => e.SlabTo <= input.MaxSlabToFilter)
                        .WhereIf(input.MinRateFilter != null, e => e.Rate >= input.MinRateFilter)
                        .WhereIf(input.MaxRateFilter != null, e => e.Rate <= input.MaxRateFilter)
                        .WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
                        .WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
                        .WhereIf(input.ActiveFilter.HasValue && input.ActiveFilter > -1, e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ModifiedByFilter), e => e.ModifiedBy == input.ModifiedByFilter)
                        .WhereIf(input.MinModifyDateFilter != null, e => e.ModifyDate >= input.MinModifyDateFilter)
                        .WhereIf(input.MaxModifyDateFilter != null, e => e.ModifyDate <= input.MaxModifyDateFilter);

            var pagedAndFilteredSlabSetup = filteredSlabSetup
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var slabSetup = from o in pagedAndFilteredSlabSetup
                            select new GetSlabSetupForViewDto()
                            {
                                SlabSetup = new SlabSetupDto
                                {
                                    TypeID = o.TypeID,
                                    SlabFrom = o.SlabFrom,
                                    SlabTo = o.SlabTo,
                                    Rate = o.Rate,
                                    Amount = o.Amount,
                                    Active = o.Active,
                                    AudtUser = o.AudtUser,
                                    AudtDate = o.AudtDate,
                                    CreatedBy = o.CreatedBy,
                                    CreateDate = o.CreateDate,
                                    ModifiedBy = o.ModifiedBy,
                                    ModifyDate = o.ModifyDate,
                                    Id = o.Id
                                }
                            };

            var totalCount = await filteredSlabSetup.CountAsync();

            return new PagedResultDto<GetSlabSetupForViewDto>(
                totalCount,
                await slabSetup.ToListAsync()
            );
        }

        public async Task<GetSlabSetupForViewDto> GetSlabSetupForView(int id)
        {
            var slabSetup = await _slabSetupRepository.GetAsync(id);

            var output = new GetSlabSetupForViewDto { SlabSetup = ObjectMapper.Map<SlabSetupDto>(slabSetup) };

            return output;
        }

        [AbpAuthorize(AppPermissions.PayRoll_SlabSetup_Edit)]
        public async Task<GetSlabSetupForEditOutput> GetSlabSetupForEdit(EntityDto input)
        {
            var slabSetup = await _slabSetupRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetSlabSetupForEditOutput { SlabSetup = ObjectMapper.Map<CreateOrEditSlabSetupDto>(slabSetup) };

            return output;
        }
        public async Task<CreateOrEditSlabSetupDto> GetSlabSetupForSalary(Int32 amt)
        {
            var tenantid = (int)AbpSession.TenantId;
            var connctions = _IDBPerTenantAppService.GetAllConnections(tenantid, "HRM");
           var conStr=connctions.Result.Items[0].VConnectionString;

            
            SqlConnection con = new SqlConnection(conStr);
            
            CreateOrEditSlabSetupDto model = new CreateOrEditSlabSetupDto();
            SqlCommand cmd = new SqlCommand("TaxSlabForSalary",con);
            cmd.CommandType =CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@TenantID", tenantid);
            cmd.Parameters.AddWithValue("@Salary", amt);
            con.Open();
            using (SqlDataReader rdr=cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    model = new CreateOrEditSlabSetupDto
                    {
                        SlabFrom=Convert.ToDouble(rdr["SlabFrom"].ToString()),
                        SlabTo=Convert.ToDouble(rdr["SlabTo"].ToString()),
                        Rate = Convert.ToDouble(rdr["Rate"].ToString()),
                        Amount = Convert.ToDouble(rdr["Amount"].ToString())
                    };
                }
            }




                return model;
        }

        public async Task CreateOrEdit(CreateOrEditSlabSetupDto input)
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

        [AbpAuthorize(AppPermissions.PayRoll_SlabSetup_Create)]
        protected virtual async Task Create(CreateOrEditSlabSetupDto input)
        {
            var slabSetup = ObjectMapper.Map<SlabSetup>(input);

            if (AbpSession.TenantId != null)
            {
                slabSetup.TenantId = (int)AbpSession.TenantId;
            }

            await _slabSetupRepository.InsertAsync(slabSetup);
        }

        [AbpAuthorize(AppPermissions.PayRoll_SlabSetup_Edit)]
        protected virtual async Task Update(CreateOrEditSlabSetupDto input)
        {
            var slabSetup = await _slabSetupRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, slabSetup);
        }

        [AbpAuthorize(AppPermissions.PayRoll_SlabSetup_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _slabSetupRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetSlabSetupToExcel(GetAllSlabSetupForExcelInput input)
        {

            var filteredSlabSetup = _slabSetupRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter) || e.ModifiedBy.Contains(input.Filter))
                        .WhereIf(input.MinTypeIDFilter != null, e => e.TypeID >= input.MinTypeIDFilter)
                        .WhereIf(input.MaxTypeIDFilter != null, e => e.TypeID <= input.MaxTypeIDFilter)
                        .WhereIf(input.MinSlabFromFilter != null, e => e.SlabFrom >= input.MinSlabFromFilter)
                        .WhereIf(input.MaxSlabFromFilter != null, e => e.SlabFrom <= input.MaxSlabFromFilter)
                        .WhereIf(input.MinSlabToFilter != null, e => e.SlabTo >= input.MinSlabToFilter)
                        .WhereIf(input.MaxSlabToFilter != null, e => e.SlabTo <= input.MaxSlabToFilter)
                        .WhereIf(input.MinRateFilter != null, e => e.Rate >= input.MinRateFilter)
                        .WhereIf(input.MaxRateFilter != null, e => e.Rate <= input.MaxRateFilter)
                        .WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
                        .WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
                        .WhereIf(input.ActiveFilter.HasValue && input.ActiveFilter > -1, e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser == input.AudtUserFilter)
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy == input.CreatedByFilter)
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ModifiedByFilter), e => e.ModifiedBy == input.ModifiedByFilter)
                        .WhereIf(input.MinModifyDateFilter != null, e => e.ModifyDate >= input.MinModifyDateFilter)
                        .WhereIf(input.MaxModifyDateFilter != null, e => e.ModifyDate <= input.MaxModifyDateFilter);

            var query = (from o in filteredSlabSetup
                         select new GetSlabSetupForViewDto()
                         {
                             SlabSetup = new SlabSetupDto
                             {
                                 TypeID = o.TypeID,
                                 SlabFrom = o.SlabFrom,
                                 SlabTo = o.SlabTo,
                                 Rate = o.Rate,
                                 Amount = o.Amount,
                                 Active = o.Active,
                                 AudtUser = o.AudtUser,
                                 AudtDate = o.AudtDate,
                                 CreatedBy = o.CreatedBy,
                                 CreateDate = o.CreateDate,
                                 ModifiedBy = o.ModifiedBy,
                                 ModifyDate = o.ModifyDate,
                                 Id = o.Id
                             }
                         });

            var slabSetupListDtos = await query.ToListAsync();

            return _slabSetupExcelExporter.ExportToFile(slabSetupListDtos);
        }

    }
}