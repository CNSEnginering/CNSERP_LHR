

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ERP.GeneralLedger.Transaction.GLTransfer.Dtos;
using ERP.GeneralLedger.Transaction.GLTransfer.Exporting;
using ERP.GeneralLedger.SetupForms;
using ERP.CommonServices;

namespace ERP.GeneralLedger.Transaction.GLTransfer
{
	[AbpAuthorize(AppPermissions.Pages_GLTransfer)]
    public class GLTransferAppService : ERPAppServiceBase, IGLTransferAppService
    {
		private readonly IRepository<GLTransfer> _glTransferRepository;
		private readonly IGLTransferExcelExporter _glTransferExcelExporter;
        private readonly IRepository<GLLocation> _glLocationRepository;
        private readonly IRepository<Bank, int> _bankRepository;
        private readonly IRepository<ChartofControl, string> _chartofControlRepository;


        public GLTransferAppService(
              IRepository<GLTransfer> glTransferRepository, 
              IGLTransferExcelExporter glTransferExcelExporter,
              IRepository<GLLocation> glLocationRepository,
              IRepository<Bank, int> bankRepository,
              IRepository<ChartofControl, string> chartofControlRepository
        ) 
		  {
			_glTransferRepository = glTransferRepository;
			_glTransferExcelExporter = glTransferExcelExporter;
            _glLocationRepository = glLocationRepository;
            _bankRepository = bankRepository;
            _chartofControlRepository = chartofControlRepository;

        }

		 public async Task<PagedResultDto<GetGLTransferForViewDto>> GetAll(GetAllGLTransferInput input)
         {

            var filteredGLTransfer = _glTransferRepository.GetAll()
                        .WhereIf(input.MinDOCIDFilter != null, e => e.DOCID >= input.MinDOCIDFilter)
                        .WhereIf(input.MaxDOCIDFilter != null, e => e.DOCID <= input.MaxDOCIDFilter)
                        .WhereIf(input.MinDOCDATEFilter != null, e => e.DOCDATE >= input.MinDOCDATEFilter)
                        .WhereIf(input.MaxDOCDATEFilter != null, e => e.DOCDATE <= input.MaxDOCDATEFilter)
                        .WhereIf(input.MinTRANSFERDATEFilter != null, e => e.TRANSFERDATE >= input.MinTRANSFERDATEFilter)
                        .WhereIf(input.MaxTRANSFERDATEFilter != null, e => e.TRANSFERDATE <= input.MaxTRANSFERDATEFilter);

            var pagedAndFilteredGLTransfer = filteredGLTransfer
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

			var glTransfer = from o in pagedAndFilteredGLTransfer
                         select new GetGLTransferForViewDto() {
							GLTransfer = new GLTransferDto
							{
                                DOCID = o.DOCID,
                                DOCDATE = o.DOCDATE,
                                TRANSFERDATE = o.TRANSFERDATE,
                                DESCRIPTION = o.DESCRIPTION,
                                FROMLOCID = o.FROMLOCID,
                                FROMBANKID = o.FROMBANKID,
                                FROMCONFIGID = o.FROMCONFIGID,
                                FROMBANKACCID = o.FROMBANKACCID,
                                FROMACCID = o.FROMACCID,
                                TOLOCID = o.TOLOCID,
                                TOBANKID = o.TOBANKID,
                                TOCONFIGID = o.TOCONFIGID,
                                TOBANKACCID = o.TOBANKACCID,
                                TOACCID = o.TOACCID,
                                TRANSFERAMOUNT = o.TRANSFERAMOUNT,
                                STATUS = o.STATUS,
                                GLLINKIDFROM = o.GLLINKIDFROM,
                                GLLINKIDTO = o.GLLINKIDTO,
                                GLDOCIDFROM = o.GLDOCIDFROM,
                                GLDOCIDTO = o.GLDOCIDTO,
                                AUDTUSER = o.AUDTUSER,
                                AUDTDATE = o.AUDTDATE,
                                CreatedBy = o.CreatedBy,
                                CreatedOn = o.CreatedOn,
                                Id = o.Id
							}
						};

            var totalCount = await filteredGLTransfer.CountAsync();

            return new PagedResultDto<GetGLTransferForViewDto>(
                totalCount,
                await glTransfer.ToListAsync()
            );
         }

        public async Task<GetGLTransferForViewDto> GetGLTransferForView(int id)
        {
            var glTransfer = await _glTransferRepository.GetAsync(id);

            var output = new GetGLTransferForViewDto { GLTransfer = ObjectMapper.Map<GLTransferDto>(glTransfer) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_GLTransfer_Edit)]
		 public async Task<GetGLTransferForEditOutput> GetGLTransferForEdit(EntityDto input)
         {
            var glTransfer = await _glTransferRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetGLTransferForEditOutput {GLTransfer = ObjectMapper.Map<CreateOrEditGLTransferDto>(glTransfer) };
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditGLTransferDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_GLTransfer_Create)]
		 protected virtual async Task Create(CreateOrEditGLTransferDto input)
         {
            var glTransfer = ObjectMapper.Map<GLTransfer>(input);

			
			if (AbpSession.TenantId != null)
			{
                glTransfer.TenantId = (int) AbpSession.TenantId;
			}
		

            await _glTransferRepository.InsertAsync(glTransfer);
         }

		 [AbpAuthorize(AppPermissions.Pages_GLTransfer_Edit)]
		 protected virtual async Task Update(CreateOrEditGLTransferDto input)
         {
            var glTransfer = await _glTransferRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, glTransfer);
         }

		 [AbpAuthorize(AppPermissions.Pages_GLTransfer_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _glTransferRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetGLTransferToExcel(GetAllGLTransferForExcelInput input)
         {
			
			var filteredGLTransfer = _glTransferRepository.GetAll()
                        .WhereIf(input.MinDOCIDFilter != null, e => e.DOCID >= input.MinDOCIDFilter)
                        .WhereIf(input.MaxDOCIDFilter != null, e => e.DOCID <= input.MaxDOCIDFilter)
                        .WhereIf(input.MinDOCDATEFilter != null, e => e.DOCDATE >= input.MinDOCDATEFilter)
                        .WhereIf(input.MaxDOCDATEFilter != null, e => e.DOCDATE <= input.MaxDOCDATEFilter)
                        .WhereIf(input.MinTRANSFERDATEFilter != null, e => e.TRANSFERDATE >= input.MinTRANSFERDATEFilter)
                        .WhereIf(input.MaxTRANSFERDATEFilter != null, e => e.TRANSFERDATE <= input.MaxTRANSFERDATEFilter);

            var query = (from o in filteredGLTransfer
                         select new GetGLTransferForViewDto() { 
							GLTransfer = new GLTransferDto
                            {
                                DOCID = o.DOCID,
                                DOCDATE = o.DOCDATE,
                                TRANSFERDATE = o.TRANSFERDATE,
                                DESCRIPTION = o.DESCRIPTION,
                                FROMLOCID = o.FROMLOCID,
                                FROMBANKID = o.FROMBANKID,
                                FROMCONFIGID = o.FROMCONFIGID,
                                FROMBANKACCID = o.FROMBANKACCID,
                                FROMACCID = o.FROMACCID,
                                TOLOCID = o.TOLOCID,
                                TOBANKID = o.TOBANKID,
                                TOCONFIGID = o.TOCONFIGID,
                                TOBANKACCID = o.TOBANKACCID,
                                TOACCID = o.TOACCID,
                                TRANSFERAMOUNT = o.TRANSFERAMOUNT,
                                STATUS = o.STATUS,
                                GLLINKIDFROM = o.GLLINKIDFROM,
                                GLLINKIDTO = o.GLLINKIDTO,
                                GLDOCIDFROM = o.GLDOCIDFROM,
                                GLDOCIDTO = o.GLDOCIDTO,
                                AUDTUSER = o.AUDTUSER,
                                AUDTDATE = o.AUDTDATE,
                                CreatedBy = o.CreatedBy,
                                CreatedOn = o.CreatedOn,
                                Id = o.Id

                            }
						 });


            var glTransferListDtos = await query.ToListAsync();

            return _glTransferExcelExporter.ExportToFile(glTransferListDtos);
         }

        public int GetMaxID()
        {
            var maxid = ((from tab1 in _glTransferRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId) select (int?)tab1.DOCID).Max() ?? 0) + 1;
            return maxid;
        }

        public string GetLocationName(int locID, string type)
        {
            string locName = "";
            switch (type)
            {
                case "FromLoc":
                    locName = _glLocationRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LocId == locID).Count() > 0 ?
                                               _glLocationRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LocId == locID).SingleOrDefault().LocDesc : "";
                    break;

                case "ToLoc":
                    locName = _glLocationRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LocId == locID).Count() > 0 ?
                                               _glLocationRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.LocId == locID).SingleOrDefault().LocDesc : "";
                    break;

                default:
                    break;
            }
            return locName;

        }

        public string GetBankName(string bankID, string type)
        {
            string bankName = "";
            switch (type)
            {
                case "FromBank":
                    bankName = _bankRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.BANKID == bankID).Count() > 0 ?
                                       _bankRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.BANKID == bankID).SingleOrDefault().BANKNAME : "";
                    break;
                case "ToBank":
                    bankName = _bankRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.BANKID == bankID).Count() > 0 ?
                                       _bankRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.BANKID == bankID).SingleOrDefault().BANKNAME : "";
                    break;

                default:
                    break;
            }
            return bankName;

        }

        public string GetChartOfAccountName(string accID, string type)
        {
            string accName;

            switch (type)
            {
                case "FromBankAcc":
                    accName = _chartofControlRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == accID).Count() > 0 ?
                                         _chartofControlRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == accID).SingleOrDefault().AccountName : "";
                    break;
                case "ToBankAcc":
                    accName = _chartofControlRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == accID).Count() > 0 ?
                                         _chartofControlRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == accID).SingleOrDefault().AccountName : "";
                    break;
                case "FromAcc":
                    accName = _chartofControlRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == accID).Count() > 0 ?
                                         _chartofControlRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == accID).SingleOrDefault().AccountName : "";
                    break;
                case "ToAcc":
                    accName = _chartofControlRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == accID).Count() > 0 ?
                                         _chartofControlRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.Id == accID).SingleOrDefault().AccountName : "";
                    break;
                default:
                    accName = null;
                    break;
            }
            return accName;

        }

    }

   
}