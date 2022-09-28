

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.GeneralLedger.SetupForms.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ERP.GeneralLedger.SetupForms.GLPLCategory.Dtos;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace ERP.GeneralLedger.SetupForms
{
    [AbpAuthorize(AppPermissions.Pages_GLBSCtg)]
    public class GLBSCtgAppService : ERPAppServiceBase, IGLBSCtgAppService
    {
        private readonly IRepository<GLBSCtg> _glbsCtgRepository;


        public GLBSCtgAppService(IRepository<GLBSCtg> glbsCtgRepository)
        {
            _glbsCtgRepository = glbsCtgRepository;

        }

        public async Task<PagedResultDto<GetGLBSCtgForViewDto>> GetAll(GetAllGLBSCtgInput input)
        {

            var filteredGLBSCtg = _glbsCtgRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.BSType.Contains(input.Filter) || e.BSAccType.Contains(input.Filter) || e.BSAccDesc.Contains(input.Filter))
                        .WhereIf(input.MinTenantIDFilter != null, e => e.TenantID >= input.MinTenantIDFilter)
                        .WhereIf(input.MaxTenantIDFilter != null, e => e.TenantID <= input.MaxTenantIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BSTypeFilter), e => e.BSType == input.BSTypeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BSAccTypeFilter), e => e.BSAccType == input.BSAccTypeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BSAccDescFilter), e => e.BSAccDesc == input.BSAccDescFilter)
                        .WhereIf(input.MinSortOrderFilter != null, e => e.SortOrder >= input.MinSortOrderFilter)
                        .WhereIf(input.MaxSortOrderFilter != null, e => e.SortOrder <= input.MaxSortOrderFilter)
                        .WhereIf(input.MinBSGIDFilter != null, e => e.BSGID >= input.MinBSGIDFilter)
                        .WhereIf(input.MaxBSGIDFilter != null, e => e.BSGID <= input.MaxBSGIDFilter);

            var pagedAndFilteredGLBSCtg = filteredGLBSCtg
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var glbsCtg = from o in pagedAndFilteredGLBSCtg
                          select new GetGLBSCtgForViewDto()
                          {
                              GLBSCtg = new GLBSCtgDto
                              {
                                  TenantID = o.TenantID,
                                  BSType = o.BSType,
                                  BSAccType = o.BSAccType,
                                  BSAccDesc = o.BSAccDesc,
                                  SortOrder = o.SortOrder,
                                  BSGID = o.BSGID,
                                  Id = o.Id
                              }
                          };

            var totalCount = await filteredGLBSCtg.CountAsync();

            return new PagedResultDto<GetGLBSCtgForViewDto>(
                totalCount,
                await glbsCtg.ToListAsync()
            );
        }

        public async Task<GetGLBSCtgForViewDto> GetGLBSCtgForView(int id)
        {
            var glbsCtg = await _glbsCtgRepository.GetAsync(id);

            var output = new GetGLBSCtgForViewDto { GLBSCtg = ObjectMapper.Map<GLBSCtgDto>(glbsCtg) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_GLBSCtg_Edit)]
        public async Task<GetGLBSCtgForEditOutput> GetGLBSCtgForEdit(EntityDto input)
        {
            var glbsCtg = await _glbsCtgRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetGLBSCtgForEditOutput { GLBSCtg = ObjectMapper.Map<CreateOrEditGLBSCtgDto>(glbsCtg) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditGLBSCtgDto input)
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

        [AbpAuthorize(AppPermissions.Pages_GLBSCtg_Create)]
        protected virtual async Task Create(CreateOrEditGLBSCtgDto input)
        {
            var glbsCtg = ObjectMapper.Map<GLBSCtg>(input);


            if (AbpSession.TenantId != null)
            {
                glbsCtg.TenantId = (int)AbpSession.TenantId;
            }


            await _glbsCtgRepository.InsertAsync(glbsCtg);
        }

        [AbpAuthorize(AppPermissions.Pages_GLBSCtg_Edit)]
        protected virtual async Task Update(CreateOrEditGLBSCtgDto input)
        {
            var glbsCtg = await _glbsCtgRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, glbsCtg);
        }

        [AbpAuthorize(AppPermissions.Pages_GLBSCtg_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _glbsCtgRepository.DeleteAsync(input.Id);
        }

        public async Task<ListResultDto<PLCategoryComboboxItemDto>> getCFCategoryList(string input)
        {

            string str = ConfigurationManager.AppSettings["ConnectionString"];
            List<PLCategoryComboboxItemDto> glcfDtoList = new List<PLCategoryComboboxItemDto>();
            SqlCommand cmd;
            using (SqlConnection cn = new SqlConnection(str))
            {


                cmd = new SqlCommand("sp_GetGLCFCtg", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CFtype", input);
                cmd.Parameters.AddWithValue("@TenantId", AbpSession.TenantId);
                cn.Open();
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    PLCategoryComboboxItemDto QutationData1 = new PLCategoryComboboxItemDto()
                    {
                        Id = -1,
                        DisplayText = "Please Select",
                        SortOrder = 0,
                        IsSelected = true
                    };
                    glcfDtoList.Add(QutationData1);
                    while (rdr.Read())
                    {
                        PLCategoryComboboxItemDto QutationData = new PLCategoryComboboxItemDto()
                        {
                            Id = rdr["Id"] is DBNull ? 0 : Convert.ToInt32(rdr["Id"]),
                            DisplayText = rdr["CFTypeDesc"] is DBNull ? "" : Convert.ToString(rdr["CFTypeDesc"]),
                            SortOrder = rdr["SortOrder"] is DBNull ? 0 : Convert.ToInt32(rdr["SortOrder"]),
                        };

                        glcfDtoList.Add(QutationData);

                    }


                }

                return new ListResultDto<PLCategoryComboboxItemDto>(
                    glcfDtoList
                    );
            }
        }
        public async Task<ListResultDto<PLCategoryComboboxItemDto>> getBSCategoryList(string input)
        {

            var filteredCategories = _glbsCtgRepository.GetAll().Where(e => e.TenantId == AbpSession.TenantId)
                .WhereIf(!string.IsNullOrWhiteSpace(input), e => e.BSAccType.ToLower() == input.ToLower());

            var query = (from o in filteredCategories
                         select new PLCategoryComboboxItemDto
                         {
                             SortOrder = (int?)o.SortOrder,
                             DisplayText = o.BSAccDesc,
                             Id = o.Id
                         });


            var categoryListDtos = await query.ToListAsync();
            categoryListDtos.Add(new PLCategoryComboboxItemDto
            {
                SortOrder = 0,
                DisplayText = "Please Select",
                Id = -1,
                IsSelected = true
            });
            categoryListDtos.Reverse();
            return new ListResultDto<PLCategoryComboboxItemDto>(
                categoryListDtos
                );
        }
    }
}