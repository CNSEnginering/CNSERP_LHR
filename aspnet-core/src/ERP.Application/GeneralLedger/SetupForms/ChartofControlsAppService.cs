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
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;

namespace ERP.GeneralLedger.SetupForms
{
    [AbpAuthorize(AppPermissions.SetupForms_ChartofControls)]
    public class ChartofControlsAppService : ERPAppServiceBase, IChartofControlsAppService
    {
		 private readonly IRepository<ChartofControl, string> _chartofControlRepository;
		 private readonly IChartofControlsExcelExporter _chartofControlsExcelExporter;
		 private readonly IRepository<ControlDetail> _lookup_controlDetailRepository;
		 private readonly IRepository<SubControlDetail> _lookup_subControlDetailRepository;
		 private readonly IRepository<Segmentlevel3> _lookup_segmentlevel3Repository;
        private readonly IRepository<GroupCode,int> _lookup_groupcodeRepository;
        private readonly IRepository<GLOption> _companySetupRepository;


        public ChartofControlsAppService(IRepository<ChartofControl, string> chartofControlRepository, IChartofControlsExcelExporter chartofControlsExcelExporter , IRepository<ControlDetail> lookup_controlDetailRepository, IRepository<SubControlDetail> lookup_subControlDetailRepository, IRepository<Segmentlevel3> lookup_segmentlevel3Repository, IRepository<GroupCode, int> lookup_groupcodeRepository, IRepository<GLOption> companySetupRepository) 
		  {
			_chartofControlRepository = chartofControlRepository;
			_chartofControlsExcelExporter = chartofControlsExcelExporter;
			_lookup_controlDetailRepository = lookup_controlDetailRepository;
		_lookup_subControlDetailRepository = lookup_subControlDetailRepository;
		_lookup_segmentlevel3Repository = lookup_segmentlevel3Repository;
            _lookup_groupcodeRepository = lookup_groupcodeRepository;
            _companySetupRepository = companySetupRepository;
		
		  }

		 public async Task<PagedResultDto<GetChartofControlForViewDto>> GetAll(GetAllChartofControlsInput input)
         {

            var filteredChartofControls = from o in _chartofControlRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId)

                                          join cd in _lookup_controlDetailRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId) on o.ControlDetailId equals cd.Seg1ID into cd1
                                          from s1 in cd1.DefaultIfEmpty()


                                          join scd in _lookup_subControlDetailRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId) on o.SubControlDetailId equals scd.Seg2ID into scd1
                                          from s2 in scd1.DefaultIfEmpty()

                                          join sg3 in _lookup_segmentlevel3Repository.GetAll().Where(x => x.TenantId == AbpSession.TenantId) on o.Segmentlevel3Id equals sg3.Seg3ID into sg3_1
                                          from s3 in sg3_1.DefaultIfEmpty()
                                          select new {
                                              o.Id, o.AccountName, o.SubLedger, o.OptFld, o.SLType, o.Inactive,o.CreationDate, o.OldCode, o.ControlDetailId, o.SubControlDetailId,o.Segmentlevel3Id,o.GroupCode,
                                              SegmentName1 = s1.SegmentName,
                                              SegmentName2 =  s2.SegmentName,
                                              SegmentName3 = s3.SegmentName
                                          };
            filteredChartofControls = filteredChartofControls
                                     .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Id.Contains(input.Filter) || e.AccountName.Contains(input.Filter)
            || e.SegmentName1.Contains(input.Filter) || e.SegmentName2.Contains(input.Filter) || e.SegmentName3.Contains(input.Filter)
            )
            .WhereIf(!string.IsNullOrWhiteSpace(input.AccountIDFilter), e => e.Id.ToLower() == input.AccountIDFilter.ToLower().Trim())
            .WhereIf(!string.IsNullOrWhiteSpace(input.AccountNameFilter), e => e.AccountName.ToLower() == input.AccountNameFilter.ToLower().Trim())
            .WhereIf(input.SubLedgerFilter > -1, e => Convert.ToInt32(e.SubLedger) == input.SubLedgerFilter)
            .WhereIf(input.MinOptFldFilter != null, e => e.OptFld >= input.MinOptFldFilter)
            .WhereIf(input.MaxOptFldFilter != null, e => e.OptFld <= input.MaxOptFldFilter)
            .WhereIf(input.MinSLTypeFilter != null, e => e.SLType >= input.MinSLTypeFilter)
            .WhereIf(input.MaxSLTypeFilter != null, e => e.SLType <= input.MaxSLTypeFilter)
            .WhereIf(input.InactiveFilter > -1, e => Convert.ToInt32(e.Inactive) == input.InactiveFilter)
            .WhereIf(input.MinCreationDateFilter != null, e => e.CreationDate >= input.MinCreationDateFilter)
            .WhereIf(input.MaxCreationDateFilter != null, e => e.CreationDate <= input.MaxCreationDateFilter)


            .WhereIf(!string.IsNullOrWhiteSpace(input.OldCodeFilter), e => e.OldCode.ToLower() == input.OldCodeFilter.ToLower().Trim())
            .WhereIf(!string.IsNullOrWhiteSpace(input.ControlDetailSegmentNameFilter), e => e.SegmentName1 != null && e.SegmentName1.ToLower() == input.ControlDetailSegmentNameFilter.ToLower().Trim())
            .WhereIf(!string.IsNullOrWhiteSpace(input.SubControlDetailSegmentNameFilter), e => e.SegmentName2 != null && e.SegmentName2.ToLower() == input.SubControlDetailSegmentNameFilter.ToLower().Trim())
            .WhereIf(!string.IsNullOrWhiteSpace(input.Segmentlevel3SegmentNameFilter), e => e.SegmentName2 != null && e.SegmentName2.ToLower() == input.Segmentlevel3SegmentNameFilter.ToLower().Trim());

            var pagedAndFilteredChartofControls = filteredChartofControls
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

			var chartofControls = from o in pagedAndFilteredChartofControls
                         join o1 in _lookup_controlDetailRepository.GetAll() on o.ControlDetailId equals o1.Seg1ID into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_subControlDetailRepository.GetAll() on o.SubControlDetailId equals o2.Seg2ID into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         join o3 in _lookup_segmentlevel3Repository.GetAll() on o.Segmentlevel3Id equals o3.Seg3ID into j3
                         from s3 in j3.DefaultIfEmpty()
                         
                         select new GetChartofControlForViewDto() {
							ChartofControl = new ChartofControlDto
							{
                               
                                AccountName = o.AccountName,
                                SubLedger = o.SubLedger,
                                OptFld = o.OptFld,
                                SLType = o.SLType,
                                Inactive = o.Inactive,
                                CreationDate = o.CreationDate,
                                OldCode = o.OldCode,
                                Id = o.Id
							},
                         	ControlDetailSegmentName = s1 == null ? "" : s1.SegmentName.ToString(),
                         	SubControlDetailSegmentName = s2 == null ? "" : s2.SegmentName.ToString(),
                         	Segmentlevel3SegmentName = s3 == null ? "" : s3.SegmentName.ToString()
						};

            var totalCount = await filteredChartofControls.CountAsync();

            return new PagedResultDto<GetChartofControlForViewDto>(
                totalCount,
                await chartofControls.ToListAsync()
            );
         }
		 
		 public async Task<GetChartofControlForViewDto> GetChartofControlForView(string id)
         {
            var chartofControl = await _chartofControlRepository.GetAsync(id);

            var output = new GetChartofControlForViewDto { ChartofControl = ObjectMapper.Map<ChartofControlDto>(chartofControl) };

		    if (output.ChartofControl.ControlDetailId != null)
            {
                var _lookupControlDetail = await _lookup_controlDetailRepository.FirstOrDefaultAsync(x=>x.Seg1ID == (string)output.ChartofControl.ControlDetailId);
                output.ControlDetailSegmentName = _lookupControlDetail.SegmentName.ToString();
            }

		    if (output.ChartofControl.SubControlDetailId != null)
            {
                var _lookupSubControlDetail = await _lookup_subControlDetailRepository.FirstOrDefaultAsync(x=>x.Seg2ID == output.ChartofControl.SubControlDetailId);
                output.SubControlDetailSegmentName = _lookupSubControlDetail.SegmentName.ToString();
            }

		    if (output.ChartofControl.Segmentlevel3Id != null)
            {
                var _lookupSegmentlevel3 = await _lookup_segmentlevel3Repository.FirstOrDefaultAsync(x => x.Seg3ID == (string)output.ChartofControl.Segmentlevel3Id);
                output.Segmentlevel3SegmentName = _lookupSegmentlevel3.SegmentName.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.SetupForms_ChartofControls_Edit)]
		 public async Task<GetChartofControlForEditOutput> GetChartofControlForEdit(EntityDto<string> input)
         {
            var chartofControl = await _chartofControlRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetChartofControlForEditOutput {ChartofControl = ObjectMapper.Map<CreateOrEditChartofControlDto>(chartofControl)};

		    if (output.ChartofControl.ControlDetailId != null)
            {
                var _lookupControlDetail = await _lookup_controlDetailRepository.FirstOrDefaultAsync(x => x.Seg1ID == (string)output.ChartofControl.ControlDetailId);
                output.ControlDetailSegmentName = _lookupControlDetail.SegmentName.ToString();
            }

		    if (output.ChartofControl.SubControlDetailId != null)
            {
                var _lookupSubControlDetail = await _lookup_subControlDetailRepository.FirstOrDefaultAsync(x => x.Seg2ID == (string)output.ChartofControl.SubControlDetailId);
                output.SubControlDetailSegmentName = _lookupSubControlDetail.SegmentName.ToString();
            }

		    if (output.ChartofControl.Segmentlevel3Id != null)
            {
                var _lookupSegmentlevel3 = await _lookup_segmentlevel3Repository.FirstOrDefaultAsync(x => x.Seg3ID == (string)output.ChartofControl.Segmentlevel3Id);
                output.Segmentlevel3SegmentName = _lookupSegmentlevel3.SegmentName.ToString();
            }

            string[] contID = output.ChartofControl.Id.Split('-');
            output.ChartofControl.Id = contID[3];
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditChartofControlDto input)
         {
            if(input.Flag!= true){
                var chartofControl = await _chartofControlRepository.FirstOrDefaultAsync(input.Segmentlevel3Id + '-' + input.Id);

                if (chartofControl != null)
                {
                    throw new UserFriendlyException("Ooppps! There is a problem!", "Account ID " + input.Segmentlevel3Id + '-'+ input.Id + " already taken....");
                }
                await Create(input);
			}
			else{
				await Update(input);
			}
         }
        public async Task<ListResultDto<SegmentCodeDtoView>> GetListAccountCode(string ID)
        {
            var query = from o in  _lookup_groupcodeRepository.GetAll()
                        join s1 in _lookup_controlDetailRepository.GetAll() on o.GRPCTCODE equals s1.Family
                        where s1.Seg1ID == ID && s1.TenantId == (int?)AbpSession.TenantId
            select new {
                            Id = s1.Family, o.GRPDESC,
                GRPCODE = o.GRPCODE
            };
            var groupCategoryList = await query
                .ToListAsync();

            var userTableDto = new List<SegmentCodeDtoView>();
            foreach (var groupCategory in groupCategoryList)
            {
                userTableDto.Add(new SegmentCodeDtoView
                {

                    GroupId = Convert.ToInt32(groupCategory.Id),
                    Groupname = groupCategory.GRPDESC.ToString(),
                    GroupCode = groupCategory.GRPCODE
                });
            }

            return new ListResultDto<SegmentCodeDtoView>(
                userTableDto
            );
        }

        [AbpAuthorize(AppPermissions.SetupForms_ChartofControls_Create)]
		 protected virtual async Task Create(CreateOrEditChartofControlDto input)
         {
            var chartofControl = ObjectMapper.Map<ChartofControl>(input);

			
			if (AbpSession.TenantId != null)
			{
				chartofControl.TenantId =  Convert.ToInt32( AbpSession.TenantId);
			}

            var usersss = GetCurrentUserAsync();
            input.AuditUser = usersss.Result.UserName;
            input.CreationDate = DateTime.Now;
            input.AuditTime = DateTime.Now;

            chartofControl.Id = input.Segmentlevel3Id + '-' + input.Id;  //GetMaxAcccountID(input.Segmentlevel3Id);
           // chartofControl.GroupCode = Convert.ToInt32(input.GroupCode);
            await _chartofControlRepository.InsertAsync(chartofControl);
         }

		 [AbpAuthorize(AppPermissions.SetupForms_ChartofControls_Edit)]
		 protected virtual async Task Update(CreateOrEditChartofControlDto input)
         {
            input.Id = input.Segmentlevel3Id + '-' + input.Id;
            //input.TenantId = (int)AbpSession.TenantId;
            var chartofControl = await _chartofControlRepository.FirstOrDefaultAsync(x=>x.Id == input.Id && x.TenantId == (Int32)AbpSession.TenantId);
            //input.TenantId = (int)AbpSession.TenantId;
             ObjectMapper.Map(input, chartofControl);
         }

		 [AbpAuthorize(AppPermissions.SetupForms_ChartofControls_Delete)]
         public async Task Delete(EntityDto<string> input)
         {
            await _chartofControlRepository.DeleteAsync(x => x.Id == input.Id && x.TenantId == (Int32)AbpSession.TenantId);
         } 

		public async Task<FileDto> GetChartofControlsToExcel(GetAllChartofControlsForExcelInput input)
         {

            var filteredChartofControls = from o in _chartofControlRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId)

                                          join cd in _lookup_controlDetailRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId) on o.ControlDetailId equals cd.Seg1ID into cd1
                                          from s1 in cd1.DefaultIfEmpty()


                                          join scd in _lookup_subControlDetailRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId) on o.SubControlDetailId equals scd.Seg2ID into scd1
                                          from s2 in scd1.DefaultIfEmpty()

                                          join sg3 in _lookup_segmentlevel3Repository.GetAll().Where(x => x.TenantId == AbpSession.TenantId) on o.Segmentlevel3Id equals sg3.Seg3ID into sg3_1
                                          from s3 in sg3_1.DefaultIfEmpty()
                                          select new
                                          {
                                              o.Id,
                                              o.AccountName,
                                              o.SubLedger,
                                              o.OptFld,
                                              o.SLType,
                                              o.Inactive,
                                              o.CreationDate,
                                              o.OldCode,
                                              o.ControlDetailId,
                                              o.SubControlDetailId,
                                              o.Segmentlevel3Id,
                                              o.GroupCode,
                                              SegmentName1 = s1.SegmentName,
                                              SegmentName2 = s2.SegmentName,
                                              SegmentName3 = s3.SegmentName
                                          };

            filteredChartofControls = filteredChartofControls
.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Id.Contains(input.Filter) || e.AccountName.Contains(input.Filter)
            || e.SegmentName1.Contains(input.Filter) || e.SegmentName2.Contains(input.Filter) || e.SegmentName3.Contains(input.Filter)
            )
            .WhereIf(!string.IsNullOrWhiteSpace(input.IDFilter), e => e.Id.ToLower() == input.IDFilter.ToLower().Trim())
            .WhereIf(!string.IsNullOrWhiteSpace(input.AccountNameFilter), e => e.AccountName.ToLower() == input.AccountNameFilter.ToLower().Trim())
            .WhereIf(input.SubLedgerFilter > -1, e => Convert.ToInt32(e.SubLedger) == input.SubLedgerFilter)
            .WhereIf(input.MinOptFldFilter != null, e => e.OptFld >= input.MinOptFldFilter)
            .WhereIf(input.MaxOptFldFilter != null, e => e.OptFld <= input.MaxOptFldFilter)
            .WhereIf(input.MinSLTypeFilter != null, e => e.SLType >= input.MinSLTypeFilter)
            .WhereIf(input.MaxSLTypeFilter != null, e => e.SLType <= input.MaxSLTypeFilter)
            .WhereIf(input.InactiveFilter > -1, e => Convert.ToInt32(e.Inactive) == input.InactiveFilter)
            .WhereIf(input.MinCreationDateFilter != null, e => e.CreationDate >= input.MinCreationDateFilter)
            .WhereIf(input.MaxCreationDateFilter != null, e => e.CreationDate <= input.MaxCreationDateFilter)


            .WhereIf(!string.IsNullOrWhiteSpace(input.OldCodeFilter), e => e.OldCode.ToLower() == input.OldCodeFilter.ToLower().Trim())
            .WhereIf(!string.IsNullOrWhiteSpace(input.ControlDetailSegmentNameFilter), e => e.SegmentName1 != null && e.SegmentName1.ToLower() == input.ControlDetailSegmentNameFilter.ToLower().Trim())
            .WhereIf(!string.IsNullOrWhiteSpace(input.SubControlDetailSegmentNameFilter), e => e.SegmentName2 != null && e.SegmentName2.ToLower() == input.SubControlDetailSegmentNameFilter.ToLower().Trim())
            .WhereIf(!string.IsNullOrWhiteSpace(input.Segmentlevel3SegmentNameFilter), e => e.SegmentName2 != null && e.SegmentName2.ToLower() == input.Segmentlevel3SegmentNameFilter.ToLower().Trim());

            var query = (from o in filteredChartofControls
                         join o1 in _lookup_controlDetailRepository.GetAll() on o.ControlDetailId equals o1.Seg1ID into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_subControlDetailRepository.GetAll() on o.SubControlDetailId equals o2.Seg2ID into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         join o3 in _lookup_segmentlevel3Repository.GetAll() on o.Segmentlevel3Id equals o3.Seg3ID into j3
                         from s3 in j3.DefaultIfEmpty()
                         
                         select new GetChartofControlForViewDto() { 
							ChartofControl = new ChartofControlDto
							{
                                GroupCode = o.GroupCode,
                                ControlDetailId= o.ControlDetailId,
                                SubControlDetailId = o.SubControlDetailId,
                                Segmentlevel3Id = o.Segmentlevel3Id,
                                AccountName = o.AccountName,
                                SubLedger = o.SubLedger,
                                OptFld = o.OptFld,
                                SLType = o.SLType,
                                Inactive = o.Inactive,
                                CreationDate = o.CreationDate,
                                OldCode = o.OldCode,
                                Id = o.Id
							},
                         	ControlDetailSegmentName = s1 == null ? "" : s1.SegmentName.ToString(),
                         	SubControlDetailSegmentName = s2 == null ? "" : s2.SegmentName.ToString(),
                         	Segmentlevel3SegmentName = s3 == null ? "" : s3.SegmentName.ToString()
						 });


            var chartofControlListDtos = await query.ToListAsync();

            return _chartofControlsExcelExporter.ExportToFile(chartofControlListDtos);
         }

		[AbpAuthorize(AppPermissions.SetupForms_ChartofControls)]
         public async Task<PagedResultDto<ChartofControlControlDetailLookupTableDto>> GetAllControlDetailForLookupTable(GetAllForLookupTableInput input)
         {
            var query = _lookup_controlDetailRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => false || e.Seg1ID.Trim().Contains(input.Filter) ||  e.SegmentName.ToString().ToLower().Contains(input.Filter)
                ).Where(o => o.TenantId == AbpSession.TenantId);

            var totalCount = await query.CountAsync();

            var controlDetailList = await query
                .OrderBy(input.Sorting ?? "Seg1ID desc")
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ChartofControlControlDetailLookupTableDto>();
			foreach(var controlDetail in controlDetailList){
				lookupTableDtoList.Add(new ChartofControlControlDetailLookupTableDto
				{
					Id = controlDetail.Seg1ID,
					DisplayName = controlDetail.SegmentName?.ToString()
				});
			}

            return new PagedResultDto<ChartofControlControlDetailLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.SetupForms_ChartofControls)]
         public async Task<PagedResultDto<ChartofControlSubControlDetailLookupTableDto>> GetAllSubControlDetailForLookupTable(GetAllForLookupTableInput input, string Seg1ID)
         {

            var query = _lookup_subControlDetailRepository.GetAll().Where(c => EF.Functions.Like(c.Seg2ID, $"{Seg1ID}%") && c.TenantId == AbpSession.TenantId).WhereIf(
                  !string.IsNullOrWhiteSpace(input.Filter),
                 e => false || e.Seg2ID.Trim().Contains(input.Filter) || e.SegmentName.ToString().ToLower().Contains(input.Filter));
         

            var totalCount = await query.CountAsync();

            var subControlDetailList = await query
                .OrderBy(input.Sorting ?? "Seg2ID desc")
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ChartofControlSubControlDetailLookupTableDto>();
			foreach(var subControlDetail in subControlDetailList){
				lookupTableDtoList.Add(new ChartofControlSubControlDetailLookupTableDto
				{
					Id = subControlDetail.Seg2ID,
					DisplayName = subControlDetail.SegmentName?.ToString()
				});
			}

            return new PagedResultDto<ChartofControlSubControlDetailLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.SetupForms_ChartofControls)]
         public async Task<PagedResultDto<ChartofControlSegmentlevel3LookupTableDto>> GetAllSegmentlevel3ForLookupTable(GetAllForLookupTableInput input, string Seg2ID)
         {

            var query = _lookup_segmentlevel3Repository.GetAll().Where(c => EF.Functions.Like(c.Seg3ID, $"{Seg2ID}%") && c.TenantId == AbpSession.TenantId).WhereIf(
                  !string.IsNullOrWhiteSpace(input.Filter),
                 e => false || e.Seg3ID.Trim().Contains(input.Filter.Trim()) || e.SegmentName.ToLower().Contains(input.Filter.Trim()));
            //var query = _lookup_segmentlevel3Repository.GetAll().WhereIf(
            //       !string.IsNullOrWhiteSpace(input.Filter),
            //      e=> e.SegmentName.ToString().Contains(input.Filter)
            //   );

            var totalCount = await query.CountAsync();

            var segmentlevel3List = await query
                .OrderBy(input.Sorting ?? "Seg3ID desc")
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ChartofControlSegmentlevel3LookupTableDto>();
			foreach(var segmentlevel3 in segmentlevel3List){
				lookupTableDtoList.Add(new ChartofControlSegmentlevel3LookupTableDto
				{
					Id = segmentlevel3.Seg3ID,
					DisplayName = segmentlevel3.SegmentName?.ToString()
				});
			}

            return new PagedResultDto<ChartofControlSegmentlevel3LookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

        // make Chart of Account Id

        public string GetMaxAcccountID(string id)
        {
            string finalSting = "";
            if (id!="null")
            {
                var filteredChartofAccount = _chartofControlRepository.GetAll();
                //string x = "" +  id + "%";
                string[] xstring;
                string xformat = "";
                string nString = "";
            
                var getMaxID = filteredChartofAccount.Where(c => EF.Functions.Like(c.Id, $"{id}%") && c.TenantId == AbpSession.TenantId).OrderByDescending(o => o.Id).Select(x => x.Id).FirstOrDefault();

                if (getMaxID == null)
                {

                    xformat = string.Format("{0:0000}", 1);
                    finalSting = xformat; //id + "-" + xformat;
                }
                else
                {
                    xstring = getMaxID.Split('-');
                    nString = xstring[3];
                    xformat = string.Format("{0:0000}", Convert.ToInt32(nString) + 1);
                    finalSting = xformat; //id + "-" + xformat;
                }
                return finalSting; 
            }
            return finalSting;

        }


        public async Task<ListResultDto<ComboboxItemDto>> GetAccountsForDropdown(string AccountID)
        {
            var filteredChartofControls = _chartofControlRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).
                WhereIf(!string.IsNullOrWhiteSpace(AccountID),e => e.Id == AccountID);

            var pagedAndFilteredChartofControls = await filteredChartofControls
                .OrderBy("id desc").ToListAsync();

            var lookupTableDtoList = new List<ComboboxItemDto>();
            foreach (var account in pagedAndFilteredChartofControls)
            {
                lookupTableDtoList.Add(new ComboboxItemDto
                {
                    Value = account.Id,
                    DisplayText = account.AccountName
                });
            }
           
            return new ListResultDto<ComboboxItemDto>(
                lookupTableDtoList
                );

        }

        public async Task<PagedResultDto<GetGLOptionForViewDto>> GetSegmentName()
        {

            var filteredCompanySetups = _companySetupRepository.GetAll().Where(x=>x.TenantId == AbpSession.TenantId);

            //var pagedAndFilteredCompanySetups = filteredCompanySetups
            //    .OrderBy(input.Sorting ?? "id asc")
            //    .PageBy(input);

            var companySetups = from o in filteredCompanySetups
                                select new GetGLOptionForViewDto()
                                {
                                    GLOption = new GLOptionDto
                                    {
                                        Seg1Name = o.Seg1Name,
                                        Seg2Name = o.Seg2Name,
                                        Seg3Name = o.Seg3Name,
                                        Id = o.Id
                                    }
                                };

            var totalCount = await filteredCompanySetups.CountAsync();

            return new PagedResultDto<GetGLOptionForViewDto>(
                totalCount,
                await companySetups.ToListAsync()
            );
        }

        public string GetName(string Id)
        {
            var AccName = _chartofControlRepository.GetAll().Where(x => x.Id == Id && x.TenantId == AbpSession.TenantId).Select(x => x.AccountName).FirstOrDefault();
            return AccName;
        }
    }
}