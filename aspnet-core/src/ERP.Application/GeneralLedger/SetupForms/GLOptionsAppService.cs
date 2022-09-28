using ERP.GeneralLedger.SetupForms;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.GeneralLedger.SetupForms.Exporting;
using ERP.GeneralLedger.SetupForms.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;


namespace ERP.GeneralLedger.SetupForms
{
	[AbpAuthorize(AppPermissions.Pages_GLOptions)]
    public class GLOptionsAppService : ERPAppServiceBase, IGLOptionsAppService
    {
		 private readonly IRepository<GLOption> _glOptionRepository;
		 private readonly IGLOptionsExcelExporter _glOptionsExcelExporter;
		 private readonly IRepository<ChartofControl,string> _lookup_chartofControlRepository;
		 

		  public GLOptionsAppService(IRepository<GLOption> glOptionRepository, IGLOptionsExcelExporter glOptionsExcelExporter , IRepository<ChartofControl, string> lookup_chartofControlRepository) 
		  {
			_glOptionRepository = glOptionRepository;
			_glOptionsExcelExporter = glOptionsExcelExporter;
			_lookup_chartofControlRepository = lookup_chartofControlRepository;
		
		  }

		 public async Task<PagedResultDto<GetGLOptionForViewDto>> GetAll(GetAllGLOptionsInput input)
         {

            var filteredGLOptions = _glOptionRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.DEFAULTCLACC.Contains(input.Filter) || e.STOCKCTRLACC.Contains(input.Filter) || e.Seg1Name.Contains(input.Filter) || e.Seg2Name.Contains(input.Filter) || e.Seg3Name.Contains(input.Filter) || e.AUDTUSER.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DEFAULTCLACCFilter), e => e.DEFAULTCLACC.ToLower() == input.DEFAULTCLACCFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.STOCKCTRLACCFilter), e => e.STOCKCTRLACC.ToLower() == input.STOCKCTRLACCFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Seg1NameFilter), e => e.Seg1Name.ToLower() == input.Seg1NameFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Seg2NameFilter), e => e.Seg2Name.ToLower() == input.Seg2NameFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Seg3NameFilter), e => e.Seg3Name.ToLower() == input.Seg3NameFilter.ToLower().Trim())
                        .WhereIf(input.DirectPostFilter > -1, e => Convert.ToInt32(e.DirectPost) == input.DirectPostFilter)
                        .WhereIf(input.AutoSeg3Filter > -1, e => Convert.ToInt32(e.AutoSeg3) == input.AutoSeg3Filter)
                        .WhereIf(input.MinAUDTDATEFilter != null, e => e.AUDTDATE >= input.MinAUDTDATEFilter)
                        .WhereIf(input.MaxAUDTDATEFilter != null, e => e.AUDTDATE <= input.MaxAUDTDATEFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AUDTUSERFilter), e => e.AUDTUSER.ToLower() == input.AUDTUSERFilter.ToLower().Trim()).Where(o => o.TenantId == AbpSession.TenantId);

			var pagedAndFilteredGLOptions = filteredGLOptions
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

			var glOptions = from o in pagedAndFilteredGLOptions
                         
                         select new GetGLOptionForViewDto() {
							GLOption = new GLOptionDto
							{
                                DEFAULTCLACC = o.DEFAULTCLACC,
                                STOCKCTRLACC = o.STOCKCTRLACC,
                                Seg1Name = o.Seg1Name,
                                Seg2Name = o.Seg2Name,
                                Seg3Name = o.Seg3Name,
                                DirectPost = o.DirectPost,
                                AutoSeg3 = o.AutoSeg3,
                                FirstSignature = o.FirstSignature != null ? o.FirstSignature : "",
                                SecondSignature = o.SecondSignature != null ? o.SecondSignature : "",
                                ThirdSignature = o.ThirdSignature != null ? o.ThirdSignature : "",
                                FourthSignature = o.FourthSignature != null ? o.FourthSignature : "",
                                FifthSignature = o.FifthSignature != null ? o.FifthSignature : "",
                                SixthSignature = o.SixthSignature != null ? o.SixthSignature : "",
                                DocFrequency=o.DocFrequency,
                                AUDTDATE = o.AUDTDATE,
                                AUDTUSER = o.AUDTUSER,
                                Id = o.Id
							},
						};

            var totalCount = await filteredGLOptions.CountAsync();

            return new PagedResultDto<GetGLOptionForViewDto>(
                totalCount,
                await glOptions.ToListAsync()
            );
         }
		 
		 public async Task<GetGLOptionForViewDto> GetGLOptionForView(int id)
         {
            var glOption = await _glOptionRepository.GetAsync(id);

            var output = new GetGLOptionForViewDto { GLOption = ObjectMapper.Map<GLOptionDto>(glOption) };

		    //if (output.GLOption.ChartofControlId != null)
      //      {
      //          var _lookupChartofControl = await _lookup_chartofControlRepository.FirstOrDefaultAsync((string)output.GLOption.ChartofControlId);
      //          output.ChartofControlId = _lookupChartofControl.Id.ToString();
      //      }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_GLOptions_Edit)]
		 public async Task<GetGLOptionForEditOutput> GetGLOptionForEdit(EntityDto input)
         {
            var glOption = await _glOptionRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetGLOptionForEditOutput {GLOption = ObjectMapper.Map<CreateOrEditGLOptionDto>(glOption)};

		    //if (output.GLOption.ChartofControlId != null)
      //      {
      //          var _lookupChartofControl = await _lookup_chartofControlRepository.FirstOrDefaultAsync((string)output.GLOption.ChartofControlId);
      //          output.ChartofControlId = _lookupChartofControl.Id.ToString();
      //      }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditGLOptionDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_GLOptions_Create)]
		 protected virtual async Task Create(CreateOrEditGLOptionDto input)
         {
            var record = _glOptionRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId).Count();
            if (record == 0)
            {
                var glOption = ObjectMapper.Map<GLOption>(input);


                if (AbpSession.TenantId != null)
                {
                    glOption.TenantId = (int)AbpSession.TenantId;
                }


                await _glOptionRepository.InsertAsync(glOption);
            }
         }

		 [AbpAuthorize(AppPermissions.Pages_GLOptions_Edit)]
		 protected virtual async Task Update(CreateOrEditGLOptionDto input)
         {
            var glOption = await _glOptionRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, glOption);
         }

		 [AbpAuthorize(AppPermissions.Pages_GLOptions_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _glOptionRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetGLOptionsToExcel(GetAllGLOptionsForExcelInput input)
         {

            var filteredGLOptions = _glOptionRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.DEFAULTCLACC.Contains(input.Filter) || e.STOCKCTRLACC.Contains(input.Filter) || e.Seg1Name.Contains(input.Filter) || e.Seg2Name.Contains(input.Filter) || e.Seg3Name.Contains(input.Filter) || e.AUDTUSER.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DEFAULTCLACCFilter), e => e.DEFAULTCLACC.ToLower() == input.DEFAULTCLACCFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.STOCKCTRLACCFilter), e => e.STOCKCTRLACC.ToLower() == input.STOCKCTRLACCFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Seg1NameFilter), e => e.Seg1Name.ToLower() == input.Seg1NameFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Seg2NameFilter), e => e.Seg2Name.ToLower() == input.Seg2NameFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Seg3NameFilter), e => e.Seg3Name.ToLower() == input.Seg3NameFilter.ToLower().Trim())
                        .WhereIf(input.DirectPostFilter > -1, e => Convert.ToInt32(e.DirectPost) == input.DirectPostFilter)
                        .WhereIf(input.MinAUDTDATEFilter != null, e => e.AUDTDATE >= input.MinAUDTDATEFilter)
                        .WhereIf(input.MaxAUDTDATEFilter != null, e => e.AUDTDATE <= input.MaxAUDTDATEFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AUDTUSERFilter), e => e.AUDTUSER.ToLower() == input.AUDTUSERFilter.ToLower().Trim()).Where(o => o.TenantId == AbpSession.TenantId);

			var query = (from o in filteredGLOptions
                         
                         select new GetGLOptionForViewDto() { 
							GLOption = new GLOptionDto
							{
                                DEFAULTCLACC = o.DEFAULTCLACC,
                                STOCKCTRLACC = o.STOCKCTRLACC,
                                Seg1Name = o.Seg1Name,
                                Seg2Name = o.Seg2Name,
                                Seg3Name = o.Seg3Name,
                                DirectPost = o.DirectPost,
                                AutoSeg3 = o.AutoSeg3,
                                FirstSignature = o.FirstSignature != null ? o.FirstSignature : "",
                                SecondSignature = o.SecondSignature != null ? o.SecondSignature : "",
                                ThirdSignature = o.ThirdSignature != null ? o.ThirdSignature : "",
                                FourthSignature = o.FourthSignature != null ? o.FourthSignature : "",
                                FifthSignature = o.FifthSignature != null ? o.FifthSignature : "",
                                SixthSignature = o.SixthSignature != null ? o.SixthSignature : "",
                                AUDTDATE = o.AUDTDATE,
                                AUDTUSER = o.AUDTUSER,
                                Id = o.Id
							},
						 });


            var glOptionListDtos = await query.ToListAsync();

            return _glOptionsExcelExporter.ExportToFile(glOptionListDtos);
         }

		[AbpAuthorize(AppPermissions.Pages_GLOptions)]
         public async Task<PagedResultDto<GLOptionChartofControlLookupTableDto>> GetAllChartofControlForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_chartofControlRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Id.ToString().Contains(input.Filter)
                ).WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e => e.AccountName.ToString().Contains(input.Filter)
                ).Where(o => o.TenantId == AbpSession.TenantId);

            var totalCount = await query.CountAsync();

            var chartofControlList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<GLOptionChartofControlLookupTableDto>();
			foreach(var chartofControl in chartofControlList){
				lookupTableDtoList.Add(new GLOptionChartofControlLookupTableDto
				{
					Id = chartofControl.Id,
					DisplayName = chartofControl.AccountName?.ToString()
				});
			}

            return new PagedResultDto<GLOptionChartofControlLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }


        public bool? GetInstrumentNoChk()
        {
            return _glOptionRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId).Count() > 0 ?
                   _glOptionRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId).First().InstrumentNo : false;
        }


    }
}