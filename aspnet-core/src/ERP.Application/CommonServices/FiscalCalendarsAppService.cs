

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.CommonServices.Exporting;
using ERP.CommonServices.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ERP.CommonServices
{
	[AbpAuthorize(AppPermissions.SetupForms_FiscalCalendars)]
    public class FiscalCalendarsAppService : ERPAppServiceBase, IFiscalCalendarsAppService
    {
		 private readonly IRepository<FiscalCalendar> _fiscalCalendarRepository;
		 private readonly IFiscalCalendarsExcelExporter _fiscalCalendarsExcelExporter;
		 

		  public FiscalCalendarsAppService(IRepository<FiscalCalendar> fiscalCalendarRepository, IFiscalCalendarsExcelExporter fiscalCalendarsExcelExporter ) 
		  {
			_fiscalCalendarRepository = fiscalCalendarRepository;
			_fiscalCalendarsExcelExporter = fiscalCalendarsExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetFiscalCalendarForViewDto>> GetAll(GetAllFiscalCalendarsInput input)
         {
			
			var filteredFiscalCalendars = _fiscalCalendarRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.AUDTUSER.Contains(input.Filter))
						.WhereIf(input.MinAUDTDATEFilter != null, e => e.AUDTDATE >= input.MinAUDTDATEFilter)
						.WhereIf(input.MaxAUDTDATEFilter != null, e => e.AUDTDATE <= input.MaxAUDTDATEFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.AUDTUSERFilter),  e => e.AUDTUSER.ToLower() == input.AUDTUSERFilter.ToLower().Trim())
						.WhereIf(input.MinPERIODSFilter != null, e => e.PERIODS >= input.MinPERIODSFilter)
						.WhereIf(input.MaxPERIODSFilter != null, e => e.PERIODS <= input.MaxPERIODSFilter)
						.WhereIf(input.MinQTR4PERDFilter != null, e => e.QTR4PERD >= input.MinQTR4PERDFilter)
						.WhereIf(input.MaxQTR4PERDFilter != null, e => e.QTR4PERD <= input.MaxQTR4PERDFilter)
						.WhereIf(input.MinACTIVEFilter != null, e => e.ACTIVE >= input.MinACTIVEFilter)
						.WhereIf(input.MaxACTIVEFilter != null, e => e.ACTIVE <= input.MaxACTIVEFilter)
						.WhereIf(input.MinBGNDATE1Filter != null, e => e.BGNDATE1 >= input.MinBGNDATE1Filter)
						.WhereIf(input.MaxBGNDATE1Filter != null, e => e.BGNDATE1 <= input.MaxBGNDATE1Filter)
						.WhereIf(input.MinBGNDATE2Filter != null, e => e.BGNDATE2 >= input.MinBGNDATE2Filter)
						.WhereIf(input.MaxBGNDATE2Filter != null, e => e.BGNDATE2 <= input.MaxBGNDATE2Filter)
						.WhereIf(input.MinBGNDATE3Filter != null, e => e.BGNDATE3 >= input.MinBGNDATE3Filter)
						.WhereIf(input.MaxBGNDATE3Filter != null, e => e.BGNDATE3 <= input.MaxBGNDATE3Filter)
						.WhereIf(input.MinBGNDATE4Filter != null, e => e.BGNDATE4 >= input.MinBGNDATE4Filter)
						.WhereIf(input.MaxBGNDATE4Filter != null, e => e.BGNDATE4 <= input.MaxBGNDATE4Filter)
						.WhereIf(input.MinBGNDATE5Filter != null, e => e.BGNDATE5 >= input.MinBGNDATE5Filter)
						.WhereIf(input.MaxBGNDATE5Filter != null, e => e.BGNDATE5 <= input.MaxBGNDATE5Filter)
						.WhereIf(input.MinBGNDATE6Filter != null, e => e.BGNDATE6 >= input.MinBGNDATE6Filter)
						.WhereIf(input.MaxBGNDATE6Filter != null, e => e.BGNDATE6 <= input.MaxBGNDATE6Filter)
						.WhereIf(input.MinBGNDATE7Filter != null, e => e.BGNDATE7 >= input.MinBGNDATE7Filter)
						.WhereIf(input.MaxBGNDATE7Filter != null, e => e.BGNDATE7 <= input.MaxBGNDATE7Filter)
						.WhereIf(input.MinBGNDATE8Filter != null, e => e.BGNDATE8 >= input.MinBGNDATE8Filter)
						.WhereIf(input.MaxBGNDATE8Filter != null, e => e.BGNDATE8 <= input.MaxBGNDATE8Filter)
						.WhereIf(input.MinBGNDATE9Filter != null, e => e.BGNDATE9 >= input.MinBGNDATE9Filter)
						.WhereIf(input.MaxBGNDATE9Filter != null, e => e.BGNDATE9 <= input.MaxBGNDATE9Filter)
						.WhereIf(input.MinBGNDATE10Filter != null, e => e.BGNDATE10 >= input.MinBGNDATE10Filter)
						.WhereIf(input.MaxBGNDATE10Filter != null, e => e.BGNDATE10 <= input.MaxBGNDATE10Filter)
						.WhereIf(input.MinBGNDATE11Filter != null, e => e.BGNDATE11 >= input.MinBGNDATE11Filter)
						.WhereIf(input.MaxBGNDATE11Filter != null, e => e.BGNDATE11 <= input.MaxBGNDATE11Filter)
						.WhereIf(input.MinBGNDATE12Filter != null, e => e.BGNDATE12 >= input.MinBGNDATE12Filter)
						.WhereIf(input.MaxBGNDATE12Filter != null, e => e.BGNDATE12 <= input.MaxBGNDATE12Filter)
						.WhereIf(input.MinBGNDATE13Filter != null, e => e.BGNDATE13 >= input.MinBGNDATE13Filter)
						.WhereIf(input.MaxBGNDATE13Filter != null, e => e.BGNDATE13 <= input.MaxBGNDATE13Filter)
						.WhereIf(input.MinENDDATE1Filter != null, e => e.ENDDATE1 >= input.MinENDDATE1Filter)
						.WhereIf(input.MaxENDDATE1Filter != null, e => e.ENDDATE1 <= input.MaxENDDATE1Filter)
						.WhereIf(input.MinENDDATE2Filter != null, e => e.ENDDATE2 >= input.MinENDDATE2Filter)
						.WhereIf(input.MaxENDDATE2Filter != null, e => e.ENDDATE2 <= input.MaxENDDATE2Filter)
						.WhereIf(input.MinENDDATE3Filter != null, e => e.ENDDATE3 >= input.MinENDDATE3Filter)
						.WhereIf(input.MaxENDDATE3Filter != null, e => e.ENDDATE3 <= input.MaxENDDATE3Filter)
						.WhereIf(input.MinENDDATE4Filter != null, e => e.ENDDATE4 >= input.MinENDDATE4Filter)
						.WhereIf(input.MaxENDDATE4Filter != null, e => e.ENDDATE4 <= input.MaxENDDATE4Filter)
						.WhereIf(input.MinENDDATE5Filter != null, e => e.ENDDATE5 >= input.MinENDDATE5Filter)
						.WhereIf(input.MaxENDDATE5Filter != null, e => e.ENDDATE5 <= input.MaxENDDATE5Filter)
						.WhereIf(input.MinENDDATE6Filter != null, e => e.ENDDATE6 >= input.MinENDDATE6Filter)
						.WhereIf(input.MaxENDDATE6Filter != null, e => e.ENDDATE6 <= input.MaxENDDATE6Filter)
						.WhereIf(input.MinENDDATE7Filter != null, e => e.ENDDATE7 >= input.MinENDDATE7Filter)
						.WhereIf(input.MaxENDDATE7Filter != null, e => e.ENDDATE7 <= input.MaxENDDATE7Filter)
						.WhereIf(input.MinENDDATE8Filter != null, e => e.ENDDATE8 >= input.MinENDDATE8Filter)
						.WhereIf(input.MaxENDDATE8Filter != null, e => e.ENDDATE8 <= input.MaxENDDATE8Filter)
						.WhereIf(input.MinENDDATE9Filter != null, e => e.ENDDATE9 >= input.MinENDDATE9Filter)
						.WhereIf(input.MaxENDDATE9Filter != null, e => e.ENDDATE9 <= input.MaxENDDATE9Filter)
						.WhereIf(input.MinENDDATE10Filter != null, e => e.ENDDATE10 >= input.MinENDDATE10Filter)
						.WhereIf(input.MaxENDDATE10Filter != null, e => e.ENDDATE10 <= input.MaxENDDATE10Filter)
						.WhereIf(input.MinENDDATE11Filter != null, e => e.ENDDATE11 >= input.MinENDDATE11Filter)
						.WhereIf(input.MaxENDDATE11Filter != null, e => e.ENDDATE11 <= input.MaxENDDATE11Filter)
						.WhereIf(input.MinENDDATE12Filter != null, e => e.ENDDATE12 >= input.MinENDDATE12Filter)
						.WhereIf(input.MaxENDDATE12Filter != null, e => e.ENDDATE12 <= input.MaxENDDATE12Filter)
						.WhereIf(input.MinENDDATE13Filter != null, e => e.ENDDATE13 >= input.MinENDDATE13Filter)
						.WhereIf(input.MaxENDDATE13Filter != null, e => e.ENDDATE13 <= input.MaxENDDATE13Filter);

			var pagedAndFilteredFiscalCalendars = filteredFiscalCalendars
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var fiscalCalendars = from o in pagedAndFilteredFiscalCalendars
                         select new GetFiscalCalendarForViewDto() {
							FiscalCalendar = new FiscalCalendarDto
							{
                                AUDTDATE = o.AUDTDATE,
                                AUDTUSER = o.AUDTUSER,
                                PERIODS = o.PERIODS,
                                QTR4PERD = o.QTR4PERD,
                                ACTIVE = o.ACTIVE,
                                BGNDATE1 = o.BGNDATE1,
                                BGNDATE2 = o.BGNDATE2,
                                BGNDATE3 = o.BGNDATE3,
                                BGNDATE4 = o.BGNDATE4,
                                BGNDATE5 = o.BGNDATE5,
                                BGNDATE6 = o.BGNDATE6,
                                BGNDATE7 = o.BGNDATE7,
                                BGNDATE8 = o.BGNDATE8,
                                BGNDATE9 = o.BGNDATE9,
                                BGNDATE10 = o.BGNDATE10,
                                BGNDATE11 = o.BGNDATE11,
                                BGNDATE12 = o.BGNDATE12,
                                BGNDATE13 = o.BGNDATE13,
                                ENDDATE1 = o.ENDDATE1,
                                ENDDATE2 = o.ENDDATE2,
                                ENDDATE3 = o.ENDDATE3,
                                ENDDATE4 = o.ENDDATE4,
                                ENDDATE5 = o.ENDDATE5,
                                ENDDATE6 = o.ENDDATE6,
                                ENDDATE7 = o.ENDDATE7,
                                ENDDATE8 = o.ENDDATE8,
                                ENDDATE9 = o.ENDDATE9,
                                ENDDATE10 = o.ENDDATE10,
                                ENDDATE11 = o.ENDDATE11,
                                ENDDATE12 = o.ENDDATE12,
                                ENDDATE13 = o.ENDDATE13,
                                Id = o.Id
							}
						};

            var totalCount = await filteredFiscalCalendars.CountAsync();

            return new PagedResultDto<GetFiscalCalendarForViewDto>(
                totalCount,
                await fiscalCalendars.ToListAsync()
            );
         }
		 
		 public async Task<GetFiscalCalendarForViewDto> GetFiscalCalendarForView(int id)
         {
            var fiscalCalendar = await _fiscalCalendarRepository.GetAsync(id);

            var output = new GetFiscalCalendarForViewDto { FiscalCalendar = ObjectMapper.Map<FiscalCalendarDto>(fiscalCalendar) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.SetupForms_FiscalCalendars_Edit)]
		 public async Task<GetFiscalCalendarForEditOutput> GetFiscalCalendarForEdit(EntityDto input)
         {
            var fiscalCalendar = await _fiscalCalendarRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetFiscalCalendarForEditOutput {FiscalCalendar = ObjectMapper.Map<CreateOrEditFiscalCalendarDto>(fiscalCalendar)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditFiscalCalendarDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.SetupForms_FiscalCalendars_Create)]
		 private async Task Create(CreateOrEditFiscalCalendarDto input)
         {
            var fiscalCalendar = ObjectMapper.Map<FiscalCalendar>(input);

			
			if (AbpSession.TenantId != null)
			{
				fiscalCalendar.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _fiscalCalendarRepository.InsertAsync(fiscalCalendar);
         }

		 [AbpAuthorize(AppPermissions.SetupForms_FiscalCalendars_Edit)]
		 private async Task Update(CreateOrEditFiscalCalendarDto input)
         {
            var fiscalCalendar = await _fiscalCalendarRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, fiscalCalendar);
         }

		 [AbpAuthorize(AppPermissions.SetupForms_FiscalCalendars_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _fiscalCalendarRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetFiscalCalendarsToExcel(GetAllFiscalCalendarsForExcelInput input)
         {
			
			var filteredFiscalCalendars = _fiscalCalendarRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.AUDTUSER.Contains(input.Filter))
						.WhereIf(input.MinAUDTDATEFilter != null, e => e.AUDTDATE >= input.MinAUDTDATEFilter)
						.WhereIf(input.MaxAUDTDATEFilter != null, e => e.AUDTDATE <= input.MaxAUDTDATEFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.AUDTUSERFilter),  e => e.AUDTUSER.ToLower() == input.AUDTUSERFilter.ToLower().Trim())
						.WhereIf(input.MinPERIODSFilter != null, e => e.PERIODS >= input.MinPERIODSFilter)
						.WhereIf(input.MaxPERIODSFilter != null, e => e.PERIODS <= input.MaxPERIODSFilter)
						.WhereIf(input.MinQTR4PERDFilter != null, e => e.QTR4PERD >= input.MinQTR4PERDFilter)
						.WhereIf(input.MaxQTR4PERDFilter != null, e => e.QTR4PERD <= input.MaxQTR4PERDFilter)
						.WhereIf(input.MinACTIVEFilter != null, e => e.ACTIVE >= input.MinACTIVEFilter)
						.WhereIf(input.MaxACTIVEFilter != null, e => e.ACTIVE <= input.MaxACTIVEFilter)
						.WhereIf(input.MinBGNDATE1Filter != null, e => e.BGNDATE1 >= input.MinBGNDATE1Filter)
						.WhereIf(input.MaxBGNDATE1Filter != null, e => e.BGNDATE1 <= input.MaxBGNDATE1Filter)
						.WhereIf(input.MinBGNDATE2Filter != null, e => e.BGNDATE2 >= input.MinBGNDATE2Filter)
						.WhereIf(input.MaxBGNDATE2Filter != null, e => e.BGNDATE2 <= input.MaxBGNDATE2Filter)
						.WhereIf(input.MinBGNDATE3Filter != null, e => e.BGNDATE3 >= input.MinBGNDATE3Filter)
						.WhereIf(input.MaxBGNDATE3Filter != null, e => e.BGNDATE3 <= input.MaxBGNDATE3Filter)
						.WhereIf(input.MinBGNDATE4Filter != null, e => e.BGNDATE4 >= input.MinBGNDATE4Filter)
						.WhereIf(input.MaxBGNDATE4Filter != null, e => e.BGNDATE4 <= input.MaxBGNDATE4Filter)
						.WhereIf(input.MinBGNDATE5Filter != null, e => e.BGNDATE5 >= input.MinBGNDATE5Filter)
						.WhereIf(input.MaxBGNDATE5Filter != null, e => e.BGNDATE5 <= input.MaxBGNDATE5Filter)
						.WhereIf(input.MinBGNDATE6Filter != null, e => e.BGNDATE6 >= input.MinBGNDATE6Filter)
						.WhereIf(input.MaxBGNDATE6Filter != null, e => e.BGNDATE6 <= input.MaxBGNDATE6Filter)
						.WhereIf(input.MinBGNDATE7Filter != null, e => e.BGNDATE7 >= input.MinBGNDATE7Filter)
						.WhereIf(input.MaxBGNDATE7Filter != null, e => e.BGNDATE7 <= input.MaxBGNDATE7Filter)
						.WhereIf(input.MinBGNDATE8Filter != null, e => e.BGNDATE8 >= input.MinBGNDATE8Filter)
						.WhereIf(input.MaxBGNDATE8Filter != null, e => e.BGNDATE8 <= input.MaxBGNDATE8Filter)
						.WhereIf(input.MinBGNDATE9Filter != null, e => e.BGNDATE9 >= input.MinBGNDATE9Filter)
						.WhereIf(input.MaxBGNDATE9Filter != null, e => e.BGNDATE9 <= input.MaxBGNDATE9Filter)
						.WhereIf(input.MinBGNDATE10Filter != null, e => e.BGNDATE10 >= input.MinBGNDATE10Filter)
						.WhereIf(input.MaxBGNDATE10Filter != null, e => e.BGNDATE10 <= input.MaxBGNDATE10Filter)
						.WhereIf(input.MinBGNDATE11Filter != null, e => e.BGNDATE11 >= input.MinBGNDATE11Filter)
						.WhereIf(input.MaxBGNDATE11Filter != null, e => e.BGNDATE11 <= input.MaxBGNDATE11Filter)
						.WhereIf(input.MinBGNDATE12Filter != null, e => e.BGNDATE12 >= input.MinBGNDATE12Filter)
						.WhereIf(input.MaxBGNDATE12Filter != null, e => e.BGNDATE12 <= input.MaxBGNDATE12Filter)
						.WhereIf(input.MinBGNDATE13Filter != null, e => e.BGNDATE13 >= input.MinBGNDATE13Filter)
						.WhereIf(input.MaxBGNDATE13Filter != null, e => e.BGNDATE13 <= input.MaxBGNDATE13Filter)
						.WhereIf(input.MinENDDATE1Filter != null, e => e.ENDDATE1 >= input.MinENDDATE1Filter)
						.WhereIf(input.MaxENDDATE1Filter != null, e => e.ENDDATE1 <= input.MaxENDDATE1Filter)
						.WhereIf(input.MinENDDATE2Filter != null, e => e.ENDDATE2 >= input.MinENDDATE2Filter)
						.WhereIf(input.MaxENDDATE2Filter != null, e => e.ENDDATE2 <= input.MaxENDDATE2Filter)
						.WhereIf(input.MinENDDATE3Filter != null, e => e.ENDDATE3 >= input.MinENDDATE3Filter)
						.WhereIf(input.MaxENDDATE3Filter != null, e => e.ENDDATE3 <= input.MaxENDDATE3Filter)
						.WhereIf(input.MinENDDATE4Filter != null, e => e.ENDDATE4 >= input.MinENDDATE4Filter)
						.WhereIf(input.MaxENDDATE4Filter != null, e => e.ENDDATE4 <= input.MaxENDDATE4Filter)
						.WhereIf(input.MinENDDATE5Filter != null, e => e.ENDDATE5 >= input.MinENDDATE5Filter)
						.WhereIf(input.MaxENDDATE5Filter != null, e => e.ENDDATE5 <= input.MaxENDDATE5Filter)
						.WhereIf(input.MinENDDATE6Filter != null, e => e.ENDDATE6 >= input.MinENDDATE6Filter)
						.WhereIf(input.MaxENDDATE6Filter != null, e => e.ENDDATE6 <= input.MaxENDDATE6Filter)
						.WhereIf(input.MinENDDATE7Filter != null, e => e.ENDDATE7 >= input.MinENDDATE7Filter)
						.WhereIf(input.MaxENDDATE7Filter != null, e => e.ENDDATE7 <= input.MaxENDDATE7Filter)
						.WhereIf(input.MinENDDATE8Filter != null, e => e.ENDDATE8 >= input.MinENDDATE8Filter)
						.WhereIf(input.MaxENDDATE8Filter != null, e => e.ENDDATE8 <= input.MaxENDDATE8Filter)
						.WhereIf(input.MinENDDATE9Filter != null, e => e.ENDDATE9 >= input.MinENDDATE9Filter)
						.WhereIf(input.MaxENDDATE9Filter != null, e => e.ENDDATE9 <= input.MaxENDDATE9Filter)
						.WhereIf(input.MinENDDATE10Filter != null, e => e.ENDDATE10 >= input.MinENDDATE10Filter)
						.WhereIf(input.MaxENDDATE10Filter != null, e => e.ENDDATE10 <= input.MaxENDDATE10Filter)
						.WhereIf(input.MinENDDATE11Filter != null, e => e.ENDDATE11 >= input.MinENDDATE11Filter)
						.WhereIf(input.MaxENDDATE11Filter != null, e => e.ENDDATE11 <= input.MaxENDDATE11Filter)
						.WhereIf(input.MinENDDATE12Filter != null, e => e.ENDDATE12 >= input.MinENDDATE12Filter)
						.WhereIf(input.MaxENDDATE12Filter != null, e => e.ENDDATE12 <= input.MaxENDDATE12Filter)
						.WhereIf(input.MinENDDATE13Filter != null, e => e.ENDDATE13 >= input.MinENDDATE13Filter)
						.WhereIf(input.MaxENDDATE13Filter != null, e => e.ENDDATE13 <= input.MaxENDDATE13Filter);

			var query = (from o in filteredFiscalCalendars
                         select new GetFiscalCalendarForViewDto() { 
							FiscalCalendar = new FiscalCalendarDto
							{
                                AUDTDATE = o.AUDTDATE,
                                AUDTUSER = o.AUDTUSER,
                                PERIODS = o.PERIODS,
                                QTR4PERD = o.QTR4PERD,
                                ACTIVE = o.ACTIVE,
                                BGNDATE1 = o.BGNDATE1,
                                BGNDATE2 = o.BGNDATE2,
                                BGNDATE3 = o.BGNDATE3,
                                BGNDATE4 = o.BGNDATE4,
                                BGNDATE5 = o.BGNDATE5,
                                BGNDATE6 = o.BGNDATE6,
                                BGNDATE7 = o.BGNDATE7,
                                BGNDATE8 = o.BGNDATE8,
                                BGNDATE9 = o.BGNDATE9,
                                BGNDATE10 = o.BGNDATE10,
                                BGNDATE11 = o.BGNDATE11,
                                BGNDATE12 = o.BGNDATE12,
                                BGNDATE13 = o.BGNDATE13,
                                ENDDATE1 = o.ENDDATE1,
                                ENDDATE2 = o.ENDDATE2,
                                ENDDATE3 = o.ENDDATE3,
                                ENDDATE4 = o.ENDDATE4,
                                ENDDATE5 = o.ENDDATE5,
                                ENDDATE6 = o.ENDDATE6,
                                ENDDATE7 = o.ENDDATE7,
                                ENDDATE8 = o.ENDDATE8,
                                ENDDATE9 = o.ENDDATE9,
                                ENDDATE10 = o.ENDDATE10,
                                ENDDATE11 = o.ENDDATE11,
                                ENDDATE12 = o.ENDDATE12,
                                ENDDATE13 = o.ENDDATE13,
                                Id = o.Id
							}
						 });


            var fiscalCalendarListDtos = await query.ToListAsync();

            return _fiscalCalendarsExcelExporter.ExportToFile(fiscalCalendarListDtos);
         }


    }
}